using System;
using System.Collections.Generic;

namespace X.DTOs
{
    public class GroupChatDto
    {
        public string Name { get; set; }
        public IEnumerable<Guid> Participants { get; set; }
    }
}
