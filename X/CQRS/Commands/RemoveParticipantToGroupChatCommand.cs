using MediatR;
using System;

namespace X.CQRS.Commands
{
    public class RemoveParticipantFromGroupChatCommand : IRequest<CommandResult>
    {
        public Guid GroupChatId { get; set; }
        public Guid ParticipantId { get; set; }

        public RemoveParticipantFromGroupChatCommand(Guid groupChatId, Guid participantId)
        {
            GroupChatId = groupChatId;
            ParticipantId = participantId;
        }
    }
}
