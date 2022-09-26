using System.Threading.Tasks;

namespace SmithBot.Quartz
{
    public interface IQuartzService
    {
        Task SetNewBalanceJob();
    }
}
