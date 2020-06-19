using AutoMapper;
using FlyawayComments.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyawayComment.Functions.Models
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //Only want some fields and not all the fields from the table 
            CreateMap<LaxgroundTransportation, LaxgroundTransportationDTO>();
        }
    }
}
