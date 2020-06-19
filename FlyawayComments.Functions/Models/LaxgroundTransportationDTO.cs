using System;
using System.Collections.Generic;
using System.Text;

namespace FlyawayComments.Functions.Models
{
    public class LaxgroundTransportationDTO
    {
        public int TransportId { get; set; }
        public string ServiceType { get; set; }
        public string ServiceSubType { get; set; }
        public string servicetypeother { get; set; }
        public bool? istaxicab { get; set; }
        public bool? isservicedelay { get; set; }
        public bool? isdrivermisconduct { get; set; }
        public bool? isamericanscomp { get; set; }
        public bool? isovercharge { get; set; }
        public bool? issolicitation { get; set; }
        public bool? IsVehicle { get; set; }
        // public bool? IsOther { get; set; } //comment out to demo automapper
        //  public string CommentOtherText { get; set; } //comment out to demo automapper
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
