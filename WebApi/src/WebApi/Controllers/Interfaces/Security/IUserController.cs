using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Security;

namespace WebApi.Controllers.Interfaces.Security
{
    public interface IUserController
    {
        IActionResult LogIn(UserLogInInfo userInfo);
    }
}