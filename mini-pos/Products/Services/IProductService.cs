using mini_pos.Core.Dtos;
using mini_pos.Core.ServiceResponse;
using mini_pos.Products.Dtos;

namespace mini_pos.Products.Services;

public interface IProductService
{
   Task<IServiceResponse<PagedResult<ProductResponseDto>>> List(ProductFilter filter);
    Task<IServiceResponse<ProductResponseDto>> GetById(Guid id);
    Task<IServiceResponse<ValueTuple>> Create(CreateProductRequestDto productRequest);
    Task<IServiceResponse<ValueTuple>> Update(Guid id, CreateProductRequestDto productRequest);
    Task<IServiceResponse<ValueTuple>> Delete(Guid id);
}