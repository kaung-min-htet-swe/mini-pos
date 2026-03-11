namespace mini_pos.Core.ServiceResponse;

public interface IServiceResponse<out TValue>
{
    bool IsSuccess();
    int StatusCode();
    string Message();
    TValue? Data();
    
}