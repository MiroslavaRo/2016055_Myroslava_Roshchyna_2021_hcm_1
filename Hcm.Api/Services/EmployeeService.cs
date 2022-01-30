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
    public class EmployeeService
    {
        private readonly IMapper _mapper;
        private readonly PasswordService _passwordService;
        private readonly IUserRepository _userRepository;
        private readonly ISallaryRepository _sallaryRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public EmployeeService(
            IMapper mapper,
            PasswordService passwordService,
            IUserRepository userRepository,
            ISallaryRepository sallaryRepository,
            IEmployeeRepository employeeRepository, 
            IAssignmentRepository assignmentRepository)
        {
            _mapper = mapper;
            _passwordService = passwordService;
            _userRepository = userRepository;
            _sallaryRepository = sallaryRepository;
            _employeeRepository = employeeRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<EmployeeDto> GetDetailsAsync(string employeeId)
        {
            return await _employeeRepository.Query()
                .Where(e => e.Id == employeeId.ToLower().Trim())
                .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<EmployeeDto>> GetListAsync()
        {
            return await _employeeRepository.Query()
                .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<EmployeeDto> CreateAsync(
            EmployeeCreateDto employeeCreateDto)
        {
            var isEmailUnique = await _employeeRepository.IsEmailUniqueAsync(
                employeeCreateDto.Email);
            
            if (!isEmailUnique)
            {
                throw new DomainException(
                    $"Employee email is alredy in used. Email: {employeeCreateDto.Email}");
            }

            var isPhoneUnique = await _employeeRepository.IsPhoneUniqueAsync(
                employeeCreateDto.Phone);

            if (!isPhoneUnique)
            {
                throw new DomainException(
                    $"Employee phone is alredy in used. Phone: {employeeCreateDto.Phone}");
            }

            var isUserUnique = await _userRepository.IsUniqueAsync(
                employeeCreateDto.Email, 
                employeeCreateDto.Phone, 
                Roles.Employee);

            if (!isUserUnique)
            {
                throw new DomainException(
                    $"User phone or email is alredy in used. " +
                    $"Phone: {employeeCreateDto.Phone}, Email: {employeeCreateDto.Email}");
            }

            // Create user for login
            var dbUser = _mapper.Map<User>(employeeCreateDto);
            dbUser.Password = _passwordService.GenerateRandomPassword();

            await _userRepository.SaveAsync(dbUser);

            // Create employee
            var dbEmployee = _mapper.Map<Employee>(employeeCreateDto);
            dbEmployee.UserId = dbUser.Id;
            await _employeeRepository.SaveAsync(dbEmployee);

            // Return the response to the caller
            return _mapper.Map<EmployeeDto>(dbEmployee);
        }

        public async Task<EmployeeDto> UpdateAsync(
            string employeeId, 
            EmployeeUpdateDto employeeUpdateDto)
        {
            var dbEmployee = await _employeeRepository.GetAsync(employeeId);
            
            if (dbEmployee is null)
            {
                throw new DomainException(
                    $"Could not find employee with such id. Id: {employeeId}");
            }

            var isEmailUnique = await _employeeRepository.IsEmailUniqueAsync(
                employeeId,
                employeeUpdateDto.Email);

            if (!isEmailUnique)
            {
                throw new DomainException(
                    $"Employee email is alredy in used. Email: {employeeUpdateDto.Email}");
            }

            var isPhoneUnique = await _employeeRepository.IsPhoneUniqueAsync(
                employeeId,
                employeeUpdateDto.Phone);

            if (!isPhoneUnique)
            {
                throw new DomainException(
                    $"Employee phone is alredy in used. Phone: {employeeUpdateDto.Phone}");
            }
   
            var isUserUnique = await _userRepository.IsUniqueAsync(
                dbEmployee.UserId,
                employeeUpdateDto.Email,
                employeeUpdateDto.Phone,
                Roles.Employee);

            if (!isUserUnique)
            {
                throw new DomainException(
                    $"User phone or email is alredy in used. " +
                    $"Phone: {employeeUpdateDto.Phone}, Email: {employeeUpdateDto.Email}");
            }

            var dbUser = await _userRepository.GetAsync(dbEmployee.UserId);
            if (dbUser is null)
            {
                throw new DomainException(
                    $"Could not find user with such id. Id: {dbEmployee.UserId}");
            }

            _mapper.Map(employeeUpdateDto, dbEmployee);
            _mapper.Map(employeeUpdateDto, dbUser);

            await _employeeRepository.UpdateAsync(dbEmployee);
            await _userRepository.UpdateAsync(dbUser);

            return _mapper.Map<EmployeeDto>(dbEmployee);
        }

        public async Task<EmployeeDto> DeleteAsync(
            string employeeId)
        {
            var dbEmployee = await _employeeRepository.GetAsync(employeeId);

            if (dbEmployee is null)
            {
                throw new DomainException(
                    $"Could not find employee with such id. Id: {employeeId}");
            }

            foreach(var assignment in dbEmployee.Assignments)
            {
                foreach(var sallary in assignment.Sallaries)
                {
                    await _sallaryRepository.DeleteAsync(sallary);
                }

                await _assignmentRepository.DeleteAsync(assignment);
            }

            await _employeeRepository.DeleteAsync(dbEmployee);

            var dbUser = await _userRepository.GetAsync(dbEmployee.UserId);
            await _userRepository.DeleteAsync(dbUser);

            return _mapper.Map<EmployeeDto>(dbEmployee);
        }
    }
}
