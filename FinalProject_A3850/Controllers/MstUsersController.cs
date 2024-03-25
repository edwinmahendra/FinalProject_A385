using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject_A3850.Models;

namespace FinalProject_A3850.Controllers
{
    public class MstUsersController : Controller
    {
        private readonly FinalprojectdbContext _context;

        public MstUsersController(FinalprojectdbContext context)
        {
            _context = context;
        }

        // GET: MstUsers
        public async Task<IActionResult> Index()
        {
            var finalprojectdbContext = _context.MstUsers.Include(m => m.IdSalesNavigation);
            return View(await finalprojectdbContext.ToListAsync());
        }

        // GET: MstUsers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mstUser = await _context.MstUsers
                .Include(m => m.IdSalesNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mstUser == null)
            {
                return NotFound();
            }

            return View(mstUser);
        }

        // GET: MstUsers/Create
        public IActionResult Create()
        {
            ViewData["IdSales"] = new SelectList(_context.MstSales, "IdSales", "IdSales");
            return View();
        }

        // POST: MstUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,Status,IdSales,DtAdded,DtUpdated,IdUserAdded,IdUserUpdated")] MstUser mstUser)
        {
            if (ModelState.IsValid)
            {
                mstUser.Id = Guid.NewGuid();
                _context.Add(mstUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdSales"] = new SelectList(_context.MstSales, "IdSales", "IdSales", mstUser.IdSales);
            return View(mstUser);
        }

        // GET: MstUsers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mstUser = await _context.MstUsers.FindAsync(id);
            if (mstUser == null)
            {
                return NotFound();
            }
            ViewData["IdSales"] = new SelectList(_context.MstSales, "IdSales", "IdSales", mstUser.IdSales);
            return View(mstUser);
        }

        // POST: MstUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Username,Password,Status,IdSales,DtAdded,DtUpdated,IdUserAdded,IdUserUpdated")] MstUser mstUser)
        {
            if (id != mstUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mstUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MstUserExists(mstUser.Id))
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
            ViewData["IdSales"] = new SelectList(_context.MstSales, "IdSales", "IdSales", mstUser.IdSales);
            return View(mstUser);
        }

        // GET: MstUsers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mstUser = await _context.MstUsers
                .Include(m => m.IdSalesNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mstUser == null)
            {
                return NotFound();
            }

            return View(mstUser);
        }

        // POST: MstUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var mstUser = await _context.MstUsers.FindAsync(id);
            if (mstUser != null)
            {
                _context.MstUsers.Remove(mstUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MstUserExists(Guid id)
        {
            return _context.MstUsers.Any(e => e.Id == id);
        }
    }
}
