using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Database.Repositories.Interfaces;
using WebApi.Domain.Models;
using WebApi.Infrastructure;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public CompanyController(ICompanyRepository companyRepository, IUserRepository userRepository,
            ICurrentUser currentUser)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var companyId = new CompanyId(id);
            var company = _companyRepository.Get(companyId);
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

        [HttpGet]
        [Route("my")]
        public IActionResult GetMy()
        {
            return Get(_currentUser.User.CompanyId.Value);
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] DTO.CompanyInfo companyInfo)
        {
            var companyId = new CompanyId(id);
            var company = _companyRepository.Get(companyId);

            if (company == null)
                return BadRequest();

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

        [HttpPut]
        [Route("{id}/workers/add")]
        public IActionResult AddWorker(int id, [FromBody] DTO.User worker)
        {
            var companyId = new CompanyId(id);
            if (!_companyRepository.Exists(companyId))
                return BadRequest();

            var user = new User(
                worker.Username,
                worker.FirstName,
                worker.LastName,
                worker.CompanyRole,
                worker.Email,
                worker.PhoneNumber);

            user.From(companyId);

            _userRepository.Save(user);

            return Ok(user);
        }

        [HttpPut]
        [Route("{id}/workers/delete")]
        public IActionResult DeleteWorkers(int id, [FromBody]IEnumerable<int> usersId)
        {
            var companyId = new CompanyId(id);
            if (!_companyRepository.Exists(companyId))
                return BadRequest();

            _companyRepository.DeleteWorkers(companyId, usersId.Select(uId => new UserId(uId)));

            return Ok();
        }
    }
}
