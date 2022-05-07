using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;
using Volo.Blogging.Comments;
using Volo.Blogging.Comments.Dtos;
using Volo.Blogging.Posts;
using Volo.Blogging.Tagging;
using Volo.Blogging.Tagging.Dtos;

namespace Volo.Blogging
{
    public class BloggingApplicationAutoMapperProfile : Profile
    {
        public BloggingApplicationAutoMapperProfile()
        {
            CreateMap<Blog, BlogDto>();
            CreateMap<IdentityUser, BlogUserDto>();
            CreateMap<Post, PostWithDetailsDto>().Ignore(x=>x.Writer).Ignore(x=>x.CommentCount).Ignore(x=>x.Tags);
            CreateMap<Comment, CommentWithDetailsDto>().Ignore(x => x.Writer);
            CreateMap<Tag, TagDto>();
            CreateMap<Post, PostCacheItem>().Ignore(x=>x.CommentCount).Ignore(x=>x.Tags);
            CreateMap<PostCacheItem, PostWithDetailsDto>()
                .IgnoreModificationAuditedObjectProperties()
                .IgnoreDeletionAuditedObjectProperties()
                .Ignore(x => x.ConcurrencyStamp)
                .Ignore(x => x.Writer)
                .Ignore(x => x.CommentCount)
                .Ignore(x => x.Tags);
        }
    }
}
