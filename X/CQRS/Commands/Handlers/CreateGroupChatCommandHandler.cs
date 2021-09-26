using MediatR;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using X.Storage;

namespace X.CQRS.Commands.Handlers
{
    public class CreateGroupChatCommandHandler : IRequestHandler<CreateGroupChatCommand, CommandResult>
    {
        private readonly MessagingDbContext _dbContext;

        public CreateGroupChatCommandHandler(MessagingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CommandResult> Handle(CreateGroupChatCommand request, CancellationToken cancellationToken)
        {
            Guid groupChatId = Guid.NewGuid();

            try
            {
                var participants = _dbContext.Users.Where(x => request.Participants.Contains(x.Id));

                if (!participants.Any())
                    return new CommandResult(HttpStatusCode.InternalServerError, new Exception("No Users were found"));

                _dbContext.Chats.Add(new Storage.Models.Chat
                {
                    Id = groupChatId,
                    Name = request.Name,
                    Participants = participants.ToList(),
                    IsGroup = true
                });

                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return new CommandResult(HttpStatusCode.InternalServerError, ex);
            }

            return new CommandResult(HttpStatusCode.OK, groupChatId);
        }
    }
}
