namespace mini_pos.Core.ServiceResponse.ServiceFailure;

public class NotFound<TValue>(string message)
    : ServiceFailure<TValue>(StatusCodes.Status404NotFound, message, default)
{
}