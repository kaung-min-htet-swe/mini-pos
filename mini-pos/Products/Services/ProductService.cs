using Microsoft.EntityFrameworkCore;
using mini_pos.Core.Dtos;
using mini_pos.Core.ServiceResponse;
using mini_pos.Core.ServiceResponse.ServiceFailure;
using mini_pos.Core.ServiceResponse.ServiceSuccess;
using mini_pos.Products.Dtos;
using ms_sql;

namespace mini_pos.Products.Services;

public class ProductService(PosContext db) : IProductService
{
    public async Task<IServiceResponse<PagedResult<ProductResponseDto>>> List(ProductFilter filter)
    {
        try
        {
            var skip = (filter.PageNumber - 1) * filter.Limit;
            var take = filter.Limit;
            var query = db.Products.AsNoTracking().AsQueryable();
            if (filter.SearchTerm != null) query = query.Where(p => p.Name.Contains(filter.SearchTerm));

            var totalCount = await query.CountAsync();
            var products = await query.OrderByDescending(p => p.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            var productResponseDtos = products
                .Select(p => new ProductResponseDto(p.Id, p.Name, p.Price, p.Sku, p.StockQuantity)).ToList();

            var result = new PagedResult<ProductResponseDto>(productResponseDtos, totalCount, filter.PageNumber,
                filter.Limit);
            return new Ok<PagedResult<ProductResponseDto>>(
                "Products retrieved successfully", result);
        }
        catch (Exception e)
        {
            return new InternalServerError<PagedResult<ProductResponseDto>>(e.Message);
        }
    }

    public async Task<IServiceResponse<ProductResponseDto>> GetById(Guid id)
    {
        try
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return new NotFound<ProductResponseDto>(
                    "$Product with {id} not found");

            var productResponseDto = new ProductResponseDto(product.Id, product.Name, product.Price, product.Sku,
                product.StockQuantity);

            return new Ok<ProductResponseDto>("Product retrieved successfully", productResponseDto);
        }
        catch (Exception e)
        {
            return new InternalServerError<ProductResponseDto>(e.Message);
        }
    }

    public async Task<IServiceResponse<ValueTuple>> Create(CreateProductRequestDto productRequest)
    {
        try
        {
            var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == Guid.Parse(productRequest.CategoryId));
            if (category == null)
                return new NotFound<ValueTuple>(
                    "$Category with {id} not found");

            var product = new Product
            {
                Name = productRequest.Name,
                Price = productRequest.Price,
                Sku = productRequest.Sku,
                StockQuantity = productRequest.StockQuantity,
                CategoryId = Guid.Parse(productRequest.CategoryId)
            };

            db.Products.Add(product);
            var result = await db.SaveChangesAsync();
            if (result > 0) return new Created<ValueTuple>("Product created successfully");

            return new InternalServerError<ValueTuple>("Product creation failed");
        }
        catch (Exception e)
        {
            return new InternalServerError<ValueTuple>(e.Message);
        }
    }

    public async Task<IServiceResponse<ValueTuple>> Update(Guid id, CreateProductRequestDto productRequest)
    {
        try
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return new NotFound<ValueTuple>(
                    "$Product with {id} not found");

            product.Name = productRequest.Name;
            product.Price = productRequest.Price;
            product.Sku = productRequest.Sku;
            product.StockQuantity = productRequest.StockQuantity;
            product.CategoryId = Guid.Parse(productRequest.CategoryId);
            product.UpdatedAt = DateTime.Now;

            var result = await db.SaveChangesAsync();
            if (result > 0) return new NoContent<ValueTuple>("Product updated successfully");

            return new InternalServerError<ValueTuple>("Product update failed");
        }
        catch (Exception e)
        {
            return new InternalServerError<ValueTuple>(e.Message);
        }
    }

    public async Task<IServiceResponse<ValueTuple>> Delete(Guid id)
    {
        try
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return new NotFound<ValueTuple>(
                    "$Product with {id} not found");

            product.DeletedAt = DateTime.Now;
            var result = await db.SaveChangesAsync();

            if (result > 0) return new NoContent<ValueTuple>("Product deleted successfully");

            return new InternalServerError<ValueTuple>("Product deletion failed");
        }
        catch (Exception e)
        {
            return new InternalServerError<ValueTuple>(e.Message);
        }
    }
}