using MediatR;
using System;
using System.Collections.Generic;

namespace X.CQRS.Commands
{
    public class CreateGroupChatCommand : IRequest<CommandResult>
    {
        public string Name { get; set; }
        public IEnumerable<Guid> Participants { get; set; }

        public CreateGroupChatCommand(string name, IEnumerable<Guid> participants)
        {
            Name = name;
            Participants = participants;
        }
    }
}
