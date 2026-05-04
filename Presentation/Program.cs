using Application;
using Infraestructure;
using Infraestructure.Models.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.Extensions;
using Presentation.Middlewares;
using Serilog;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configurations) => configurations.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRateLimiterPolicies();

var conn = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services
    .AddApplicationLayer()
    .AddInfraestructureLayer(builder.Configuration, conn);


builder.Services.AddCors(o => o.AddPolicy("AllowOrigin", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("x-pagination");
}));

builder.Services.AddHttpClient();

builder.Services.AddHttpContextAccessor();

# region SWAGGER

var version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
var environmentName = builder.Environment;
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = $" App Api ", Version = $"{version} - {environmentName}" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKey",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter Bearer [space] ant then your token in the text input below"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            }
                        },
                        new string[] { }
                    }
                });
});

#endregion  SWAGGER

#region Auth Configs
builder.Services.AddAuthorization();

var jwtConfig = builder.Configuration.GetSection(TokenManageOptions.SectionName);

var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig["SecretKey"]));
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = signingKey,
    ValidateIssuer = true,
    ValidIssuer = jwtConfig["Issuer"],
    ValidateAudience = true,
    ValidAudience = jwtConfig["Audience"],
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero,
    RequireExpirationTime = true,
};

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = "ApiKey";
    o.DefaultChallengeScheme = "ApiKey";
})
.AddJwtBearer("ApiKey", x =>
{
    x.RequireHttpsMetadata = false;
    x.TokenValidationParameters = tokenValidationParameters;
});

#endregion


var app = builder.Build();


if (app.Environment.IsEnvironment("Local") || app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "app-name/swagger/{documentname}/swagger.json";
    });

    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "app-name/swagger";
        c.SwaggerEndpoint("v1/swagger.json", "App Name Api");
    });

    app.UseDeveloperExceptionPage();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");

app.UseExceptionHandlerMiddleware();


app.UseAuthentication();

app.UseRateLimiter();

app.UseAuthorization();



app.MapControllers();


app.Run();

public partial class Program { }
