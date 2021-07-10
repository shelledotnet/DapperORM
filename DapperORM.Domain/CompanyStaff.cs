using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DapperORM.Domain
{
    public class CompanyStaff
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }

        public List<Employee> Employees { get; set; } = new List<Employee>();

      
        public DateTime DateCreated { get; set; }
    }
}
