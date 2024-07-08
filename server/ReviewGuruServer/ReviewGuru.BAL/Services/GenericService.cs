using AutoMapper;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Exceptions;
using ReviewGuru.DAL.Repositories;
using ReviewGuru.DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services
{
    public class GenericService<TDTO>(
        IGenericRepository<TDTO> genericRepository, 
        IMapper mapper
        ) :  IGenericService<TDTO> where TDTO : class, IEntity, new ()
    {
        private readonly IGenericRepository<TDTO> _genericRepository = genericRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<List<TDTO>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var entity = await _genericRepository.GetListAsync(pageNumber, pageSize, cancellationToken: cancellationToken);

            return _mapper.Map<List<TDTO>>(entity);
        }

        public async Task<TDTO> CreateAsync(TDTO dto, CancellationToken cancellationToken = default)
        {
            var createdEntity = await _genericRepository.AddAsync(dto, cancellationToken: cancellationToken);
            return _mapper.Map<TDTO>(createdEntity);
        }

        public async Task<TDTO> UpdateAsync(TDTO dto, CancellationToken cancellationToken = default)
        {
            var entity = await _genericRepository.GetAsync(x => x.Id == dto.Id, cancellationToken: cancellationToken) ?? throw new NotFoundException("Entity not found");
            var updatedEntity = await _genericRepository.UpdateAsync(dto, cancellationToken: cancellationToken);
            return updatedEntity;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entityToDelete = await _genericRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken) ?? throw new NotFoundException("Entity not found");
            await _genericRepository.DeleteAsync(entityToDelete, cancellationToken: cancellationToken);
        }
    }
}
