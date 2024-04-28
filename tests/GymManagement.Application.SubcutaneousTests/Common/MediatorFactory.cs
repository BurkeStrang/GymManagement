using System.Security.Claims;
using GymManagement.Api;
using GymManagement.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;

namespace GymManagement.Application.SubcutaneousTests.Common;

public class MediatorFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    private SqliteTestDatabase _testDatabase = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _testDatabase = SqliteTestDatabase.CreateAndInitialize();

        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<DbContextOptions<GymManagementDbContext>>()
                .AddDbContext<GymManagementDbContext>(
                    (sp, options) => options.UseSqlite(_testDatabase.Connection)
                );

            var httpContextAccessorSubstitute = Substitute.For<IHttpContextAccessor>();

            var user = new ClaimsPrincipal(
                new ClaimsIdentity(
                    [
                        new("id", Guid.NewGuid().ToString()),
                        new(ClaimTypes.Name, "TestUser"),
                        new(ClaimTypes.Role, "Admin")
                    ],
                    "TestAuthenticationType"
                )
            );

            var httpContext = new DefaultHttpContext { User = user };

            httpContextAccessorSubstitute.HttpContext.Returns(httpContext);

            services.AddSingleton(httpContextAccessorSubstitute);
        });
    }

    public IMediator CreateMediator()
    {
        var serviceScope = Services.CreateScope();

        _testDatabase.ResetDatabase();

        return serviceScope.ServiceProvider.GetRequiredService<IMediator>();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public new Task DisposeAsync()
    {
        _testDatabase.Dispose();

        return Task.CompletedTask;
    }
}
