using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperORM.Domain.DbContexts
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DapperConnectionString");
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        #region MyRegion
        /*
        We inject the IConfiguration interface to enable access to the connection string from our appsettings.json 
        file.Also, we create the CreateConnection method, which returns a new SQLConnection object. 
        
        using Microsoft.Data.SqlClient;
        using Microsoft.Extensions.Configuration;
        using System.Data;
         
         */
        #endregion
    }
}
