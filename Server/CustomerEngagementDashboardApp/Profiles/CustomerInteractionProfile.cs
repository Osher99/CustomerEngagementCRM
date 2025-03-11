using AutoMapper;
using CustomerEngagementDashboardApp.DbModel;
using CustomerEngagementDashboardApp.Dtos;

public class CustomerInteractionProfile : Profile
{
    public CustomerInteractionProfile()
    {
        CreateMap<CustomerInteractionDTO, CustomerInteraction>();
        CreateMap<CustomerInteraction, CustomerInteractionDTO>();
    }
}
