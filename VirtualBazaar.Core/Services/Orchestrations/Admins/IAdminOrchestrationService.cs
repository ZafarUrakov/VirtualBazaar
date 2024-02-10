using System.Threading.Tasks;
using System.Threading;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace VirtualBazaar.Core.Services.Orchestrations.Admins
{
    public interface IAdminOrchestrationService
    {
        void StartWork();
    }
}
