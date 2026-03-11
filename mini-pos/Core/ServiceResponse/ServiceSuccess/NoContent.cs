namespace mini_pos.Core.ServiceResponse.ServiceSuccess;

public class NoContent<TValue>(string message):ServiceSuccess<TValue?>(StatusCodes.Status204NoContent, message, default)
{
    
}