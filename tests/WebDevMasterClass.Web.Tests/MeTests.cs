﻿using System.Net;
using WebDevMasterClass.Testing;

namespace WebDevMasterClass.Web.Tests;

public class MeTests
{
    [Fact]
    public Task GET_returns_HTTP_401_Unauthorized_if_not_authenticated()
        => TestHelper.ExecuteTest<Program>(
            isAuthenticated: false,
            test: async client => {
                var response = await client.GetAsync("/api/me");

                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        );

    [Fact]
    public Task Get_returns_HTTP_200_OK_and_name_if_authenticated()
        => TestHelper.ExecuteTest<Program>(
                test: async client =>
                {
                    var response = await client.GetAsync("/api/me");

                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    Assert.Equal("\"test\"", await response.Content.ReadAsStringAsync());
                }
            );
}
