using ClassAssignmentWEBAPI.Data;
using ClassAssignmentWEBAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassAssignmentWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitedStateController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UnitedStateController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnitedState>>> GetUnitedStates()
        {
            return await _context.UnitedStates.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UnitedState>> GetUnitedState(int id)
        {
            var unitedState = await _context.UnitedStates.FindAsync(id);

            if (unitedState == null)
            {
                return NotFound();
            }

            return unitedState;
        }








    }
}
