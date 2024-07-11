using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Journey.Infrastructure;

namespace Journey.Application.UseCases.Activities.Delete
{
    public class DeleteActivityUseCase
    {
        public void Execute(Guid tripId, Guid ActivityId)
        {
            var dbContext = new JourneyDbContext();
            var activity = dbContext.Activities.FirstOrDefault(activity =>
                activity.Id == ActivityId && activity.TripId == tripId
            );

            if(activity == null)
            {
                throw new NotFoundException(ResourceErrorMessages.ACTIVITY_NOT_FOUND);
            }

            dbContext.Activities.Remove(activity);
            dbContext.SaveChanges();
        }
    }
}
