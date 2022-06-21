using System.Linq;
using AutoMapper;
using Business.MapModels;
using Data.Models;

namespace Business
{
    /// <inheritdoc />
    public class AutoMapperBllProfile : Profile
    {
        /// <summary>
        /// Configures mapping between domain and business models.
        /// </summary>
        public AutoMapperBllProfile()
        {
            CreateMap<Comment, CommentModel>()
                .ForMember(dst => dst.AuthorName, opt =>
                    opt.MapFrom(src => src.Author.UserName))
                .ForMember(dst => dst.CommunityName, opt =>
                    opt.MapFrom(src => src.Topic.Community.Title))
                .ForMember(dst => dst.PostingDate, opt =>
                    opt.MapFrom(src => $"{src.PostingDate.ToShortDateString()} {src.PostingDate.ToShortTimeString()}"))
                .ForMember(dst => dst.TopicName, opt =>
                    opt.MapFrom(src => src.Topic.Title))
                .ReverseMap();

            CreateMap<Topic, TopicModel>()
                .ForMember(dst => dst.CommentsCount, opt => opt.MapFrom(src => src.Comments.Count()))
                .ForMember(dst => dst.AuthorName, opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dst => dst.CommunityName, opt => opt.MapFrom(src => src.Community.Title))
                .ForMember(dst => dst.CommentModels, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dst => dst.PostingDate, opt => opt.MapFrom(src => $"{src.PostingDate.ToShortDateString()} {src.PostingDate.ToShortTimeString()}"))
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
                .ForMember(dst => dst.CreationDate, opt =>
                    opt.MapFrom(src => src.CreationDate.ToShortDateString()))
                .ReverseMap();

            CreateMap<User, UserModel>()
                .ForMember(dst => dst.PostModels, opt => opt.MapFrom(src => src.Posts))
                .ForMember(dst => dst.CommentModels, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dst => dst.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToShortDateString()))
                .ForMember(dst => dst.CommunitiesIds, opt => opt.MapFrom(src => src.Communities.Select(c => c.CommunityId)))
                .ReverseMap();
        }
    }
}
