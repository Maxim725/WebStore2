﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities.Identity;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Identity.Roles)]
    [ApiController]
    public class RolesApiController : ControllerBase
    {
        private readonly RoleStore<Role> _roleStore;

        public RolesApiController(WebStoreDB db)
        {
            _roleStore = new RoleStore<Role>(db);
        }

        [HttpGet("all")]
        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _roleStore
                .Roles // Запрос в БД на выборкуц всех пользователей
                .ToArrayAsync();
        }
    }
}
