namespace Cartomatic.Utils.Data
{
    public enum DataSourceProvider
    {
        Unknown = 0,
        Npgsql = 1,
        SqlServer = 2,
        Oracle = 3

        //TODO - at some point mongo, redis, neo4j, etc
    }
}