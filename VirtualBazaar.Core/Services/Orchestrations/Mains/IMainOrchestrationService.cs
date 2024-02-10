using System.Threading.Tasks;

namespace VirtualBazaar.Core.Services.Orchestrations.Mains
{
    public interface IMainOrchestrationService
    {
        Task SendMessageToAdminIfUserWantPlaceAnOrder(string message);
    }
}
