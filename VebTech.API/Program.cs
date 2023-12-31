using System.Net.Mime;
using System.Reflection;
using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VebTech.Application.Requests;
using VebTech.Application.Requests.ExceptionsHandling;
using VebTech.Domain.Services;
using VebTech.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

// Add services to the container.
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    containerBuilder.RegisterModule(new InfrastructureModule()));
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    containerBuilder.RegisterModule(new ApplicationModule()));
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => 
    containerBuilder.RegisterModule(new DomainModule()));

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions( op => op.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
            new BadRequestObjectResult(context.ModelState)
            {
                ContentTypes =
                {
                    MediaTypeNames.Application.Json,
                    MediaTypeNames.Application.Xml
                }
            };
    })
    .AddXmlSerializerFormatters();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add services for documentation using Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName!.Replace('+', '.'));
    
    var basePath = AppContext.BaseDirectory;
    var xmlPath = Path.Combine(basePath, "VebTech.API.xml");
    options.IncludeXmlComments(xmlPath);
    xmlPath = @"D:\AspNet\VebTech\VebTech.Domain.Models\bin\Debug\VebTech.Domain.Models.xml";
    options.IncludeXmlComments(xmlPath);
});

// Add a filter for error handling requests at the middleware level
builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
});

builder.Services.AddControllers().AddJsonOptions(option =>
    option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

app.UseCors(corsPolicyBuilder => corsPolicyBuilder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();