using ClassLibrary1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyawayComments.Data.Repositories
{
    public interface IFlyawayRepository
    {
        public IQueryable<LaxgroundTransportation> GetFlyawayComments(DateTime date);

    }
}
