using EventifyCommon.Models.AbstractModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventifyCommon.Models
{
    public class Follow : BaseModel
    {
        public string FollowerId { get; set; }
        public ApplicationUser Follower { get; set; }

        public string FollowedUserId { get; set; }
        public ApplicationUser FollowedUser { get; set; }
    }
}
