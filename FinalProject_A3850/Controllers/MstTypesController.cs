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
    public class MstTypesController : Controller
    {
        private readonly ICarService _service;
        private readonly ILogger<MstTypesController> _logger;

        public MstTypesController(ICarService service, ILogger<MstTypesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: MstTypes
        // public async Task<IActionResult> Index()
        // {
        //     return View(await _context.MstTypes.ToListAsync());
        // }

        public async Task<IActionResult> Index()
        {
            var type = await _service.GetTypeInfoFromView();
            return View(type);
        }

        // GET: MstTypes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var brand = _service.GetType(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // GET: MstTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // // POST: MstTypes/Create
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("Id,Name,DtAdded,DtUpdated,IdUserAdded,IdUserUpdated")] MstType mstType)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Add(mstType);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(mstType);
        // }

        // // GET: MstTypes/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var mstType = await _context.MstTypes.FindAsync(id);
        //     if (mstType == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(mstType);
        // }

        // // POST: MstTypes/Edit/5
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DtAdded,DtUpdated,IdUserAdded,IdUserUpdated")] MstType mstType)
        // {
        //     if (id != mstType.Id)
        //     {
        //         return NotFound();
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(mstType);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!MstTypeExists(mstType.Id))
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
        //     return View(mstType);
        // }

        // // GET: MstTypes/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var mstType = await _context.MstTypes
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (mstType == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(mstType);
        // }

        // // POST: MstTypes/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var mstType = await _context.MstTypes.FindAsync(id);
        //     if (mstType != null)
        //     {
        //         _context.MstTypes.Remove(mstType);
        //     }

        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }

        // private bool MstTypeExists(int id)
        // {
        //     return _context.MstTypes.Any(e => e.Id == id);
        // }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteType([FromForm] ReqTypeDto type)
        {
            try
            {
                _service.DeleteType(type.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReqTypeDto type)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _service.InsertOrUpdateTypeAsync(type);
                    _logger.LogInformation("Type operation successful: {Message}", result.MESSAGE);
                    TempData["Message"] = result.MESSAGE;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in brand operation for: {BrandName}", type.Name);
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }
            return View(type);
        }
    }
}

