using AutoMapper;
using QueueMessageApp.DAL.Models;
using QueueMessageApp.Web.Models;

namespace QueueMessageApp.Web.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateMessageModel, Message>().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
