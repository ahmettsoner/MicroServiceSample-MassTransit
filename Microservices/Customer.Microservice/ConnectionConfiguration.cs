namespace Customer.Microservice
{
    public class ConnectionConfiguration 
    {
        public string ConnectionString { get; }

        public ConnectionConfiguration(string connectionString) => ConnectionString = connectionString;
    }
}