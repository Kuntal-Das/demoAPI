namespace Catalog.Config
{
    public class MongoDBLocalConfig
    {
        public string Host { get; }
        public int Port { get; }

        public MongoDBLocalConfig(string hostname, int port)
        {
            this.Host = hostname;
            this.Port = port;
        }
        public string ConnectionString
        {
            get
            {
                if (Host != null && Port > 1023 && Port < 65353)
                {
                    return $"mongodb://{Host}:{Port}";
                }
                return null;
            }
        }

    }
}