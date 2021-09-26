using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using X.Storage;
using X.Storage.Models;

namespace X.CQRS.Commands.Handlers
{
    public class AddParticipantToGroupChatCommandHandler : IRequestHandler<AddParticipantToGroupChatCommand, CommandResult>
    {
        private readonly MessagingDbContext _dbContext;

        public AddParticipantToGroupChatCommandHandler(MessagingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CommandResult> Handle(AddParticipantToGroupChatCommand request, CancellationToken cancellationToken)
        {
            Chat chat;

            try
            {
                chat = _dbContext.Chats.Include(chat => chat.Participants).FirstOrDefault(x => x.Id == request.GroupChatId);

                if (chat == null)
                    return new CommandResult(HttpStatusCode.InternalServerError, new Exception("Chat was not found"));

                var participant = _dbContext.Users.FirstOrDefault(x => x.Id == request.ParticipantId);

                if (participant == null)
                    return new CommandResult(HttpStatusCode.InternalServerError, new Exception("Participant to add was not found"));

                chat.Participants.Add(participant);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new CommandResult(HttpStatusCode.InternalServerError, ex);
            }

            return new CommandResult(HttpStatusCode.OK, chat?.Participants);
        }
    }
}
