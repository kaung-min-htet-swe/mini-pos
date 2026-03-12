using mini_pos.Core.Dtos;
using mini_pos.Core.ServiceResponse;
using mini_pos.Features.Users.Dtos;

namespace mini_pos.Features.Users.Services;

public interface ICustomerService
{
    Task<IServiceResponse<PagedResult<CustomerDto>>> List(CustomerFilter filter);
    Task<IServiceResponse<CustomerDto>> GetById(Guid id);
    Task<IServiceResponse<ValueTuple>> Create(CustomerCreateDto customerDto);
    Task<IServiceResponse<ValueTuple>> Update(Guid id, CustomerCreateDto customer);
    Task<IServiceResponse<ValueTuple>> Delete(Guid id);
}