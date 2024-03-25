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
    public class TrnCarPurchasesController : Controller
    {
        private readonly ICarService _service; // Updated to use ICarPurchaseService
        private readonly ILogger<TrnCarPurchasesController> _logger;

        public TrnCarPurchasesController(ICarService service, ILogger<TrnCarPurchasesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: TrnCarPurchases
        public async Task<IActionResult> Index()
        {
            var purchases = await _service.GetPurchaseWithView();
            return View(purchases);
        }

        // GET: TrnCarPurchases/Create
        // public IActionResult Create()
        // {
        //     return View();
        // }

        public async Task<IActionResult> Create()
        {
            ViewBag.CustomerId = new SelectList(_service.GetCustomer(), "Id", "Name");
            ViewBag.CarId = new SelectList(_service.GetCar(), "Id", "Id");
            return View();
        }

        // POST: TrnCarPurchases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReqBuyDto purchaseDto)
        {
            try
            {
                var result = await _service.InsertOrUpdateBuyAsync(purchaseDto);
                _logger.LogInformation("Purchase operation successful: {Message}", result.MESSAGE);
                TempData["Message"] = result.MESSAGE;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in creating purchase: {PurchaseDto}", purchaseDto);
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }
            return View(purchaseDto);
        }



        public async Task<IActionResult> Edit(Guid id)
        {
            var purchase = _service.GetPurchase(id);
            if (purchase == null)
            {
                return NotFound();
            }

            var cars = _service.GetCar();
            var customers = _service.GetCustomer();


            ViewBag.CarId = cars?.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Id.ToString()
            })?.ToList() ?? new List<SelectListItem>();

            ViewBag.CustomerId = customers?.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })?.ToList() ?? new List<SelectListItem>();

            return View(purchase);
        }


        // GET: TrnCarPurchases/Edit/Guid
        // public async Task<IActionResult> Edit(Guid id)
        // {
        //     var purchase = _service.GetPurchase(id); // Assuming this method is synchronous. If not, await it.
        //     if (purchase == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(purchase);
        // }

        // POST: TrnCarPurchases/Edit/Guid
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ReqBuyDto purchaseDto)
        {
            if (id != purchaseDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _service.InsertOrUpdateBuyAsync(purchaseDto);
                    _logger.LogInformation("Purchase update successful: {Message}", result.MESSAGE);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating purchase: {PurchaseId}", purchaseDto.Id);
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }
            return View(purchaseDto);
        }

        // GET: TrnCarPurchases/Delete/Guid
        public IActionResult Delete(Guid id)
        {
            var purchase = _service.GetPurchase(id);
            if (purchase == null)
            {
                return NotFound();
            }
            return View(purchase);
        }

        // POST: TrnCarPurchases/Delete/Guid
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _service.DeletePurchase(id);
            return RedirectToAction(nameof(Index));
        }
    }
}