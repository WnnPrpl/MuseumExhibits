using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MuseumExhibits.Application.DTO;
using MuseumExhibits.Core.Filters;

namespace MuseumExhibits.Application.Abstractions
{
    public interface IExhibitService
    {
        Task<ExhibitResponse> GetById(Guid id);
        Task<PagedResult<ExhibitSummaryDTO>> Get(ExhibitQueryParameters queryParams, bool isAdmin = false);
        Task<Guid> Create([FromForm] ExhibitRequest exhibitDto);
        Task Update(Guid id, ExhibitRequest exhibitDto);
        Task PartialUpdate(Guid id, JsonPatchDocument<ExhibitRequest> patchDocument);
        Task Delete(Guid id);
        
    }
}
