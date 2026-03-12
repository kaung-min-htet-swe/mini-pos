using mini_pos.Core.Dtos;
using mini_pos.Core.ServiceResponse;
using mini_pos.Features.Users.Dtos;

namespace mini_pos.Features.Users.Services;

public interface IMerchantService
{
    Task<IServiceResponse<PagedResult<MerchantDto>>> List(MerchantFilter filter);
    Task<IServiceResponse<MerchantDto>> GetById(Guid id);
    Task<IServiceResponse<ValueTuple>> Create(MerchantCreateDto merchantDto);
    Task<IServiceResponse<ValueTuple>> Update(Guid id, MerchantCreateDto merchant);
    Task<IServiceResponse<ValueTuple>> Delete(Guid id);
}