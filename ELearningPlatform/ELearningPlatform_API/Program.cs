using ELearningPlatform_Authentication.Handlers;
using ELearningPlatform_Authentication.Requirements;
using ELearningPlatform_Authentication.TokenValidators;
using ELearningPlatform_Data.DBContext;
using ELearningPlatform_Data.Infracstructure.Implementation;
using ELearningPlatform_Data.Infracstructure.Interfaces;
using ELearningPlatform_Domain.Features.UserFeatures.Commands;
using ELearningPlatform_Domain.Service.Implementation.User;
using ELearningPlatform_Domain.Service.Interface.User;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

AddService(builder);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ELearningPlatform_API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


void AddService(WebApplicationBuilder build)
{
    // Connect to SQL
    build.Services.AddDbContext<ELearningPlatformDbContext>(options =>
        options.UseSqlServer(build.Configuration.GetConnectionString("ELearningPlatformContext")), ServiceLifetime.Transient);
    build.Services.AddScoped<DbContext, ELearningPlatformDbContext>();

    // Injection configuration
    build.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    build.Services.AddTransient<IUnitOfWork, UnitOfWork>();

    // Add Service
    build.Services.AddTransient<IUserService, UserService>();

    // Add MediatR
    build.Services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(CreateUserCommand).Assembly);

    // Authen Swagger
    build.Services.Configure<JwtModel>(build.Configuration.GetSection("appJwt"));
    ServiceProvider? serviceProvider = build.Services.BuildServiceProvider();
    var jwtBearerSettings = serviceProvider.GetService<IOptions<JwtModel>>().Value;

    var authenticationBuilder = build.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
    authenticationBuilder.AddJwtBearer(o =>
    {
        // You also need to update /wwwroot/app/scripts/app.js
        o.SecurityTokenValidators.Clear();
        o.SecurityTokenValidators.Add(new JwtBearerValidator());

        var tokenValidationParameters = new TokenValidationParameters();
        tokenValidationParameters.ValidAudience = jwtBearerSettings.Audience;
        tokenValidationParameters.ValidIssuer = jwtBearerSettings.Issuer;
        tokenValidationParameters.IssuerSigningKey = jwtBearerSettings.SigningKey;

        o.TokenValidationParameters = tokenValidationParameters;
        o.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Path.ToString()
                    .StartsWith("/HUB/", StringComparison.InvariantCultureIgnoreCase))
                    context.Token = context.Request.Query["access_token"];
                return Task.CompletedTask;
            }
        };
    });

    // Add required Authenticated
    build.Services.AddMvc(mvcOptions =>
    {
        // Only allow authenticated users
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()    // Require users to be authenticated 
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)    // Specify that JWT Bearer should be used for authentication
            .AddRequirements(new SolidAccountRequirement())    // Add a specific requirement: users must have a valid account via SolidAccountRequirement
            .Build();

        // Apply the AuthorizeFilter with the policy created
        // This ensures all actions in the application adhere to this authorization policy
        mvcOptions.Filters.Add(new AuthorizeFilter(policy));
    });

    // Add Swagger
    build.Services.AddSwaggerGen(c =>
    {
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            Scheme = "bearer",
            BearerFormat = "JWT",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "TheCodeBuzz-Service",
            Version = "v1"
        });

        c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                jwtSecurityScheme, Array.Empty<string>()
            }
        });
    });

    // Authen
    build.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    build.Services.AddScoped<IProfileService, ProfileService>();

    // Requirement Handler
    build.Services.AddScoped<IAuthorizationHandler, SolidAccountRepuirementHandler>();
    build.Services.AddScoped<IAuthorizationHandler, RoleRequirementHandler>();
}