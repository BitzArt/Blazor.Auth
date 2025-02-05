using System.Security.Claims;

namespace BitzArt.Blazor.Auth;

internal class ClaimDto
{
    public required string Type { get; set; }
    public required string Value { get; set; }

    public string? ValueType { get; set; }

    public string? Issuer { get; set; }
    public string? OriginalIssuer { get; set; }
}

internal static class ClaimMappingExtensions
{
    public static ClaimDto ToDto(this Claim model) => new()
    {
        Issuer = model.Issuer,
        OriginalIssuer = model.OriginalIssuer,
        Type = model.Type,
        Value = model.Value,
        ValueType = model.ValueType
    };

    public static Claim ToModel(this ClaimDto dto) => new(dto.Type, dto.Value, dto.ValueType, dto.Issuer, dto.OriginalIssuer);
}