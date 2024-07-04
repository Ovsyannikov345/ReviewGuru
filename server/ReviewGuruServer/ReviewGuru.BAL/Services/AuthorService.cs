﻿using ReviewGuru.BLL.DTOs;
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
    public class AuthorService(IGenericRepository<AuthorDTO> genericRepository) : GenericService<AuthorDTO>(genericRepository), IAuthorService
    {
    }
}