using Api.LiveHub.Domain.Models;
using Api.LiveHub.ViewModel;
using AutoMapper;

namespace Api.LiveHub.AutoMapper
{
    public class DomainToViewModelMappingProfile:Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<SignIn, SignInViewModel>();
            CreateMap<Business, BusinessViewModel>()
                .ForMember(dest => dest.StrStatus, opts => opts.MapFrom(src => src.BusinessStatus.ToString()));
            CreateMap<Account, AccountViewModel>();
            CreateMap<Role, RoleViewModel>();
            CreateMap<ToDoList, ToDoViewModel>();
            CreateMap<Business, BusinessExcelViewModel>()
                .ForMember(dest=>dest.AccountName ,opts=>opts.MapFrom(src=>src.Account.AccountName))
                .ForMember(dest => dest.AccoutNo, opts => opts.MapFrom(src => src.Account.AccountNo));
        }
    }
}
