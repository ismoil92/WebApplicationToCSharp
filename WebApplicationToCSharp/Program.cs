using Autofac;
using Autofac.Extensions.DependencyInjection;
using ClassLibrary.ContextAndRepository.Context;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

var config = new ConfigurationBuilder();
config.AddJsonFile("appsettings.json");
var cfg = config.Build();

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.Register(c =>
    new StorageContext(cfg.GetConnectionString("DefaultConnection")!))
    .InstancePerDependency();
});

var staticFIlesPath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
Directory.CreateDirectory(staticFIlesPath);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(staticFIlesPath),
    RequestPath ="/static"
});

app.UseAuthorization();

app.MapControllers();

app.Run();
