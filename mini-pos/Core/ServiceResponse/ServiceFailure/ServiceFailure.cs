namespace mini_pos.Core.ServiceResponse.ServiceFailure;

public abstract class ServiceFailure<T>(int statusCode, string message, T? data) : IServiceResponse<T>
{
    public T? GetData()
    {
        return data;
    }

    public bool IsSuccess()
    {
        return false;
    }

    public int StatusCode()
    {
        return statusCode;
    }

    public string Message()
    {
        return message;
    }

    public T? Data()
    {
        return default;
    }
}