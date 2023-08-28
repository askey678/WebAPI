using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.BAL.Payloads;
using WebApi.BAL.Services;
using WebApi.BAL.ViewModels;
using WebApi.DAL.ExceptionHandler;

namespace WebApi.Api.Controllers
{
    [ApiController]
    [Route("Webapi/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IService userData;

        public UserController(ILogger<UserController> logger, IService userService)
        {
            this.logger = logger;
            this.userData = userService;


        }


        //[Authorize(Roles = "ADMIN")]
        [HttpGet]
        [SwaggerOperation("Get list of Users")]
        [SwaggerResponse(StatusCodes.Status200OK, "All Users fetched Successfully", typeof(IEnumerable<UserResponseDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "FAILED to retrieve Users", typeof(string))]
        public IActionResult GetAllUsers()
        {
            try
            {
                List<UserResponseDto> users = userData.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to retrieve users. Error: "+ex.Message);
            }
        }


        [HttpGet("{key}")]
        [SwaggerOperation(Summary = "Get a user by ID", OperationId = "GetUserById")]
        [SwaggerResponse(StatusCodes.Status200OK, "User Found", typeof(UserResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "FAILED!!", typeof(string))]
        public IActionResult GetUserById(string key)
        {
            try
            {
                UserResponseDto user = userData.GetUserById(key);
                return Ok(user);
            }
            catch (ResourceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{key}")]
        [SwaggerOperation(Summary = "Delete a User by Key", OperationId = "DeleteOneUser")]
        [SwaggerResponse(StatusCodes.Status200OK, "User Deleted successfully", typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Deletion FAILED!!", typeof(string))]
        public IActionResult DeleteOneUser(string key)
        {
            try
            {
                userData.DeleteUserById(key);
                return Ok(new ApiResponse($"User deleted successfully with key: {key}", 200));
            }
            catch (ResourceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpPut("{key}")]
        [SwaggerOperation(Summary = "Update a User by Key")]
        [SwaggerResponse(StatusCodes.Status200OK, "User Updated successfully", typeof(UserResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Updation FAILED!!", typeof(string))]
        public IActionResult UpdateUser(string key, [FromBody] UpdateDto user)
        {
            try
            {
                UserResponseDto updatedUser = userData.UpdateUser(key, user);
                return Ok(new ApiResponse($"User profile updated: {updatedUser.ToString()}", 200));
            }
            catch (ResourceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "ADMIN")]
        [HttpGet("ArchivedUsers/")]
        [SwaggerOperation(Summary = "Get all Archived users")]
        [SwaggerResponse(StatusCodes.Status200OK, "All Archived Users fetched Successfully", typeof(List<UserResponseDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "FAILED to Load Archived users", typeof(string))]
        public IActionResult GetArchivedUsers()
        {

            List<UserResponseDto> archivedUsers = userData.GetAllArchivedUsers();
            return Ok(archivedUsers);


        }


        [Authorize(Roles = "ADMIN")]
        [HttpGet("UnArchivedUsers/")]
        [SwaggerOperation(Summary = "Get all UnArchived users")]
        [SwaggerResponse(StatusCodes.Status200OK, "All UnArchived Users fetched Successfully", typeof(List<UserResponseDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "FAILED to Load UnArchived users", typeof(string))]
        public IActionResult GetUnArchivedUsers()
        {

            List<UserResponseDto> archivedUsers = userData.GetAllUnArchivedUsers();
            return Ok(archivedUsers);


        }



        [Authorize(Roles = "ADMIN")]
        [HttpPut("RestoreUser/{key}")]
        [SwaggerOperation(Summary = "Restore a Archived user by key")]
        [SwaggerResponse(StatusCodes.Status200OK, "User Restored successfully", typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Restoration FAILED!!", typeof(string))]
        public IActionResult RestoreUser(string key)
        {
            try
            {
                userData.RestoreArchivedUser(key);
                return Ok(new ApiResponse("User Restored", 200));
            }
            catch (ResourceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("ForceDelete/{key}")]
        [SwaggerOperation(Summary = "Delete a User permanently")]
        [SwaggerResponse(StatusCodes.Status200OK, "User Deleted Peramanently", typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Deletion FAILED!!", typeof(string))]
        public IActionResult ForceDelete(string key)
        {
            try
            {
                userData.DeleteUserPermanently(key);
                return Ok(new ApiResponse($"User deleted successfully with key and is Inactivated: {key}", 200));
            }
            catch (ResourceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("DeleteArchived/{key}")]
        [SwaggerOperation(Summary = "Delete Archived User permanently")]
        [SwaggerResponse(StatusCodes.Status200OK, "Archived User Deleted Peramanently", typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Deletion FAILED!!", typeof(string))]
        public IActionResult DeleteArchived(string key)
        {
            try
            {
                userData.DeleteArchivedUser(key);
                return Ok(new ApiResponse($"User deleted successfully with key and is Inactivated: {key}", 200));
            }
            catch (ResourceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
