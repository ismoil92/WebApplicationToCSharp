using Autofac;
using Autofac.Extensions.DependencyInjection;
using ClassLibrary.ModelsAbstractsServicesContext.Abstractions;
using ClassLibrary.ModelsAbstractsServicesContext.Context;
using ClassLibrary.ModelsAbstractsServicesContext.Mapper;
using ClassLibrary.ModelsAbstractsServicesContext.Models.DTO;
using ClassLibrary.ModelsAbstractsServicesContext.Mutation;
using ClassLibrary.ModelsAbstractsServicesContext.Query;
using ClassLibrary.ModelsAbstractsServicesContext.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddTransient<IProductService<ProductDTO>, ProductService>();
builder.Services.AddTransient<IStorageService<StorageDTO>, StorageService>();
builder.Services.AddTransient<ICategoryService<CategoryDTO>, CategoryService>();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(cb =>
    cb.Register(c => new StorageDbContext(builder.Configuration.GetConnectionString("db"))).InstancePerDependency());


builder.Services.AddGraphQLServer()
    .AddQueryType<QueryClass>()
    .AddMutationType<MutationClass>();


var app = builder.Build();

app.MapGraphQL();



app.Run();
