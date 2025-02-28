using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MuseumExhibits.Application.DTO;
using MuseumExhibits.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseumExhibits.Application.Abstractions
{
    public interface IExhibitService
    {
        Task<ExhibitDTO> GetById(Guid id);
        Task<PagedResult<ExhibitDTO>> Get(ExhibitQueryParameters queryParams, bool isAdmin = false);
        Task<Guid> Create([FromForm] ExhibitDTO exhibitDto);
        Task Update(Guid id, ExhibitDTO exhibitDto);
        Task PartialUpdate(Guid id, JsonPatchDocument<ExhibitDTO> patchDocument);
        Task Delete(Guid id);
        
    }
}
