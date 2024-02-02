using EventifyCommon.Models.AbstractModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EventifyCommon.Models
{
    public class Tag : BaseModel
    {
        public string Name { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
