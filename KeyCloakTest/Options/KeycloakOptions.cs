namespace KeyCloakTest.Options;
public record KeycloakOptions
{
    public string ClientId { get; set; }
    public string Authority { get; set; }
    public string RedirectUri { get; set; }
}
