using AutoMapper;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
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
    public class AuthorService(IGenericRepository<AuthorDTO> genericRepository, IMapper mapper, IAuthorRepository authorRepository) : GenericService<AuthorDTO>(genericRepository, mapper), IAuthorService
    {
        private readonly IAuthorRepository _authorRepository = authorRepository;
        private readonly IMapper _mapper = mapper;
        public new async Task<AuthorDTO> CreateAsync(AuthorDTO authorDTO, CancellationToken cancellationToken = default)
        {
            var createdAuthor = await _authorRepository.AddAsync(_mapper.Map<Author>(authorDTO), cancellationToken: cancellationToken);

            return _mapper.Map<AuthorDTO>(createdAuthor);
        }
    }
}
