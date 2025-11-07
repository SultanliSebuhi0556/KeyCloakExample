using KeyCloakTest.Extension;
using KeyCloakTest.Options;
using SwaggerThemes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KeycloakOptions>(builder.Configuration.GetSection(nameof(KeycloakOptions)));

builder.Services.AddKeycloakAuthentications(builder.Configuration);
builder.Services.ConfigureSwaggerAuthentication();
//builder.Services.AddSwaggerWithKeycloak(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(Theme.UniversalDark);
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();