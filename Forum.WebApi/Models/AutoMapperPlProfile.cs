using AutoMapper;
using Business.MapModels;
using Business.MapModels.Identity;
using Forum.WebApi.Models.Comment;
using Forum.WebApi.Models.Community;
using Forum.WebApi.Models.Identity;
using Forum.WebApi.Models.Rule;
using Forum.WebApi.Models.Topic;

namespace Forum.WebApi.Models
{
    /// <inheritdoc />
    public class AutoMapperPlProfile : Profile
    {
        /// <summary>
        /// Configures mapping between DTOs and business models.
        /// </summary>
        public AutoMapperPlProfile()
        {
            CreateMap<CreateCommunityDto, CommunityModel>().ReverseMap();
            CreateMap<UpdateCommunityDto, CommunityModel>().ReverseMap();

            CreateMap<CreateRuleDto, RuleModel>().ReverseMap();
            CreateMap<UpdateRuleDto, RuleModel>().ReverseMap();

            CreateMap<CreateCommentDto, CommentModel>().ReverseMap();
            CreateMap<UpdateCommentDto, CommentModel>().ReverseMap();

            CreateMap<CreateTopicDto, TopicModel>().ReverseMap();
            CreateMap<UpdateTopicDto, TopicModel>().ReverseMap();

            CreateMap<LoginModelDto, LoginModel>().ReverseMap();
            CreateMap<RegistrationModelDto, RegistrationModel>().ReverseMap();
            CreateMap<UpdateUserDto, UserModel>().ReverseMap();
        }
    }
}
