using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("")]
        public async Task<IActionResult> Post([FromForm] IFormFile files)
        {
            var file = Request.Form.Files[0];
            var result = await _fileUploadService.Upload(file);
            if (!result)
            {
                NotifyError("", "Could not upload File. For more information see Server logs.");
            }
            return Response(result);
        }

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

        [HttpDelete("")]
        public async Task<IActionResult> DeleteAll()
        {
            var result = await _fileUploadService.DeleteAll();
            if(!result)
            {
                NotifyError("", "Could not delete all Files. For more information see Server logs.");
            }
            return Response(result);
        }
    }
}
