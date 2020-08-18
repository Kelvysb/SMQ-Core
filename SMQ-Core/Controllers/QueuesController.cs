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
    public class QueuesController : ControllerBase
    {
        private IQueueBusiness queueBusiness;

        public QueuesController(IQueueBusiness queueBusiness,
                                IPermissionCheck permissionCheck) : base(permissionCheck)
        {
            this.queueBusiness = queueBusiness;
        }

        [HttpGet("{queue}")]
        public async Task<ActionResult<MessageDto>> Dequeue([FromRoute] string queue)
        {
            try
            {
                var user = await CheckPermission(Permissions.User);
                var result = await queueBusiness.Dequeue(queue, user);
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

        [HttpGet("all/{queue}")]
        public async Task<ActionResult<List<MessageDto>>> DequeueAll([FromRoute] string queue)
        {
            try
            {
                var user = await CheckPermission(Permissions.User);
                var result = await queueBusiness.DequeueAll(queue, user);
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

        [HttpPost("{queue}")]
        public async Task<ActionResult> Enqueue([FromRoute] string queue, [FromBody] string message)
        {
            try
            {
                var user = await CheckPermission(Permissions.User);
                await queueBusiness.Enqueue(queue, message, user);
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

        [HttpDelete("{queue}")]
        public async Task<ActionResult> Clear([FromRoute] string queue)
        {
            try
            {
                var user = await CheckPermission(Permissions.User);
                await queueBusiness.Clear(queue, user);
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

        [HttpGet("view/{queue}")]
        public async Task<ActionResult<List<MessageDto>>> List([FromRoute] string queue)
        {
            try
            {
                var user = await CheckPermission(Permissions.User);
                var result = await queueBusiness.List(queue, user);
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

        [HttpGet("view")]
        public async Task<ActionResult<List<MessageDto>>> List()
        {
            try
            {
                var user = await CheckPermission(Permissions.User);
                var result = await queueBusiness.List(user);
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
    }
}