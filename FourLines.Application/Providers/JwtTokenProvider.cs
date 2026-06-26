using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace FourLines.Application.Providers;

public sealed class JwtTokenProvider(IConfiguration configuration) : ITokenProvider
{
    public string Create(User user)
    {
        string secretKey = configuration["Jwt:Secret"]!;

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials? credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                //TODO: add email verification
                //new Claim(JwtRegisteredClaimNames.EmailVerified, user.IsActive.ToString()),
            ]
            ),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:DurationMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        JsonWebTokenHandler handler = new();
        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}
