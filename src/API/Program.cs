using Application;
using Infrastructure;
using Infrastructure.Database;
using Serilog;
using Webly;
using Webly.SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

var _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();


builder.Host.UseSerilog();

builder.Services.AddPresentation(_config)
    .AddInfrastructure(_config)
    .AddApplicationLayer();


builder.Services.AddControllers();

var app = builder.Build();

await MigrationManager.ApplyMigrationsAsync(app.Services);
await Seeder.SeedAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCors(o =>
{
    o.WithOrigins(_config.GetValue<string>("WebAppUrl")!)
     .AllowAnyHeader()
     .AllowAnyMethod()
     .AllowCredentials();
});
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseRouting();
app.UseAuthentication();
app.UseRateLimiter();
app.UseAuthorization();
app.MapControllers();

app.MapHub<NotificationHub>("/notifications");

app.Run();


namespace Webly
{
    public partial class Program;
}




