using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Application.Authentication;

public class JsonClaimConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(Claim));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var claims = new List<Claim>();
        var jArray = JArray.Load(reader);

        foreach (var jobj in jArray)
        {
            string type = (string)jobj["Type"];
            JToken token = jobj["Value"];
            string value = token.Type == JTokenType.String ? (string)token : token.ToString(Formatting.None);
            string valueType = (string)jobj["ValueType"];
            string issuer = (string)jobj["Issuer"];
            string originalIssuer = (string)jobj["OriginalIssuer"];
            claims.Add(new Claim(type, value, valueType, issuer, originalIssuer));
        }
        return claims;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));

        var claims = (IEnumerable<Claim>)value;
        if (claims != null)
        {
            writer.WriteStartArray();
            foreach (var claim in claims)
            {
                JObject jo = new JObject
                 {
                   { "Type", claim.Type },
                   { "Value", claim.Value },
                   { "ValueType", claim.ValueType },
                   { "Issuer", claim.Issuer },
                   { "OriginalIssuer", claim.OriginalIssuer }
                 };
                jo.WriteTo(writer);
            }
            writer.WriteEndArray();
        }
    }
}