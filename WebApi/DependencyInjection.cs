using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Application;
using Application.GlobalExceptionHandling;
using Application.Services;
using Application.Services.Interfaces;
using Infrastructures.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scrutor;
using WebApi.Services;
using WebAPI.Middlewares;

namespace WebApi;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddWebApplicationService(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(MapperConfigurationProfile).Assembly);
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddCors(options
        => options.AddDefaultPolicy(policy
        => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        builder.Services.AddHttpClient();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        builder.Services.AddHttpClient();
        builder.Services.AddRouting(x =>
        {
            x.LowercaseQueryStrings = true;
            x.LowercaseUrls = true;
        });
        builder.Services.AddScoped<IClaimsService, ClaimsService>();
        builder.Services.AddScoped<IActivitiesService, ActivityService>();
        #region  DI_Appsettings
        var configuration = builder.Configuration.Get<AppSettings>() ?? throw new Exception("Null configuration");

        List<Assembly> assemblies = new List<Assembly>
            {
                typeof(Program).Assembly,
                Application.AssemblyReference.Assembly,
                Infrastructures.AssemblyReference.Assembly
            };
        builder.Services.AddSingleton(configuration);

        #endregion

        #region DI_Services
        builder.Services.Scan(scan => scan
            .FromAssemblies(
                Infrastructures.AssemblyReference.Assembly,
                Application.AssemblyReference.Assembly,
                AssemblyReference.Assembly)
            .AddClasses()
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsMatchingInterface()
            .WithScopedLifetime());
        #endregion
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>
                        {
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ValidIssuer = configuration.JWTOptions.Issuer,
                                ValidAudience = configuration.JWTOptions.Audience,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.JWTOptions.Secret)),
                                ClockSkew = TimeSpan.FromSeconds(1)
                            };
                        });
        builder.Services.AddSwaggerGen(opt =>
           {
               opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend API", Version = "v1" });

               var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
               var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
               opt.IncludeXmlComments(xmlPath);

               opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
               {
                   In = ParameterLocation.Header,
                   Description = "Please enter token",
                   Name = "Authorization",
                   Type = SecuritySchemeType.Http,
                   BearerFormat = "JWT",
                   Scheme = "bearer"
               });

               opt.AddSecurityRequirement(new OpenApiSecurityRequirement
               {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
               });
           });

        builder.Services.AddSingleton<GlobalErrorHandlingMiddleware>();
        builder.Services.AddSingleton<PerformanceMiddleware>();
        builder.Services.AddSingleton<Stopwatch>();
        return builder;
    }
}