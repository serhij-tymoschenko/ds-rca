using Newtonsoft.Json;

namespace ds_rca.data.remote.dto;

public class AuthDto
{
    [JsonProperty("access_token")] public required string Token { get; init; }
}