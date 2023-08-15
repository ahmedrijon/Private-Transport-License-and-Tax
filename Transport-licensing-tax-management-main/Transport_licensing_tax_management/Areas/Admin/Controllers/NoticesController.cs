using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Transport_licensing_tax_management.Data;
using Transport_licensing_tax_management.DataModel;
using Transport_licensing_tax_management.Migrations;

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

        //// GET: Admin/Notices/Details/5
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

        //// GET: Admin/Notices/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Notices model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
               
                return Json(new
                {
                    icon = "success",
                    message = "completed",
                    title = "Success"
                    
                });
               
            }
            else
            {
                return Json(new
                {
                    icon = "error",
                    message = "Duplicate found",
                    title = "Error"
                });
            }
        }

        public IActionResult Edit(int Id)
        {
            var notice = _context.Notices.FirstOrDefault(x => x.Id == Id);
            if (notice == null)
            {
                return NotFound();
            }
            return View(notice);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Notices model)
        {
            var notice = _context.Notices.FirstOrDefault(x => x.Id == model.Id);
            if (notice != null)
            {
                if (ModelState.IsValid)
                {  
                    notice.Heading = model.Heading;
                    notice.Detail = model.Detail;
                    notice.Date = model.Date;

                    _context.Update(notice);
                    await _context.SaveChangesAsync();

                    return Json(new
                    {
                        icon = "success",
                        message = "Notice updated",
                        title = "Success"
                    });
                }
                else
                {
                    return Json(new
                    {
                        icon = "error",
                        message = "Invalid input data",
                        title = "Error"
                    });
                }
            }
            else
            {
                return Json(new
                {
                    icon = "error",
                    message = "Notice not found",
                    title = "Error"
                });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Notices.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            try
            {
                _context.Notices.Remove(entity);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    icon = "success",
                    message = "Deleted successfully",
                    title = "Success"
                });
            }
            catch
            {
                return Json(new
                {
                    icon = "error",
                    message = "Error deleting",
                    title = "Error"
                });
            }
        }
        public async Task<IActionResult> Notice()
        {
            return _context.Notices != null ?
                        View(await _context.Notices.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Notices'  is null.");
        }
        private bool NoticesExists(int id)
        {
            return (_context.Notices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
