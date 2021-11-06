namespace Catalog.Config
{
    public sealed class MongoDBConfig
    {
        private string UserName;
        private string Password;
        private string DBName;

        public MongoDBConfig(string username, string password, string dbName)
        {
            this.UserName = username;
            this.Password = password;
            this.DBName = dbName;
        }
        public string ConnectionString
        {
            get
            {
                if (UserName != null && Password != null && DBName != null)
                {
                    return $"mongodb+srv://{UserName}:{Password}@catalog.3qvwr.mongodb.net/{DBName}?retryWrites=true&w=majority";
                }
                return null;
            }
        }
    }
}