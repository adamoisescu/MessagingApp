using System;

namespace X.DTOs
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid From { get; set; }
        public string Text { get; set; }
    }
}
