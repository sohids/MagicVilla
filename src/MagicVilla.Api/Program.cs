using System.Text;
using Asp.Versioning;
using MagicVilla.Api;
using MagicVilla.Api.Data;
using MagicVilla.Api.Repository;
using MagicVilla.Api.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
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
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);

}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
});

//var apiVersioningBuilder = builder.Services.AddApiVersioning(options =>
//{
//    options.ReportApiVersions = true;
//    //options.DefaultApiVersion = new ApiVersion(1, 0);
//    options.AssumeDefaultVersionWhenUnspecified = true;
//    // Use whatever reader you want
//    options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
//        new HeaderApiVersionReader("x-api-version"),
//        new MediaTypeApiVersionReader("x-api-version"));
//}); // Nuget Package: Asp.Versioning.Mvc

//apiVersioningBuilder.AddApiExplorer(options =>
//{
//    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
//    // note: the specified format code will format the version as "'v'major[.minor][-status]"
//    options.GroupNameFormat = "'v'VVV";

//    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
//    // can also be used to control the format of the API version in route templates
//    options.SubstituteApiVersionInUrl = true;
//}); 
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
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
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