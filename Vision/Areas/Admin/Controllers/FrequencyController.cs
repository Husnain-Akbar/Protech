﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vision.Data.IRepository;
using Vision.Models;
using Vision.Utility;

namespace Vision.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]

    public class FrequencyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FrequencyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Upsert(int? id)
        {
            Frequency frequency = new Frequency();
            if (id == null)
            {

                return View(frequency);
            }

            frequency = _unitOfWork.Frequency.Get(id.GetValueOrDefault());
            if (frequency == null)
            {
                return NotFound();
            }
            return View(frequency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Frequency frequency)
        {

            if (ModelState.IsValid)
            {
                if (frequency.Id == 0)
                {
                    _unitOfWork.Frequency.Add(frequency);
                }
                else
                {
                    _unitOfWork.Frequency.Update(frequency);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(frequency);

        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Frequency.Get(id);
            if (objFromDb == null)
            {
                return NotFound();
            }
            _unitOfWork.Frequency.Remove(objFromDb);
            _unitOfWork.Save();
            return RedirectToAction("Index");

        }
        #region API Calls

        public IActionResult GetAll()
        {

            return Json(new { data = _unitOfWork.Frequency.GetAll() });
        }

        #endregion

    }
}