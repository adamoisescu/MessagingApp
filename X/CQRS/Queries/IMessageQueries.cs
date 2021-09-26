using System;
using System.Collections.Generic;
using X.Storage.Models;

namespace X.CQRS.Queries
{
    public interface IMessageQueries
    {
        public IEnumerable<Message> GetByChat(Guid chatId);
    }
}
