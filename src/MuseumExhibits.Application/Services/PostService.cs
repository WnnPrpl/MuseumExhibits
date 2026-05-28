using AutoMapper;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;
using MuseumExhibits.Core.Abstractions;
using MuseumExhibits.Core.Filters;
using MuseumExhibits.Core.Models;

namespace MuseumExhibits.Application.Services
{
    public class PostService(IPostRepository postRepository, IMapper mapper) : IPostService
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<PostResponse> GetById(Guid id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            return _mapper.Map<PostResponse>(post);
        }

        public async Task<PagedResult<PostResponse>> Get(PostQueryParameters queryParams, bool isAdmin = false)
        {
            var filter = _mapper.Map<PostFilter>(queryParams);
            var (posts, totalCount) = await _postRepository.GetAsync(filter, isAdmin);
            var dtos = _mapper.Map<IEnumerable<PostResponse>>(posts);
            return new PagedResult<PostResponse>(dtos, totalCount, queryParams.PageNumber, queryParams.PageSize);
        }

        public async Task<Guid> Create(PostRequest request)
        {
            var post = _mapper.Map<Post>(request);
            await _postRepository.CreateAsync(post);
            return post.Id;
        }

        public async Task Update(Guid id, PostRequest request)
        {
            var post = await _postRepository.GetByIdAsync(id);
            _mapper.Map(request, post);
            await _postRepository.UpdateAsync(post);
        }

        public async Task Delete(Guid id)
        {
            await _postRepository.DeleteAsync(id);
        }
    }
}
