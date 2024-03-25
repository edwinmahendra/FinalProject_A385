using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject_A3850_DAL.Models;
using FinalProject_A3850_DAL.Models.Dto.Req;
using FinalProject_A3850_DAL.Repositories.Service.Interface;

namespace FinalProject_A3850.Controllers
{
    public class MstCarsController : Controller
    {
        private readonly ICarService _service;

        public MstCarsController(ICarService service)
        {
            _service = service;
        }

        // GET: MstCars
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetCarWithView());
        }

        // GET: MstCars/Create
        public IActionResult FormCreate()
        {
            ViewBag.Brands = _service.GetBrand().Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();
            ViewBag.Models = _service.GetModels().Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();
            ViewBag.Types = _service.GetType().Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();
            ViewBag.Usages = _service.GetUsage().Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();
            return View();
        }

        // POST: MstCars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult AddCar(ReqCarDto car)
        {
            try
            {
                _service.ExecPrcCar(car, "Status: %");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: MstCars/Edit/5
        public IActionResult Edit(Guid id)
        {
            var car = _service.GetCar(id);
            ViewBag.Brands = _service.GetBrand().Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();
            ViewBag.Models = _service.GetModel().Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();
            ViewBag.Types = _service.GetType().Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();
            ViewBag.Usages = _service.GetUsage().Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();
            return View(car);
        }

        // POST: MstCars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ReqCarDto car)
        {
            try
            {
                _service.UpdateCar(id, car);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult Deletecar(ReqCarDto car)
        {
            try
            {
                _service.DeleteCar(car.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
