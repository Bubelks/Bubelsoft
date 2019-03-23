using System.Collections.Generic;
using System.Linq;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using BubelSoft.Core.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BubelSoft.Core.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMailService _mailService;

        public CompanyController(ICompanyRepository companyRepository, IUserRepository userRepository,
            ICurrentUser currentUser, IMailService mailService)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _currentUser = currentUser;
            _mailService = mailService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var companies = _companyRepository.GetAll();

            return Ok(companies.Select(c => new
            {
                Value = c.Id.Value,
                DisplayValue = c.Name
            }));
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var companyId = new CompanyId(id);
            var company = _companyRepository.Get(companyId);

            if (company == null)
                return NotFound();

            var companyUsers = _userRepository.GetWorkers(companyId);

            return Ok(new DTO.Company
            {
                Id = id,
                Name = company.Name,
                Nip = company.Nip,
                PhoneNumber = company.PhoneNumber,
                Email = company.Email,
                City = company.City,
                PostCode = company.PostCode,
                Street = company.Street,
                PlaceNumber = company.PlaceNumber,
                CanManageWorkers = _currentUser.User.CanManageWorkers(companyId),
                CanEdit = _currentUser.User.CanEdit(companyId),
                Workers = companyUsers.Select(u => new DTO.User
                {
                    Id = u.Id.Value,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Username = u.Name,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    CompanyRole = u.CompanyRole
                })
            });
        }

        [Authorize]
        [HttpGet]
        [Route("my")]
        public IActionResult GetMy()
        {
            return Get(_currentUser.User.CompanyId.Value);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] DTO.CompanyInfo companyInfo)
        {
            var companyId = new CompanyId(id);
            var company = _companyRepository.Get(companyId);

            if (company == null)
                return NotFound();

            if (!_currentUser.User.CanEdit(companyId))
                return Forbid();

            company.Update(
                companyInfo.Name,
                companyInfo.Nip,
                companyInfo.PhoneNumber,
                companyInfo.Email,
                companyInfo.City,
                companyInfo.PostCode,
                companyInfo.Street,
                companyInfo.PlaceNumber
            );

            _companyRepository.Save(company);

            return Ok();
        }

        [HttpPost]
        public IActionResult Add([FromBody] DTO.BuildingCompany companyInfo)
        {
            var company = new Company(
                companyInfo.Name,
                companyInfo.Nip,
                companyInfo.PhoneNumber,
                companyInfo.Email,
                companyInfo.City,
                companyInfo.PostCode,
                companyInfo.Street,
                companyInfo.PlaceNumber
            );

            var companyId = _companyRepository.Save(company);

            var user = new User("", "", "", UserCompanyRole.Admin, "", "");
            user.From(companyId);
            var userId = _userRepository.Save(user);

            _companyRepository.AddSubContract(new BuildingId(companyInfo.BuildingId), companyId);

            return Ok(userId.Value);
        }

        [Authorize]
        [HttpGet("{id}/workers")]
        public IActionResult GetWorkers(int id)
        {
            var companyId = new CompanyId(id);
            var workers = _userRepository.GetWorkers(companyId);

            return Ok(workers.Select(w => new
            {
                Value = w.Id.Value,
                DisplayValue = $"{w.FirstName} {w.LastName} ({w.Name})"
            }));
        }


        [Authorize]
        [HttpPut]
        [Route("{id}/workers/add")]
        public IActionResult AddWorker(int id, [FromBody] DTO.User worker)
        {
            var companyId = new CompanyId(id);
            var company = _companyRepository.Get(companyId);

            if (company == null)
                return NotFound();
            
            if (!_currentUser.User.CanManageWorkers(companyId))
                return Forbid();

            var user = new User(
                worker.Username,
                worker.FirstName,
                worker.LastName,
                worker.CompanyRole,
                worker.Email,
                worker.PhoneNumber);

            user.From(companyId);

            _userRepository.Save(user);

            _mailService.SendWorkerAddedInfo(user, _currentUser.User, company);
            return Ok(user.Id.Value);
        }

        [Authorize]
        [HttpPut]
        [Route("{id}/workers/delete")]
        public IActionResult DeleteWorkers(int id, [FromBody]IEnumerable<int> usersId)
        {
            var companyId = new CompanyId(id);

            if (!_companyRepository.Exists(companyId))
                return NotFound();

            if (!_currentUser.User.CanManageWorkers(companyId))
                return Forbid();

            _companyRepository.DeleteWorkers(companyId, usersId.Select(uId => new UserId(uId)));

            return Ok();
        }
    }
}
