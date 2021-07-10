using Dapper;
using DapperORM.Domain.DbContexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperORM.Domain.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DapperContext _context;

        public CompanyRepository(DapperContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<CompanyOnly>> GetCompanies()
        {
            var query = "SELECT * FROM Company";

            using (var connection = _context.CreateConnection())
            {
                var companies = await connection.QueryAsync<CompanyOnly>(query);
                return companies.ToList();
            }
        }
        public async Task<IEnumerable<CompanyMappingToDifferentName>> GetCompaniesMapping()
        {
            var query = "SELECT Id AS CacNO, Name AS CompanyName, Address AS Location, Country AS CountrySide FROM  Company";

            using (var connection = _context.CreateConnection())
            {
                var companies = await connection.QueryAsync<CompanyMappingToDifferentName>(query);
                return companies.ToList();
            }
        }

        public async Task<Company> GetCompany(int id)
        {
            var query = "SELECT * FROM Company WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                var company = await connection.QuerySingleOrDefaultAsync<Company>(query, new { id });

                return company;
            }
        }

        //Adding a New Entity in the Database with the Execute(Async) below
        public async Task CreateCompany(CompanyForCreationDto company)
        {
            var query = "INSERT INTO Company (Name, Address, Country) VALUES (@Name, @Address, @Country)";

            var parameters = new DynamicParameters();
            parameters.Add("Name", company.Name, DbType.String);
            parameters.Add("Address", company.Address, DbType.String);
            parameters.Add("Country", company.Country, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<Company> CreateCompanyReturnCompany(CompanyForCreationDto company)
        {
            var query = "INSERT INTO Company (Name, Address, Country) VALUES (@Name, @Address, @Country)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("Name", company.Name, DbType.String);
            parameters.Add("Address", company.Address, DbType.String);
            parameters.Add("Country", company.Country, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);

                var createdCompany = new Company
                {
                    Id = id,
                    Name = company.Name,
                    Address = company.Address,
                    Country = company.Country
                };

                return createdCompany;
            }
        }

        public async Task UpdateCompany(int id, CompanyForUpdateDto company)
        {
            var query = "UPDATE Company SET Name = @Name, Address = @Address, Country = @Country WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Name", company.Name, DbType.String);
            parameters.Add("Address", company.Address, DbType.String);
            parameters.Add("Country", company.Country, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteCompany(int id)
        {
            var query = "DELETE FROM Company WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

        public async Task<Company> GetCompanyByEmployeeId(int id)
        {
            var procedureName = "ShowCompanyForProvidedEmployeeId";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
                var company = await connection.QueryFirstOrDefaultAsync<Company>
                    (procedureName, parameters, commandType: CommandType.StoredProcedure);

                return company;
            }
        }

        //Executing Multiple SQL Statements with a Single Query
        public async Task<Company> GetCompanyEmployeesMultipleResults(int id)
        {
            var query = "SELECT * FROM Company WHERE Id = @Id;" +
                        "SELECT * FROM Employee WHERE CompanyId = @Id";

            using (var connection = _context.CreateConnection())
            using (var multi = await connection.QueryMultipleAsync(query, new { id }))
            {
                var company = await multi.ReadSingleOrDefaultAsync<Company>();
                if (company != null)
                    company.Employees = (await multi.ReadAsync<Employee>()).ToList();

                return company;
            }
        }

        //Full JOIN 
        public async Task<List<Company>> GetCompaniesEmployeesFullJOIN()
        {
            var query = "SELECT * FROM Company c Full JOIN Employee e ON c.Id = e.CompanyId";

            using (var connection = _context.CreateConnection())
            {
                var companyDict = new Dictionary<int, Company>();

                var companies = await connection.QueryAsync<Company, Employee, Company>(
                    query, (company, employee) =>
                    {
                        if (!companyDict.TryGetValue(company.Id, out var currentCompany))
                        {
                            currentCompany = company;
                            companyDict.Add(currentCompany.Id, currentCompany);
                        }

                        currentCompany.Employees.Add(employee);
                        return currentCompany;
                    }
                );

                return companies.Distinct().ToList();
            }
        }

        //Right OUTER JOIN 
        public async Task<List<Company>> GetCompaniesEmployeesRightOUTERJOIN()
        {
            var query = "SELECT * FROM Company c Right OUTER JOIN  Employee e ON c.Id = e.CompanyId";

            using (var connection = _context.CreateConnection())
            {
                var companyDict = new Dictionary<int, Company>();

                var companies = await connection.QueryAsync<Company, Employee, Company>(
                    query, (company, employee) =>
                    {
                        if (!companyDict.TryGetValue(company.Id, out var currentCompany))
                        {
                            currentCompany = company;
                            companyDict.Add(currentCompany.Id, currentCompany);
                        }

                        currentCompany.Employees.Add(employee);
                        return currentCompany;
                    }
                );

                return companies.Distinct().ToList();
            }
        }

        //Left OUTER JOIN 
        public async Task<List<Company>> GetCompaniesEmployeesLeftOUTERJOIN()
        {
            var query = "SELECT * FROM Company c Left OUTER JOIN  Employee e ON c.Id = e.CompanyId";

            using (var connection = _context.CreateConnection())
            {
                var companyDict = new Dictionary<int, Company>();

                var companies = await connection.QueryAsync<Company, Employee, Company>(
                    query, (company, employee) =>
                    {
                        if (!companyDict.TryGetValue(company.Id, out var currentCompany))
                        {
                            currentCompany = company;
                            companyDict.Add(currentCompany.Id, currentCompany);
                        }

                        currentCompany.Employees.Add(employee);
                        return currentCompany;
                    }
                );

                return companies.Distinct().ToList();
            }
        }

        //Bulk insert Transactions
        public async Task<int> CreateMultipleCompanies(List<CompanyForCreationDto> companies)
        {
            var query = "INSERT INTO Company (Name, Address, Country) VALUES (@Name, @Address, @Country)";

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                int counter = 0;
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var company in companies)
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("Name", company.Name, DbType.String);
                        parameters.Add("Address", company.Address, DbType.String);
                        parameters.Add("Country", company.Country, DbType.String);

                        await connection.ExecuteAsync(query, parameters, transaction: transaction);
                        counter++;
                    }

                    transaction.Commit();
                    return counter;
                }
            }
        }
    }
}
