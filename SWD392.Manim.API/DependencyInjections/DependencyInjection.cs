using SWD392.Manim.Repositories.Repository.Implement;
using SWD392.Manim.Repositories.Repository.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories;
using SWD392.Manim.Services.Mapper;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Google;
using SWD392.Manim.Services.Services;

namespace SWD392.Manim.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            services.AddDbContext<Swd392Context>(options => options.UseLazyLoadingProxies().UseSqlServer(CreateConnectionString(configuration)));
            return services;
        }

        private static string CreateConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("ConnectionStrings:MyConnectionString");
            return connectionString ?? "";
        }

        //public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        //{

        //    return services;
        //}
        private static string CreateClientId(IConfiguration configuration)
        {
            var clientId = Environment.GetEnvironmentVariable("GOOGLE_OAUTH_CLIENT_ID")
                           ?? configuration.GetValue<string>("Oauth:ClientId");
            return clientId;
        }

        public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
        {
            services.AddHttpClient(); // Registers HttpClient
            return services;
        }

        private static string CreateClientSecret(IConfiguration configuration)
        {
            var clientSecret = Environment.GetEnvironmentVariable("GOOGLE_OAUTH_CLIENT_SECRET")
                               ?? configuration.GetValue<string>("Oauth:ClientSecret");
            return clientSecret;
        }

        public static IServiceCollection AddGoogleAuthentication(this IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddGoogle(options =>
            {
                options.ClientId = CreateClientId(configuration);
                options.ClientSecret = CreateClientSecret(configuration);
                options.SaveTokens = true;

            });
            return services;
        }
        public static IServiceCollection AddConfigSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Manim System", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
                });
                options.MapType<TimeOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "time",
                    Example = OpenApiAnyFactory.CreateFromJson("\"13:45:42.0000000\"")
                });
            });
            return services;
        }
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepository();
            services.AddAutoMapper();
            services.AddServices();
            services.SeedData();
            services.AddAutoMapper();
        }
        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        }
        private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserProfile).Assembly);
            services.AddAutoMapper(typeof(ChapterProfile).Assembly);
            services.AddAutoMapper(typeof(ProblemProfile).Assembly);
            services.AddAutoMapper(typeof(SolutionProfile).Assembly);
            services.AddAutoMapper(typeof(SubjectProfile).Assembly);
            services.AddAutoMapper(typeof(TopicProfile).Assembly);
            services.AddAutoMapper(typeof(ParameterProfile).Assembly);

        }
        public static void AddServices(this IServiceCollection services)
        {
            // Load the assembly of the Services project
            //Assembly servicesAssembly = Assembly.Load("SWD392.Manim.Services"); // Replace with your actual Services project assembly name

            //// Find all classes in the Services assembly with interfaces and register them
            //var serviceTypes = servicesAssembly
            //    .GetTypes()
            //    .Where(t => t.IsClass && !t.IsAbstract)
            //    .SelectMany(t => t.GetInterfaces(), (t, i) => new { Implementation = t, Interface = i })
            //    .Where(t => t.Interface != null) // Ensure the type has an interface
            //    .ToList();

            //foreach (var service in serviceTypes)
            //{
            //    // Register the service with scoped lifetime
            //    services.AddScoped(service.Interface, service.Implementation);
            //}
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IChapterService, ChapterService>();
            services.AddScoped<IGoogleAuthenticationService, AuthService>();
            services.AddScoped<IParameterService, ParameterService>();
            services.AddScoped<IPayService, PayService>();
            services.AddScoped<IProblemService, ProblemService>();
            services.AddScoped<ISolutionService, SolutionService>();
            services.AddScoped<ISolutionTypeService, SolutionTypeService>();
            services.AddScoped<ISolutionOutputService, SolutionOutputService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
        }

        public static void SeedData(this IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<Swd392Context>();
            var initialiser = new SeedData(context);
            initialiser.SeedingData();
        }
        public class ServicesType
        {
            public Type? Implementation { get; set; }
            public Type? Interface { get; set; }
        }
    }
}
