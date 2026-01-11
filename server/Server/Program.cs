using DotNetEnv;
using Server.Application.Services;
using Server.Application;
using Server.Infrastructure;
using Server.Infrastructure.Contexts;
using Server.Infrastructure.Providers;
using Server.Network;
using Server.Network.Session;
using Server.Middleware;

namespace Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try {
                Env.Load("./.env");
                var builder = WebApplication.CreateBuilder(args);

                string? pgConn = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
                string? redisConn = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
                pgConn ??= string.Empty;
                redisConn ??= string.Empty;
                Console.WriteLine($"[Postgres] Connection String: {pgConn}");
                Console.WriteLine($"[Redis] Connection String: {redisConn}");

                builder.Services.AddDbContext<AppDbContext>();
                builder.Services.AddAutoMapper(typeof(MapperProfile));
                builder.Services.AddRepositories();

                builder.Services.AddMemoryCache();
                builder.Services.AddSingleton<RedisManager>(sp =>
                {
                    var redisConn = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING") ?? "localhost";
                    return new RedisManager(redisConn);
                });
                
                // 게임 세션 매니저 등록
                builder.Services.AddSingleton<IGameSessionManager>(sp =>
                {
                    var redis = sp.GetRequiredService<RedisManager>();
                    return new RedisGameSessionManager(redis.Connection);
                });
                
                builder.Services.AddSingleton<IAdminWebSocketService, AdminWebSocketService>();
                builder.Services.AddScoped<RefreshTokenService>();
                builder.Services.AddScoped<SessionService>(sp => new SessionService(sp.GetService<RedisManager>()));

                // JWT 인증 미들웨어 등록
                builder.Services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("WIW_JWT_SECRET"))),
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = true
                        };
                    });

                // CORS 정책 등록 (React 관리자 페이지 허용)
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAdminReact",
                        policy => policy
                            .WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                    );
                });

                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                });

                var app = builder.Build();

                // WebSocket 지원
                app.UseWebSockets();

                // Admin WebSocket 엔드포인트 등록
                app.Map("/ws/admin", async context =>
                {
                    var wsService = context.RequestServices.GetRequiredService<IAdminWebSocketService>();
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        var accountService = context.RequestServices.GetRequiredService<AccountService>();
                        var sessionService = context.RequestServices.GetRequiredService<SessionService>();
                        await AdminWebSocketHandler.Handle(context, webSocket, wsService, accountService, sessionService, context.RequestAborted);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                });

                app.UseMiddleware<GlobalExceptionMiddleware>();
                app.UseMiddleware<RequestResponseLoggingMiddleware>();
                app.UseMiddleware<SecurityHeadersMiddleware>();
                if (!app.Environment.IsEnvironment("Testing"))
                {
                    app.UseMiddleware<RateLimitingMiddleware>();
                }

                app.UseRouting();
                app.UseCors("AllowAdminReact");
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server v1");
                    c.RoutePrefix = "swagger";
                });

                Console.WriteLine("[LOG] Swagger 설정 완료");

                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();

                // 부가 서비스(Task)는 여기서 실행
                _ = Task.Run(async () =>
                {
                    var serviceProvider = app.Services;
                    var sessionManager = serviceProvider.GetRequiredService<IGameSessionManager>();
                    var tcpLogger = serviceProvider.GetRequiredService<ILogger<TcpServer>>();
                    var udpLogger = serviceProvider.GetRequiredService<ILogger<UdpServer>>();
                    
                    var tcpServer = new TcpServer(7000, sessionManager, tcpLogger);
                    await tcpServer.StartAsync();
                    
                    var udpServer = new UdpServer(6000, sessionManager, udpLogger);
                    await udpServer.StartAsync();
                });

                await app.RunAsync();
            }
            catch (Exception ex) {
                Console.WriteLine("[FATAL ERROR] " + ex);
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}