namespace Earthly.Contracts.V1;

public static class ApiRoutes
{
    private const string Root = "api";
    private const string Version = "v1";
    private const string Base = Root + "/" + Version;

    public static class Earth
    {
        public const string GetAll = Base + "/Earth";
        public const string Create = Base + "/Earth";
    } 
    
    public static class Countries
    {
        public const string GetAll = Base + "/Countries";
        public const string Create = Base + "/Countries";
        public const string Get = Base + "/Countries/{countryName}";
    }
}



