using System.Text.Json.Serialization;

namespace Dashboard.Dtos.Payload.Auth;

public record class AuthResponseDto
(
    [property: JsonPropertyName("userName")]
    string UserName,
    [property: JsonPropertyName("email")]
    string Email,
    [property: JsonPropertyName("token")]
    string Token
);