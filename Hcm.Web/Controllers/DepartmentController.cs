using Hcm.Api.Client;
using Hcm.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcm.Web.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentClient _departmentClient;

        public DepartmentController(IDepartmentClient departmentClient)
        {
            _departmentClient = departmentClient;
        }

        // GET: DepartmentController
        public async Task<ActionResult> Index()
        {
            var result = await _departmentClient.GetAllAsync();
            return View(result.Select(e => new DepratmentViewModel
            {
                City = e.City,
                Country = e.Country,
                DepartmentId = e.Id,
                Name = e.Name
            }).ToArray());
        }

        // GET: DepartmentController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var result = await _departmentClient.GetAsync(id);
            return View(new DepratmentViewModel
            {
                City = result.City,
                Country = result.Country,
                DepartmentId = result.Id,
                Name = result.Name
            });
        }

        // GET: DepartmentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DepartmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DepratmentViewModel depratmentViewModel)
        {
            try
            {
                var result = await _departmentClient.PostAsync(new DepartmentCreateDto
                {
                    Name = depratmentViewModel.Name,
                    City = depratmentViewModel.City,
                    Country = depratmentViewModel.Country
                });

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DepartmentController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var result = await _departmentClient.GetAsync(id);
            return View(new DepratmentViewModel
            {
                City = result.City,
                Country = result.Country,
                DepartmentId = result.Id,
                Name = result.Name
            });
        }

        // POST: DepartmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            string id, 
            DepratmentViewModel depratmentViewModel)
        {
            try
            {
                var result = await _departmentClient.PutAsync(id, new DepartmentUpdateDto
                {
                    Name = depratmentViewModel.Name,
                    City = depratmentViewModel.City,
                    Country = depratmentViewModel.Country,
                    Id = id,
                });

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DepartmentController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _departmentClient.GetAsync(id);
            return View(new DepratmentViewModel
            {
                City = result.City,
                Country = result.Country,
                DepartmentId = result.Id,
                Name = result.Name
            });
        }

        // POST: DepartmentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            try
            {
                var result = await _departmentClient.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
