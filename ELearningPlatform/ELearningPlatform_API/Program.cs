using ELearningPlatform_Data.DBContext;
using ELearningPlatform_Data.Infracstructure.Implementation;
using ELearningPlatform_Data.Infracstructure.Interfaces;
using Microsoft.EntityFrameworkCore;

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
    app.UseSwaggerUI();
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

    // Authen
    build.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
}