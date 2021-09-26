using System;

namespace X.DTOs
{
    public class SendMessageDto
    {
        public Guid To { get; set; } // person or groupChat
        public string Text { get; set; }
    }
}
