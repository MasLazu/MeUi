
using System.Text.Json;
using FastEndpoints;
using FastEndpoints.Swagger;
using MeUi.Entry.Extensions;
using Serilog;
using MeUi.Authentication.Core.Extension;
using MeUi.Authentication.Password.Extenstion;
using System.Data.Common;
using Npgsql;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDbConnection(builder.Configuration);
builder.Services.AddAuthenticationCore(builder.Configuration);
builder.Services.AddAuthenticationPassword(builder.Configuration);

builder.Services.AddFastEndpointsSwagger(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

WebApplication app = builder.Build();

app.UseCustomSerilogRequestLogging();
app.UseExceptionHandlerMiddleware();
app.UseFastEndpoints(c => c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase).UseSwaggerGen();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.Run();
