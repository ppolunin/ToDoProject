using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetProject.Data;
using PetProject.DTO;

namespace PetProject.Controllers
{
    [ApiController, Produces("application/json"), Route("[controller]")]
    public sealed class TodoController(ToDoDB db) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateToDoDTO rec, CancellationToken ct)
        {
            var id = Guid.NewGuid();
            db.ToDos.Add(rec.ToEntity(id));
            await db.SaveChangesAsync(ct);
            return id;
        }

        [HttpPatch]
        public async Task<ActionResult<Guid>> Change([FromBody] ToDoDTO rec, CancellationToken ct)
        {
            if (await db.ToDos.Where(item => item.Id == rec.Id).ExecuteUpdateAsync(item => 
                item.SetProperty(p => p.Content, rec.Content)
                    .SetProperty(p => p.IsDone, rec.IsDone), ct) != 0)
            {
                return rec.Id;
            }

            return NotFound();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ToDoDTO>> Get(Guid id, CancellationToken ct)
        {
            var item = (await db.ToDos.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id, ct));
            if (item != null)
                return item.ToDTO();

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoDTO>>> Get(CancellationToken ct)
        {
            return (await db.ToDos.AsNoTracking().ToArrayAsync(ct)).Select(item => item.ToDTO()).ToArray();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Remove(Guid id, CancellationToken ct)
        {
            if (await db.ToDos.Where(item => item.Id == id).ExecuteDeleteAsync(ct) != 0)
                return Ok();
            else
                return NotFound();
        }
    }
}
