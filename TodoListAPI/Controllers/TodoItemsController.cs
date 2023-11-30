using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListAPI.Models;

namespace TodoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todoitem>>> GetTodoitems()
        {
          if (_context.Todoitems == null)
          {
              return NotFound();
          }
            return await _context.Todoitems.ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Todoitem>> GetTodoitem(int id)
        {
          if (_context.Todoitems == null)
          {
              return NotFound();
          }
            var todoitem = await _context.Todoitems.FindAsync(id);

            if (todoitem == null)
            {
                return NotFound();
            }

            return todoitem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoitem(int id, Todoitem todoitem)
        {
            if (id != todoitem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoitem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoitemExists(id))
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

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Todoitem>> PostTodoitem(Todoitem todoitem)
        {
          if (_context.Todoitems == null)
          {
              return Problem("Entity set 'TodoContext.Todoitems'  is null.");
          }
            _context.Todoitems.Add(todoitem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoitem", new { id = todoitem.Id }, todoitem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoitem(int id)
        {
            if (_context.Todoitems == null)
            {
                return NotFound();
            }
            var todoitem = await _context.Todoitems.FindAsync(id);
            if (todoitem == null)
            {
                return NotFound();
            }

            _context.Todoitems.Remove(todoitem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoitemExists(int id)
        {
            return (_context.Todoitems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
