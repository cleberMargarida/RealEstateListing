using Microsoft.EntityFrameworkCore;
using RealEstateListing.API.Extensions;
using RealEstateListing.API.Middleware;
using RealEstateListing.API.ViewModels;
using RealEstateListing.Application;
using RealEstateListing.Infrastructure;
using RealEstateListing.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.TypeInfoResolverChain.Insert(0, ViewModelsJsonContext.Default);
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddInfrastructure();
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseExceptionHandling();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Real Estate Listing API v1"));
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
