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
    public class UserService
    {
        private readonly IMapper _mapper;
        private readonly PasswordService _passwordService;
        private readonly IUserRepository _userRepository;

        public UserService(
            IMapper mapper,
            PasswordService passwordService,
            IUserRepository userRepository)
        {
            _mapper = mapper;
            _passwordService = passwordService;
            _userRepository = userRepository;
        }

        public async Task<UserDto> GetDetailsAsync(
            string userId)
        {
            return await _userRepository.Query()
                .Where(e => e.Id == userId.ToLower().Trim())
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserDto>> GetListAsync()
        {
            return await _userRepository.Query()
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<UserDto> CreateAdministratorAsync(
            CreateAdministratorDto createAdministratorDto)
        {
            var isUserUnique = await _userRepository.IsUniqueAsync(
                createAdministratorDto.Email,
                createAdministratorDto.Phone,
                Roles.Administrator);

            if (!isUserUnique)
            {
                throw new DomainException(
                    $"User phone or email is alredy in used. " +
                    $"Phone: {createAdministratorDto.Phone}. " +
                    $"Email: {createAdministratorDto.Email}");
            }

            var dbUser = _mapper.Map<User>(createAdministratorDto);
            dbUser.Password = _passwordService.GenerateRandomPassword();

            await _userRepository.SaveAsync(dbUser);

            return _mapper.Map<UserDto>(dbUser);
        }

        public async Task<UserDto> UpdateAdministratorAsync(
            string userId,
            UpdateAdministratorDto updateAdministratorDto)
        {
            var isUserUnique = await _userRepository.IsUniqueAsync(
                updateAdministratorDto.Id,
                updateAdministratorDto.Email,
                updateAdministratorDto.Phone,
                Roles.Administrator);

            if (!isUserUnique)
            {
                throw new DomainException(
                    $"User phone or email is alredy in used. " +
                    $"Phone: {updateAdministratorDto.Phone}. " +
                    $"Email: {updateAdministratorDto.Email}");
            }

            var dbUser = await _userRepository.GetAsync(userId);
            if (dbUser is null)
            {
                throw new DomainException(
                    $"Could not find user with such id. " +
                    $"Id: {userId}");
            }

            if (dbUser.Id != updateAdministratorDto.Id)
            {
                throw new DomainException(
                    $"Invalid user data provided. " +
                    $"Ids are miss matching");
            }

            _mapper.Map(updateAdministratorDto, dbUser);
            await _userRepository.UpdateAsync(dbUser);

            return _mapper.Map<UserDto>(dbUser);
        }

        public async Task<UserDto> DeleteAdministratorAsync(
            string userId)
        {
            var dbUser = await _userRepository.GetAsync(userId);
            if (dbUser is null)
            {
                throw new DomainException(
                    $"Could not find user with such id. " +
                    $"Id: {userId}");
            }

            if (dbUser.Role != Roles.Administrator)
            {
                throw new DomainException(
                    $"Invalid user provided. " +
                    $"User is not administrator." +
                    $"Role: {dbUser.Role}");
            }

            await _userRepository.DeleteAsync(dbUser);
            return _mapper.Map<UserDto>(dbUser);
        }

        public async Task ChangePasswordAsync(
            string userId, 
            string oldPassword, 
            string newPassword)
        {
            var dbUser = await _userRepository.GetAsync(userId);
            if (dbUser is null)
            {
                throw new DomainException(
                    $"Could not find user with such id. " +
                    $"Id: {userId}");
            }

            if (!_passwordService.IsValid(newPassword))
            {
                throw new DomainException(
                   $"Password is not valid. " +
                   $"A stronger pssword is required");
            }

            if (!_passwordService.AreEqual(dbUser.Password, oldPassword))
            {
                throw new DomainException(
                    "Invalid password. Password not matching");
            }

            if (_passwordService.AreEqual(dbUser.Password, newPassword))
            {
                throw new DomainException(
                    "Current password and new password are the same.");
            }

            dbUser.Password = _passwordService.Hash(newPassword);
            await _userRepository.UpdateAsync(dbUser);
        }
    }
}
