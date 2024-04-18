using DoughnutBank.Authentication.ApiAccess;
using DoughnutBank.Entities.DBContext;
using DoughnutBank.Services.Implementations;
using DoughnutBank.Services.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using DoughnutBank.Repositories.Interfaces;
using DoughnutBank.Repositories.Implementations;
using DoughnutBank.Authentication;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "Allow Frontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins(["https://localhost:3000"]);

                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                      });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => addAuthButtonForSwaggerThroughOptions(options));
void addAuthButtonForSwaggerThroughOptions(SwaggerGenOptions options)
{
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "The API key to access the API",
        Type = SecuritySchemeType.ApiKey,
        Name = "x-api-key",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });

    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };

    var requirement = new OpenApiSecurityRequirement
    {
        {scheme, new List<string>() }
    };

    options.AddSecurityRequirement(requirement);
}


builder.Services.AddDbContext<DoughnutBankContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddScoped<IOTPGenerator, OTPCryptoGenerator>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<OTPService>();
builder.Services.AddTransient<ApiAccessMiddleware>();
builder.Services.AddScoped<AuthorizationFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseMiddleware<ApiAccessMiddleware>();


app.UseAuthorization();

app.MapControllers();

app.Run();
