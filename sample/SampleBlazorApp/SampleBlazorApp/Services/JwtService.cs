using BitzArt.Blazor.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace SampleBlazorApp.Services;

public class JwtService
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly SigningCredentials _signingCredentials;
    private readonly TimeSpan _accessTokenDuration;
    private readonly TimeSpan _refreshTokenDuration;

    public JwtService()
    {
        var options = new JwtOptions
        {
            PublicKey = "MIIBCgKCAQEA12zIJKpaIuNNk2yAdQ4e/EsT7al1hozyi/qFeTduf7BJFS4niFK7k9OL4VJFoUbpDt18y7Yqlz0nsEyinu/7wZJjf646yYymA8jBib/4kxQw6zH7C3qaam283k72pxb+aZOeJ6iU9KNkwTbfMHxKuTHoxySS6VH0vt3Sn0FYWryp8BVdPFlbuJp6K5otksTbdFOPgzgvwNreoI3TgA0e2clRKaEv+FGwhmY6WqR/hp/ebo0mflL2hPwJI1PLzjXdlx1sPHmYYfDTA02eJWkGYVti4oUZ9UTI5pZeRMNItSu1IyjHi45iLDQ+kRaPsx2bL/YZ7NXJu/g+dk7Lb4KdfQIDAQAB",
            PrivateKey = "MIIEogIBAAKCAQEA12zIJKpaIuNNk2yAdQ4e/EsT7al1hozyi/qFeTduf7BJFS4niFK7k9OL4VJFoUbpDt18y7Yqlz0nsEyinu/7wZJjf646yYymA8jBib/4kxQw6zH7C3qaam283k72pxb+aZOeJ6iU9KNkwTbfMHxKuTHoxySS6VH0vt3Sn0FYWryp8BVdPFlbuJp6K5otksTbdFOPgzgvwNreoI3TgA0e2clRKaEv+FGwhmY6WqR/hp/ebo0mflL2hPwJI1PLzjXdlx1sPHmYYfDTA02eJWkGYVti4oUZ9UTI5pZeRMNItSu1IyjHi45iLDQ+kRaPsx2bL/YZ7NXJu/g+dk7Lb4KdfQIDAQABAoIBAE6nMQvyBqbmRtSksOIMHdQPtV74mChgHc5t0X3Id1e3jXdmOpjTXBlFC7VgzHtt4HnE9GOMR1Cgy3TbBiTxigHK6PkdK+maqKKJEeCxbpiErrewr/Ao+2gQWPzx56xqAMmbVAs2yevoHElPN34EY2PqjQrol5sIiUuGwffTa+b0f5fuhPuqsvjBRcHhZX8aCCpqrvoblwVVjWKZKsFv7oWH3+KMdm9iwmuuDdOqq51TNgqaJTSpdTl+luHq8bv7hjkx3omF8R8/JHIc2UTP5a6FA0wO77448WriyIjsUpAN4fROBVOS4a4tuHy8lEQr3udxQ1KWFj3sLBy7ls3+FiECgYEA4ZY5sJ2KmZ6xG/gwI96Skd6sc7jA+XZyo3gGeVHyh7yhooZ4JndyJZtjZ4kdiuRyuxrHOG7ZQo07DFJDDbjgRPqIcyiwbLTVkvV+FE7hPwhhN2OR2TUd4D7w9oY8eZ+C0ABXJwT2breS5XCcehAGgK+A31V3rLY4BdhIoOdNOOMCgYEA9HfVt0uz+6qZW22f4XihehELEBlwGJS8bDMoSjuDRHYDwxFErBVb+KlF0w6MLy1bv6Netdcr4aQtVzHDPpDI3MbG3/aNOq8+TXppIvDhMZTycoYzk3YGC8qbhppFH7/O4CX8Z5dd80Xni9sCVU7ePSFdk5BVi6XHTamq46WDfh8CgYAtWZz5Y4J0hZGHVOqgm2MNzh0PGoo43FYJhNyQUSgXn5VC7hODcCnTY5ylOMxmmqxx7t0z/BzTIz9Gp9bxEESNuWvq8rgc8nGpHI8fGAhyOoYIs4yjhOkfpqecd7n6nVWX6SmcH4RHF8KBO5VJeKVGA4I945mub+dtTWC0cCt3DwKBgAXplAif0xWGFblpWFGKqlUabmsQQm7FwhzXy+SntdAFDqg8Fa4XwiasaVzmYCuP7EUhPVwmfRAy+Um/kVpFBCaaxBqMivPdYyNaj4phywB4+rgcWMj7NMA6QTKrLnrLF8TCBm228nW8vhHa1R6dDrDpyqqT9g2vj7doIBLrYNe/AoGAObf5KfkjrpBBO5n//H1thZzR05g5U/hKDbwhCIzhVqP2RgJxtYkadsA3XA29RXOCD58MRsYH6nYpEw52JyP3tTp6jGSCxOM9ltYji2MEpd82yT3lOsVwJXwDL8uFFQNW2VV2xfFV41mk+5B2VqPjPZa7ewFUKluSkXy1oN8Wg1w=",
            AccessTokenDuration = new TimeSpan(0, 1, 0),
            RefreshTokenDuration = new TimeSpan(0, 2, 0)
        };

        _tokenHandler = new JwtSecurityTokenHandler();

        var privateRsa = RSA.Create();
        var privateKey = Convert.FromBase64String(options.PrivateKey!);
        privateRsa.ImportRSAPrivateKey(privateKey, out _);

        var privateSecurityKey = new RsaSecurityKey(privateRsa);

        _signingCredentials = new SigningCredentials(privateSecurityKey, SecurityAlgorithms.RsaSha256);

        _accessTokenDuration = options.AccessTokenDuration;
        _refreshTokenDuration = options.RefreshTokenDuration;
    }

    public JwtPair BuildJwtPair()
    {
        var issuedAt = DateTime.UtcNow;
        var accessTokenExpiresAt = issuedAt + _accessTokenDuration;

        var accessToken = _tokenHandler.WriteToken(new JwtSecurityToken(
            claims: new[]
            {
                new Claim("tt", "a")
            },
            notBefore: issuedAt,
            expires: accessTokenExpiresAt,
            signingCredentials: _signingCredentials
        ));

        var refreshTokenExpiresAt = issuedAt + _refreshTokenDuration;

        var refreshToken = _tokenHandler.WriteToken(new JwtSecurityToken(
            claims: new[]
            {
                new Claim("tt", "r")
            },
            notBefore: issuedAt,
            expires: refreshTokenExpiresAt,
            signingCredentials: _signingCredentials
        ));

        return new JwtPair
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAt = accessTokenExpiresAt,
            RefreshTokenExpiresAt = refreshTokenExpiresAt
        };
    }
}

internal class JwtException(string errorMessage, Exception? innerException = null) : Exception(errorMessage, innerException);

internal class JwtOptions
{
    public required string PublicKey { get; set; }
    public required string PrivateKey { get; set; }
    public TimeSpan AccessTokenDuration { get; set; }
    public TimeSpan RefreshTokenDuration { get; set; }
}