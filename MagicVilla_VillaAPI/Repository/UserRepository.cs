using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string SecretKey;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDbContext db,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager, IMapper mapper, IConfiguration configuration) : base(db)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            SecretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(l => l.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponsDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(l => l.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
            bool isValid =await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (user == null||isValid == false)
            {
                return new LoginResponsDTO
                {
                    Token = "",
                    User = null
                };
            }
            //if user was found generate JWT Token
            var roles =await _userManager.GetRolesAsync(user);
            Claim[] claims = new Claim[] {
             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
             new Claim(ClaimTypes.Name, user.UserName),
             new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
             new Claim("username",user.UserName)
            };
            var tokenHander = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretKey);
            var tokenDescroptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHander.CreateToken(tokenDescroptor);
            LoginResponsDTO loginResponsDTO = new LoginResponsDTO
            {
                Token = tokenHander.WriteToken(token),
                User = _mapper.Map<UserDTO>(user)
            };
            return loginResponsDTO;

        }

        public async Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = registerationRequestDTO.UserName,
                Email=registerationRequestDTO.UserName,
                NormalizedEmail=registerationRequestDTO.UserName.ToUpper(),
                Name = registerationRequestDTO.Name,
               
            };
            try
            {
                var result = await _userManager.CreateAsync(user, registerationRequestDTO.Password);
                if(result.Succeeded)
                {
                    if(!await _roleManager.RoleExistsAsync("Admin"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Admin"));
                        await _roleManager.CreateAsync(new IdentityRole("Customer"));
                    }
                    await _userManager.AddToRoleAsync(user, "Admin");
                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(a => a.UserName == registerationRequestDTO.UserName);
                    return _mapper.Map<UserDTO>(userToReturn);
                    
                }

            }
            catch(Exception ex)
            {

            }
            
            return new UserDTO();
        }
    }
}
