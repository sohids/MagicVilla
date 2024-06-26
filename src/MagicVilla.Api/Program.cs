using System.Text;
using Asp.Versioning;
using MagicVilla.Api;
using MagicVilla.Api.Data;
using MagicVilla.Api.Repository;
using MagicVilla.Api.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//swagger configuration 
builder.Services.AddSwaggerGen(option =>
{
    //Swagger configuration to add security token in Swagger UI - begin 

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT authorization header using the bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            
            new List<string>()
        }

    });
    //Swagger configuration to add security token in Swagger UI - end 

    //Customizing the swagger documentation 
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Magic Villa V1",
        Description = "Api to manage villa",
        TermsOfService = new Uri("https://localhost:7002/terms"),
        Contact = new OpenApiContact
        {
            Name = "DotnetMastery",
            Url = new Uri("https://localhost:7002/")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://localhost:7002/license")
        }
    });

    option.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2.0",
        Title = "Magic Villa V2",
        Description = "Api to manage villa",
        TermsOfService = new Uri("https://localhost:7002/terms"),
        Contact = new OpenApiContact
        {
            Name = "DotnetMastery",
            Url = new Uri("https://localhost:7002/")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://localhost:7002/license")
        }
    });
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true; //setup default api version 
    options.DefaultApiVersion = new ApiVersion(1, 0); //this is the default api version 
    options.ReportApiVersions = true; //shows all api version in the response 

}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; //For api url api/v{version:apiVersion}/
    options.SubstituteApiVersionInUrl = true; //set automated default api version in the url api/v1}
});
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionStr"));
});

builder.Host.UseSerilog((ctx, lc) => lc.MinimumLevel.Debug().MinimumLevel.Override("Microsoft", LogEventLevel.Warning).Enrich.FromLogContext().ReadFrom.Configuration(builder.Configuration));

try
{
    var app = builder.Build();
    Log.Information("Application Starting up");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            //setting up default endpoint / Customizing the swagger endpoint title 
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "MagicVillaV1");
            options.SwaggerEndpoint("/swagger/v2/swagger.json", "MagicVillaV2");
        });
    }
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization(); //authorization always should be put under the authentication 
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}