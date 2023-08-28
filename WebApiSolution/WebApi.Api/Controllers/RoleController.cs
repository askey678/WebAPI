using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.BAL.Payloads;
using WebApi.BAL.Services;
using WebApi.BAL.ViewModels;
using WebApi.DAL.ExceptionHandler;

namespace WebApi.Api.Controllers
{
    [ApiController]
    [Route("Webapi/[Controller]")]
    [Authorize(Roles = "ADMIN")]
    public class RoleController : ControllerBase
    {

        private IService? userData;

        public RoleController(IService userservice)
        {

            this.userData = userservice;
        }

        

        [HttpGet]
        [SwaggerOperation("Get List of Roles")]
        [SwaggerResponse(200, "All Roles fetched Successfully", typeof(RoleResponseDto))]
        [SwaggerResponse(400, "FAILED to retreive roles", typeof(string))]
        public IEnumerable<RoleResponseDto> getAllRoles()
        {
            List<RoleResponseDto> roles = userData.GetAllRoles();
            return roles;
        }

        

        [HttpPost]
        [SwaggerOperation("Add New Role")]
        [SwaggerResponse(200, "ADDED Successfully", typeof(RoleResponseDto))]
        [SwaggerResponse(400, "FAILED!!", typeof(string))]
        public IActionResult SignUp(RoleRegisterDto roledto)
        {
            RoleResponseDto newRole = userData.AddNewRole(roledto);

            if (newRole != null)
            {
                return Ok(new { message = $"New Role created with Key: {newRole.RoleKey} and Name: {newRole.RoleName}" });
            }
            // status code: 409 - conflict 
            return BadRequest(new ApiResponse("Request Failed for Adding new Role", 400));

        }


        [Route("{key}")]
        [HttpGet]
        [SwaggerOperation("Get Role by Key")]
        [SwaggerResponse(200, "User Found Successfully", typeof(RoleResponseDto))]
        [SwaggerResponse(400, "FAILED!!", typeof(string))]
        public IActionResult getRoleById(string key)
        {
            try
            {
                RoleResponseDto role = userData.GetRoleByKey(key);
                return Ok(role);
            }
            catch (ResourceNotFoundException ex)
            {
                string response = ex.Message;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        

        [Route("{key}")]
        [HttpDelete]
        [SwaggerOperation("Delete Role by Key")]
        [SwaggerResponse(200, "Role Deleted Successfully", typeof(ApiResponse))]
        [SwaggerResponse(400, "Deletion FAILED!!", typeof(string))]
        public IActionResult DeleteOneUser(string key)
        {
            try
            {
                userData.DeleteRoleById(key);
                return Ok(new ApiResponse($"Role deleted successfully with key: {key}", 200));
            }
            catch (ResourceNotFoundException ex)
            {
                string response = ex.Message;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        [Route("{key}")]
        [HttpPut]
        [SwaggerOperation("Update Role")]
        [SwaggerResponse(200, "Role Updated Successfully", typeof(RoleResponseDto))]
        [SwaggerResponse(400, "Updation FAILED!!", typeof(string))]
        public IActionResult UpdateUser(string key, [FromBody] RoleRegisterDto role)
        {
            try
            {
                RoleResponseDto updatedRole = userData.UpdateRole(key, role);
                return Ok(new ApiResponse($"Role updated: {updatedRole.ToString()}", 200));
            }
            catch (ResourceNotFoundException ex)
            {
                string response = ex.Message;
                return BadRequest(response);
            }
        }
    }
}
