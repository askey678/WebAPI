

using WebApi.BAL.Payloads;
using WebApi.BAL.ViewModels;

namespace WebApi.BAL.Services
{
    public interface IService
    {
        UserResponseDto Registeration(UserRegisterDto userdto);

        UserAndRoles Login(string email, string password);

        List<UserResponseDto> GetAllUsers();

        UserResponseDto GetUserById(string key);

        void DeleteUserById(string key);

        UserResponseDto UpdateUser(string key, UpdateDto user);

        RoleResponseDto AddNewRole(RoleRegisterDto roledto);

        RoleResponseDto GetRoleByKey(string key);

        List<RoleResponseDto> GetAllRoles();

        RoleResponseDto UpdateRole(string key, RoleRegisterDto role);

        void DeleteRoleById(string key);

        void TokenRegistration(TokenRegisterDto token);

        List<UserResponseDto> GetAllArchivedUsers();

        List<UserResponseDto> GetAllUnArchivedUsers();

        void RestoreArchivedUser(string key);

        void DeleteArchivedUser(string key);

        void DeleteUserPermanently(string key);

    }
}
