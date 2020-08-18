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
    public class AppsController : ControllerBase
    {
        private IAppsBusiness appsBusiness;

        public AppsController(IAppsBusiness appsBusiness,
                              IPermissionCheck permissionCheck) : base(permissionCheck)
        {
            this.appsBusiness = appsBusiness;
        }

        [HttpPost]
        public async Task<ActionResult> AddApp([FromBody] AppDto appDto)
        {
            try
            {
                await CheckPermission(Permissions.SuperUser);
                await appsBusiness.AddApp(appDto);
                return Ok();
            }
            catch (AccessViolationException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<AppDto>>> GetAllApps()
        {
            try
            {
                await CheckPermission(Permissions.SuperUser);
                var result = await appsBusiness.GetAllApps();
                return Ok(result);
            }
            catch (AccessViolationException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{appId}")]
        public async Task<ActionResult<AppDto>> GetApp([FromRoute] int appId)
        {
            try
            {
                await CheckPermission(Permissions.SuperUser);
                var result = await appsBusiness.GetApp(appId);
                return Ok(result);
            }
            catch (AccessViolationException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{appId}")]
        public async Task<ActionResult> RemoveApp([FromRoute] int appId)
        {
            try
            {
                await CheckPermission(Permissions.SuperUser);
                await appsBusiness.RemoveApp(appId);
                return Ok();
            }
            catch (AccessViolationException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateApp([FromBody] AppDto appDto)
        {
            try
            {
                await CheckPermission(Permissions.SuperUser);
                await appsBusiness.UpdateApp(appDto);
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
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }
    }
}