using System;
using System.Threading.Tasks;

namespace SearchEngineService.Publishers
{
    public interface IPublishers
    {
        void SendMessage();
        void SendMessageAsync();
    }
}
