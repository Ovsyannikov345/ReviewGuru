using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Constants;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories;
using ReviewGuru.DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services
{
    public class ReviewService(IGenericRepository<Review> genericRepository, IMapper mapper) :  IReviewService
    {
        private readonly IGenericRepository<Review> _genericRepository = genericRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<IEnumerable<ReviewDTO>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var entities = await _genericRepository.GetAllAsync(pageNumber, pageSize, cancellationToken: cancellationToken);

            var filteredEntities = entities.Where(x => x.DateOfDeleting == null).ToList();

            return _mapper.Map<IEnumerable<ReviewDTO>>(entities);
        }

        public async Task<ReviewDTO> CreateAsync(ReviewDTO dto, CancellationToken cancellationToken = default)
        {
            var createdEntity = await _genericRepository.AddAsync(_mapper.Map<Review>(dto), cancellationToken: cancellationToken);
            return _mapper.Map<ReviewDTO>(createdEntity);
        }

        public async Task<ReviewDTO> UpdateAsync(ReviewDTO dto, CancellationToken cancellationToken = default)
        {
            var entityToUpdate = _mapper.Map<Review>(dto);

            return _mapper.Map<ReviewDTO>(await _genericRepository.UpdateAsync(entityToUpdate, cancellationToken: cancellationToken));
        }

        public async Task<ReviewDTO> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {

            var entityToDelete = _mapper.Map<Review>(id);


            return _mapper.Map<ReviewDTO>(await _genericRepository.DeleteAsync(entityToDelete.ReviewId, cancellationToken: cancellationToken));
        }
    }
}

