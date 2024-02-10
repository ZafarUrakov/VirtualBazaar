using System.Linq;
using System.Threading.Tasks;
using VirtualBazaar.Core.Services.Foundations.Admins;
using VirtualBazaar.Core.Services.Foundations.Telegrams.Admins;
using VirtualBazaar.Core.Services.Foundations.Telegrams.Users;

namespace VirtualBazaar.Core.Services.Orchestrations.Mains
{
    public class MainOrchestrationService : IMainOrchestrationService
    {
        private readonly IAdminTelegramService adminTelegramService;
        private readonly IAdminService adminService;
        private readonly IUserTelegramService userTelegramService;

        public MainOrchestrationService(
            IAdminTelegramService adminTelegramService,
            IUserTelegramService userTelegramService,
            IAdminService adminService)
        {
            this.adminTelegramService = adminTelegramService;
            this.userTelegramService = userTelegramService;
            this.adminService = adminService;
        }

        public async Task SendMessageToAdminIfUserWantPlaceAnOrder(string message)
        {
            var admin = this.adminService.RetrieveAllAdmins().First();

            if(admin is not null)
            {
                await this.adminTelegramService.SendMessageAsync(
                    userTelegramId: admin.TelegramId,
                    message: message);
            }
        }
    }
}
