using Core;
using Core.Configs;
using Core.Enums;
using Core.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services;

public sealed class TokenService : ITokenService
{
    private readonly string userGuidKey = "Guid";
    private static readonly string issuer = ConfigsManager.TryGet(AppSettingsConfig.JwtIssuer);
    private static readonly string audience = ConfigsManager.TryGet(AppSettingsConfig.JwtAudience);
    private static readonly int expireTime = ConfigsManager.TryGetNumber(AppSettingsConfig.JwtExpireTime, 6);
    private static readonly byte[] key = Encoding.ASCII.GetBytes(ConfigsManager.TryGet(AppSettingsConfig.JwtKey));
    private static readonly EncryptingCredentials encryptingCredentials = new(new SymmetricSecurityKey(key),
                                                                   SecurityAlgorithms.Aes256KW,
                                                                   SecurityAlgorithms.Aes128CbcHmacSha256);
    public Result<TokenStatus, Exception> Validate(string token)
    {
        try
        {
            var info = ExtractToken(token);
            return ResultExtensions<TokenStatus, Exception>.Success(info.Status);
        }
        catch (Exception ex)
        {
            var tokenStatus = ex.Message.Contains("IDX10223") ? TokenStatus.Expired : TokenStatus.Invalid;
            return ResultExtensions<TokenStatus, Exception>.Success(tokenStatus);
        }
    }

    private static (TokenStatus Status, ClaimsPrincipal Principal) ExtractToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                LifetimeValidator = LifetimeValidator,
                TokenDecryptionKey = encryptingCredentials.Key
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            return (TokenStatus.Valid, principal);
        }
        catch (Exception ex)
        {
            var tokenStatus = ex.Message.Contains("IDX10223") ? TokenStatus.Expired : TokenStatus.Invalid;
            return (tokenStatus, null);
        }
    }

    private static bool LifetimeValidator(DateTime? notBefore,
                                          DateTime? expires,
                                          SecurityToken securityToken,
                                          TokenValidationParameters validationParameters)
    {
        return expires != null && expires > DateTime.UtcNow;
    }

    Result<string, Exception> ITokenService.Generate(string guid)
    {
        try
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(userGuidKey, guid),
                }),
                Expires = DateTime.UtcNow.AddHours(expireTime),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                                        (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
                EncryptingCredentials = encryptingCredentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = tokenHandler.WriteToken(token);
            return ResultExtensions<string, Exception>.Success(result);
        }
        catch (Exception ex)
        {
            return ResultExtensions<string, Exception>.Error(ex);
        }
    }

    Result<string, Exception> ITokenService.GetUserGuid(string token)
    {

        try
        {
            var info = ExtractToken(token);
            if (info.Status != TokenStatus.Valid)
                return ResultExtensions<string, Exception>.Error(new Exception("invalid token info"));

            var userGuid = info
                .Principal
                .Claims
                ?.FirstOrDefault(x => x.Type == userGuidKey)
                ?.Value;

            return ResultExtensions<string, Exception>.Success(userGuid);
        }
        catch (Exception ex)
        {
            return ResultExtensions<string, Exception>.Error(ex);
        }
    }
}
