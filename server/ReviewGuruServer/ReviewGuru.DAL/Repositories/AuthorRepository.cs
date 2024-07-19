using ReviewGuru.DAL.Data;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories.IRepositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Repositories
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly ReviewGuruDbContext _context;
        private readonly ILogger _logger;
        public AuthorRepository(ReviewGuruDbContext context, ILogger logger) : base(context, logger)
        {
            _logger = logger;
            _context = context;
        }
    }
}
