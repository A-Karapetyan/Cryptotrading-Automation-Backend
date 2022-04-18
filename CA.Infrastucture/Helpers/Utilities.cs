using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CA.Infrastucture.Helpers
{
    public static class Utilities
    {
        public static string KeyGenerator(int count)
        {
            var sb = new StringBuilder();
            for (int i = 1; i <= count; i++)
            {
                sb.Append("9");
            }
            var limitString = sb.ToString();
            int.TryParse(limitString, out int limit);
            if (limit == 0)
                return null;
            var random = new Random();
            return random.Next(0, limit).ToString($"D{count}");
        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
                throw new ApplicationException(nameof(password));
            using (var bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            var dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static bool VerifyHashedPassword(string hashPassword, string password)
        {
            byte[] buffer4;
            if (hashPassword == null)
                return false;
            if (password == null)
                throw new Exception(nameof(password));
            var src = Convert.FromBase64String(hashPassword);
            if (src.Length != 0x31 || src[0] != 0)
                return false;
            var dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            var buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (var bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArrayCompare(buffer3, buffer4);
        }

        private static bool ByteArrayCompare(IReadOnlyCollection<byte> a1, IReadOnlyList<byte> a2)
        {
            if (a1.Count != a2.Count)
                return false;
            return !a1.Where((t, i) => t != a2[i]).Any();
        }

        public static string Token(int personId)
        {
            var now = DateTime.UtcNow;
            const string secretKey = "mysupersecret_secretkey!PT";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, personId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, now.Second.ToString(), ClaimValueTypes.Integer64),
                    new Claim("personId", personId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encodedJwt = tokenHandler.WriteToken(token);
            return encodedJwt;
        }
    }
}
