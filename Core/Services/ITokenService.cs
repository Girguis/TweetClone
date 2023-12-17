using Core.Enums;

namespace Core.Services;

public interface ITokenService
{
    Result<string, Exception> Generate(string guid);
    Result<string, Exception> GetUserGuid(string token);
    Result<TokenStatus, Exception> Validate(string token);
}
