
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject_A3850.Models;
using FinalProject_A3850_DAL.Repositories.Service.Interface;
using FinalProject_A3850_DAL.Models.Dto.Req;

namespace FinalProject_A3850.Controllers
{
    public class MstUsagesController : Controller
    {
        private readonly ICarService _service;
        private readonly ILogger<MstUsagesController> _logger;

        public MstUsagesController(ICarService service, ILogger<MstUsagesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: MstUsages
        // public async Task<IActionResult> Index()
        // {
        //     return View(await _context.MstUsages.ToListAsync());
        // }

        public async Task<IActionResult> Index()
        {
            var type = await _service.GetUsageInfoFromView();
            return View(type);
        }

        // GET: MstUsages/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var brand = _service.GetUsage(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }


        // GET: MstUsages/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUsage([FromForm] ReqUsageDto type)
        {
            try
            {
                _service.DeleteUsage(type.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReqUsageDto usage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _service.InsertOrUpdateUsageAsync(usage);
                    _logger.LogInformation("Type operation successful: {Message}", result.MESSAGE);
                    TempData["Message"] = result.MESSAGE;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in brand operation for: {BrandName}", usage.Name);
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }
            return View(usage);
        }

        // POST: MstUsages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("Id,Name,DtAdded,DtUpdated,IdUserAdded,IdUserUpdated")] MstUsage mstUsage)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Add(mstUsage);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(mstUsage);
        // }

        // GET: MstUsages/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var mstUsage = await _context.MstUsages.FindAsync(id);
        //     if (mstUsage == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(mstUsage);
        // }

        // POST: MstUsages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DtAdded,DtUpdated,IdUserAdded,IdUserUpdated")] MstUsage mstUsage)
        // {
        //     if (id != mstUsage.Id)
        //     {
        //         return NotFound();
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(mstUsage);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!MstUsageExists(mstUsage.Id))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(mstUsage);
        // }

        // // GET: MstUsages/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var mstUsage = await _context.MstUsages
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (mstUsage == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(mstUsage);
        // }

        // // POST: MstUsages/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var mstUsage = await _context.MstUsages.FindAsync(id);
        //     if (mstUsage != null)
        //     {
        //         _context.MstUsages.Remove(mstUsage);
        //     }

        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }

        // private bool MstUsageExists(int id)
        // {
        //     return _context.MstUsages.Any(e => e.Id == id);
        // }
    }
}
