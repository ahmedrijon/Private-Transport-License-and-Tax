using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Transport_licensing_tax_management.Data;
using Transport_licensing_tax_management.DataModel;

namespace Transport_licensing_tax_management.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NoticesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NoticesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Notices
        public async Task<IActionResult> Index()
        {
              return _context.Notices != null ? 
                          View(await _context.Notices.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Notices'  is null.");
        }

        // GET: Admin/Notices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Notices == null)
            {
                return NotFound();
            }

            var notices = await _context.Notices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notices == null)
            {
                return NotFound();
            }

            return View(notices);
        }

        // GET: Admin/Notices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Notices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Heading,Detail,Date")] Notices notices)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notices);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notices);
        }

        // GET: Admin/Notices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Notices == null)
            {
                return NotFound();
            }

            var notices = await _context.Notices.FindAsync(id);
            if (notices == null)
            {
                return NotFound();
            }
            return View(notices);
        }

        // POST: Admin/Notices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Heading,Detail,Date")] Notices notices)
        {
            if (id != notices.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notices);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoticesExists(notices.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(notices);
        }

        // GET: Admin/Notices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Notices == null)
            {
                return NotFound();
            }

            var notices = await _context.Notices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notices == null)
            {
                return NotFound();
            }

            return View(notices);
        }

        // POST: Admin/Notices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Notices == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Notices'  is null.");
            }
            var notices = await _context.Notices.FindAsync(id);
            if (notices != null)
            {
                _context.Notices.Remove(notices);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoticesExists(int id)
        {
          return (_context.Notices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
