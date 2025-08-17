using Mango.Services.ShoppingCartAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Mango.Services.ShoppingCartAPI.Extentions;
using Mango.Services.ShoppingCartAPI;
using Mango.Web.Services.IServices;
using Mango.Services.ShoppingCartAPI.Services.Product;
using Mango.Services.ShoppingCartAPI.Utility;
using Mango.RabbitMQ.Messaging;
using Mango.MessageBus.Utility;
using Mango.MessageBus.Messaging;
using Mango.MessageBus.IMessaging; // Add this using directive at the top of the file

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<BEApiAuthHandler>();

// Add services to the container.

builder.Services.AddHttpClient("Product",
    u=>u.BaseAddress = 
    new Uri(builder.Configuration["ServiceUrls:APIGateWay"])).AddHttpMessageHandler<BEApiAuthHandler>();
builder.Services.AddHttpClient("Coupon",
    u=>u.BaseAddress = 
    new Uri(builder.Configuration["ServiceUrls:APIGateWay"])).AddHttpMessageHandler<BEApiAuthHandler>();

builder.Services.AddRabbitMQMessaging(builder.Configuration);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
IMapper mapper = MapperConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[]{}
        }
    });
});
//builder.AddAppAuthetication();
builder.AddAppAuthentication(); // Call the extension method to add authentication

builder.Services.AddAuthorization();

builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<ICouponService,CouponService>();

//builder.Services.AddScoped<IPublishMessage,AzureServiceBusPublisher>();
//builder.Services.AddScoped<IPublishMessage, RabbitMqPublisher>();




builder.Services.AddTransient<AzureServiceBusPublisher>();
builder.Services.AddTransient<RabbitMqPublisher>();
builder.Services.AddTransient<MessagePublishContext>();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
ApplyMigration();
app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (dbContext.Database.GetPendingMigrations().Count() > 0)
        {
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();

        }

    }
}