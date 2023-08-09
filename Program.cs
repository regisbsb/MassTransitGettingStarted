using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Company.Consumers;

namespace GettingStarted
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {

                        x.AddConsumer<GettingStartedConsumer>();
                        x.AddConsumer<FaultyGettingStartedConsumer>();

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Message<Contracts.GettingStarted>(x =>
                            {
                                x.SetEntityName("nomeclatura.porca.getting-started");
                            });

                            cfg.Message<Fault>(x =>
                            {
                                x.SetEntityName("nomeclatura.porca.fault");
                            });

                            cfg.Message<Fault<Contracts.GettingStarted>>(x =>
                            {
                                x.SetEntityName("nomeclatura.porca.getting-started-fault");
                            });


                            cfg.ReceiveEndpoint("nomeclatura.porca.getting-started-consumer", e =>
                            {
                                e.ConfigureConsumer<GettingStartedConsumer>(context, c =>
                                {
                                    //c.UseMessageRetry(r => r.Immediate(5));
                                });
                            });

                            cfg.ReceiveEndpoint("nomeclatura.porca.getting-started-consumer-faulty", e =>
                            {
                                e.ConfigureConsumer<FaultyGettingStartedConsumer>(context, c =>
                                {
                                    //c.UseMessageRetry(r => r.Immediate(5));
                                });
                            });

                            cfg.Host("localhost", "/", h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            });

                            cfg.ConfigureEndpoints(context);
                        });
                    });

                    services.AddHostedService<Worker>();

                });
    }
}
