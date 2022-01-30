using Hcm.Api.Client;
using Hcm.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hcm.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IDepartmentClient _departmentClient;
        private readonly IAssignmentClient _assignmentClient;
        private readonly IEmployeeClient _employeeClient;

        public EmployeeController(
            IDepartmentClient departmentClient,
            IAssignmentClient assignmentClient,
            IEmployeeClient employeeClient)
        {
            _departmentClient = departmentClient;
            _assignmentClient = assignmentClient;
            _employeeClient = employeeClient;
        }

        // GET: EmployeeController
        public async Task<ActionResult> Index()
        {
            var employees = await _employeeClient.GetAllAsync();
            return View(employees
                .Select(e => new EmployeeViewModel
                {
                    EmployeeId = e.Id,
                    Country = e.Country,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email
                }).ToArray());

        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [FromForm] EmployeeViewModel employeeViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                await _employeeClient.PostAsync(new EmployeeCreateDto
                {
                    Email = employeeViewModel.Email,
                    AddressLine = employeeViewModel.AddressLine,
                    City = employeeViewModel.City,
                    Country = employeeViewModel.Country,
                    FirstName = employeeViewModel.FirstName,
                    LastName = employeeViewModel.LastName,
                    Phone = employeeViewModel.Phone,
                    PostCode = employeeViewModel.PostCode
                });

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // GET: EmployeeController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var result = await _employeeClient.GetAsync(id);
            var assignments = await _assignmentClient.GetByEmployeeAsync(id);
            var departments = await _departmentClient.GetAllAsync();

            return View(new EmployeeViewModel
            {
                EmployeeId=id,
                Country = result.Country,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Email = result.Email,
                AddressLine = result.AddressLine,
                City = result.City,
                Phone = result.Phone,
                PostCode = result.PostCode,
                Assignments = assignments.Select(e => new AssignmentViewModel
                {
                    AssignmentId = e.Id,
                    Amount = e.Sallary.Amount,
                    Currency = e.Sallary.Currency,
                    Name = departments.FirstOrDefault(d => d.Id == e.DepartmentId)?.Name,
                    DepartmentId = e.DepartmentId,
                    End = e.End,
                    Start = e.Start,
                    JobTitle = e.JobTitle
                }).ToArray()
            });
        }

        // GET: EmployeeController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var result = await _employeeClient.GetAsync(id);
            var assignments = await _assignmentClient.GetByEmployeeAsync(id);
            var departments = await _departmentClient.GetAllAsync();

            return View(new EmployeeViewModel
            {
                EmployeeId=id,
                Country = result.Country,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Email = result.Email,
                AddressLine = result.AddressLine,
                City = result.City,
                Phone = result.Phone,
                PostCode = result.PostCode,
                Assignments = assignments.Select(e => new AssignmentViewModel
                {
                    AssignmentId = e.Id,
                    Amount = e.Sallary.Amount,
                    Currency = e.Sallary.Currency,
                    Name = departments.FirstOrDefault(d => d.Id == e.DepartmentId)?.Name,
                    DepartmentId = e.DepartmentId,
                    End =e.End,
                    Start = e.Start,
                    JobTitle = e.JobTitle
                }).ToArray()
            });
        }
        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [FromRoute] string id,
            [FromForm] EmployeeViewModel employeeViewModel)
        {
            try
            {
                var result = await _employeeClient.PutAsync(id, new EmployeeUpdateDto
                {
                    Country = employeeViewModel.Country,
                    FirstName = employeeViewModel.FirstName,
                    LastName = employeeViewModel.LastName,
                    Email = employeeViewModel.Email,
                    AddressLine = employeeViewModel.AddressLine,
                    City = employeeViewModel.City,
                    Phone = employeeViewModel.Phone,
                    PostCode = employeeViewModel.PostCode,
                });
                if (User.Identity.IsAuthenticated && User.IsInRole("Employee"))
                {
                    return RedirectToAction("Details", "Employee", new { id = id });
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _employeeClient.GetAsync(id);

            return View(new EmployeeViewModel
            {
                EmployeeId = result.Id,
                Country = result.Country,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Email = result.Email,
                AddressLine = result.AddressLine,
                City = result.City,
                Phone = result.Phone,
                PostCode = result.PostCode
            });
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection formCollection)
        {
            try
            {
                await _employeeClient.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
