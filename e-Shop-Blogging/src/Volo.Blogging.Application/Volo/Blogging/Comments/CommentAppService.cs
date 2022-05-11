using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Blogging.Comments.Dtos;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Comments
{
    public class CommentAppService : BloggingAppServiceBase, ICommentAppService
    {
        //protected IBlogUserLookupService UserLookupService { get; }

        protected ICommentRepository CommentRepository { get; }

        public CommentAppService(ICommentRepository commentRepository)
        {
            CommentRepository = commentRepository;
        }

        public async Task<List<CommentWithRepliesDto>> GetHierarchicalListOfPostAsync(Guid postId)
        {
            var comments = await GetListOfPostAsync(postId);
            var userDictionary = new Dictionary<Guid, BlogUserDto>();

            foreach (var commentDto in comments)
            {
                if (commentDto.CreatorId.HasValue)
                {
                    var creatorUser = new IdentityUser(Guid.Empty, "", "", Guid.Empty); ; //await UserLookupService.FindByIdAsync(commentDto.CreatorId.Value);

                    if (creatorUser != null && !userDictionary.ContainsKey(creatorUser.Id))
                    {
                        userDictionary.Add(creatorUser.Id, ObjectMapper.Map<IdentityUser, BlogUserDto>(creatorUser));
                    }
                }
            }

            foreach (var commentDto in comments)
            {
                commentDto.Writer = new BlogUserDto() { Email = $"{Environment.TickCount}@{Environment.CurrentManagedThreadId}", UserName = $"{Environment.CurrentManagedThreadId}", Id = Guid.NewGuid() }; //userDictionary[(Guid)commentDto.CreatorId];
                if (commentDto.CreatorId.HasValue && userDictionary.ContainsKey((Guid)commentDto.CreatorId))
                {
                }
            }

            var hierarchicalComments = new List<CommentWithRepliesDto>();

            foreach (var commentDto in comments)
            {
                var parent = hierarchicalComments.Find(c => c.Comment.Id == commentDto.RepliedCommentId);

                if (parent != null)
                {
                    parent.Replies.Add(commentDto);
                }
                else
                {
                    hierarchicalComments.Add(new CommentWithRepliesDto() { Comment = commentDto });
                }
            }

            hierarchicalComments = hierarchicalComments.OrderByDescending(c => c.Comment.CreationTime).ToList();

            return hierarchicalComments;
        }

        private async Task<List<CommentWithDetailsDto>> GetListOfPostAsync(Guid postId)
        {
            var comments = await CommentRepository.GetListOfPostAsync(postId);

            return new List<CommentWithDetailsDto>(
                ObjectMapper.Map<List<Comment>, List<CommentWithDetailsDto>>(comments));
        }

        [Authorize]
        public async Task<CommentWithDetailsDto> CreateAsync(CreateCommentDto input)
        {
            var comment = new Comment(GuidGenerator.Create(), input.PostId, input.RepliedCommentId, input.Text);

            comment = await CommentRepository.InsertAsync(comment);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<Comment, CommentWithDetailsDto>(comment);
        }

        [Authorize]
        public async Task<CommentWithDetailsDto> UpdateAsync(Guid id, UpdateCommentDto input)
        {
            var comment = await CommentRepository.GetAsync(id);

            await AuthorizationService.CheckAsync(comment, CommonOperations.Update);

            comment.SetText(input.Text);
            comment.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

            comment = await CommentRepository.UpdateAsync(comment);

            return ObjectMapper.Map<Comment, CommentWithDetailsDto>(comment);
        }

        [Authorize]
        public async Task DeleteAsync(Guid id)
        {
            var comment = await CommentRepository.GetAsync(id);

            await AuthorizationService.CheckAsync(comment, CommonOperations.Delete);

            await CommentRepository.DeleteAsync(id);

            var replies = await CommentRepository.GetRepliesOfComment(id);

            foreach (var reply in replies)
            {
                await CommentRepository.DeleteAsync(reply.Id);
            }
        }
    }
}
