using System.Security.Claims;

namespace BitzArt.Blazor.Auth;

internal class ClaimsIdentityDto
{
    public string? AuthenticationType { get; set; }
    public ICollection<ClaimDto>? Claims { get; set; }
}

internal static class ClaimsIdentityMappingExtensions
{
    public static ClaimsIdentityDto ToDto(this ClaimsIdentity model) => new()
    {
        AuthenticationType = model.AuthenticationType,
        Claims = model.Claims.Select(x => x.ToDto()).ToList()
    };

    public static ClaimsIdentity ToModel(this ClaimsIdentityDto dto)
    {
        var claims = dto.Claims?.Select(x => x.ToModel()).ToList();
        var authenticationType = dto.AuthenticationType;

        if (claims is null) return new(authenticationType);

        return new(claims, authenticationType);
    }
}