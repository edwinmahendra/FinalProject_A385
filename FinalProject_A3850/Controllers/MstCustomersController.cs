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
    public class MstCustomersController : Controller
    {
        private readonly ICarService _service;
        private readonly ILogger<MstCustomersController> _logger;

        public MstCustomersController(ICarService service, ILogger<MstCustomersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: MstCustomers
        public async Task<IActionResult> Index()
        {
            var customers = await _service.GetCustomerWithView();
            return View(customers);
        }

        // GET: MstCustomers/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: MstCustomers/Edit/Guid
        public IActionResult Edit(Guid id)
        {
            var customer = _service.GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: MstCustomers/Edit/Guid
        public async Task<IActionResult> Edit(Guid id, ReqCustomerDto customer)
        {
            try
            {
                var result = await _service.InsertOrUpdateCustomerAsync(customer);
                _logger.LogInformation("Customer operation successful: {Message}", result.MESSAGE);
                TempData["Message"] = result.MESSAGE;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in customer operation for: {CustomerName}", customer.Name);
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }

            return View(customer);
        }

        // GET: MstCustomers/Delete/Guid
        public IActionResult Delete(Guid id)
        {
            var customer = _service.GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        public IActionResult DeleteCustomer(ReqCustomerDto customer)
        {
            try
            {
                _service.DeleteCustomer(customer.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: MstCustomers/Delete/Guid
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _service.DeleteCustomer(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var customer = _service.GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReqCustomerDto customer)
        {
            try
            {
                var result = await _service.InsertOrUpdateCustomerAsync(customer);
                _logger.LogInformation("Customer operation successful: {Message}", result.MESSAGE);
                TempData["Message"] = result.MESSAGE;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in creating customer: {CustomerName}", customer.Name);
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }

            return View(customer);
        }

    }
}

