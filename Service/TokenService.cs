﻿
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

using Charity.Models;
using Charity.Repository.IRepository;
using Charity.Service.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Charity.Dtos.Auth;


namespace Charity.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SymmetricSecurityKey _scretKey;

        public TokenService(IConfiguration config, IUnitOfWork unitOfWork)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _scretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
        }

        public async Task<TokenKit> GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.FullName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var creds = new SigningCredentials(_scretKey, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            await _unitOfWork.RefreshToken.AddAsync(new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId = user.Id,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddDays(1)
            });
            await _unitOfWork.SaveAsync();

            return new TokenKit
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<User?> InvalidateRefreshToken(Guid idUser)
        {
            var customer = await _unitOfWork.User.GetAsync(c => c.Id == idUser);
            if (customer == null) return null;
            var tokens = await _unitOfWork.RefreshToken.GetAllAsync(t => 
            t.User.Id == idUser && !t.IsRevoked);

            if (tokens != null)
            {
                foreach (var token in tokens)
                {
                    token.IsRevoked = true;
                }
                await _unitOfWork.SaveAsync();
            }
            return customer;
        }

        public async Task<TokenKit> RenewToken(TokenKit tokenKit)
        {
            var tokenValidateParam = new TokenValidationParameters
            {

                ValidateIssuer = true,
                ValidIssuer = _config["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(_config["JWT:SigningKey"])
        ),

                ValidateLifetime = true
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                //CASE 1: AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenKit.AccessToken, tokenValidateParam, out var validatedToken);

                //CASE 2: Check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                        throw new BadHttpRequestException("Invalid token");
                }

                //CASE 3: Check accessToken expired?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                    throw new BadHttpRequestException("Access token has not yet expired");

                //CASE 4: Check refreshtoken existed in DB
                var storedToken = await _unitOfWork.RefreshToken.GetAsync(x => x.Token == tokenKit.RefreshToken)
                   ?? throw new BadHttpRequestException("Refresh token does not exist");

                //CASE 5: check refreshToken is used/revoked?
                if (storedToken.IsUsed)
                    throw new BadHttpRequestException("Refresh token has been used");

                if (storedToken.IsRevoked)
                    throw new BadHttpRequestException("Refresh token has been revoked");

                //CASE 6: AccessToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                    throw new BadHttpRequestException("Token doesn't match");

                //Update token is used
                await _unitOfWork.RefreshToken.UpdateRevokedAsync(storedToken);
                await _unitOfWork.SaveAsync();

                //create new token
                var user = await _unitOfWork.User.GetAsync(u => u.Id == storedToken.UserId);
                var token = await GenerateToken(user);

                return token;
            }
            catch (Exception ex)
            {
                throw new BadHttpRequestException(ex.Message);
            }
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }
    }
}
