
using System.Text.Json;
using FastEndpoints;
using FastEndpoints.Swagger;
using MeUi.Entry.Extensions;
using Serilog;
using MeUi.Authentication.Core.Extension;
using MeUi.Authentication.Password.Extenstion;
using System.Data.Common;
using Npgsql;
using MeUi.Shared.Application.interfaces;
using MeUi.Shared.Infrastructure.Data;
using MeUi.Authorization.Core.Extension;
using FastEndpoints.Security;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDbConnection(builder.Configuration);
builder.Services.AddAuthenticationCore(builder.Configuration);
builder.Services.AddAuthenticationPassword(builder.Configuration);
builder.Services.AddAuthorizationCore(builder.Configuration);
builder.Services.AddFastEndpointAuthentication(builder.Configuration);
builder.Services.AddFastEndpointsSwagger(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

WebApplication app = builder.Build();

app.UseCustomSerilogRequestLogging();
app.UseExceptionHandlerMiddleware();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints(c => c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase).UseSwaggerGen();

app.Run();
