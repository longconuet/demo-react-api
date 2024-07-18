using AutoMapper;
using DemoReactAPI.Dtos;
using DemoReactAPI.Entities;
using DemoReactAPI.Enums;
using DemoReactAPI.Helpers;
using DemoReactAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace DemoReactAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<ResponseDto<List<UserDto>>> GetAllUser()
        {
            try
            {
                var users = await _userRepository.GetUsersAsync();

                return new ResponseDto<List<UserDto>>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = _mapper.Map<List<UserDto>>(users)
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<UserDto>>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                };
            }
        }

        [HttpGet("paginated-list")]
        public async Task<ResponseDto<PagedResponse<UserDto>>> GetPaginatedUsers([FromQuery] int page = 1, [FromQuery] int limit = 2)
        {
            try
            {
                var paginatedList = await _userRepository.GetPaginatedUsersAsync(page, limit);
                var userDtos = _mapper.Map<List<UserDto>>(paginatedList);
                var data = new PagedResponse<UserDto>(new PaginatedList<UserDto>(userDtos, paginatedList.TotalCount, page, limit));

                return new ResponseDto<PagedResponse<UserDto>>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<PagedResponse<UserDto>>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                };
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ResponseDto<UserDto>> GetUserDetail(Guid id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return new ResponseDto<UserDto>
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "User not found",
                    };
                }

                return new ResponseDto<UserDto>
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Data = _mapper.Map<UserDto>(user)
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<UserDto>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                };
            }
        }

        [HttpPost("store")]
        public async Task<ResponseDto> Store([FromForm] CreateUserRequest request, IFormFile? avatar)
        {
            try
            {
                var existUserByEmail = await _userRepository.GetUserByEmailAsync(request.Email);
                if (existUserByEmail != null)
                {
                    return new ResponseDto
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Email is already exist",
                    };
                }
                 
                var existUserByPhone = await _userRepository.GetUserByPhoneAsync(request.Phone);
                if (existUserByPhone != null)
                {
                    return new ResponseDto
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Phone is already exist",
                    };
                }

                var existUserByUsername = await _userRepository.GetUserByUsernameAsync(request.Username);
                if (existUserByUsername != null)
                {
                    return new ResponseDto
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "Username is already exist",
                    };
                }

                if (avatar != null)
                {
                    var imagePath = await CommonHelper.UploadFile(avatar, "teams");
                    if (imagePath == null)
                    {
                        return new ResponseDto
                        {
                            Status = ResponseStatusEnum.FAILED,
                            Message = "Upload image failed",
                        };
                    }
                    request.Avatar = imagePath;
                }                

                request.Password = PasswordHelper.HashPasword(request.Password);
                await _userRepository.AddUserAsync(_mapper.Map<User>(request));

                return new ResponseDto
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Create new user successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                };
            }
        }

        [HttpPut("update")]
        public async Task<ResponseDto> Update([FromForm] UpdateUserRequest request, IFormFile? avatar)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(request.Id);
                if (user == null)
                {
                    return new ResponseDto
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "User not found",
                    };
                }

                if (request.Username != user.Username)
                {
                    var existUserByEmail = await _userRepository.GetUserByPhoneAsync(request.Username);
                    if (existUserByEmail != null && existUserByEmail.Id != user.Id)
                    {
                        return new ResponseDto
                        {
                            Status = ResponseStatusEnum.FAILED,
                            Message = "Username is already exist",
                        };
                    }
                    user.Username = request.Username;
                }

                if (request.Phone != user.Phone)
                {
                    var existUserByPhone = await _userRepository.GetUserByPhoneAsync(request.Phone);
                    if (existUserByPhone != null && existUserByPhone.Id != user.Id)
                    {
                        return new ResponseDto
                        {
                            Status = ResponseStatusEnum.FAILED,
                            Message = "Phone is already exist",
                        };
                    }
                    user.Phone = request.Phone;
                }

                if (request.Email != user.Email)
                {
                    var existUserByUsername = await _userRepository.GetUserByEmailAsync(request.Email);
                    if (existUserByUsername != null && existUserByUsername.Id != user.Id)
                    {
                        return new ResponseDto
                        {
                            Status = ResponseStatusEnum.FAILED,
                            Message = "Email is already exist",
                        };
                    }
                    user.Email = request.Email;
                }

                if (avatar != null)
                {
                    var imagePath = await CommonHelper.UploadFile(avatar, "teams");
                    if (imagePath == null)
                    {
                        return new ResponseDto
                        {
                            Status = ResponseStatusEnum.FAILED,
                            Message = "Upload image failed",
                        };
                    }
                    user.Avatar = imagePath;
                }

                user.FullName = request.FullName;
                user.Role = (RoleEnum)request.Role;
                await _userRepository.UpdateUserAsync(_mapper.Map<User>(user));

                return new ResponseDto
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Update user successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                };
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ResponseDto> DeleteUser(Guid id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return new ResponseDto
                    {
                        Status = ResponseStatusEnum.FAILED,
                        Message = "User not found",
                    };
                }

                await _userRepository.DeleteUserAsync(user);

                return new ResponseDto
                {
                    Status = ResponseStatusEnum.SUCCEED,
                    Message = "Delete user successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = ex.Message,
                };
            }
        }
    }
}
