

namespace Cartomatic.Utils.Data
{
#pragma warning disable 1591
    public enum DataSourceProvider
    {
        Unknown = 0,
        Npgsql = 1,
        SqlServer = 2,
        Oracle = 3,
        EfInMemory = 4

        //TODO - at some point mongo, redis, neo4j, etc
    }
#pragma warning restore 1591
}