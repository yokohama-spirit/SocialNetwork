using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Application.Requests
{
    public class UpdateChannelDescriptionDTO
    {
        public required string Description { get; set; }
    }
}
