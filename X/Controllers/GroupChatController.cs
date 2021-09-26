using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.CQRS.Commands;
using X.DTOs;

namespace X.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("v1/group_chat")]
    public class GroupChatController
    {
        private readonly ILogger<GroupChatController> _logger;
        private readonly IMediator _mediator;

        public GroupChatController(ILogger<GroupChatController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] Guid request_user_id, [FromBody] GroupChatDto group_chat_model)
        {
            if (request_user_id == Guid.Empty)
                return new BadRequestObjectResult("provide request_user_id");

            if (string.IsNullOrWhiteSpace(group_chat_model.Name))
                return new BadRequestObjectResult("provide a name for the group chat");

            if (group_chat_model.Participants == null || !group_chat_model.Participants.Any())
                return new BadRequestObjectResult("add at least one participant in the group chat");

            var participants = group_chat_model.Participants.ToList();
            participants.Add(request_user_id);

            return await _mediator.Send(new CreateGroupChatCommand(group_chat_model.Name, participants));
        }

        [HttpPost]
        [Route("{group_chat_id}/participants")]
        public async Task<IActionResult> AddParticipant(Guid group_chat_id, [FromQuery] Guid user_id)
        {
            if (group_chat_id == Guid.Empty)
                return new BadRequestObjectResult("provide group_chat_id");

            if (user_id == Guid.Empty)
                return new BadRequestObjectResult("provide participant user_id");

            return await _mediator.Send(new AddParticipantToGroupChatCommand(group_chat_id, user_id));
        }

        [HttpDelete]
        [Route("{group_chat_id}/participants/{user_id}")]
        public async Task<IActionResult> RemoveParticipant(Guid group_chat_id, Guid user_id)
        {
            if (group_chat_id == Guid.Empty)
                return new BadRequestObjectResult("provide group_chat_id");

            if (user_id == Guid.Empty)
                return new BadRequestObjectResult("provide participant user_id");

            return await _mediator.Send(new RemoveParticipantFromGroupChatCommand(group_chat_id, user_id));
        }
    }
}
