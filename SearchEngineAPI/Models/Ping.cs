using System;
namespace SearchEngineAPI.Models
{
    public class Ping
    {
        private string _ping;

        public Ping()
        {
        }

        public string Result { get => _ping; set => _ping = value; }
    }
}
