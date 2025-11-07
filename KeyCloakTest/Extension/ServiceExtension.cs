using KeyCloakTest.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace KeyCloakTest.Extension;

public static class ServiceExtension
{
    public static IServiceCollection AddKeycloakAuthentications(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("Keycloak").Get<KeycloakOptions>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.Authority = options.Authority;
                opt.Audience = options.ClientId;
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = options.Authority,
                    ValidAudience = options.ClientId,
                };
            });

        services.AddAuthorization();
        return services;
    }

    //with keycloak
    //public static IServiceCollection ConfigureSwaggerAuthentication(this IServiceCollection services, IConfiguration configuration)
    //{
    //    var options = configuration.GetSection("Keycloak").Get<KeycloakOptions>();

    //    services.AddSwaggerGen(c =>
    //    {
    //        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    //        {
    //            Type = SecuritySchemeType.OAuth2,
    //            Flows = new OpenApiOAuthFlows
    //            {
    //                AuthorizationCode = new OpenApiOAuthFlow
    //                {
    //                    AuthorizationUrl = new Uri($"{options.Authority}/protocol/openid-connect/auth"),
    //                    TokenUrl = new Uri($"{options.Authority}/protocol/openid-connect/token"),
    //                    Scopes = new Dictionary<string, string>
    //                    {
    //                        { "openid", "OpenID Connect" },
    //                        { "profile", "User profile" }
    //                    }
    //                }
    //            }
    //        });

    //        c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //        {
    //            {
    //                new OpenApiSecurityScheme
    //                {
    //                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
    //                },
    //                new[] { "openid", "profile" }
    //            }
    //        });
    //    });

    //    return services;
    //}

    public static IServiceCollection ConfigureSwaggerAuthentication(this IServiceCollection services)
    {

        services.AddSwaggerGen(opt =>
        {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }, Array.Empty<string>()
                }
            });
        });
        return services;
    }
}