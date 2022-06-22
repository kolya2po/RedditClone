using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.MapModels;
using Forum.WebApi.Models.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.WebApi.Controllers
{
    /// <summary>
    /// Represents controller that handles requests related to comments.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentsController : BaseController
    {
        private readonly ICommentService _commentService;

        /// <summary>
        /// Initializes new instance of the CommentsController class and  class's fields.
        /// </summary>
        /// <param name="mapper">>Instance of Mapper class.</param>
        /// <param name="commentService">>Instance of CommentService class.</param>
        public CommentsController(IMapper mapper, ICommentService commentService) : base(mapper)
        {
            _commentService = commentService;
        }

        //  POST: HTTP://http://localhost:5001/api/comments
        /// <summary>
        /// Adds new model to the table.
        /// </summary>
        /// <param name="model">DTO that encapsulates information needed
        /// for creation of the new comment.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        public async Task Add(CreateCommentDto model)
        {
            await _commentService.AddAsync(Mapper.Map<CommentModel>(model));
        }

        //  PUT: HTTP://http://localhost:5001/api/comments
        /// <summary>
        /// Updates model in the table.
        /// </summary>
        /// <param name="model">DTO that encapsulates information needed
        /// for updating of the comment.</param>
        /// <returns>Task.</returns>
        [HttpPut]
        public async Task Update(UpdateCommentDto model)
        {
            await _commentService.UpdateAsync(Mapper.Map<CommentModel>(model));
        }

        //  DELETE: HTTP://http://localhost:5001/api/comments/1
        /// <summary>
        /// Deletes model in the table.
        /// </summary>
        /// <param name="id">Id of model to be deleted.</param>
        /// <returns>Task.</returns>
        [HttpDelete("{id:int}")]
        public async Task Delete(int id)
        {
            await _commentService.DeleteAsync(id);
        }

        //  POST: HTTP://http://localhost:5001/api/comments/1/reply
        /// <summary>
        /// Creates reply to the comment.
        /// </summary>
        /// <param name="commentId">Id of comment to be replied.</param>
        /// <param name="model">DTO that encapsulates information needed
        /// for updating of the comment.</param>
        /// <returns>Task.</returns>
        [HttpPost("{commentId:int}/reply")]
        public async Task ReplyToComment(int commentId, CreateCommentDto model)
        {
            await _commentService.ReplyToCommentAsync(commentId, Mapper.Map<CommentModel>(model));
        }

        //  PUT: HTTP://http://localhost:5001/api/comments/1/increase-rating
        /// <summary>
        /// Increases comment's rating.
        /// </summary>
        /// <param name="id">Comment's id.</param>
        /// <returns>Task.</returns>
        [HttpPut("{id:int}/increase-rating")]
        public async Task IncreaseRating(int id)
        {
            await _commentService.IncreaseRatingAsync(id);
        }

        //  PUT: HTTP://http://localhost:5001/api/comments1/decrease-rating
        /// <summary>
        /// Decreases comment's rating.
        /// </summary>
        /// <param name="id">Comment's id.</param>
        /// <returns>Task.</returns>
        [HttpPut("{id:int}/decrease-rating")]
        public async Task DecreaseRating(int id)
        {
            await _commentService.DecreaseRatingAsync(id);
        }
    }
}
