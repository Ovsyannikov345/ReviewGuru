using AutoMapper;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services
{
    public class MediaService(IGenericRepository<Media> genericRepository, IMapper mapper) :  IMediaService
    {
        private readonly IGenericRepository<Media> _genericRepository = genericRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<MediaDTO>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var entity = await _genericRepository.GetAllAsync(pageNumber, pageSize, cancellationToken: cancellationToken);

            return _mapper.Map<IEnumerable<MediaDTO>>(entity);
        }

        public async Task<MediaDTO> CreateAsync(MediaDTO dto, CancellationToken cancellationToken = default)
        {
            var createdEntity = await _genericRepository.AddAsync(_mapper.Map<Media>(dto), cancellationToken: cancellationToken);
            return _mapper.Map<MediaDTO>(createdEntity);
        }
    }
}

 