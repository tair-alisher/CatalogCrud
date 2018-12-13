using CatalogCrud.BLL.DTO;
using CatalogCrud.BLL.Infrastructure;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CatalogCrud.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDTO);
        Task<ClaimsIdentity> Authenticate(UserDTO userDTO);
        Task<OperationDetails> ChangeEmail(UserDTO modelDTO);
        Task<OperationDetails> ChangePassword(ChangePasswordDTO modelDTO);
        Task<string> GetUserEmail(string id);
    }
}
