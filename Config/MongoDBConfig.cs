namespace Catalog.Config
{
    public sealed class MongoDBConfig
    {
        public string UserName { get; init; }
        public string Password { get; init; }
        public string DBName { get; init; }

        // public MongoDBConfig() { }
        // public MongoDBConfig(string username, string password, string dbName)
        // {
        //     this.UserName = username;
        //     this.Password = password;
        //     this.DBName = dbName;
        // }
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