using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hcm.Api.Dto;
using Hcm.Core.Database;
using Hcm.Database.Models;
using Hcm.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcm.Api.Services
{
    public class AssignmentService
    {
        private const int MaxAssignementsCount = 10;

        private readonly IMapper _mapper;
        private readonly ISallaryRepository _sallaryRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public AssignmentService(
            IMapper mapper,
            ISallaryRepository sallaryRepository,
            IDepartmentRepository departmentRepository,
            IEmployeeRepository employeeRepository,
            IAssignmentRepository assignmentRepository)
        {
            _mapper = mapper;

            _sallaryRepository = sallaryRepository;
            _departmentRepository = departmentRepository;
            _employeeRepository = employeeRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<AssignmentDto> GetDetailsAsync(
            string assignmentId)
        {
            var assignment = await _assignmentRepository.Query().Include(e => e.Sallaries)
                .Where(e => e.Id == assignmentId.ToLower().Trim())
                .FirstOrDefaultAsync();

            return _mapper.Map<AssignmentDto>(assignment);
        }

        public async Task<IEnumerable<AssignmentDto>> GetListAsync()
        {
            var assignments = await _assignmentRepository.Query()
                .Include(e => e.Sallaries)
                .ToArrayAsync();

            return assignments.Select(e => new AssignmentDto
            {
                DepartmentId = e.DepartmentId,
                EmployeeId = e.EmployeeId,
                End = e.End,
                Id = e.Id,
                JobTitle = e.JobTitle,
                Start = e.Start,
                Sallary = _mapper.Map<SallaryDto>(e.Sallaries.Last()),
                SallariesHistory = e.Sallaries
                    .Select(s => new SallaryDto 
                    { 
                        Amount = s.Amount, Currency = s.Currency
                    })
                    .ToArray()
            })
            .ToArray();
        }

        public async Task<AssignmentDto> CreateAsync(
            string employeeId,
            string departmentId,
            AssignmentCreateDto assignmentCreateDto)
        {

            if (assignmentCreateDto.End < assignmentCreateDto.Start
                && assignmentCreateDto.End != null)
            {
                throw new DomainException(
                   $"Invalid end date. End date should be " +
                   $"greater than the start date.");
            }

            var dbEmployee = await _employeeRepository.GetAsync(employeeId);
            if (dbEmployee is null)
            {
                throw new DomainException(
                    $"Could not find employee with such id. Id: {employeeId}");
            }

            var dbDepartment = await _departmentRepository.GetAsync(departmentId);
            if (dbDepartment is null)
            {
                throw new DomainException(
                    $"Could not find department with such id. Id: {departmentId}");
            }

            if (dbEmployee.Assignments.Count() >= MaxAssignementsCount)
            {
                throw new DomainException(
                    $"Employee has reached the maximum number " +
                    $"of assignments which is {MaxAssignementsCount}");
            }

            var assignments = dbEmployee.Assignments
                .Where(e => e.DepartmentId == departmentId)
                .ToArray();

            // Throws an exception if an overlapp occures
            ValidateForOverlapps(
                assignmentCreateDto.Start,
                assignmentCreateDto.End,
                assignments);

            var dbAssignment = _mapper.Map<Assignment>(assignmentCreateDto);
            dbAssignment.EmployeeId = dbEmployee.Id;
            dbAssignment.DepartmentId = dbDepartment.Id;

            await _assignmentRepository.SaveAsync(dbAssignment);

            var dbSallary = _mapper.Map<Sallary>(assignmentCreateDto.Sallary);
            dbSallary.AssignmentId = dbAssignment.Id;
            await _sallaryRepository.SaveAsync(dbSallary);

            return _mapper.Map<AssignmentDto>(dbAssignment);
        }

        public async Task<AssignmentDto> UpdateAsync(
            string assignmentId,
            AssignmentUpdateDto assignmentUpdateDto)
        {
            var dbAssignment = await _assignmentRepository.GetAsync(assignmentId);
            if (dbAssignment is null)
            {
                throw new DomainException(
                    $"Invalid assignment id. " +
                    $"Could not find assignment with that id. Id: {assignmentId}");
            }

            if (dbAssignment.Id != assignmentUpdateDto.Id)
            {
                throw new DomainException(
                   $"Invalid assignment data provided. " +
                   $"Ids are miss matching");
            }

            _mapper.Map(assignmentUpdateDto, dbAssignment);

            var dbEmployee = await _employeeRepository.GetAsync(dbAssignment.EmployeeId);
            if (dbEmployee is null)
            {
                throw new DomainException(
                   $"Invalid assignment data provided. " +
                   $"No employee found for this assignment. " +
                   $"Employee Id: {dbAssignment.EmployeeId}");
            }

            var assignments = dbEmployee.Assignments
                .Where(e => e.DepartmentId == dbAssignment.DepartmentId)
                .Where(e => e.Id != dbAssignment.Id)
                .ToArray();

            // Throws an exception if an overlapp occures
            ValidateForOverlapps(
                dbAssignment.Start,
                dbAssignment.End,
                assignments);

            await _assignmentRepository.UpdateAsync(dbAssignment);

            if (assignmentUpdateDto.Sallary != null)
            {
                var dbSallary = _mapper.Map<Sallary>(assignmentUpdateDto.Sallary);
                dbSallary.AssignmentId = dbAssignment.Id;
                await _sallaryRepository.SaveAsync(dbSallary);
            }

            return _mapper.Map<AssignmentDto>(dbAssignment);
        }

        public async Task<AssignmentDto> DeleteAsync(
            string assignmentId)
        {
            var dbAssignment = await _assignmentRepository.GetAsync(assignmentId);
            if (dbAssignment is null)
            {
                throw new DomainException(
                    $"Invalid assignment id. " +
                    $"Could not find assignment with that id. Id: {assignmentId}");
            }

            foreach(var sallary in dbAssignment.Sallaries)
            {
                await _sallaryRepository.DeleteAsync(sallary);
            }

            await _assignmentRepository.DeleteAsync(dbAssignment);
            return _mapper.Map<AssignmentDto>(dbAssignment);
        }

        private void ValidateForOverlapps(
            DateTime? startDate,
            DateTime? endDate,
            IEnumerable<Assignment> assignments)
        {
            // TODO: Figure out the logic for overlaps
            foreach (var assignment in assignments)
            {
                if (assignment.Start >= startDate
                    && endDate == null)
                {
                    throw new DomainException(
                        $"Invalid assignment dates. " +
                        $"Asssignment overlaps with existing one." +
                        $"Start date: {startDate}");
                }

                if (assignment.Start >= startDate
                    && assignment.End <= startDate)
                {
                    throw new DomainException(
                        $"Invalid assignment dates. " +
                        $"Asssignment overlaps with existing one. " +
                        $"Start date: {startDate}");
                }

                if (assignment.Start >= endDate
                    && assignment.End >= endDate)
                {
                    throw new DomainException(
                        $"Invalid assignment dates. " +
                        $"Asssignment overlaps with existing one. " +
                        $"End date: {endDate}");
                }

                if (assignment.Start >= startDate
                    && assignment.Start <= endDate
                    && endDate != null)
                {
                    throw new DomainException(
                        $"Invalid assignment dates. " +
                        $"Asssignment overlaps with existing one." +
                        $"Start date: {startDate} End date: {endDate}");
                }
            }

        }
    }
}
