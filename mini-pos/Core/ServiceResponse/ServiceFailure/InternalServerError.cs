namespace mini_pos.Core.ServiceResponse.ServiceFailure;

public class InternalServerError<TValue>(string message)
    : ServiceFailure<TValue>(StatusCodes.Status500InternalServerError, message, default)
{
}