using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Aplicatie.API.Data;
using Microsoft.EntityFrameworkCore;
using Aplicatie.API.Models;

namespace Aplicatie.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class UsersController : Controller
    {
        private readonly AppDbContext _aplicatieDbContext;

        public UsersController(AppDbContext aplicatieDbContext)
        {
            _aplicatieDbContext = aplicatieDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _aplicatieDbContext.Users.ToListAsync();

            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User userRequest)
        {
            await _aplicatieDbContext.Users.AddAsync(userRequest);
            await _aplicatieDbContext.SaveChangesAsync();
            return Ok(userRequest);
        }
    }
}
