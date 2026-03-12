namespace mini_pos.Core.ServiceResponse.ServiceFailure;

public class BadRequest<TValue>(string message)
    : ServiceFailure<TValue>(StatusCodes.Status400BadRequest, message, default)
{
}