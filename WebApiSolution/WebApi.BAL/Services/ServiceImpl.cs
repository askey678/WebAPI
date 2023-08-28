using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Api.Helpers;
using WebApi.BAL.Payloads;
using WebApi.BAL.ViewModels;
using WebApi.DAL;
using WebApi.DAL.ExceptionHandler;
using WebApi.DAL.Model;

namespace WebApi.BAL.Services
{
    public class ServiceImpl : IService
    {

       

        public List<UserResponseDto> GetAllUsers()
        {
            using (var context = new WebDbContext())
            {
                List<User> userlist = context.Users.Where(e => e.IsActive).ToList();
                List<UserResponseDto> userRespList = userlist.Select(u => new UserResponseDto
                {
                    UserKey = u.UserKey,
                    Name = u.UserName,
                    Email = u.Email,
                    About = u.About,
                    Roles = listOfRolesByUserKey(u.UserKey)
                }).ToList();
                return userRespList;

            }

        }

        public void DeleteUserById(string key)
        {
            using (var context = new WebDbContext())
            {

                User user = context.Users.Where(e => e.UserKey==key && e.IsArchived == false && e.IsActive).FirstOrDefault();
                if (user == null)
                {
                    throw new ResourceNotFoundException("User", "Id", "");
                }
                user.IsArchived = true;
                user.DateUpdated = DateTime.Now;
                context.SaveChanges();

            }
        }


        public UserResponseDto GetUserById(string key)
        {
            using (var context = new WebDbContext())
            {

                User user = context.Users.Where(a => a.UserKey == key && a.IsArchived == false && a.IsActive).FirstOrDefault();
                if (user != null)
                {
                    UserResponseDto userResp = userToUserResponse(user);
                    userResp.Roles=listOfRolesByUserKey(user.UserKey);
                    return userResp;
                }
                else
                {
                    throw new ResourceNotFoundException("User", "Id", key);
                }

            }

        }



        public UserResponseDto UpdateUser(string key, UpdateDto user)
        {
            using (var context = new WebDbContext())
            {

                User existingUser = context.Users.Where(a => a.UserKey == key && a.IsArchived==false && a.IsActive).FirstOrDefault();

                if (existingUser != null)
                {

                    if (user.Name != null)
                    {
                        existingUser.UserName=user.Name;
                    }
                     if (user.Email != null)
                    {
                        existingUser.Email=user.Email;
                    }
                     if (user.Password != null)
                    {
                        existingUser.Password=user.Password;
                    }
                     if (user.About != null)
                    {
                        existingUser.About=user.About;
                    }
                    existingUser.DateUpdated = DateTime.Now;
                    //context.Update(existingUser);
                    context.SaveChanges();
                    return userToUserResponse(existingUser);
                }
                else
                {
                    throw new ResourceNotFoundException("User", "Id", key);
                }

            }
        }

        public UserResponseDto Registeration(UserRegisterDto userdto)
        {
            using (var context = new WebDbContext())
            {

                var userExists = context.Users.Any(p => p.Email == userdto.Email);
                if (!userExists)
                {
                    // add new user
                    User user = dtoToUser(userdto);
                    user.Password=EncryptDecryptHelper.EncrpytStringBase64(userdto.Password);
                    user.UserKey = Guid.NewGuid().ToString();
                    user.DateCreated = DateTime.Now;
                    user.DateUpdated = DateTime.Now;
                    context.Users.Add(user);
                    context.SaveChanges();

                    // find the role using role name coming from the userRegiserDto from roles table
                    Role role = context.Roles.Where(a => a.RoleName == userdto.Role).First();
                    if (role == null)
                    {
                        throw new ResourceNotFoundException("Role", "Name", userdto.Role);
                    }


                    // fetching the newly added user again
                    UserResponseDto newUser = userToUserResponse(context.Users.Where(a => a.UserKey == user.UserKey).FirstOrDefault());

                    // updating the UserRole table with UserKey of user and RoleKey  
                    UserRole userRole = new UserRole();
                    userRole.UserRoleKey=Guid.NewGuid().ToString();
                    userRole.UserKey=newUser.UserKey;
                    userRole.RoleKey=role.RoleKey;
                    userRole.DateCreated = DateTime.Now;
                    userRole.DateUpdated = DateTime.Now;
                    context.Userroles.Add(userRole);
                    context.SaveChanges();

                    return newUser;


                }
                return null;
            }

        }

        public UserAndRoles Login(string Email, string Password)
        {
            using (var context = new WebDbContext())
            {

                User user = context.Users.Where(a => a.Email == Email && a.Password == Password && a.IsArchived==false && a.IsActive).FirstOrDefault();
                if (user != null)
                {
                    // finding the rolekeys for the respective userkey from the junction table UserRole
                    // a single user can have single or multiple role
                    List<UserRole> UserRoles = context.Userroles.Where(a => a.UserKey == user.UserKey).ToList();

                    List<string> RoleKeys = UserRoles.Select(u => u.RoleKey).ToList();

                    // fetching the all roles assigned to a logged user from the roles table using rolekey found from userRole table
                    List<Role> roles = context.Roles.Where(role => RoleKeys.Contains(role.RoleKey))
                                        .ToList();


                    List<string> AllRoleNames = roles.Select(r => r.RoleName).ToList();

                    UserAndRoles response = userToUserAndRoles(user);
                    response.Roles = AllRoleNames;

                    return response;


                }
                else
                {
                    throw new ResourceNotFoundException("User", "Id and password", $"{Email}, {Password}");
                }
            }

        }




        RoleResponseDto IService.AddNewRole(RoleRegisterDto roledto)
        {
            using (var context = new WebDbContext())
            {
                Role role = registerDtoToRole(roledto);
                context.Roles.Add(role);
                context.SaveChanges();

                Role newRole = context.Roles.Where(a => a.RoleName == roledto.RoleName).First();
                return roleToResponseDto(newRole);

            }
        }

        RoleResponseDto IService.GetRoleByKey(string key)
        {
            using (var context = new WebDbContext())
            {
                Role newRole = context.Roles.Where(a => a.RoleKey == key).FirstOrDefault();
                if (newRole == null)
                {
                    throw new ResourceNotFoundException("Role", "key", key);
                }
                return roleToResponseDto(newRole);
            }
        }

        List<RoleResponseDto> IService.GetAllRoles()
        {
            using (var context = new WebDbContext())
            {
                List<Role> roleList = context.Roles.ToList();
                List<RoleResponseDto> roleResponseList = roleList.Select(u => new RoleResponseDto
                {
                    // Set properties of UserDto based on User object
                    RoleKey = u.RoleKey,
                    RoleName = u.RoleName

                }).ToList();
                return roleResponseList;

            }
        }

        RoleResponseDto IService.UpdateRole(string key, RoleRegisterDto role)
        {
            using (var context = new WebDbContext())
            {
                Role existingrole = context.Roles.FirstOrDefault(a => a.RoleKey == key);
                if (existingrole != null)
                {
                    if (role.RoleName != null)
                    {
                        existingrole.RoleName= role.RoleName;
                        existingrole.DateUpdated = DateTime.Now;
                    }
                    context.SaveChanges();
                    return roleToResponseDto((Role)existingrole);
                }
                else
                {
                    throw new ResourceNotFoundException("Role", "Key", key);
                }


            }
        }

        void IService.DeleteRoleById(string key)
        {
            using (var context = new WebDbContext())
            {
                Role role = context.Roles.Where(a=>a.RoleKey==key).FirstOrDefault();
                if (role == null)
                {
                    throw new ResourceNotFoundException("Role", "Key", key);
                }
                role.DateUpdated=DateTime.Now;
                role.IsArchived=true;
                context.Remove(role);
                context.SaveChanges();
            }
        }
        void IService.TokenRegistration(TokenRegisterDto token)
        {
            using (var context = new WebDbContext())
            {
                UserToken tokenDetails = new UserToken();

                tokenDetails.TokenKey=Guid.NewGuid().ToString();
                tokenDetails.UserKey=token.UserKey;
                tokenDetails.TokenHash=token.TokenHash;
                tokenDetails.TokenType=token.TokenType;
                tokenDetails.DateCreated=DateTime.Now;
                tokenDetails.DateUpdated=DateTime.Now;

                context.Usertokens.Add(tokenDetails);
                context.SaveChanges();
            }
        }

        List<UserResponseDto> IService.GetAllArchivedUsers()
        {
            using (var context = new WebDbContext())
            {
                List<User> userlist = context.Users.Where(e => e.IsActive && e.IsArchived).ToList();
                List<UserResponseDto> userRespList = userlist.Select(u => new UserResponseDto
                {
                    UserKey = u.UserKey,
                    Name = u.UserName,
                    Email = u.Email,
                    About = u.About,
                    Roles = listOfRolesByUserKey(u.UserKey)
                }).ToList();
                return userRespList;

            }
        }

        List<UserResponseDto> IService.GetAllUnArchivedUsers()
        {
            using (var context = new WebDbContext())
            {
                List<User> userlist = context.Users.Where(e => e.IsActive && !e.IsArchived).ToList();
                List<UserResponseDto> userRespList = userlist.Select(u => new UserResponseDto
                {
                    UserKey = u.UserKey,
                    Name = u.UserName,
                    Email = u.Email,
                    About = u.About,
                    Roles=listOfRolesByUserKey(u.UserKey)
                }).ToList();
                return userRespList;

            }
        }

        void IService.RestoreArchivedUser(string key)
        {
            using (var context = new WebDbContext())
            {
                User archivedUser = context.Users.Where(e => e.UserKey == key && e.IsActive
                && e.IsArchived == true).FirstOrDefault();

                if (archivedUser != null)
                {
                    archivedUser.IsArchived = false;
                    context.SaveChanges();
                }
                else
                {
                    throw new ResourceNotFoundException("User", "Key", key);
                }
            }
        }

        void IService.DeleteUserPermanently(string key)
        {
            using (var context = new WebDbContext())
            {
                User archivedUser = context.Users.Where(e => e.UserKey == key && e.IsActive)
                    .FirstOrDefault();

                if (archivedUser != null)
                {
                    archivedUser.IsActive = false;
                    context.SaveChanges();
                }
                else
                {
                    throw new ResourceNotFoundException("User", "Key", key);
                }

            }
        }

        void IService.DeleteArchivedUser(string key)
        {
            using (var context = new WebDbContext())
            {
                User archivedUser = context.Users.Where(e => e.UserKey == key && e.IsActive)
                    .FirstOrDefault();

                if (archivedUser != null)
                {
                    archivedUser.IsActive = false;
                    context.SaveChanges();
                }
                else
                {
                    throw new ResourceNotFoundException("User", "Key", key);
                }

            }
        }


        public UserRegisterDto userToDto(User user)
        {
            UserRegisterDto userdto = new UserRegisterDto();
            userdto.Name=user.UserName;
            userdto.Email=user.Email;
            userdto.Password=user.Password;
            userdto.About=user.About;


            return userdto;
        }

        public User dtoToUser(UserRegisterDto userdto)
        {
            User user = new User();
            user.UserName = userdto.Name;
            user.Email = userdto.Email;
            user.Password=userdto.Password;
            user.About = userdto.About;

            return user;

        }

        public UserResponseDto userToUserResponse(User user)
        {
            UserResponseDto userResp = new UserResponseDto();
            userResp.UserKey = user.UserKey;
            userResp.Name=user.UserName;
            userResp.Email=user.Email;
            userResp.About=user.About;


            return userResp;
        }

        public User responseToUser(UserResponseDto userResp)
        {
            User user = new User();
            user.UserKey = userResp.UserKey;
            user.UserName = userResp.Name;
            user.Email = userResp.Email;
            user.About = userResp.About;

            return user;

        }

        public Role registerDtoToRole(RoleRegisterDto roleRegisterDto)
        {
            Role role = new Role();
            role.RoleKey= Guid.NewGuid().ToString();
            role.RoleName=roleRegisterDto.RoleName;
            role.DateCreated = DateTime.Now;
            role.DateUpdated = DateTime.Now;
            return role;

        }

        public RoleResponseDto roleToResponseDto(Role role)
        {
            RoleResponseDto resp = new RoleResponseDto();

            resp.RoleKey = role.RoleKey;
            resp.RoleName = role.RoleName;

            return resp;
        }

        public UserAndRoles userToUserAndRoles(User user)
        {
            UserAndRoles userResp = new UserAndRoles();
            userResp.UserKey = user.UserKey;
            userResp.Name=user.UserName;
            userResp.Email=user.Email;
            userResp.About=user.About;

            return userResp;
        }

        public List<string> listOfRolesByUserKey(string key)
        {

            using (var context = new WebDbContext())
            {
                List<UserRole> UserRoles = context.Userroles.Where(a => a.UserKey == key).ToList();

                List<string> RoleKeys = UserRoles.Select(u => u.RoleKey).ToList();

                // fetching the all roles assigned to a logged user from the roles table using rolekey found from userRole table
                List<Role> roles = context.Roles.Where(role => RoleKeys.Contains(role.RoleKey))
                                    .ToList();


                List<string> AllRoleNames = roles.Select(r => r.RoleName).ToList();
                return AllRoleNames;
            }

        }

    }
}
