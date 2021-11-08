namespace Catalog.Api.Config
{
    public class MongoDBLocalConfig
    {
        public string Host { get; }
        public int Port { get; }

        public string User { get; set; }
        public string Password { get; set; }
        public string ConnectionString
        {
            get
            {
                if (Host != null && Port > 1023 && Port < 65353)
                {
                    return $"mongodb://{User}:{Password}@{Host}:{Port}";
                }
                return null;
            }
        }

    }
}