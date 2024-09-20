namespace RhoMicro.ApplicationFramework.Hosting;
sealed class ApiServicesSettings
{
    public required String BaseUri { get; set; }
    public ApiServiceSettings[] Services { get; set; } = [];

    public Boolean GetIsValid(Boolean ignoreBaseUri) =>
        ( ignoreBaseUri || Uri.IsWellFormedUriString(BaseUri, UriKind.Absolute) )
        && Services.All(s => s is not null && s.IsValid);
}
