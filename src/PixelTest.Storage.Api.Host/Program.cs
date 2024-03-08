using Microsoft.AspNetCore.HttpOverrides;
using PixelTest.Core.Extensions;
using PixelTest.Storage.Services.Extensions;
using PixelTest.Storage.Services.Workers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
	.ReadFrom.Configuration(context.Configuration)
	.ReadFrom.Services(services)
	.Enrich.FromLogContext());

builder.Services.AddControllers();

builder.Services
	.AddHostedService<BackgroundDataSaverWorker>();

builder.Services.AddSwagger();

builder.Services
	.AddTrackDataSaver(builder.Configuration)
	.AddBackgroundDataSaverWorkerConfiguration(builder.Configuration)
	.AddStorageService();

builder.Services.AddHealthChecks();
builder.Services.ConfigureCors();

var app = builder.Build();

app.UseRouting();

app.UseHttpsRedirection();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
	ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors();
if (builder.Environment.IsDevelopment())
{
	app.AddSwaggerEndpoint();
}

app.AddExceptionMiddleware();
app.MapControllers();
app.MapHealthChecks("/health")
	.AllowAnonymous();
app.Run();
