using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server;
using Server.Data.Contexts;
using System;
using Server.Data.Providers;
using System.Linq;

public class CustomWebApplicationFactory : WebApplicationFactory<Server.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            // 모든 DbContext, DbContextOptions 및 파생 타입 ServiceDescriptor 제거
            var dbContextDescriptors = services
                .Where(d =>
                    d.ServiceType == typeof(AppDbContext) ||
                    d.ServiceType == typeof(DbContext) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    (d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>)))
                .ToList();
            foreach (var d in dbContextDescriptors)
                services.Remove(d);

            // InMemory DB로 대체
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });
            // DbContext(추상) → AppDbContext(구현) 매핑 추가
            services.AddScoped<DbContext, AppDbContext>();
        });
    }
}
