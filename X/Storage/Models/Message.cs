using System;

namespace X.Storage.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public Guid From { get; set; }
        public string Text { get; set; }
    }
}
