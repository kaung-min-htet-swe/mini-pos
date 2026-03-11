using System.Net;

namespace mini_pos.Core.ServiceResponse.ServiceSuccess;

public class Ok<TValue>(string message, TValue data) : ServiceSuccess<TValue>(StatusCodes.Status200OK, message, data)
{
}