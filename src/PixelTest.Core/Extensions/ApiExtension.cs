using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace PixelTest.Core.Extensions;

public static class ApiExtension
{
	public static List<string>? SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
	{
		if (string.IsNullOrWhiteSpace(csvList))
			return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

		return csvList
			.TrimEnd(',')
			.Split(',')
			.AsEnumerable<string>()
			.Select(s => s.Trim())
			.ToList();
	}

	public static bool IsNullOrWhitespace(this string? s)
	{
		return string.IsNullOrWhiteSpace(s);
	}

	public static IApplicationBuilder AddExceptionMiddleware(this IApplicationBuilder app)
	{
		return app.UseMiddleware<ExceptionMiddleware>(Array.Empty<object>());
	}

	public static IServiceCollection ConfigureCors(this IServiceCollection services)
	{
		return services.AddCors(delegate (CorsOptions opt)
		{
			opt.AddDefaultPolicy(delegate (CorsPolicyBuilder builder)
			{
				builder.AllowAnyOrigin();
				builder.AllowAnyHeader();
				builder.AllowAnyMethod();
			});
		});
	}

	public static IServiceCollection AddSwagger(this IServiceCollection services)
	{
		return services.AddEndpointsApiExplorer().AddSwaggerGen(delegate (SwaggerGenOptions opt)
		{
			opt.SwaggerDoc("v1", new OpenApiInfo
			{
				Version = "v1",
				Title = Assembly.GetEntryAssembly()?.GetName().Name
			});
			opt.ResolveConflictingActions((IEnumerable<ApiDescription> apiDescriptions) => apiDescriptions.First());
			opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Name = "Authorization",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer"
			});
			opt.AddSecurityRequirement(new OpenApiSecurityRequirement {
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
			} });
		});
	}

	public static IApplicationBuilder AddSwaggerEndpoint(this IApplicationBuilder builder)
	{
		return builder.UseSwagger().UseSwaggerUI(delegate (SwaggerUIOptions opt)
		{
			opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
			opt.RoutePrefix = string.Empty;
		});
	}
}