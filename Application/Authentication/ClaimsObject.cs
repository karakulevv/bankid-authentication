using Newtonsoft.Json;
using System.Security.Claims;

namespace Application.Authentication;

public class ClaimsObject
{
    public ClaimsObject(IEnumerable<Claim> claims)
    {
        if (!claims.Any(claim => claim.Type == IdentityModel.JwtClaimTypes.Subject))
        {
            throw new ArgumentException("Should contain sub claim");
        }

        Claims = claims;
    }

    [JsonIgnore]
    public Claim Sub => Claims.FirstOrDefault(claims => claims.Type == IdentityModel.JwtClaimTypes.Subject);

    [JsonConverter(typeof(JsonClaimConverter))]
    public IEnumerable<Claim> Claims { get; }
}