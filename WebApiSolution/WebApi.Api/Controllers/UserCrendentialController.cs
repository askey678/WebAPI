
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Api.Helpers;
using WebApi.BAL.Payloads;
using WebApi.BAL.Services;
using WebApi.BAL.ViewModels;
using WebApi.DAL.ExceptionHandler;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("Webapi/[Controller]")]
    public class UserCredentialController : ControllerBase
    {
        private IConfiguration _config;

        private IService userData;

        public UserCredentialController( IService userdata, IConfiguration config)
        {    
            userData = userdata;
            _config = config;
        }

       
        [AllowAnonymous]
        [HttpGet("login")]
        [EnableCors]
        [SwaggerOperation("Login")]
        [SwaggerResponse(200, "Login Successful", typeof(UserLoginResponse))]
        [SwaggerResponse(400, "Login Failed", typeof(string))]
        public IActionResult Login([FromQuery]LoginModel loginDetails)
        {
            try
            {
                UserAndRoles user_ = userData.Login(loginDetails.Email, loginDetails.Password);
                
                if (user_ != null)
                {
                    var token = TokenHelper.GenerateTokens(user_, _config);

                    TokenRegisterDto tokendetails = new TokenRegisterDto();
                    tokendetails.TokenType="Auth";
                    tokendetails.UserKey=user_.UserKey;
                    tokendetails.TokenHash=token;
                    userData.TokenRegistration(tokendetails);

                    return Ok(new UserLoginResponse(token, user_.UserKey, user_.Name, user_.Email, user_.About, user_.Roles));
                }
                ApiResponse response = new ApiResponse();
                response.message="Wrong Crendentials!! User Not Found with Email and Password";
                response.status=400;

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

      
        [HttpPost("signUp")]
        [SwaggerOperation("Register")]
        [SwaggerResponse(StatusCodes.Status200OK, "User Registered Successfully", typeof(UserResponseDto))]
        [SwaggerResponse(400, "Registration Failed", typeof(string))]
        public IActionResult SignUp(UserRegisterDto userdto)
        {
            try
            {
                UserResponseDto Newuser = userData.Registeration(userdto);

                if (Newuser != null)
                {
                    return Ok(new { message = $"New User created with Name: {Newuser.Name} and Email: {Newuser.Email}" });
                }
                // status code: 409 - conflict 
                return BadRequest(new ApiResponse("Email Id already Exists", 409));

            }
            catch (ResourceNotFoundException ex)
            {
                return BadRequest(ex);
            }
        }
    }

}
