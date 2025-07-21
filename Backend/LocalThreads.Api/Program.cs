using LocalThreads.Api.Configurations;
using LocalThreads.Api.Services.Implementations.Shared;
using LocalThreads.Api.Services.Implementations.Shopkeeper;
using LocalThreads.Api.Services.Interfaces.Shared;
using LocalThreads.Api.Services.Interfaces.Shopkeeper;
using LocalThreads.Api.Repositories.Implementations.Shopkeeper;
using LocalThreads.Api.Repositories.Interfaces.Shopkeeper;
using LocalThreads.Services.Interfaces.Customer;
using LocalThreads.Repositories.Implementations.Customer;
using LocalThreads.Repositories.Interfaces.Customer;
using LocalThreads.Services.Implementations.Customer;
using LocalThreads.Api.Repositories.Interfaces.Shared;
using LocalThreads.Api.Repositories.Implementations.Shared;
using MongoDB.Driver;
using LocalThreads.Api.Services.Interfaces.Customer;
using LocalThreads.Api.Services.Implementations.Customer;
using LocalThreads.Api.Repositories.Interfaces.Customer;
using LocalThreads.Api.Repositories.Implementations.Customer;

var builder = WebApplication.CreateBuilder(args);

// ✅ Load Configuration from appsettings
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// ✅ Firebase Initialization
FirebaseInitializer.InitializeFirebase("Configurations/firebase-service-account.json");

// ✅ MongoDB Configuration
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = builder.Configuration.GetSection("MongoDB").Get<MongoDbSettings>();
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<IMongoDbService, MongoDbService>();

builder.Services.AddSingleton(sp =>
{
    var settings = builder.Configuration.GetSection("MongoDB").Get<MongoDbSettings>();
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.Database);
});


// ✅ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// ✅ Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "LocalThreads API",
        Version = "v1"
    });
});

// ✅ Services & Repositories

// 🔐 Auth
builder.Services.AddSingleton<FirebaseAuthService>();

// ☁️ Shared Services
builder.Services.AddSingleton<IS3Service, S3Service>();
builder.Services.AddScoped<IUploadService, UploadService>();

// 🛍️ Shopkeeper
builder.Services.AddScoped<IShopRepository, ShopRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShopkeeperService, ShopkeeperService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IListingService, ListingService>();
// 👤 Customer
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<ICustomerService, CustomerService>();

// ✅ Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// ✅ Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var productRepo = scope.ServiceProvider.GetRequiredService<IProductRepository>();
    await productRepo.GenerateCategoriesFromExistingProducts();
}

app.Run();