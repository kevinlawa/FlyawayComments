using FlyawayComments.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyawayComments.Data.Repositories
{
    public class FlyawayRepository : IFlyawayRepository
    {
        private readonly lawasitecore91prodexternaldbContext context;
        public FlyawayRepository(lawasitecore91prodexternaldbContext context)
        {
            this.context = context;
        }
        public IQueryable<LaxgroundTransportation> GetFlyawayComments(DateTime date)
        {
            return context.LaxgroundTransportation.Where(c => c.AddedDateTime >= date.Date);
        }
    }
}
