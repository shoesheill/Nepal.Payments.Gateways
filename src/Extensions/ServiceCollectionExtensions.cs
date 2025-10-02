using Microsoft.Extensions.DependencyInjection;
using Nepal.Payments.Gateways.WebSocket;
using System;

namespace Nepal.Payments.Gateways.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNepalPaymentGateways(this IServiceCollection services)
        {
            // Register WebSocket manager as Singleton to maintain connections
            services.AddSingleton<IPaymentWebSocketManager, FonepayWebSocketManager>();

            // Register HTTP client for payment operations
            services.AddHttpClient("PaymentGateways", client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Add("User-Agent", "Nepal.Payments.Gateways/1.0");
            });

            return services;
        }
    }
}
