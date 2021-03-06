﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using DatabaseAccess.Repository;
using UsersAPI.Models;
using System.Linq;
using System.Security.Claims;
using Cryptography;
using System.Threading.Tasks;
using System;

namespace UsersAPI.Controllers
{
    [Route("api/admins")]
    [Authorize(Policy = "Admin")]
    public class AdminsController : Controller
    {
        private readonly Repo<AdminInfo> repo;
        private readonly Repo<AdminPublicInfo> publicRepo;
        private readonly Repo<UserPublicInfo> userRepo;

        public AdminsController(Repo<AdminInfo> repo, Repo<AdminPublicInfo> publicRepo, Repo<UserPublicInfo> userRepo)
        {
            this.repo = repo;
            this.publicRepo = publicRepo;
            this.userRepo = userRepo;
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Get()
        {
            var result = await this.publicRepo.ExecuteOperationAsync("GetAllAdmins");

            if (result == null)
                return new StatusCodeResult(204);

            return Ok(result);

        }

        [HttpGet("{id}", Name = "GetAdminById")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetAdminById(int id)
        {
            var res = await this.publicRepo.ExecuteOperationAsync("GetAdmin", new[] { new KeyValuePair<string, object>("id", id) });
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }
        [HttpGet("login/{login}", Name = "GetAdminByLogin")]

        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetAdminByLogin(string login)
        {
            var res = await this.publicRepo.ExecuteOperationAsync("GetAdminByName", new[] { new KeyValuePair<string, object>("login", login) });
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Post([FromBody]AdminInfo admin)
        {
            var userName = GetCurrentUserName();
            if (userName == "Admin888") //our super admin 
            {
                if ((int)this.userRepo.ExecuteOperation("ExistsLogin", new[] { new KeyValuePair<string, object>("login", admin.Login) }) == 1)
                {
                    throw new System.Exception("Username already exists");
                }
                var res = await this.repo.ExecuteOperationAsync("CreateAdmin", new[] { new KeyValuePair<string, object>("name", admin.Name), new KeyValuePair<string, object>("email", admin.Email), new KeyValuePair<string, object>("login", admin.Login), new KeyValuePair<string, object>("password", MyCryptography.Encrypt(admin.Password)) });
                return Ok(res);
            }
            return NotFound();
        }

        [HttpPut]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody]AdminInfo admin)
        {
            var userName = GetCurrentUserName();
            if (userName == "Admin888")
            {
                await this.repo.ExecuteOperationAsync("UpdateAdmin", new[] { new KeyValuePair<string, object>("id", id), new KeyValuePair<string, object>("name", admin.Name ?? DBNull.Value.ToString()), new KeyValuePair<string, object>("email", admin.Email?? DBNull.Value.ToString()), new KeyValuePair<string, object>("password",MyCryptography.Encrypt( admin.Password) ?? DBNull.Value.ToString()) });
                return Ok(await this.repo.ExecuteOperationAsync("GetAdmin", new[] { new KeyValuePair<string, object>("id", id) }));
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var userName = GetCurrentUserName();
            if (userName == "Admin888")
            {
                await this.repo.ExecuteOperationAsync("DeleteAdmin", new[] { new KeyValuePair<string, object>("id", id) });
                return new StatusCodeResult(200);
            }
            return NotFound();
        }
        public int GetCurrentUserId()
        {
            return int.Parse(((ClaimsIdentity)this.User.Identity).Claims.Where(claim => claim.Type == "user_id").First().Value);
        }
        public string GetCurrentUserName()
        {
            return ((ClaimsIdentity)this.User.Identity).Claims.Where(claim => claim.Type == "name").First().Value.ToString();
        }
    }
}