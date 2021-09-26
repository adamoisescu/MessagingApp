using MediatR;
using System;

namespace X.CQRS.Commands
{
    public class SendMessageCommand : IRequest<CommandResult>
    {
        public Guid From { get; set; } // user
        public Guid To { get; set; } // group chat or user
        public string Text { get; set; }

        public SendMessageCommand(Guid from, Guid to, string text)
        {
            From = from;
            To = to;
            Text = text;
        }
    }
}
