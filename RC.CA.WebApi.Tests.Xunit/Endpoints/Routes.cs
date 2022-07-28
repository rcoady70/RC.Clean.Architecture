namespace RC.CA.WebApi.Tests.Xunit.Endpoints;

public static class Routes
{
    public const string BaseRoute = "";

    public static class Members
    {
        private const string BaseAuthorsRoute = $"{Routes.BaseRoute}/api/club/members";
        public static string Get() => $"{BaseAuthorsRoute}/get";
        public static string List() => $"{BaseAuthorsRoute}/list";
        public static string Delete(int id) => $"{BaseAuthorsRoute}/delete/{id}";
        public static string Create() => $"{BaseAuthorsRoute}/create";
        public static string Update() => $"{BaseAuthorsRoute}/create";
        public static string List(int perPage, int page) => $"{BaseAuthorsRoute}?perPage={perPage}&page={page}";
    }
    public static class Authentication
    {
        private const string BaseAuthRoute = $"{Routes.BaseRoute}/api/useraccount";
        public static string Login() => $"{BaseAuthRoute}/login";
    }
}
