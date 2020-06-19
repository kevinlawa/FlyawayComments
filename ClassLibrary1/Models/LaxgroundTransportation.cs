using System;
using System.Collections.Generic;

namespace ClassLibrary1.Models
{
    public partial class LaxgroundTransportation
    {
        public int TransportId { get; set; }
        public string ServiceType { get; set; }
        public string ServiceSubType { get; set; }
        public string ServiceTypeOther { get; set; }
        public bool? IsTaxicab { get; set; }
        public bool? IsServiceDelay { get; set; }
        public bool? IsDriverMisconduct { get; set; }
        public bool? IsAmericansComp { get; set; }
        public bool? IsOvercharge { get; set; }
        public bool? IsSolicitation { get; set; }
        public bool? IsVehicle { get; set; }
        public bool? IsOther { get; set; }
        public string CommentOtherText { get; set; }
        public string BoardWhere { get; set; }
        public DateTime? WhatDate { get; set; }
        public string WhatTime { get; set; }
        public string HowLong { get; set; }
        public string License { get; set; }
        public string Comments { get; set; }
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool? IsMail { get; set; }
        public bool? IsPhone { get; set; }
        public bool? IsEmail { get; set; }
        public DateTime? AddedDateTime { get; set; }
    }
}
