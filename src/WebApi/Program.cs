using System.Security.Authentication;
using System.Text;
using Api.Test.Domain.Application.Commands;
using Api.Test.Domain.Application.Interfaces;
using Api.Test.Domain.Application.Services;
using Api.Test.Domain.Application.Validations;
using Serilog;
using Api.Test.Helpers;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Serilog
    builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

    Log.Information("Iniciando {ApplicationName}", builder.Configuration["Serilog:Properties:ApplicationName"]);

    // Add services to the container.
    builder.Services.AddTransient<ITestCommandHandler, TestCommandHandler>();
    builder.Services.AddTransient<ITestHttpClientService, TestHttpClientService>();
    builder.Services.AddSpiDbContext(builder.Configuration);

    builder.Services.AddControllers()
        .AddXmlSerializerFormatters().AddXmlDataContractSerializerFormatters()
        .AddFluentValidation(config =>
        {
            config.RegisterValidatorsFromAssemblyContaining<Program>();
            config.DisableDataAnnotationsValidation = true;
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = c =>
            {
                var errors = string.Join('\n', c.ModelState.Values.Where(v => v.Errors.Count > 0)
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage));

                try
                {
                    if (errors.Contains("|"))
                    {
                        var respuesta = errors.Split("|");
                        var codigo = respuesta[0];
                        var mensaje = respuesta[1];

                        var error = new ErrorResponse
                        {
                            Codigo = codigo,
                            Mensaje = mensaje
                        };

                        Log.Logger.Warning("Modelo invalido. {@Error}", error);

                        return new BadRequestObjectResult(error);
                    }
                }
                catch (Exception e)
                {
                    Log.Logger.Error(e, "Error al formatear bad requests");
                }

                var errorInterno = new ErrorResponse
                {
                    Codigo = "99",
                    Mensaje = errors
                };
                Log.Logger.Warning("Modelo invalido. {@Error}", errorInterno);

                return new BadRequestObjectResult(errorInterno);
            };
        });
            
    //Configure the Versioning Services
    builder.Services.AddApiVersioning(options =>
    {
        // ReportApiVersions will return the "api-supported-versions" and "api-deprecated-versions" headers.
        options.ReportApiVersions = true;
        // Set a default version when it's not provided,
        // e.g., for backward compatibility when applying versioning on existing APIs
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        // Combine (or not) API Versioning Mechanisms:
        options.ApiVersionReader = ApiVersionReader.Combine(
            // The Default versioning mechanism which reads the API version from the "api-version" Query String paramater.
            new QueryStringApiVersionReader("api-version"),
            // Use the following, if you would like to specify the version as a custom HTTP Header.
            new HeaderApiVersionReader("Accept-Version"),
            // Use the following, if you would like to specify the version as a Media Type Header.
            new MediaTypeApiVersionReader("api-version")
        );
    });
    
    
    // JWT
    var securityScheme = new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JSON Web Token based security",
    };

    var securityReq = new OpenApiSecurityRequirement()
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
            new string[] {}
        }
    };

    var contact = new OpenApiContact()
    {
        Name = "Roman Veron",
        Email = "romanjuniorveron@fpuna.edu.py",
        Url = new Uri("https://www.linkedin.com/in/roman-veron/")
    };

    var license = new OpenApiLicense()
    {
        Name = "Free License",
        Url = new Uri("https://www.linkedin.com/in/roman-veron/")
    };

    var info = new OpenApiInfo()
    {
        Version = "v1",
        Title = "API Test - JWT Authentication with Swagger",
        Description = "Implementing JWT Authentication in Rest API .NET CORE 6",
        TermsOfService = new Uri("http://www.example.com"),
        Contact = contact,
        License = license
    };

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", info);
        options.AddSecurityDefinition("Bearer", securityScheme);
        options.AddSecurityRequirement(securityReq);
    });
    
    
    // Add JWT configuration
    builder.Services.AddAuthentication(o =>
    {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true
        };
    });
    
    // Typed HttpClients
    builder.Services.AddHttpClient<ITestHttpClientService, TestHttpClientService>()
        .ConfigurePrimaryHttpMessageHandler(() => {
            return new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = SslProtocols.Tls12,
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
            };
        });

    var app = builder.Build();

	// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseSwagger();
    app.UseSwaggerUI(o => { o.SwaggerEndpoint("/swagger/v1/swagger.json", builder.Configuration["Serilog:Properties:ApplicationName"]); });

    app.UseHttpsRedirection();
    
    app.UseSerilogRequestLogging(opts
        => opts.EnrichDiagnosticContext = LogRequestEnricher.EnrichFromRequest);

    app.UseAuthentication();
    
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La API fallo al iniciar");
}
finally
{
    Log.CloseAndFlush();
}
