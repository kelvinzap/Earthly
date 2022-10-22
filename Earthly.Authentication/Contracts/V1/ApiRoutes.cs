namespace Earthly.Authentication.Contracts.V1;

public static class ApiRoutes
{
    private const string Root = "api";
    private const string Version = "v1";
    private const string Base = Root + "/" + Version;

    public static class Identity
    {
        public const string Register = Base + "/identity/register";
        public const string Login = Base + "/identity/login";
        public const string Regenerate = Base + "/identity/regenerate";
        public const string Verify = Base + "/identity/verify";
        public const string ConfirmEmail = Base + "/identity/confirmEmail";
    }

}