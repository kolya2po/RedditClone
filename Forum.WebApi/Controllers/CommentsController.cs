using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.MapModels;
using Forum.WebApi.Models.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentsController : BaseController
    {
        private readonly ICommentService _commentService;

        public CommentsController(IMapper mapper, ICommentService commentService) : base(mapper)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task Add(CreateCommentDto model)
        {
            await _commentService.AddAsync(Mapper.Map<CommentModel>(model));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCommentDto model)
        {
            await _commentService.UpdateAsync(Mapper.Map<CommentModel>(model));
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task Delete(int id)
        {
            await _commentService.DeleteAsync(id);
        }

        [HttpPost("{commentId:int}/reply")]
        public async Task ReplyToComment(int commentId, CreateCommentDto model)
        {
            await _commentService.ReplyToCommentAsync(commentId, Mapper.Map<CommentModel>(model));
        }

        [HttpPut("{id:int}/increase-rating")]
        public async Task IncreaseRating(int id)
        {
            await _commentService.IncreaseRatingAsync(id);
        }

        [HttpPut("{id:int}/decrease-rating")]
        public async Task DecreaseRating(int id)
        {
            await _commentService.DecreaseRatingAsync(id);
        }
    }
}
