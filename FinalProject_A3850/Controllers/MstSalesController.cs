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
    public class MstSalesController : Controller
    {
        private readonly FinalprojectdbContext _context;

        public MstSalesController(FinalprojectdbContext context)
        {
            _context = context;
        }

        // GET: MstSales
        public async Task<IActionResult> Index()
        {
            return View(await _context.MstSales.ToListAsync());
        }

        // GET: MstSales/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mstSale = await _context.MstSales
                .FirstOrDefaultAsync(m => m.IdSales == id);
            if (mstSale == null)
            {
                return NotFound();
            }

            return View(mstSale);
        }

        // GET: MstSales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MstSales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSales,Name,JumlahPenjualan,Komisi,DtAdded,DtUpdated,IdUserAdded,IdUserUpdated")] MstSale mstSale)
        {
            if (ModelState.IsValid)
            {
                mstSale.IdSales = Guid.NewGuid();
                _context.Add(mstSale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mstSale);
        }

        // GET: MstSales/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mstSale = await _context.MstSales.FindAsync(id);
            if (mstSale == null)
            {
                return NotFound();
            }
            return View(mstSale);
        }

        // POST: MstSales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdSales,Name,JumlahPenjualan,Komisi,DtAdded,DtUpdated,IdUserAdded,IdUserUpdated")] MstSale mstSale)
        {
            if (id != mstSale.IdSales)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mstSale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MstSaleExists(mstSale.IdSales))
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
            return View(mstSale);
        }

        // GET: MstSales/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mstSale = await _context.MstSales
                .FirstOrDefaultAsync(m => m.IdSales == id);
            if (mstSale == null)
            {
                return NotFound();
            }

            return View(mstSale);
        }

        // POST: MstSales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var mstSale = await _context.MstSales.FindAsync(id);
            if (mstSale != null)
            {
                _context.MstSales.Remove(mstSale);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MstSaleExists(Guid id)
        {
            return _context.MstSales.Any(e => e.IdSales == id);
        }
    }
}
