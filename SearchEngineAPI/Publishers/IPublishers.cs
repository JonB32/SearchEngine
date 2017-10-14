using System;
using System.Threading.Tasks;

namespace SearchEngineAPI.Publishers
{
    public interface IPublishers
    {
        string GetMessage(string query);
        Task<string> GetMessageAsync(string query);
        void CloseConnection();
    }
}
