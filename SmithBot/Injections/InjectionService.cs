using Microsoft.Extensions.DependencyInjection;
using SmithBot.Bot;
using SmithBot.Messages;
using SmithBot.Quartz;
using SmithBot.Quartz.Jobs;

namespace SmithBot.Injections
{
    public static class InjectionService
    {
        public static IServiceCollection AddInjections(this IServiceCollection services)
        {
            services
                .AddTransient<IQuartzService, QuartzService>()
                .AddTransient<MessageExecutor>()
                .AddTransient<BotWorker>()
                .AddTransient<BotService>()
                .AddTransient<SetNewBalanceJob>();
            return services;
        }
    }
}
