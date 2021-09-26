using AutoMapper;
using X.DTOs;
using X.Storage.Models;

namespace X.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Message, MessageDto>().ReverseMap();
        }
    }
}
