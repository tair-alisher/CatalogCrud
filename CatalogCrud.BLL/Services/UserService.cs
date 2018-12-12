using CatalogCrud.BLL.DTO;
using CatalogCrud.BLL.Infrastructure;
using CatalogCrud.BLL.Interfaces;
using CatalogCrud.DAL.Entities;
using CatalogCrud.DAL.Intefaces;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CatalogCrud.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _worker;

        public UserService(IUnitOfWork uow)
        {
            _worker = uow;
        }

        public async Task<OperationDetails> Create(UserDTO userDTO)
        {
            ApplicationUser user = await _worker.UserManager.FindByNameAsync(userDTO.UserName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = userDTO.UserName, Email = userDTO.Email };
                var result = await _worker.UserManager.CreateAsync(user, userDTO.Password);
                if (result.Errors.Count() > 0)
                {
                    if (result.Errors.Any(x => x.Contains("Password")))
                        return new OperationDetails(false, "Ненадежный пароль.", "Password");

                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }

                await _worker.UserManager.AddToRoleAsync(user.Id, userDTO.Role);
                await _worker.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена.", "");
            }
            else
                return new OperationDetails(false, "Пользователь с таким логином уже существует.", "UserName");
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDTO)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = await _worker.UserManager.FindAsync(userDTO.UserName, userDTO.Password);
            if (user != null)
                claim = await _worker.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public async Task<OperationDetails> ChangeEmail(UserDTO modelDTO)
        {
            var user = await _worker.UserManager.FindByIdAsync(modelDTO.Id);
            user.Email = modelDTO.Email;
            await _worker.UserManager.UpdateAsync(user);
            await _worker.SaveAsync();

            return new OperationDetails(true, "Почта успешно изменена.", "");
        }

        public async Task<OperationDetails> ChangePassword(ChangePasswordDTO modelDTO)
        {
            ApplicationUser user = await _worker.UserManager.FindByIdAsync(modelDTO.UserId);
            var oldPasswordConfirm = await _worker.UserManager.CheckPasswordAsync(user, modelDTO.OldPassword);
            if (!oldPasswordConfirm)
                return new OperationDetails(false, "Старый пароль неверен.", "OldPassword");

            IdentityResult result = await _worker.UserManager.ChangePasswordAsync(
                modelDTO.UserId,
                modelDTO.OldPassword,
                modelDTO.NewPassword);

            if (result.Errors.Count() > 0)
                return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

            await _worker.SaveAsync();
            return new OperationDetails(true, "Пароль успешно изменен.", "");
        }

        public void Dispose()
        {
            _worker.Dispose();
        }

        public async Task<string> GetUserEmail(string id)
        {
            var user = await _worker.UserManager.FindByIdAsync(id);
            return user.Email;
        }
    }
}
