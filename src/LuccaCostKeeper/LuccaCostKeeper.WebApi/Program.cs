using LuccaCostKeeper.Business;
using LuccaCostKeeper.Business.Datas;
using LuccaCostKeeper.Model;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(opt =>
{
	opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Lucca CostKeeper API", Version = "v1" });

	opt.EnableAnnotations();
	opt.AddSecurityDefinition("ApiKeyHeader", new OpenApiSecurityScheme()
	{
		Name = Constants.API_KEY_HEADER,
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Description = $"Authorization by {Constants.API_KEY_HEADER} inside request's header",
	});
	opt.AddSecurityRequirement(new OpenApiSecurityRequirement{
	  {
		  new OpenApiSecurityScheme
		  {
			  Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKeyHeader" }
		  },
		  new string[] {}
	  }
  });
});

builder.Services.AddBusiness(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Initialize the database
using (var scope = app.Services.CreateScope())
{
	var initialiser = scope.ServiceProvider.GetRequiredService<CostKeeperDbInitializer>();
	await initialiser.SeedAsync();
}

app.MapControllers();

app.Run();
