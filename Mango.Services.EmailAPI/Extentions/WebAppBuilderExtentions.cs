using Mango.Services.EmailAPI.Messaging.AzureServiceBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Mango.Services.EmailAPI.Extentions
{
    public static class AppBuilderExtentions
    {
        private static IAzureServiceBusConsumer AzureServiceBusConsumer { get; set; }

        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            AzureServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();

            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            hostApplicationLife.ApplicationStarted.Register(Onstart);
            hostApplicationLife.ApplicationStopping.Register(OnStop);
            return app;
        }

        private static void OnStop()
        {
            AzureServiceBusConsumer.Stop();
        }

        private static void Onstart()
        {
            AzureServiceBusConsumer.Start();
        }
    }







    //public static class ApplicationBuilderExtensions
    //{
    //    private static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }

    //    public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
    //    {
    //        ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
    //        var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

    //        hostApplicationLife.ApplicationStarted.Register(OnStart);
    //        hostApplicationLife.ApplicationStopping.Register(OnStop);

    //        return app;
    //    }

    //    private static void OnStop()
    //    {
    //        ServiceBusConsumer.Stop();
    //    }

    //    private static void OnStart()
    //    {
    //        ServiceBusConsumer.Start();
    //    }
    //}




}
