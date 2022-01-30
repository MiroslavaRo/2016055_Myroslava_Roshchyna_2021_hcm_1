using AutoMapper;
using Hcm.Api.Dto;
using Hcm.Database.Models;
using System.Linq;

namespace Hcm.Api.Configuration
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<EmployeeCreateDto, User>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(_ => Roles.Employee))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToLower().Trim()))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Email.ToLower().Trim()))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(_ => string.Empty));

            CreateMap<EmployeeCreateDto, Employee>();
            CreateMap<Employee, EmployeeDto>();

            CreateMap<EmployeeUpdateDto, Employee>();
            CreateMap<EmployeeUpdateDto, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToLower().Trim()))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Email.ToLower().Trim()));

            CreateMap<AssignmentCreateDto, Assignment>();
            CreateMap<AssignmentUpdateDto, Assignment>();

            CreateMap<SallaryDto, Sallary>();
            CreateMap<Sallary, SallaryDto>();

            CreateMap<Assignment, AssignmentDto>()
                .AfterMap((src, dest) =>
                {
                    dest.SallariesHistory = src.Sallaries
                        .OrderByDescending(e => e.Id)
                        .Select(e => new SallaryDto
                        {
                            Amount = e.Amount,
                            Currency = e.Currency
                        })
                        .ToArray();

                    dest.Sallary = dest.SallariesHistory.First();
                });

            CreateMap<DepartmentCreateDto, Department>();
            CreateMap<DepartmentUpdateDto, Department>();
            CreateMap<Department, DepartmentDto>();

            CreateMap<CreateAdministratorDto, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToLower().Trim()))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Email.ToLower().Trim()))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(_ => Roles.Administrator))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(_ => string.Empty));

            CreateMap<UpdateAdministratorDto, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToLower().Trim()))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Email.ToLower().Trim()));

            CreateMap<User, UserDto>();
        }
    }
}
