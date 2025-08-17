using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Extentions;
using Mango.Services.EmailAPI.Messaging.AzureServiceBus;
using Mango.Services.EmailAPI.Messaging.RabbitMQ;
using Mango.Services.EmailAPI.Models.Dto.RabbitMQ;
using Mango.Services.EmailAPI.Services;
using Mango.Services.EmailAPI.Utility;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"));
});
var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();


optionBuilder.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddSingleton(new EmailService(optionBuilder.Options));




builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
//builder.Services.AddSingleton<IRabbitMQConsummer, RabbitMQConsumer>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRabbitMQMessaging(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
ApplyMigration();
app.UseAzureServiceBusConsumer();
app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (dbContext.Database.GetPendingMigrations().Count() > 0)
        {
            dbContext.Database.Migrate();

        }

    }
}
