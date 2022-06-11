using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Business.MapModels;
using Data.Models;

namespace Business
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<Comment, CommentModel>()
                .ForMember(dst => dst.AuthorName, opt =>
                    opt.MapFrom(src => src.Author.UserName))
                .ForMember(dst => dst.CommunityName, opt =>
                    opt.MapFrom(src => src.Topic.Community.Title))
                .ReverseMap();

            CreateMap<Topic, TopicModel>()
                .ForMember(dst => dst.CommentsCount, opt => opt.MapFrom(src => src.Comments.Count()))
                .ForMember(dst => dst.AuthorName, opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dst => dst.CommunityName, opt => opt.MapFrom(src => src.Community.Title))
                .ForMember(dst => dst.CommentModels, opt => opt.MapFrom(src => src.Comments))
                .ReverseMap();

            CreateMap<Rule, RuleModel>().ReverseMap();
            CreateMap<Community, CommunityModel>()
                .ForMember(dst => dst.MembersCount, opt =>
                    opt.MapFrom(src => src.Members.Count()))
                .ForMember(dst => dst.PostModels, opt =>
                    opt.MapFrom(src => src.Posts))
                .ForMember(dst => dst.RuleModels, opt =>
                    opt.MapFrom(src => src.Rules))
                .ForMember(dst => dst.ModeratorModels, opt =>
                    opt.MapFrom(src => src.Moderators))
                .ForMember(dst => dst.CreatorName, opt =>
                    opt.MapFrom(src => src.Creator.UserName))
                .ReverseMap();

            CreateMap<User, UserModel>()
                .ForMember(dst => dst.PostsIds, opt => opt.MapFrom(src => src.Posts.Select(p => p.Id)))
                .ForMember(dst => dst.CommentsIds, opt => opt.MapFrom(src => src.Comments.Select(c => c.Id)))
                .ReverseMap();
        }
    }
}
