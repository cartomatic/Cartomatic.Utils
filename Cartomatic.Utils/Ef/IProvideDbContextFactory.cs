using System;
using System.Collections.Generic;
using System.Text;
using Cartomatic.Utils.Data;

#if NETFULL
using System.Data.Entity;
#endif

#if NETSTANDARD
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
#endif

namespace Cartomatic.Utils.Ef
{
    /// <summary>
    /// Used to ensure hooks for obtaining a valid instance in scenarios, where one cannot be sure of a presence of specific constructors.
    /// This way a valid, properly configured context can be obtained
    /// </summary>
    public interface IProvideDbContextFactory
    {
        DbContext ProduceDbContextInstance(string connStrName = null, bool isConnStr = false, DataSourceProvider provider = DataSourceProvider.EfInMemory);

        DbContext ProduceDefaultDbContextInstance();
    }
}
