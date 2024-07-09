using AutoMapper;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Exceptions;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories;
using ReviewGuru.DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services
{
    public class GenericService<TDTO, TEntity>(
        IGenericRepository<TEntity> genericRepository, 
        IMapper mapper
        ) :  IGenericService<TDTO, TEntity> 
        where TDTO : class, new ()
        where TEntity : class, new()

    {
        private readonly IGenericRepository<TEntity> _genericRepository = genericRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<List<TDTO>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var entity = await _genericRepository.GetListAsync(pageNumber, pageSize, cancellationToken: cancellationToken);

            return _mapper.Map<List<TDTO>>(entity);
        }

        public async Task<TDTO> CreateAsync(TDTO dto, CancellationToken cancellationToken = default)
        {
            var createdEntity = await _genericRepository.AddAsync(_mapper.Map<TEntity>(dto), cancellationToken: cancellationToken);
            return _mapper.Map<TDTO>(createdEntity);
        }

        public async Task<TDTO> UpdateAsync(TDTO dto, CancellationToken cancellationToken = default)
        {
            //var entity = await _genericRepository.GetAsync(x => x.Id == dto.Id, cancellationToken: cancellationToken) ?? throw new NotFoundException("Entity not found");

            var entityToUpdate = _mapper.Map<TEntity>(dto);

            return _mapper.Map<TDTO>(await _genericRepository.UpdateAsync(entityToUpdate, cancellationToken: cancellationToken));
        }

        public async Task<TDTO> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {

            var entityToDelete = _mapper.Map<TEntity>(id);


            return _mapper.Map<TDTO>(await _genericRepository.DeleteAsync(entityToDelete, cancellationToken: cancellationToken));
        }
    }
}
