﻿using System;
using System.Collections.Generic;
using System.Text;
using Cartomatic.Utils.Data;


#if NETSTANDARD
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
#endif

namespace Cartomatic.Utils.Ef
{
#if NETSTANDARD
    public static class DbContextFactory
    {
        private static IConfiguration Configuration { get; set; }

        static DbContextFactory()
        {
            Configuration = Cartomatic.Utils.NetCoreConfig.GetNetCoreConfig();
        }

        private static readonly Dictionary<DataSourceProvider, Action<DbContextOptionsBuilder, string>> ProvidersConfiguration =
            new Dictionary<DataSourceProvider, Action<DbContextOptionsBuilder, string>>
            {
                {
                    DataSourceProvider.EfInMemory,
                    (builder, connStr) => { builder.UseInMemoryDatabase(connStr); }
                },
                {
                    DataSourceProvider.Npgsql,
                    (builder, connStr) =>
                    {
                        builder.UseNpgsql(connStr);
                    }
                }
            };

        public static DbContextOptionsBuilder ConfigureProvider(this DbContextOptionsBuilder builder,
            DataSourceProvider provider, string connStr)
        {
            if(!ProvidersConfiguration.ContainsKey(provider) || ProvidersConfiguration[provider] == null)
                throw new ArgumentException($"Db provider not configured: {provider}");

            ProvidersConfiguration[provider](builder, connStr);

            return builder;
        }

        /// <summary>
        /// Sets provider configuration
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="providerConfig"></param>
        public static void SetDataSourceProvider(DataSourceProvider provider, Action<DbContextOptionsBuilder, string> providerConfig)
        {
            ProvidersConfiguration[provider] = providerConfig;
        }

        /// <summary>
        /// Creates DbContext with the specified connection string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connStrName">name of conn string to retrieve from configuration</param>
        /// <param name="isConnStr">whether ot not it is the actual connection string supplied</param>
        /// <returns></returns>
        public static T CreateDbContext<T>(string connStrName, bool isConnStr = false, DataSourceProvider provider = DataSourceProvider.EfInMemory)
            where T: DbContext
        {
            var ctxType = typeof(T);
            var opts = GetDbContextOptions<T>(connStrName, isConnStr);

            var newCtx = (T)Activator.CreateInstance(ctxType, new object[] { opts });

            return newCtx;
        }


        /// <summary>
        /// Gets DbContextOptions for a specified db context
        /// </summary>
        /// <param name="connStrName"></param>
        /// <param name="isConnStr"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static DbContextOptions GetDbContextOptions<T>(string connStrName, bool isConnStr = false, DataSourceProvider provider = DataSourceProvider.EfInMemory)
            where T: DbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>();

            var connStr = GetConnStr(connStrName, isConnStr);

            optionsBuilder.ConfigureProvider(provider, connStr);

            return optionsBuilder.Options;
        }

        /// <summary>
        /// Gets a conn str; obtains it from cfg if needed
        /// </summary>
        /// <param name="connStrName"></param>
        /// <param name="isConnStr"></param>
        /// <returns></returns>
        public static string GetConnStr(string connStrName, bool isConnStr = false)
        {
            //use a specified connection string or grab it from the cfgs
            return isConnStr ? connStrName : Configuration.GetConnectionString(connStrName);
        }
    }
#endif
}