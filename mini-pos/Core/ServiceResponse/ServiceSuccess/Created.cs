namespace mini_pos.Core.ServiceResponse.ServiceSuccess;

public class Created<TValue>(string message)
    : ServiceSuccess<TValue>(StatusCodes.Status201Created, message, default)
{
}