
using EventManager.Database;
using EventManager.Services;
using EventManager.Shared.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;

namespace EventManager
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var connectionStrings = new List<string>()
            {
                builder.Configuration.GetConnectionString("EventManagerDb") ?? "",
            };

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionStrings[0]);
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build();
            builder.Services.AddDbContext<EventManagerDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseNpgsql(dataSource, options =>
                {
                    options.EnableRetryOnFailure(2);
                });
            }
            );

            builder.Services.AddAuthorizationBuilder();
            builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            })
            .AddEntityFrameworkStores<EventManagerDbContext>()
            .AddDefaultTokenProviders();

            //Services registration
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IEventService, EventService>();


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            if (Environment.GetEnvironmentVariable("IS_RUNNING_TESTS") == null || Environment.GetEnvironmentVariable("IS_RUNNING_TESTS") == "false")
            {
                builder.Services.AddCors(option => option.AddPolicy("frontend",
                    policy => policy.WithOrigins(builder.Configuration["BackendUrl"] ?? "",
                    builder.Configuration["FrontendUrl"] ?? "")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    ));
            }

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())

            {
                var dbContext = scope.ServiceProvider.GetRequiredService<EventManagerDbContext>();
                await dbContext.Database.MigrateAsync();

                //Seed the db with preset users
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                string[] names = new string[4] { "tester", "janos", "vonat", "arlie.donnelly" };
                string[] emails = new string[4] { "tester@chester.com", "janos@fennajanoshegyen.hu", "vonat@nemvar.mav", "arlie.donnelly@yahoo.com" };
                string password = "String123";
                for (int i = 0; i < 4; i++)
                {
                    var exists =await userService.GetUserByName(names[i]);
                    if (!exists.Success)
                    {
                        Log.Information("Creating test user {username} {email}", names[i], emails[i]);
                        await userService.Create(new RegisterRequest() { Username = names[i], Email = emails[i], ConfirmPassword = password, Password = password });
                    }
                    else {
                        Log.Information("Test user {username} already exists skipping it", names[i]);
                    }
                }
            }

            if (Environment.GetEnvironmentVariable("IS_RUNNING_TESTS") == null || Environment.GetEnvironmentVariable("IS_RUNNING_TESTS") == "false")
            {
                app.UseCors("frontend");
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            await app.RunAsync();
        }
    }
}
