using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orbit.Api.Misc;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Notifications;
using Orbit.Infra.FileUpload.Interfaces;

namespace Orbit.Api.Controllers
{
    [Authorize(Roles = "Administrator,Gamemaster,Developer")]
    [Route("file-upload")]
    public class FileUploadController : ApiController
    {
        private readonly IFileUploadService _fileUploadService;

        public FileUploadController(IFileUploadService fileUploadService,INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator) : base(notifications, mediator)
        {
            _fileUploadService = fileUploadService;
        }

        [ProducesResponseType(typeof(ApiResult<IEnumerable<string>>), 200)]
        [ProducesResponseType(typeof(ApiResult<IEnumerable<string>>), 400)]
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var result = await _fileUploadService.GetAll();
            if(result == null)
            {
                NotifyError("", "Could not get all Files. For more information see Server logs.");
            }
            return Response(result);
        }

        [ProducesResponseType(typeof(ApiResult<IFormFile>), 200)]
        [ProducesResponseType(typeof(ApiResult<IFormFile>), 400)]
        [HttpPost("")]
        public async Task<IActionResult> Post([FromForm] IFormFile files)
        {
            var file = Request.Form.Files[0];
            var result = await _fileUploadService.Upload(file);
            if (!result)
            {
                NotifyError("", "Could not upload File. For more information see Server logs.");
            }
            return Response(file);
        }

        [ProducesResponseType(typeof(ApiResult<string>), 200)]
        [ProducesResponseType(typeof(ApiResult<string>), 400)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _fileUploadService.Delete(id);
            if(!result)
            {
                NotifyError("", "Could not delete File. For more information see Server logs.");
            }
            return Response(id);
        }

        [ProducesResponseType(typeof(ApiResult<object>), 200)]
        [ProducesResponseType(typeof(ApiResult<object>), 400)]
        [HttpDelete("")]
        public async Task<IActionResult> DeleteAll()
        {
            var result = await _fileUploadService.DeleteAll();
            if(!result)
            {
                NotifyError("", "Could not delete all Files. For more information see Server logs.");
            }
            return Response<object>(null);
        }
    }
}
