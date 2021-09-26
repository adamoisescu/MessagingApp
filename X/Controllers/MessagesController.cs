using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.CQRS.Commands;
using X.CQRS.Queries;
using X.DTOs;
using X.Storage.Models;

namespace X.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IMediator _mediator;
        private readonly IMessageQueries _messageQueries;
        private readonly IMapper _mapper;

        public MessagesController(ILogger<MessagesController> logger, IMediator mediator, IMessageQueries messageQueries, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _messageQueries = messageQueries;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromQuery] Guid from_user_id, [FromBody] SendMessageDto message)
        {
            if (from_user_id == Guid.Empty)
                return new BadRequestObjectResult("provide from_user_id");

            if (message == null)
                return new BadRequestObjectResult("provide a message into the request body");

            if (message.To == Guid.Empty)
                return new BadRequestObjectResult($"provide a user as receiver of the message");

            if (string.IsNullOrWhiteSpace(message.Text))
                return new BadRequestObjectResult($"provide a text message");
            // FluentValidator can be used for validating commands

            return await _mediator.Send(new SendMessageCommand(from_user_id, message.To, message.Text)); 
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid chat_Id)
        {
            if (chat_Id == Guid.Empty)
                return BadRequest("provide chat_Id");

            return Retrieve<IEnumerable<Message>, IEnumerable<MessageDto>>(() => _messageQueries.GetByChat(chat_Id));
        }

        // extract to base if needed
        protected virtual IActionResult Retrieve<T, TDestination>(Func<T> func) where T : class
        {
            try
            {
                var result = func();

                if (result == null)
                    return Ok();

                return Ok(_mapper.Map<TDestination>(result));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e);
            }
        }
    }
}
