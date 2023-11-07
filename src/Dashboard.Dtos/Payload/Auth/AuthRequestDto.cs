using System.Text.Json.Serialization;

namespace Dashboard.Dtos.Payload.Auth;

public record class AuthRequestDto
(
    [property: JsonPropertyName("email")]
    string Email,
    [property: JsonPropertyName("password")]
    string Password
);