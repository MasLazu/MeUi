
using System.Text.Json;
using Ardalis.Specification;
using FastEndpoints;
using FastEndpoints.Swagger;
using MeUi.Entry.Extensions;
using MeUi.Shared.Application.interfaces;
using MeUi.Shared.Infrastructure.Data.Repositories;
using Serilog;
using MeUi.Authentication.Core.Extension;
using MeUi.Authentication.Password.Extenstion;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddAuthenticationCore(builder.Configuration);
builder.Services.AddAuthenticationPassword(builder.Configuration);

// builder.Services.AddOpenApi();
builder.Services.AddFastEndpointsSwagger(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IReadRepositoryBase<>), typeof(Repository<>));


WebApplication app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

app.UseFastEndpoints(c => c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase).UseSwaggerGen();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.Run();
