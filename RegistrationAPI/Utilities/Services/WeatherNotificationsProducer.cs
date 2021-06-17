using Lib.Net.Http.WebPush;
using Lib.Net.Http.WebPush.Authentication;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RegistrationAPI.Utilities.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RegistrationAPI.Utilities.Services
{
    public class WeatherNotificationsProducer : BackgroundService
    {
        private const int NOTIFICATION_FREQUENCY = 60000;

        private readonly Random _random = new Random();

        private readonly IPushSubscriptionsService _pushSubscriptionsService;
        private readonly PushServiceClient _pushClient;

        public WeatherNotificationsProducer(IOptions<PushNotificationsOptions> options, IPushSubscriptionsService pushSubscriptionsService, PushServiceClient pushClient)
        {
            _pushSubscriptionsService = pushSubscriptionsService;

            _pushClient = pushClient;
            _pushClient.DefaultAuthentication = new VapidAuthentication(options.Value.PublicKey, options.Value.PrivateKey)
            {
                Subject = "https://angular-pushnotifications.demo.io"
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(NOTIFICATION_FREQUENCY, stoppingToken);

                SendNotifications(_random.Next(-20, 55), stoppingToken);
            }
        }

        private void SendNotifications(int temperatureC, CancellationToken stoppingToken)
        {
            var date = DateTime.Now;
            PushMessage notification = new AngularPushNotification
            {
                Title = "Thông báo mới",
                Body = $"Sản phẩm mới đã được ra mắt lúc: {date}",
                //Body = $"Temp. (C): {temperatureC} | Temp. (F): {32 + (int)(temperatureC / 0.5556)}",
                Icon = "assets/icons/icon-96x96.png"
            }.ToPushMessage();

            foreach (PushSubscription subscription in _pushSubscriptionsService.GetAll())
            {
                // Fire-and-forget 
                _pushClient.RequestPushMessageDeliveryAsync(subscription, notification, stoppingToken);
            }
        }
    }
}
