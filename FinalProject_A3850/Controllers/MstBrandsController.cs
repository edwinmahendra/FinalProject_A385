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
    public class MstBrandsController : Controller
    {
        private readonly ICarService _service;
        private readonly ILogger<MstBrandsController> _logger;

        public MstBrandsController(ICarService service, ILogger<MstBrandsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var brands = await _service.GetBrandInfoFromView();
            return View(brands);
        }

        // GET: MstBrands/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: MstBrands/Edit/5
        public IActionResult Edit(int id)
        {
            var brand = _service.GetBrand(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: MstBrands/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ReqBrandDto brand)
        {
            if (ModelState.IsValid)
            {
                _service.UpdateBrand(brand);
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: MstBrands/Delete/5
        public IActionResult Delete(int id)
        {
            var brand = _service.GetBrand(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: MstBrands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.DeleteBrand(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBrand([FromForm] ReqBrandDto brand)
        {
            try
            {
                _service.DeleteBrand(brand.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var brand = _service.GetBrand(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReqBrandDto brand)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _service.InsertOrUpdateBrandAsync(brand);
                    _logger.LogInformation("Brand operation successful: {Message}", result.MESSAGE);
                    TempData["Message"] = result.MESSAGE;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in brand operation for: {BrandName}", brand.Name);
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }
            return View(brand);
        }
    }
}
