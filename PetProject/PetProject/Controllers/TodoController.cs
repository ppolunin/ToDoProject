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
        public async Task<ActionResult<Guid>> AddItem([FromBody] DTOToDoForPost rec, CancellationToken ct)
        {
            var id = Guid.NewGuid();
            db.Items.Add(rec.ToEntity(id));
            await db.SaveChangesAsync(ct);
            return id;
        }

        [HttpPatch]
        public async Task<ActionResult<Guid>> ChangeItem([FromBody] DTOToDo rec, CancellationToken ct)
        {
            var id = rec.Id;
            try
            {
                db.Items.Update(rec.ToEntity());
                await db.SaveChangesAsync(ct);
                return id;
            }
            catch (DbUpdateException) // Если обновление пошло не по плану.
            {
                /**
                 * https://haacked.com/archive/2022/12/05/recover-from-dbupdate-exception/
                 */
                if (await db.Items.AsNoTracking().SingleOrDefaultAsync(item => item.Id == id, ct) is null)
                    return NotFound();

                throw;
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DTOToDo>> GetItem(Guid id, CancellationToken ct)
        {
            try
            {
                return (await db.Items.AsNoTracking().FirstAsync(item => item.Id == id, ct)).ToDTO();
            }
            catch (InvalidOperationException) // FirstAsync если не найдено
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTOToDo>>> GetItems(CancellationToken ct)
        {
            return (await db.Items.AsNoTracking().ToArrayAsync(ct)).Select(item => item.ToDTO()).ToArray();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> RemoveItem(Guid id, CancellationToken ct)
        {
            if (await db.Items.Where(item => item.Id == id).ExecuteDeleteAsync(ct) != 0)
                return Ok();
            else
                return NotFound();
        }
    }
}
