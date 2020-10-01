using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Interfaces.Services
{
    public interface IUsersClient :
        // Позволяет управлять ролями пользователя
        IUserRoleStore<User>,
        // Хранилище пароля
        IUserPasswordStore<User>,
        // Хранилище почты
        IUserEmailStore<User>,
        // Хранилище телефона
        IUserPhoneNumberStore<User>,
        // Хранилище фактов двухфакторной авторизации
        IUserTwoFactorStore<User>,
        // Хранилище Клеймов (утверждений)
        IUserClaimStore<User>,
        // Хранилище фактов входа пользователя
        IUserLoginStore<User>
    {
    }
}
