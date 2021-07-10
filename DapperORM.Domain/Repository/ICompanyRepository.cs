using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperORM.Domain.Repository
{
    public interface ICompanyRepository
    {
        public Task<IEnumerable<CompanyOnly>> GetCompanies();

        public Task<IEnumerable<CompanyMappingToDifferentName>> GetCompaniesMapping();

        public Task<Company> GetCompany(int id);
        public Task CreateCompany(CompanyForCreationDto company);
        public Task<Company> CreateCompanyReturnCompany(CompanyForCreationDto company);
        public Task UpdateCompany(int id, CompanyForUpdateDto company);
        public Task DeleteCompany(int id);
        public Task<Company> GetCompanyByEmployeeId(int id);

        public Task<Company> GetCompanyEmployeesMultipleResults(int id);
        public Task<List<Company>> GetCompaniesEmployeesFullJOIN();
        public Task<List<Company>> GetCompaniesEmployeesRightOUTERJOIN();
        public Task<List<Company>> GetCompaniesEmployeesLeftOUTERJOIN();
        public Task<int> CreateMultipleCompanies(List<CompanyForCreationDto> companies);
    }
}
