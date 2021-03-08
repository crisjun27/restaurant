using RestaurantCollection.WebApi.DTO.Forms;
using AutoMapper;

namespace RestaurantCollection.WebApi.Models
{  
    public class MappingProfileConfiguration : Profile
    {
        public MappingProfileConfiguration()
        {
            CreateMap<Restaurant, Querys>().ReverseMap();
            CreateMap<Restaurant, UpdateForm>().ReverseMap();
            CreateMap<Restaurant, CreateForm>().ReverseMap(); 
            CreateMap<Restaurant, RestaurantQueryResponse>().ReverseMap();
        }
    }
}
