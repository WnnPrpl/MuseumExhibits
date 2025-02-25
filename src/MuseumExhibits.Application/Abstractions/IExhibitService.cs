using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MuseumExhibits.Application.DTO;
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
        Task<IEnumerable<ExhibitDTO>> GetAll(bool isAdmin = true);
        Task<Guid> Create([FromForm] ExhibitDTO exhibitDto);
        Task Update(Guid id, ExhibitDTO exhibitDto);
        Task PartialUpdateAsync(Guid id, JsonPatchDocument<ExhibitDTO> patchDocument);
        Task Delete(Guid id);
        Task<IEnumerable<ExhibitDTO>> GetByCategoryId(Guid categoryId, bool isAdmin = true);
        Task<IEnumerable<ExhibitDTO>> GetByPage(int page, int pageSize, bool isAdmin = true);
    }
}
