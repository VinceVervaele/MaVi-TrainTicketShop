using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trains_FSD.Areas.Data;
using Trains_FSD.Models.Entities;
using Trains_FSD.Services.Interfaces;
using Trains_FSD.ViewModels;

namespace Trains_FSD.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("GetAllCustomers")] // /api/user/getallcustomers
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = _mapper.Map<List<UserSafeVM>>(users);
            return Ok(result);
        }
    }
}