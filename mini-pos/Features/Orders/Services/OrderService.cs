using Microsoft.EntityFrameworkCore;
using mini_pos.Core.Dtos;
using mini_pos.Core.ServiceResponse;
using mini_pos.Core.ServiceResponse.ServiceFailure;
using mini_pos.Core.ServiceResponse.ServiceSuccess;
using mini_pos.Features.Orders.Dtos;
using ms_sql;
using OrderItem = ms_sql.OrderItem;

namespace mini_pos.Features.Orders.Services;

public class OrderService(PosContext db) : IOrderService
{
    private readonly DbSet<Admin> _adminDb = db.Admins;
    private readonly DbSet<Customer> _customerDb = db.Customers;
    private readonly DbSet<Order> _orderDb = db.Orders;
    private readonly DbSet<OrderItem> _orderItemDb = db.OrderItems;
    private readonly DbSet<Product> _productDb = db.Products;

    public async Task<IServiceResponse<PagedResult<OrderDto>>> List(OrderFilter filter)
    {
        try
        {
            var skip = (filter.PageNumber - 1) * filter.Limit;
            var take = filter.Limit;
            var query = db.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                .AsQueryable();

            var totalCount = await query.CountAsync();
            var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var orderDtos = orders.Select(o => new OrderDto(o.Id, o.OrderDate, o.TotalAmount)).ToList();
            var result = new PagedResult<OrderDto>(orderDtos, totalCount, filter.PageNumber, filter.Limit);

            return new Ok<PagedResult<OrderDto>>("Orders retrieved successfully", result);
        }
        catch (Exception e)
        {
            return new InternalServerError<PagedResult<OrderDto>>(e.Message);
        }
    }

    public async Task<IServiceResponse<OrderDto>> GetById(Guid id)
    {
        try
        {
            var order = await db.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                .ThenInclude(item => item.Product)
                .Include(o => o.ProcessedBy)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync();

            if (order == null)
                return new NotFound<OrderDto>("$Order with {id} not found");

            var orderItems = order.OrderItems
                .Select(oi => new OrderItemDto(oi.Id, oi.Quantity, oi.UnitPrice, oi.SubTotal,
                        new ProductDto(oi.Product.Id, oi.Product.Name, oi.Product.Price, oi.Product.Sku)
                    )
                ).ToList();

            var adminDto = new AdminDto(order.ProcessedBy.Id, order.ProcessedBy.Username);
            var customerDto = new CustomerDto(order.Customer.Id, order.Customer.Name);
            var orderDto = new OrderDto(
                order.Id,
                order.OrderDate,
                order.TotalAmount,
                orderItems,
                adminDto,
                customerDto
            );

            return new Ok<OrderDto>("Order retrieved successfully", orderDto);
        }
        catch (Exception e)
        {
            return new InternalServerError<OrderDto>(e.Message);
        }
    }

    public async Task<IServiceResponse<ValueTuple>> Create(CreateOrderDto orderDto)
    {
        try
        {
            var customerExists = await _customerDb.AnyAsync(c => c.Id == orderDto.CustomerId);
            if (!customerExists)
                return new NotFound<ValueTuple>("Customer not found");

            var merchantExists = await _adminDb.AnyAsync(a => a.Id == orderDto.MerchantId);
            if (!merchantExists)
                return new NotFound<ValueTuple>("Merchant not found");

            var productIds = orderDto.OrderItems.Select(oi => oi.ProductId).ToList();
            var products = await _productDb.Where(p => productIds.Contains(p.Id)).ToListAsync();

            if (products.Count != productIds.Count)
                return new NotFound<ValueTuple>("One or more products in the order were not found.");

            var orderItems = new List<OrderItem>();
            decimal totalOrderAmount = 0;

            foreach (var oi in orderDto.OrderItems)
            {
                var product = products.First(p => p.Id == oi.ProductId);

                if (product.StockQuantity < oi.Quantity)
                    return new BadRequest<ValueTuple>($"Insufficient stock for: {product.Name}");

                product.StockQuantity -= oi.Quantity;

                var calculatedSubTotal = oi.Quantity * oi.UnitPrice;
                totalOrderAmount += calculatedSubTotal;

                orderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    SubTotal = calculatedSubTotal
                });
            }

            var order = new Order
            {
                CustomerId = orderDto.CustomerId,
                ProcessedById = orderDto.MerchantId,
                OrderDate = orderDto.OrderDate,
                TotalAmount = totalOrderAmount,
                OrderItems = orderItems
            };

            _orderDb.Add(order);
            var result = await db.SaveChangesAsync();

            if (result == 0) return new InternalServerError<ValueTuple>("Failed to create order");

            return new Created<ValueTuple>("Order created successfully");
        }
        catch (Exception e)
        {
            return new InternalServerError<ValueTuple>(e.Message);
        }
    }

    public Task<IServiceResponse<ValueTuple>> Update(Guid id, CreateOrderDto order)
    {
        throw new NotImplementedException();
    }

    public async Task<IServiceResponse<ValueTuple>> Delete(Guid id)
    {
        var order = await _orderDb.FirstOrDefaultAsync(order => order.Id == id);
        if (order is null) return new NotFound<ValueTuple>($"Order with ${id} does not exist.");

        order.DeletedAt = DateTime.Now;
        var result = await db.SaveChangesAsync();

        if (result == 0) return new InternalServerError<ValueTuple>("Failed to delete order");

        return new NoContent<ValueTuple>("Order deleted successfully");
    }
}