using mini_pos.Core.Dtos;
using mini_pos.Core.ServiceResponse;
using mini_pos.Features.Orders.Dtos;

namespace mini_pos.Features.Orders.Services;

public interface IOrderService
{
    Task<IServiceResponse<PagedResult<OrderDto>>> List(OrderFilter filter);
    Task<IServiceResponse<OrderDto>> GetById(Guid id);
    Task<IServiceResponse<ValueTuple>> Create(CreateOrderDto orderDto);
    Task<IServiceResponse<ValueTuple>> Update(Guid id, CreateOrderDto order);
    Task<IServiceResponse<ValueTuple>> Delete(Guid id);
}