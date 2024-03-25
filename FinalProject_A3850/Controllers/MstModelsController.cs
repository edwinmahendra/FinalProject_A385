using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject_A3850.Models;
using FinalProject_A3850_DAL.Models.Dto.Req;
using FinalProject_A3850_DAL.Models.Dto.Res;
using FinalProject_A3850_DAL.Repositories.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_A3850.Controllers
{
    public class MstModelsController : Controller
    {
        private readonly ICarService _service;
        private readonly ILogger<MstModelsController> _logger;

        public MstModelsController(ICarService service, ILogger<MstModelsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var models = await _service.GetModelInfoFromView();
            ViewBag.Brands = _service.GetBrand().Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();
            return View(models);
        }

        // GET: MstModels/Create
        public IActionResult FormCreate()
        {
            ViewBag.Brands = _service.GetBrand().Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();

            return View();
        }

        [HttpPost]
        public IActionResult AddModel(ReqModelDto model)
        {
            try
            {
                _service.ExecPrcModel(model, "Status: %");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        public IActionResult Edit(int id)
        {
            var model = _service.GetModel(id);
            ViewBag.Brands = _service.GetBrand().Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReqModelDto model)
        {
            try
            {
                _service.UpdateModel(id, model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult Delete(int id)
        {
            var model = _service.GetModel(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: MstModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.DeleteModel(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
