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
    public class DepartmentService
    {
        private readonly IMapper _mapper;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly ISallaryRepository _sallaryRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(
            IMapper mapper,
            IAssignmentRepository assignmentRepository,
            ISallaryRepository sallaryRepository,
            IDepartmentRepository departmentRepository)
        {
            _mapper = mapper;
            _assignmentRepository = assignmentRepository;
            _sallaryRepository = sallaryRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<DepartmentDto> GetDetailsAsync(
            string departmentId)
        {
            return await _departmentRepository.Query()
                .Where(e => e.Id == departmentId.ToLower().Trim())
                .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DepartmentDto>> GetListAsync()
        {
            return await _departmentRepository.Query()
                .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<DepartmentDto> CreateAsync(
            DepartmentCreateDto departmentCreateDto)
        {
            var isUnique = await _departmentRepository.IsUniqueName(
                departmentCreateDto.Name);

            if (!isUnique)
            {
                throw new DomainException(
                  $"Department name is alredy in used. " +
                  $"Name: {departmentCreateDto.Name}");
            }

            var dbDepartment = _mapper.Map<Department>(departmentCreateDto);
            await _departmentRepository.SaveAsync(dbDepartment);

            return _mapper.Map<DepartmentDto>(dbDepartment);
        }

        public async Task<DepartmentDto> UpdateAsync(
            string departmentId,
            DepartmentUpdateDto departmentUpdateDto)
        {
            var isUnique = await _departmentRepository.IsUniqueName(
                departmentUpdateDto.Id,
                departmentUpdateDto.Name);

            if (!isUnique)
            {
                throw new DomainException(
                  $"Department name is alredy in used. " +
                  $"Name: {departmentUpdateDto.Name}");
            }

            var dbDepartment = await _departmentRepository.GetAsync(departmentId);
            if (dbDepartment is null)
            {
                throw new DomainException(
                    $"Could not find department with such id. " +
                    $"Id: {departmentId}");
            }

            if (dbDepartment.Id != departmentUpdateDto.Id)
            {
                throw new DomainException(
                    $"Invalid department data provided. " +
                    $"Ids are miss matching");
            }

            _mapper.Map(departmentUpdateDto, dbDepartment);
            await _departmentRepository.UpdateAsync(dbDepartment);

            return _mapper.Map<DepartmentDto>(dbDepartment);
        }

        public async Task<DepartmentDto> DeleteAsync(
            string departmentId)
        {
            var dbDepartment = await _departmentRepository.GetAsync(departmentId);
            if (dbDepartment is null)
            {
                throw new DomainException(
                    $"Could not find department with such id. " +
                    $"Id: {departmentId}");
            }

            foreach (var dbAssignment in dbDepartment.Assignments)
            {
                foreach (var dbSallary in dbAssignment.Sallaries)
                {
                    await _sallaryRepository.DeleteAsync(dbSallary);
                }

                await _assignmentRepository.DeleteAsync(dbAssignment);
            }

            await _departmentRepository.DeleteAsync(dbDepartment);
            return _mapper.Map<DepartmentDto>(dbDepartment);
        }
    }
}
