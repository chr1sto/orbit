using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Orbit.Api.Misc;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Api.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediator;

        protected ApiController(INotificationHandler<DomainNotification> notifications,
                                IMediatorHandler mediator)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediator = mediator;
        }

        protected IEnumerable<DomainNotification> Notifications => _notifications.GetNotifications();

        protected bool IsValidOperation()
        {
            return (!_notifications.HasNotifications());
        }

        protected new IActionResult Response<T>(in T result, bool b = false) where T : struct
        {
            if (IsValidOperation())
            {
                return Ok(new ApiResult<T>(result, true));
            }

            return BadRequest(new ApiResult<T>(result, false, _notifications.GetNotifications().Select(n => n.Value)));
        }

        protected new IActionResult Response<T>(in T result = null) where T : class
        {
            if (IsValidOperation())
            {
                return Ok(new ApiResult<T>(result, true));
            }

            return BadRequest(new ApiResult<T>(result, false, _notifications.GetNotifications().Select(n => n.Value)));
        }

        protected void NotifyModelStateErrors()
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(string.Empty, erroMsg);
            }
        }

        protected void NotifyError(string code, string message)
        {
            _mediator.RaiseEvent(new DomainNotification(code, message));
        }

        protected void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                NotifyError(result.ToString(), error.Description);
            }
        }
    }
}
