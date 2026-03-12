using Microsoft.EntityFrameworkCore;
using mini_pos.Core.Dtos;
using mini_pos.Core.ServiceResponse;
using mini_pos.Core.ServiceResponse.ServiceFailure;
using mini_pos.Core.ServiceResponse.ServiceSuccess;
using mini_pos.Features.Users.Dtos;
using ms_sql;

namespace mini_pos.Features.Users.Services;

public class CustomerService(PosContext db) : ICustomerService
{
    public async Task<IServiceResponse<PagedResult<CustomerDto>>> List(CustomerFilter filter)
    {
        try
        {
            var skip = filter.PageNumber * filter.Limit;
            var take = filter.Limit;
            var query = db.Customers.AsNoTracking().AsQueryable();
            if (filter.SearchTerm != null)
                query = query.Where(customer => customer.Name.Contains(filter.SearchTerm));

            var totalCount = await query.CountAsync();
            var customers = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var customerDtos = customers
                .Select(customer => new CustomerDto(customer.Id, customer.Name, customer.PhoneNumber, customer.Email))
                .ToList();

            var result = new PagedResult<CustomerDto>(customerDtos, totalCount, filter.PageNumber, filter.Limit);
            return new Ok<PagedResult<CustomerDto>>("Customers retrieved successfully", result);
        }
        catch (Exception e)
        {
            return new InternalServerError<PagedResult<CustomerDto>>(e.Message);
        }
    }

    public async Task<IServiceResponse<CustomerDto>> GetById(Guid id)
    {
        try
        {
            var customer = await db.Customers.FirstOrDefaultAsync(customer => customer.Id == id);
            if (customer is null) return new NotFound<CustomerDto>("$Customer with {id} not found");

            var customerDto = new CustomerDto(customer.Id, customer.Name, customer.PhoneNumber, customer.Email);
            return new Ok<CustomerDto>("Customer retrieved successfully", customerDto);
        }
        catch (Exception e)
        {
            return new InternalServerError<CustomerDto>(e.Message);
        }
    }

    public async Task<IServiceResponse<ValueTuple>> Create(CustomerCreateDto customerDto)
    {
        try
        {
            var isEmailExists = await db.Customers.AnyAsync(customer => customer.Email == customerDto.Email);
            if (isEmailExists) return new BadRequest<ValueTuple>($"Email {customerDto.Email} already exists");

            var customer = new Customer
            {
                Name = customerDto.Name,
                PhoneNumber = customerDto.PhoneNumber,
                Email = customerDto.Email
            };
            db.Customers.Add(customer);
            var result = await db.SaveChangesAsync();
            if (result > 0) return new Created<ValueTuple>("Customer created successfully");

            return new InternalServerError<ValueTuple>("Customer creation failed");
        }
        catch (Exception e)
        {
            return new InternalServerError<ValueTuple>(e.Message);
        }
    }

    public async Task<IServiceResponse<ValueTuple>> Update(Guid id, CustomerCreateDto customerDto)
    {
        try
        {
            var customer = await db.Customers.FirstOrDefaultAsync(customer => customer.Id == id);
            if (customer is null) return new NotFound<ValueTuple>("$Customer with {id} not found");

            customer.Name = customerDto.Name;
            customer.PhoneNumber = customerDto.PhoneNumber;
            customer.Email = customerDto.Email;
            customer.UpdatedAt = DateTime.Now;

            var result = await db.SaveChangesAsync();
            if (result > 0) return new NoContent<ValueTuple>("Customer updated successfully");

            return new InternalServerError<ValueTuple>("Customer update failed");
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
            var customer = await db.Customers.FirstOrDefaultAsync(customer => customer.Id == id);
            if (customer is null) return new NotFound<ValueTuple>("$Customer with {id} not found");

            customer.DeletedAt = DateTime.Now;
            var result = await db.SaveChangesAsync();
            if (result > 0) return new NoContent<ValueTuple>("Customer deleted successfully");

            return new InternalServerError<ValueTuple>("Customer deletion failed");
        }
        catch (Exception e)
        {
            return new InternalServerError<ValueTuple>(e.Message);
        }
    }
}