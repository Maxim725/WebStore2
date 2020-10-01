﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.DTO.Identity;
using WebStore.Domain.Entities.Identity;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Identity.Users)]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly UserStore<User, Role, WebStoreDB> _userStore;
        public UsersApiController(WebStoreDB db)
        {
            _userStore = new UserStore<User, Role, WebStoreDB>(db);
        }

        [HttpGet("all")]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userStore
                .Users // Запрос в БД на выборкуц всех пользователей
                .ToArrayAsync(); // асинхронный перевод пользователей в массив
        }

        #region User

        [HttpPost("UserId")] // POST: api/users/UserId
        public async Task<string> GetUserIdAsync([FromBody] User user) // Такой подход позволяет передавать данные в теле запроса, а не в строке запроса, более безопасный способ даже при http соединении
            => await _userStore.GetUserIdAsync(user);

        [HttpPost("UserName")]
        public async Task<string> GetUserNameAsync([FromBody] User user)
            => await _userStore.GetUserNameAsync(user);

        [HttpPost("UserName/{name}")] // api/users/UserName/{что-то сюда}
        public async Task SetUserNameAsync([FromBody] User user, string name)
        {
            await _userStore.SetUserNameAsync(user, name);
            await _userStore.UpdateAsync(user); // Сохранение изменений в БД
        }

        [HttpPost("NormalUserName")]
        public async Task<string> GetNormalizedUserName([FromBody] User user)
            => await _userStore.GetNormalizedUserNameAsync(user); // Нормализация здесь - это приведение к верхнему регистру.

        [HttpPost("NormalUserName/{name}")]
        public async Task SetNormalizedUserName([FromBody] User user, string name)
        {
            await _userStore.SetNormalizedUserNameAsync(user, name);
            await _userStore.UpdateAsync(user);
        }

        [HttpPost("User")] // api/users/User
        public async Task<bool> CreateAsync([FromBody] User user)
        {
            var creationResult = await _userStore.CreateAsync(user);
            // добавление ошибок создания нового пользователя в журнал
            return creationResult.Succeeded;
        }

        [HttpPut("User")]
        public async Task<bool> UpdateAsync([FromBody] User user)
        {
            var updateResult = await _userStore.UpdateAsync(user);
            // будет логирования
            return updateResult.Succeeded;
        }

        [HttpPost("User/Delete")]
        public async Task<bool> DeleteAsync([FromBody] User user)
        {
            var deleteResult = await _userStore.DeleteAsync(user);
            // будет логирование
            return deleteResult.Succeeded;
        }

        [HttpGet("User/Find/{id}")] // api/users/user/Find/9E5CB5E7-41DE-4449-829E-45F4C97AA54B
        public async Task<User> FindByIdAsync(string id) 
            => await _userStore.FindByIdAsync(id);

        [HttpGet("User/Normal/{name}")] // api/users/user/Normal/TestUser
        public async Task<User> FindByNameAsync(string name) 
            => await _userStore.FindByNameAsync(name);

        [HttpPost("Role/{role}")]
        public async Task AddToRoleAsync([FromBody] User user, string role, [FromServices] WebStoreDB db)
        {
            await _userStore.AddToRoleAsync(user, role);
            await db.SaveChangesAsync();
        }

        [HttpPost("Role/Delete/{role}")]
        public async Task RemoveFromRoleAsync([FromBody] User user, string role, [FromServices] WebStoreDB db)
        {
            await _userStore.RemoveFromRoleAsync(user, role);
            await db.SaveChangesAsync();
        }

        [HttpPost("Roles")]
        public async Task<IList<string>> GetRolesAsync([FromBody] User user) 
            => await _userStore.GetRolesAsync(user);

        [HttpPost("InRole/{role}")]
        public async Task<bool> IsInRoleAsync([FromBody] User user, string role) 
            => await _userStore.IsInRoleAsync(user, role);

        [HttpGet("UsersInRole/{role}")]
        public async Task<IList<User>> GetUsersInRoleAsync(string role) 
            => await _userStore.GetUsersInRoleAsync(role);

        [HttpPost("GetPasswordHash")]
        public async Task<string> GetPasswordHashAsync([FromBody] User user) 
            => await _userStore.GetPasswordHashAsync(user);

        [HttpPost("SetPasswordHash")]
        public async Task<string> SetPasswordHashAsync([FromBody] PasswordHashDTO hash)
        {
            await _userStore.SetPasswordHashAsync(hash.User, hash.Hash);
            await _userStore.UpdateAsync(hash.User);
            return hash.User.PasswordHash;
        }

        [HttpPost("HasPassword")]
        public async Task<bool> HasPasswordAsync([FromBody] User user) 
            => await _userStore.HasPasswordAsync(user);

        #endregion

        #region Claims

        [HttpPost("GetClaims")]
        public async Task<IList<Claim>> GetClaimsAsync([FromBody] User user) => await _userStore.GetClaimsAsync(user);

        [HttpPost("AddClaims")]
        public async Task AddClaimsAsync([FromBody] AddClaimDTO ClaimInfo, [FromServices] WebStoreDB db)
        {
            await _userStore.AddClaimsAsync(ClaimInfo.User, ClaimInfo.Claims);
            await db.SaveChangesAsync();
        }

        [HttpPost("ReplaceClaim")]
        public async Task ReplaceClaimAsync([FromBody] ReplaceClaimDTO ClaimInfo, [FromServices] WebStoreDB db)
        {
            await _userStore.ReplaceClaimAsync(ClaimInfo.User, ClaimInfo.Claim, ClaimInfo.NewClaim);
            await db.SaveChangesAsync();
        }

        [HttpPost("RemoveClaim")]
        public async Task RemoveClaimsAsync([FromBody] RemoveClaimDTO ClaimInfo, [FromServices] WebStoreDB db)
        {
            await _userStore.RemoveClaimsAsync(ClaimInfo.User, ClaimInfo.Claims);
            await db.SaveChangesAsync();
        }

        [HttpPost("GetUsersForClaim")]
        public async Task<IList<User>> GetUsersForClaimAsync([FromBody] Claim claim) =>
            await _userStore.GetUsersForClaimAsync(claim);

        #endregion

        #region TwoFactor

        [HttpPost("GetTwoFactorEnabled")]
        public async Task<bool> GetTwoFactorEnabledAsync([FromBody] User user) => await _userStore.GetTwoFactorEnabledAsync(user);

        [HttpPost("SetTwoFactor/{enable}")]
        public async Task SetTwoFactorEnabledAsync([FromBody] User user, bool enable)
        {
            await _userStore.SetTwoFactorEnabledAsync(user, enable);
            await _userStore.UpdateAsync(user);
        }

        #endregion

        #region Email/Phone

        [HttpPost("GetEmail")]
        public async Task<string> GetEmailAsync([FromBody] User user) => await _userStore.GetEmailAsync(user);

        [HttpPost("SetEmail/{email}")]
        public async Task SetEmailAsync([FromBody] User user, string email)
        {
            await _userStore.SetEmailAsync(user, email);
            await _userStore.UpdateAsync(user);
        }

        [HttpPost("GetEmailConfirmed")]
        public async Task<bool> GetEmailConfirmedAsync([FromBody] User user) => await _userStore.GetEmailConfirmedAsync(user);

        [HttpPost("SetEmailConfirmed/{enable}")]
        public async Task SetEmailConfirmedAsync([FromBody] User user, bool enable)
        {
            await _userStore.SetEmailConfirmedAsync(user, enable);
            await _userStore.UpdateAsync(user);
        }

        [HttpGet("UserFindByEmail/{email}")]
        public async Task<User> FindByEmailAsync(string email) => await _userStore.FindByEmailAsync(email);

        [HttpPost("GetNormalizedEmail")]
        public async Task<string> GetNormalizedEmailAsync([FromBody] User user) => await _userStore.GetNormalizedEmailAsync(user);

        [HttpPost("SetNormalizedEmail/{email?}")]
        public async Task SetNormalizedEmailAsync([FromBody] User user, string email)
        {
            await _userStore.SetNormalizedEmailAsync(user, email);
            await _userStore.UpdateAsync(user);
        }

        [HttpPost("GetPhoneNumber")]
        public async Task<string> GetPhoneNumberAsync([FromBody] User user) => await _userStore.GetPhoneNumberAsync(user);

        [HttpPost("SetPhoneNumber/{phone}")]
        public async Task SetPhoneNumberAsync([FromBody] User user, string phone)
        {
            await _userStore.SetPhoneNumberAsync(user, phone);
            await _userStore.UpdateAsync(user);
        }

        [HttpPost("GetPhoneNumberConfirmed")]
        public async Task<bool> GetPhoneNumberConfirmedAsync([FromBody] User user) =>
            await _userStore.GetPhoneNumberConfirmedAsync(user);

        [HttpPost("SetPhoneNumberConfirmed/{confirmed}")]
        public async Task SetPhoneNumberConfirmedAsync([FromBody] User user, bool confirmed)
        {
            await _userStore.SetPhoneNumberConfirmedAsync(user, confirmed);
            await _userStore.UpdateAsync(user);
        }

        #endregion

        #region Login/Lockout

        [HttpPost("AddLogin")]
        public async Task AddLoginAsync([FromBody] AddLoginDTO login, [FromServices] WebStoreDB db)
        {
            await _userStore.AddLoginAsync(login.User, login.UserLoginInfo);
            await db.SaveChangesAsync();
        }

        [HttpPost("RemoveLogin/{LoginProvider}/{ProviderKey}")]
        public async Task RemoveLoginAsync([FromBody] User user, string LoginProvider, string ProviderKey, [FromServices] WebStoreDB db)
        {
            await _userStore.RemoveLoginAsync(user, LoginProvider, ProviderKey);
            await db.SaveChangesAsync();
        }

        [HttpPost("GetLogins")]
        public async Task<IList<UserLoginInfo>> GetLoginsAsync([FromBody] User user) => await _userStore.GetLoginsAsync(user);

        [HttpGet("User/FindByLogin/{LoginProvider}/{ProviderKey}")]
        public async Task<User> FindByLoginAsync(string LoginProvider, string ProviderKey) => await _userStore.FindByLoginAsync(LoginProvider, ProviderKey);

        [HttpPost("GetLockoutEndDate")]
        public async Task<DateTimeOffset?> GetLockoutEndDateAsync([FromBody] User user) => await _userStore.GetLockoutEndDateAsync(user);

        [HttpPost("SetLockoutEndDate")]
        public async Task SetLockoutEndDateAsync([FromBody] SetLockoutDTO LockoutInfo)
        {
            await _userStore.SetLockoutEndDateAsync(LockoutInfo.User, LockoutInfo.LockoutEnd);
            await _userStore.UpdateAsync(LockoutInfo.User);
        }

        [HttpPost("IncrementAccessFailedCount")]
        public async Task<int> IncrementAccessFailedCountAsync([FromBody] User user)
        {
            var count = await _userStore.IncrementAccessFailedCountAsync(user);
            await _userStore.UpdateAsync(user);
            return count;
        }

        [HttpPost("ResetAccessFailedCount")]
        public async Task ResetAccessFailedCountAsync([FromBody] User user)
        {
            await _userStore.ResetAccessFailedCountAsync(user);
            await _userStore.UpdateAsync(user);
        }

        [HttpPost("GetAccessFailedCount")]
        public async Task<int> GetAccessFailedCountAsync([FromBody] User user) => await _userStore.GetAccessFailedCountAsync(user);


        // Пермач
        [HttpPost("GetLockoutEnabled")]
        public async Task<bool> GetLockoutEnabledAsync([FromBody] User user) => await _userStore.GetLockoutEnabledAsync(user);

        [HttpPost("SetLockoutEnabled/{enable}")]
        public async Task SetLockoutEnabledAsync([FromBody] User user, bool enable)
        {
            await _userStore.SetLockoutEnabledAsync(user, enable);
            await _userStore.UpdateAsync(user);
        }

        #endregion
    }
}