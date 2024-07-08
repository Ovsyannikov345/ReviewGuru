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
    public class ReviewService(IGenericRepository<ReviewDTO> genericRepository, IMapper mapper) : GenericService<ReviewDTO>(genericRepository, mapper), IReviewService
    {
        private readonly IGenericRepository<ReviewDTO> _genericRepository = genericRepository;
        public new async Task<List<ReviewDTO>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var entities = await _genericRepository.GetListAsync(pageNumber, pageSize, cancellationToken: cancellationToken);

            var filteredEntities = entities.Where(x => x.DateOfDeleting == null).ToList();

            return filteredEntities;
        }
    }
}
