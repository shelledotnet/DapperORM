using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperORM.Domain.DbContexts
{
   public class DapperOrm : DbContext
    {
        public DapperOrm(DbContextOptions<DapperOrm> options)
            : base(options)
        {
            
        }
        public DbSet<Company> Company { get; set; }
        public DbSet<Employee> Employee { get; set; }


    }
}
