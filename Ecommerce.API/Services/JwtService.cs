using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce.BD.Models;
using Ecommerce.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.API.Services
{
    public interface IJwtService
    {
        string GenerateToken(Usuario usuario);
        string GenerateRefreshToken();
        ClaimsPrincipal? ValidateToken(string token);
        bool ValidateRefreshToken(string refreshToken);
    }

    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public string GenerateToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuId.ToString()),
                new Claim(ClaimTypes.Name, $"{usuario.UsuNombre} {usuario.UsuApellido}"),
                new Claim(ClaimTypes.Email, usuario.UsuEmail),
                new Claim("UsuId", usuario.UsuId.ToString()),
                new Claim("UsuNombre", usuario.UsuNombre),
                new Claim("UsuApellido", usuario.UsuApellido),
                new Claim("UsuEmail", usuario.UsuEmail),
                new Claim("UsuEstadoCuenta", usuario.UsuEstadoCuenta ?? "Activo")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public bool ValidateRefreshToken(string refreshToken)
        {
            // En una implementación real, validarías el refresh token contra la base de datos
            // Por ahora, solo verificamos que no esté vacío
            return !string.IsNullOrEmpty(refreshToken);
        }
    }
} 