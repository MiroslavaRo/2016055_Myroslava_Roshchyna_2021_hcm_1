using Hcm.Api.Client;
using Hcm.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Hcm.Web.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly IDepartmentClient _departmentClient;
        private readonly IAssignmentClient _assignmentClient;

        public AssignmentController(
            IDepartmentClient departmentClient,
            IAssignmentClient assignmentClient)
        {
            _departmentClient = departmentClient;
            _assignmentClient = assignmentClient;
        }

        public async Task<ActionResult> Edit(
            [FromRoute] string id, 
            [FromQuery] string employeeId)
        {
            var result = await _assignmentClient.GetAsync(id);
            var departments = await _departmentClient.GetAllAsync();

            return View(new AssignmentViewModel
            {
                AssignmentId = result.Id,
                Amount = result.Sallary.Amount,
                Currency = result.Sallary.Currency,
                Name = departments
                    .Where(e => e.Id == result.DepartmentId)
                    .Select(e => e.Name)
                    .FirstOrDefault(),
                DepartmentId = result.DepartmentId,
                End = result.End,
                Start =result.Start,
                JobTitle = result.JobTitle
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [FromRoute] string id, 
            [FromQuery] string employeeId,
            [FromForm] AssignmentViewModel assignment)
        {
            var result = await _assignmentClient.PutAsync(id, new AssignmentUpdateDto
            {
                Start = assignment.Start.Value,
                JobTitle = assignment.JobTitle,
                End = assignment.End,
                Id = assignment.AssignmentId,
                Sallary = new SallaryDto
                {
                    Amount = assignment.Amount,
                    Currency = assignment.Currency,
                }
            });

            return RedirectToAction("Edit", "Employee", new { id = employeeId });
        }

        public async Task<ActionResult> Create(
           [FromQuery] string employeeId)
        {
            var departments = await _departmentClient.GetAllAsync();

            return View(new AssignmentViewModel
            {
                Departments = departments
                    .Select(e => (e.Id, e.Name))
                    .ToArray()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [FromQuery] string employeeId,
            [FromForm] AssignmentViewModel assignment)
        {
            var result = await _assignmentClient.PostAsync(
                employeeId, assignment.DepartmentId, new AssignmentCreateDto
            {
                Start = assignment.Start.Value,
                JobTitle = assignment.JobTitle,
                End = assignment.End,
                Sallary = new SallaryDto
                {
                    Amount = assignment.Amount,
                    Currency = assignment.Currency,
                }
            });

            return RedirectToAction("Edit", "Employee", new { id = employeeId });
        }

        
    }
}
