using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Application.Requests
{
    public class CreateChannelRequest : IRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
