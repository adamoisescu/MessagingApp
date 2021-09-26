using System;
using System.Collections.Generic;
using System.Linq;
using X.Storage;
using X.Storage.Models;

namespace X.CQRS.Queries
{
    public class MessageQueries : IMessageQueries
    {
        private readonly MessagingDbContext _dbContext;

        public MessageQueries(MessagingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Message> GetByChat(Guid chatId)
        {
            return _dbContext.Messages.Where(m => m.ChatId == chatId);
        }
    }
}
