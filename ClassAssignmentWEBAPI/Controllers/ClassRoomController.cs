using ClassAssignmentWEBAPI.Data;
using ClassAssignmentWEBAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassAssignmentWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassRoomController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClassRoomController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassRoom>>> GetClassRooms()
        {
            return await _context.ClassRooms.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClassRoom>> GetClassRoom(int id)
        {
            var classRoom = await _context.ClassRooms.FindAsync(id);

            if (classRoom == null)
            {
                return NotFound();
            }

            return classRoom;
        }

        [HttpPost]
        public async Task<ActionResult<ClassRoom>> PostClassRoom(ClassRoom classRoom)
        {
            classRoom.ClassRoomID = 0;
            //diaryEntry.Title = "Post URL";

            //_context.DiaryEntries.Add(diaryEntry);
            //await _context.SaveChangesAsync();

            var resourceUrl = Url.Action(nameof(GetClassRooms), new { ClassRoomID = classRoom.ClassRoomID });

            if (resourceUrl != null)
            {
                //diaryEntry.Content = resourceUrl;
                //diaryEntry.Title = diaryEntry.Title;
                classRoom.BuildingNumber = classRoom.BuildingNumber;
                classRoom.RoomNumber = classRoom.RoomNumber;
                classRoom.Capacity = classRoom.Capacity;
                classRoom.Unavailable = classRoom.Unavailable;
            }
            else
            {
                classRoom.BuildingNumber = 0;
                classRoom.RoomNumber = 0;
                classRoom.Capacity = 0;
                classRoom.Unavailable = "T";
            }

            _context.ClassRooms.Add(classRoom);
            await _context.SaveChangesAsync();

            return Created(resourceUrl, classRoom);
        }

        // Indicates that this action handles HTTP PUT requests at the URL pattern "api/DiaryEntries/{id}"
        // PUT: api/Daiary/Entries/5 - request to update record with ID = 5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassRoom(int id, [FromBody] ClassRoom classRoom)
        {
            if (id != classRoom.ClassRoomID)
            {
                return BadRequest();
            }

            _context.Entry(classRoom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassRoomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClassRoom(int id)
        {
            var classRoom = await _context.ClassRooms.FindAsync(id);

            if (classRoom == null)
            {
                return NotFound();
            }

            _context.ClassRooms.Remove(classRoom);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassRoomExists(int id)
        {
            return _context.ClassRooms.Any(e => e.ClassRoomID == id);
        }
    }
}
