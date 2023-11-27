using FinalLEssionBotTelegram.Models;

namespace FinalLEssionBotTelegram
{
    internal interface IStorage
    {
        internal Session GetSession(long chatId);
    }
}
