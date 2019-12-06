using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;

namespace HTQLNP.Models
{
    public class StudentPunishment
    {
        public enum PunishmentType
        {
            PAY = 1, PUSH = 2
        }

        public string Rollnumber { get; set; }

        public PunishmentType PunishmentTypeId { get; set; }

        public double Value { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}