using System.Threading.Tasks;
using AutoMapper;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOS;
using NormStarr.Extensions;

namespace NormStarr.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IApplicationUserRepo _appRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _token;
        public AccountController(ITokenService token, IApplicationUserRepo appRepo, UserManager<AppUser> userManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _token = token;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _appRepo = appRepo;

        }

        // [HttpGet]
        // public async Task<ActionResult<LoginDTO>> CurrentLoginUser()
        // {
        //     var User = await _userManager.RetrieveEmail(HttpContext.User);
        //     var newUser = new LoginDTO
        //     {
        //         Email = User.Email,
        //         token = _token.CreateToken(User)
        //     };
        //     return Ok(newUser);
        // }

        [HttpPost("Register")]
        public async Task<ActionResult<RegisterDTO>> Register(RegisterDTO registerDTO)
        {
            if (await UserExist(registerDTO.Email)) return BadRequest("Emaill already exist!");
            var mappedUser = _mapper.Map<RegisterDTO, RegisterModel>(registerDTO);
            var User = await _appRepo.SignUp(mappedUser);
            if (!User.Succeeded) return BadRequest(User);
            return Ok(User);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginDTO>> Login(LoginDTO loginModel)
        {
            var mappedUser = _mapper.Map<LoginDTO, LoginModel>(loginModel);
            var logUser = await _appRepo.Login(mappedUser);
            if (logUser == null) return BadRequest();
            return Ok(logUser);
        }
        
        [HttpPost("ConfirmEmail")]
        public async Task<ActionResult<ConfirmEmailModelDTO>> ConfirmEmail(ConfirmEmailModel confirmEmailModel)
        {
            var mappedUser = _mapper.Map<ConfirmEmailModel,ConfirmEmailModelDTO>(confirmEmailModel);
            var user = await _appRepo.ConfirmEmail(mappedUser);
            if(user != null) return Ok(user.Succeeded);
            return BadRequest(user.Errors);
        }
        private async Task<bool> UserExist(string Username)
        {
            return await _userManager.Users.AnyAsync(u => u.UserName == Username.ToLower());
        }
    }
}