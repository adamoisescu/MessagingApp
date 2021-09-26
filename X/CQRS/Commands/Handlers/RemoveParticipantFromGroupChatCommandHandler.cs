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
    public class RemoveParticipantFromGroupChatCommandHandler : IRequestHandler<RemoveParticipantFromGroupChatCommand, CommandResult>
    {
        private readonly MessagingDbContext _dbContext;

        public RemoveParticipantFromGroupChatCommandHandler(MessagingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CommandResult> Handle(RemoveParticipantFromGroupChatCommand request, CancellationToken cancellationToken)
        {
            Chat chat; 

            try
            {
                chat = _dbContext.Chats.Include(chat => chat.Participants).FirstOrDefault(x => x.Id == request.GroupChatId);

                if (chat == null)
                    return new CommandResult(HttpStatusCode.InternalServerError, new Exception("Chat was not found"));

                var participantToRemove = chat.Participants.FirstOrDefault(p => p.Id == request.ParticipantId);

                if (participantToRemove == null)
                    // or maybe just move on
                    return new CommandResult(HttpStatusCode.InternalServerError, new Exception("Participant to remove was not found"));

                chat.Participants.Remove(participantToRemove);

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
