using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    public class CommonRepository
    {
        protected NpgsqlConnection conn;

        public CommonRepositories()
        {
            IConfiguration myConfig = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            conn = new NpgsqlConnection(myConfig.GetConnectionString("pgconn"));
        }

    }
}