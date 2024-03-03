using Web.ApiDapper.Endpoint;
using Web.ApiDapper.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(ServiceProvider =>
{
    var configuration = ServiceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection") ??
    throw new ApplicationException("The Connection String Is Null");
    return new SqlConnectionFactory(connectionString);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCustomerEndpoints();

app.Run();













/*
 * 
 * using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Web.ApiDapper.Endpoint;
using Web.ApiDapper.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer(); // Adds the API explorer services to the service collection.
builder.Services.AddSwaggerGen(); // Adds Swagger services to generate OpenAPI documentation.

// Registers a singleton service to provide SqlConnectionFactory instances.
// The SqlConnectionFactory is used to create SqlConnection instances with a specified connection string.
builder.Services.AddSingleton(serviceProvider =>
{
    // Retrieve the IConfiguration service from the service provider.
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

    // Get the connection string named "DefaultConnection" from the configuration.
    var connectionString = configuration.GetConnectionString("DefaultConnection") ??
        throw new ApplicationException("The Connection String Is Null"); // Throws an exception if the connection string is null.

    // Create and return a new SqlConnectionFactory instance with the retrieved connection string.
    return new SqlConnectionFactory(connectionString);
});

var app = builder.Build(); // Builds the application.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // Checks if the application is running in the development environment.
{
    app.UseSwagger(); // Enables Swagger middleware to generate OpenAPI documentation.
    app.UseSwaggerUI(); // Adds middleware to serve the Swagger UI.
}

app.UseHttpsRedirection(); // Adds middleware to redirect HTTP requests to HTTPS.

app.MapCustomerEndpoints(); // Maps customer-related endpoints defined in the 'MapCustomerEndpoints' extension method.

app.Run(); // Runs the application.

 
 */
