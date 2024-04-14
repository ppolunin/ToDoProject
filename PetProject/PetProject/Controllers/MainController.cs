using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetProject.Data;
using PetProject.Model;

namespace PetProject.Controllers
{
    [ApiController, Produces("application/json"), Route("")]
    public sealed class MainController(MainDB db) : ControllerBase
    {
        private async Task<ActionResult<Guid>> ProcessItem(Func<DbSet<ToDoRecord>, Guid> func, CancellationToken ct)
        {
            try
            {
                var id = func(db.Items);
                await db.SaveChangesAsync(ct);
                return id;
            }
            catch (OperationCanceledException)
            {
                throw; // По идее, этот отвал обрабатывается там, ниже где-то по CancellationToken
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult<Guid>> AddItem([FromBody] NewToDoRecord rec, CancellationToken ct)
        {
            return await ProcessItem(items =>
            {
                var id = Guid.NewGuid();
                items.Add(new ToDoRecord(rec, id));
                return id;
            }, ct);
        }

        [HttpPatch("change")]
        public async Task<ActionResult<Guid>> ChangeItem([FromBody] ToDoRecord rec, CancellationToken ct)
        {
            return await ProcessItem(items =>
            {
                items.Update(rec);
                return rec.Id;
            }, ct);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ToDoRecord>> GetItem(Guid id, CancellationToken ct)
        {
            try
            {
                return await db.Items.AsNoTracking().FirstAsync(item => item.Id == id, ct);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch 
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoRecord>>> GetItems(CancellationToken ct)
        {
            return await db.Items.AsNoTracking().ToArrayAsync(ct);
        }

        [HttpDelete("remove/{id:guid}")]
        public async Task<IActionResult> RemoveItem(Guid id, CancellationToken ct)
        {
            try
            {
                var temp = new ToDoRecord { Id = id };
                db.Items.Attach(temp);
                db.Items.Remove(temp);
                await db.SaveChangesAsync(ct);
                return Ok();
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
