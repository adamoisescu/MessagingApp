using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using X.Storage;
using X.Storage.Models;

namespace X.CQRS.Commands.Handlers
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, CommandResult>
    {
        private readonly MessagingDbContext _dbContext;

        public SendMessageCommandHandler(MessagingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CommandResult> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userFrom = _dbContext.Users.FirstOrDefault(u => u.Id == request.From);
                if (userFrom == null)
                    return new CommandResult(HttpStatusCode.InternalServerError, new Exception("Message Sender user was not found"));

                Guid chatId;
                // group message
                var groupChat = _dbContext.Chats.FirstOrDefault(c => c.IsGroup && c.Id == request.To);
                if (groupChat != null)
                {
                    chatId = groupChat.Id;
                }
                // private message
                else
                {
                    var userTo = _dbContext.Users.FirstOrDefault(u => u.Id == request.To);
                    if (userTo == null)
                        return new CommandResult(HttpStatusCode.InternalServerError, new Exception("Message Receiver user was not found. If group chat was intended make sure the group chat exists."));

                    var privateChat = _dbContext.Chats.Include(c => c.Participants).Where(c => c.Participants.Count() == 2)
                                                                                   .FirstOrDefault(c => c.Participants.Any(p => p.Id == request.From) && c.Participants.Any(p => p.Id == request.To));
                    if (privateChat == null)
                    {
                        // create private chat
                        chatId = Guid.NewGuid();
                        _dbContext.Chats.Add(new Storage.Models.Chat
                        {
                            Id = chatId,
                            Name = userTo.Name,
                            IsGroup = false,
                            Participants = new List<User>() { userFrom, userTo }
                        });
                    }
                    else
                    {
                        chatId = privateChat.Id;
                    }
                }

                _dbContext.Messages.Add(new Storage.Models.Message
                {
                    ChatId = chatId,
                    From = request.From,
                    Text = request.Text
                });

                await _dbContext.SaveChangesAsync();

                return new CommandResult(HttpStatusCode.OK, chatId);
            }
            catch (Exception ex)
            {
                return new CommandResult(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
