using System.Security.Claims;

namespace BitzArt.Blazor.Auth;

internal class ClaimsPrincipalDto
{
    public ICollection<ClaimsIdentityDto>? Identities { get; set; }
}

internal static class ClaimsPrincipalMappingExtensions
{
    public static ClaimsPrincipalDto ToDto(this ClaimsPrincipal model) => new()
    {
        Identities = model.Identities.Select(x => x.ToDto()).ToList()
    };

    public static ClaimsPrincipal ToModel(this ClaimsPrincipalDto dto)
    {
        var identities = dto.Identities?.Select(x => x.ToModel()).ToList();

        if (identities is null) return new();

        return new(identities);
    }
}