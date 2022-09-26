using Quartz;
using System.Threading.Tasks;

namespace SmithBot.Quartz.Jobs
{
    public class SetNewBalanceJob : IJob
    {
        private readonly IQuartzService _quartzService;

        public SetNewBalanceJob(IQuartzService quartzService)
        {
            _quartzService = quartzService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _quartzService.SetNewBalanceJob();
        }
    }
}
