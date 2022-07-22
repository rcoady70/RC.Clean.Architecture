namespace RC.CA.WebApi.Test.Extensions;

static class HttpClientExtensions
{
    public static HttpClient CreateIdempotentClient(this TestServer server)
    {
        var client = server.CreateClient();
        client.DefaultRequestHeaders.Add("x-requestid", Guid.NewGuid().ToString());
        return client;
    }
}
