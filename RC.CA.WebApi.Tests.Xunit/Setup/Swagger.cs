﻿using Xunit.Abstractions;

namespace RC.CA.WebApi.Tests.Xunit.Setup;

public class Swagger
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Swagger(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    //	[Fact]
    //	public async Task SwaggerUI_Responds_OK_In_Development()
    //	{
    //		var logger = XUnitLogger.CreateLogger<Swagger>(_testOutputHelper);

    //		await using var application = new WebApiApplication(_testOutputHelper);

    //		var client = application.CreateClient(new() { BaseAddress = new Uri("https://localhost") });
    ////		_client = new WebApiApplication(_testOutputHelper).CreateClient(new() { BaseAddress = new Uri("https://localhost") });

    //		var response = await client.GetAsync("/index.html");

    //		logger.LogInformation($"URL: {response.RequestMessage.RequestUri.AbsoluteUri}");

    //		response.EnsureSuccessStatusCode();
    //	}


}
