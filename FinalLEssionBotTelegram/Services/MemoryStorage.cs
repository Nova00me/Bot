using FinalLEssionBotTelegram.Models;
using System.Collections.Concurrent;

namespace FinalLEssionBotTelegram.Services
{
    internal class MemoryStorage : IStorage
    {
        private readonly ConcurrentDictionary<long, Session> _session;
        public MemoryStorage() 
        {   
            _session = new ConcurrentDictionary<long, Session>();
        }
        public Session GetSession(long chatID)
        {
            if (_session.ContainsKey(chatID))
            {
                return _session[chatID];
            }
            var session = new Session() { Info = "str" };
            _session.TryAdd(chatID, session);
            return session;
        }
    }
}
