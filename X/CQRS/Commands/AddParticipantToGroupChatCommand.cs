using MediatR;
using System;

namespace X.CQRS.Commands
{
    public class AddParticipantToGroupChatCommand : IRequest<CommandResult>
    {
        public Guid GroupChatId { get; set; }
        public Guid ParticipantId { get; set; }

        public AddParticipantToGroupChatCommand(Guid groupChatId, Guid participantId)
        {
            GroupChatId = groupChatId;
            ParticipantId = participantId;
        }
    }
}
