using EventifyCommon.Models.AbstractModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventifyCommon.Models
{
    public class Like : BaseModel
    {
        public int EventId { get; set; }
        public string UserId { get; set; }

        public Event Event { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
