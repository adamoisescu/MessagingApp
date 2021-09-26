using System;
using System.Collections.Generic;

namespace X.Storage.Models
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsGroup { get; set; }
        public ICollection<User> Participants { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
