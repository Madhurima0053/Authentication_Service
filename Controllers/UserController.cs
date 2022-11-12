using AuthenticationService.Repository;
using AuthenticationService.UserModel;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserDetailContext _context;
        public IConfiguration _config;

        private IDictionary<string, dynamic> response = new Dictionary<string, dynamic>();
        public UserController(IUserRepository userRepository, IConfiguration config,UserDetailContext context)
        {
            _userRepository = userRepository;
            _config = config;
            _context = context;

        }
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [AllowAnonymous]
        [HttpPost]
        public string Post([FromBody] UserDetail value)
        {

            try
            {
                var _temp = _userRepository.Signup(value);
                if (_temp == "2")
                {
                    response.Add("error", false);
                    response.Add("message", "User Added");

                    string jsonResponse = JsonConvert.SerializeObject(response);

                    return jsonResponse;
                }
                else if (_temp == "3")
                {
                    response.Add("error", true);
                    response.Add("message", "User not Added");

                    string jsonResponse = JsonConvert.SerializeObject(response);

                    return jsonResponse;
                }
                else
                {
                    response.Add("error", true);
                    response.Add("message", "This username already exists");

                    string jsonResponse = JsonConvert.SerializeObject(response);

                    return jsonResponse;
                }
            }
            catch (Exception ex)
            {

                response.Add("error", true);
                response.Add("message", ex.Message);

                string jsonResponse = JsonConvert.SerializeObject(response);

                return jsonResponse;
            }
        }
        [AllowAnonymous]
        [HttpPost("Signin")]
        public string Signin([FromBody] UserDetail value)
        {

            try
            {
                if (value != null && value.UserName != null && value.Password != null)
                {
                    var user = _context.UserDetails.Where(p => p.UserName == value.UserName && p.Password == value.Password).FirstOrDefault();

                    if (user != null)
                    {

                        //create claims details based on the user information
                        var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("Name", user.UserName),
                       
                    };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _config["Jwt:Issuer"],
                            _config["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(60),
                            signingCredentials: signIn);

                        //return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                        response.Add("error", false);
                        response.Add("message", "Login Successful");
                        response.Add("token", new JwtSecurityTokenHandler().WriteToken(token));


                        string jsonResponse = JsonConvert.SerializeObject(response);

                        return jsonResponse;
                    }
                    else
                    {
                        response.Add("error", true);
                        response.Add("message", "Invalid Email or Password");

                        string jsonResponse = JsonConvert.SerializeObject(response);

                        return jsonResponse;
                    }
                }
                else
                {
                    response.Add("error", true);
                    response.Add("message", "Please Enter Email and Password");

                    string jsonResponse = JsonConvert.SerializeObject(response);

                    return jsonResponse;
                }
            }
            catch (Exception ex)
            {

                response.Add("error", true);
                response.Add("message", ex.Message);

                string jsonResponse = JsonConvert.SerializeObject(response);

                return jsonResponse;
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
