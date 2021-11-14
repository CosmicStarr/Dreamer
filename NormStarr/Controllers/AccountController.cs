using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOS;
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
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(
            ITokenService token, 
            IApplicationUserRepo appRepo, 
            UserManager<AppUser> userManager, 
            IMapper mapper, IUnitOfWork unitOfWork,
            SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _token = token;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _appRepo = appRepo;

        }

        // [HttpGet]
        // public async Task<ActionResult<LoginDTO>> CurrentLoginUser()
        // {
        //     var user = await _userManager.RetrieveEmail(HttpContext.User);
        //     var role = await _userManager.GetRolesAsync(user);
        //     IList<Claim> Claim = await _userManager.GetClaimsAsync(user); 
        //     var newUser = new LoginDTO
        //     {
        //         Email = user.Email,
        //         token =  _token.Token(user,role,Claim)
        //     };
        //     return Ok(newUser);
        // }

        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<ForgotPasswordDTO>> ForgotPasswordAsync([FromBody]ForgotPasswordDTO appUserDTO)
        {
            var mappedUser = _mapper.Map<ForgotPasswordDTO,ForgotPassword>(appUserDTO);
            var logUser = await _appRepo.ForgotPassword(mappedUser);
            if(logUser == null) return Unauthorized(new ApiErrorResponse(404));
            return Ok(logUser);
        }

        [HttpPost("ResetPassword")]
        public async Task<ActionResult<ResetPasswordDTO>> ResetPasswordAsync([FromBody]ResetPasswordDTO appUserDTO)
        {
            var mappedUser = _mapper.Map<ResetPasswordDTO,ResetPassword>(appUserDTO);
            var logUser = await _appRepo.ResetPassword(mappedUser);
            if(logUser == null) return Unauthorized(new ApiErrorResponse(404));
            return Ok(logUser);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegisterDTO>> Register([FromBody]RegisterModel registerDTO)
        {
         
            if (await UserExist(registerDTO.Email)) return BadRequest("Email already exist!");
            var mappedUser = _mapper.Map<RegisterModel, RegisterDTO>(registerDTO);
            var User = await _appRepo.SignUp(mappedUser);
            if (User == null) return BadRequest(new ApiErrorResponse(400));
            return Ok(User);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginDTO>> Login([FromBody]LoginModel loginModel)
        {
            var mappedUser = _mapper.Map<LoginModel,LoginDTO>(loginModel);
            var logUser = await _appRepo.Login(mappedUser);
            if (logUser == null) return NotFound(new ApiErrorResponse(404));
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
        [HttpGet("GetAddress")]
        [Authorize]
        public async Task<ActionResult<UserAddressDTO>> GetAddressAsync()
        {
            var UserAdd = await _userManager.RetrieveEmail(HttpContext.User);
            var Address = await _unitOfWork.Repository<UserAddress>().GetFirstOrDefault(x =>x.AppUserId == UserAdd.Id);
            return Ok(_mapper.Map<UserAddress,UserAddressDTO>(Address)); 
        }

        [HttpPost("Address")]
        [Authorize]
        public async Task<ActionResult<UserAddressDTO>> UpdateUserAddress(UserAddressDTO address)
        {
            // You have to make this method a real put method. Retrieve the address thats equal to the Current User!
            var UserAdd = await _userManager.RetrieveEmail(HttpContext.User);
            UserAdd.Addresses = await _unitOfWork.Repository<UserAddress>().GetFirstOrDefault(x =>x.AppUserId == UserAdd.Id);
            if(UserAdd.Addresses != null)
            {
                _unitOfWork.Repository<UserAddress>().Remove(UserAdd.Addresses);
            }
            UserAdd.Addresses = _mapper.Map<UserAddressDTO, UserAddress>(address);
            var result = await _userManager.UpdateAsync(UserAdd);
            if(result.Succeeded)
            {
                return Ok(_mapper.Map<UserAddress, UserAddressDTO>(UserAdd.Addresses));             
            }
            
            return BadRequest("Request not granted!");
        }
    }
}