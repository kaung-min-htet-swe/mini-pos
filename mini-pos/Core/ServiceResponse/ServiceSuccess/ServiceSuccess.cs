using System.Net;

namespace mini_pos.Core.ServiceResponse.ServiceSuccess;

public abstract class ServiceSuccess<TValue>(int statusCode, string message, TValue? data) : IServiceResponse<TValue>
{
    private readonly int _statusCode = statusCode;
    private readonly string _message = message;
    private readonly TValue? _data = data;

    public bool IsSuccess()
    {
        return true;
    }

    public int StatusCode()
    {
        return _statusCode;
    }

    public string Message()
    {
        return _message;
    }

    public TValue? Data()
    {
        return _data;
    }
}