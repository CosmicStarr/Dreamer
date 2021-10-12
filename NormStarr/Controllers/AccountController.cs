using System.Threading.Tasks;
using AutoMapper;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOS;
using Models.Orders;
using NormStarr.ErrorHandling;
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
        public async Task<ActionResult<RegisterDTO>> Register(RegisterModel registerDTO)
        {
            if (await UserExist(registerDTO.Email)) return BadRequest("Emaill already exist!");
            var mappedUser = _mapper.Map<RegisterModel, RegisterDTO>(registerDTO);
            var User = await _appRepo.SignUp(mappedUser);
            if (!User.Succeeded) return BadRequest(new ApiErrorResponse(400));
            return Ok(User);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginDTO>> Login([FromBody]LoginModel loginModel)
        {
            var mappedUser = _mapper.Map<LoginModel,LoginDTO>(loginModel);
            var logUser = await _appRepo.Login(mappedUser);
            if (logUser == null) return Unauthorized(new ApiErrorResponse(404));
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

        [HttpPut("Address")]
     
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO addressDTO)
        {
            var UpdatedAdd = await _userManager.RetrieveEmail(HttpContext.User);
            UpdatedAdd.Addresses = _mapper.Map<AddressDTO, Address>(addressDTO);
            var result = await _userManager.UpdateAsync(UpdatedAdd);
            if(result.Succeeded)
            {
                return Ok(_mapper.Map<Address, AddressDTO>(UpdatedAdd.Addresses));             
            }
            return BadRequest("Request not granted!");

        }
    }
}