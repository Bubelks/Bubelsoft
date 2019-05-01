using System;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using BubelSoft.Security;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.DTO.Core;

namespace WebApi.Controllers.Core
{
    [Route("api/[controller]")]
    public class CompanyController: ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserSession _userSession;
        private readonly MainContext _context;

        public CompanyController(
            IUserRepository userRepository,
            ICompanyRepository companyRepository,
            IUserSession userSession,
            MainContext context)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _userSession = userSession;
            _context = context;
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register(CompanyRegister companyRegister)
        {
            try
            {
                var company = new Company(
                    companyRegister.Company.Name,
                    companyRegister.Company.Number);
                var user = new User(
                    companyRegister.Administrator.FirstName,
                    companyRegister.Administrator.LastName,
                    UserCompanyRole.Admin,
                    companyRegister.Administrator.Email);


                using (var tx = _context.Database.BeginTransaction())
                {
                    _companyRepository.Save(company);
                    user.From(company.Id);
                    _userRepository.Save(user, companyRegister.Administrator.Password);

                    tx.Commit();
                }

                return Ok(company.Id.Value);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("{companyId}")]
        [HttpGet]
        public IActionResult Get(int companyId)
        {
            var company = _companyRepository.Get(new CompanyId(companyId));

            if (company == null)
                return NotFound();

            if (_userSession.User.CompanyId != company.Id)
                return Forbid();

            return Ok(new CompanyResponse
            {
                Id = company.Id.Value,
                Name = company.Name,
                Number = company.Number
            });
        }
    }
}
