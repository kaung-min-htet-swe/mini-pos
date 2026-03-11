using mini_pos.Core.Dtos;
using mini_pos.Core.ServiceResponse;
using mini_pos.Products.Dtos;

namespace mini_pos.Products.Services;

public interface ICategoryService
{
    Task<IServiceResponse<PagedResult<CategoryResponseDto>>> List(CategoryFilter filter);
    Task<IServiceResponse<CategoryResponseDto>> GetById(Guid id);
    Task<IServiceResponse<ValueTuple>> Create(CreateCategoryRequestDto request);
    Task<IServiceResponse<ValueTuple>> Update(Guid id, CreateCategoryRequestDto request);
    Task<IServiceResponse<ValueTuple>> Delete(Guid id);
}