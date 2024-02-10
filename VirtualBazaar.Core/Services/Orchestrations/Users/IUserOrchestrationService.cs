using System.Threading.Tasks;
using System.Threading;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace VirtualBazaar.Core.Services.Orchestrations.Users
{
    public interface IUserOrchestrationService
    {
        void StartWork();
    }
}
