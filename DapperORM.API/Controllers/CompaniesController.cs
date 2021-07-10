using DapperORM.Domain;
using DapperORM.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperORM.API.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepo;

        public CompaniesController(ICompanyRepository companyRepo)
        {
            _companyRepo = companyRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            try
            {
                var companies = await _companyRepo.GetCompanies();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("mapping")]
        public async Task<IActionResult> GetCompaniesMapping()
        {
            try
            {
                var companies = await _companyRepo.GetCompaniesMapping();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}", Name = "CompanyById")]
        public async Task<IActionResult> GetCompany(int id)
        {
            try
            {
                var company = await _companyRepo.GetCompany(id);
                if (company == null)
                    return NotFound();

                return Ok(company);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(CompanyForCreationDto company)
        {
            try
            {
                var createdCompany = await _companyRepo.CreateCompanyReturnCompany(company);
                return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyForUpdateDto company)
        {
            try
            {
                var dbCompany = await _companyRepo.GetCompany(id);
                if (dbCompany == null)
                    return NotFound();

                await _companyRepo.UpdateCompany(id, company);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            try
            {
                var dbCompany = await _companyRepo.GetCompany(id);
                if (dbCompany == null)
                    return NotFound();

                await _companyRepo.DeleteCompany(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("ByEmployeeId/{id}")]
        public async Task<IActionResult> GetCompanyForEmployee(int id)
        {
            try
            {
                var company = await _companyRepo.GetCompanyByEmployeeId(id);
                if (company == null)
                    return NotFound();

                return Ok(company);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{id}/MultipleResult")]
        public async Task<IActionResult> GetCompanyEmployeesMultipleResult(int id)
        {
            try
            {
                var company = await _companyRepo.GetCompanyEmployeesMultipleResults(id);
                if (company == null)
                    return NotFound();

                return Ok(company);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("FullJoin")]
        public async Task<IActionResult> GetCompaniesEmployeesMultipleMapping()
        {
            try
            {
                var company = await _companyRepo.GetCompaniesEmployeesFullJOIN();

                return Ok(company);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("RightJoin")]
        public async Task<IActionResult> GetCompaniesEmployeesMultipleMappingRightJoin()
        {
            try
            {
                var company = await _companyRepo.GetCompaniesEmployeesRightOUTERJOIN();

                return Ok(company);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("LeftJoin")]
        public async Task<IActionResult> GetCompaniesEmployeesMultipleMappingLeftJoin()
        {
            try
            {
                var company = await _companyRepo.GetCompaniesEmployeesLeftOUTERJOIN();

                return Ok(company);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("bulk_Create_Company")]
        public async Task<IActionResult>CreateMultipleCompany([FromBody]List<CompanyForCreationDto> companies)
        {
            try
            {
                var company = await _companyRepo.CreateMultipleCompanies(companies);
                if (company == 0)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
