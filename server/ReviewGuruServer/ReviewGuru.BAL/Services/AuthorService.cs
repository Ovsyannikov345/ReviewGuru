using AutoMapper;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Constants;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories;
using ReviewGuru.DAL.Repositories.IRepositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services
{
    public class AuthorService(IAuthorRepository authorRepository, ILogger logger) : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository = authorRepository;

        private readonly ILogger _logger = logger;

        public async Task<IEnumerable<Author>> GetAuthorListAsync(
            int pageNumber = Pagination.PageNumber,
            int pageSize = Pagination.MaxPageSize,
            CancellationToken cancellationToken = default)
        {
            _logger.Information("Page {PageNumber} of authors has been returned", pageNumber);

            return await _authorRepository.GetAllAsync(pageNumber, pageSize, cancellationToken: cancellationToken);
        }
    }
}
