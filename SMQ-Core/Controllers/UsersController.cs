using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMQCore.Business.Interfaces;
using SMQCore.Helpers;
using SMQCore.Shared.Models.Dtos;

namespace SMQCore.Controllers
{
    [Authorize]
    [Route("[Controller]")]
    public class UsersController : ControllerBase
    {
        private IUsersBusiness usersBusiness;

        public UsersController(IUsersBusiness usersBusiness,
                                IPermissionCheck permissionCheck) : base(permissionCheck)
        {
            this.usersBusiness = usersBusiness;
        }

        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody] UserDto userDto)
        {
            try
            {
                var user = await CheckPermission(Permissions.AppAdmin);
                await usersBusiness.AddUser(userDto, user);
                return Ok();
            }
            catch (AccessViolationException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            try
            {
                var user = await CheckPermission(Permissions.AppAdmin);
                var result = await usersBusiness.GetAllUsers(user);
                return Ok(result);
            }
            catch (AccessViolationException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> GetUser([FromRoute] int userId)
        {
            try
            {
                await CheckPermission(Permissions.AppAdmin);
                var result = await usersBusiness.GetUser(userId);
                return Ok(result);
            }
            catch (AccessViolationException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveUser([FromBody] UserDto user)
        {
            try
            {
                await CheckPermission(Permissions.AppAdmin);
                await usersBusiness.RemoveUser(user);
                return Ok();
            }
            catch (AccessViolationException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromBody] UserDto userDto)
        {
            try
            {
                var user = await CheckPermission(Permissions.AppAdmin);
                await usersBusiness.UpdateUser(userDto, user);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (AccessViolationException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("ChangePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] UserDto userDto)
        {
            try
            {
                var user = await CheckPermission(Permissions.AppAdmin);
                await usersBusiness.ChangePassword(userDto, user);
                return Ok();
            }
            catch (AccessViolationException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] UserDto login)
        {
            try
            {
                var result = await usersBusiness.Login(login);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}