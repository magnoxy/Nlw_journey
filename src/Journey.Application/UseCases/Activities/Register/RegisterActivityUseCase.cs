using FluentValidation.Results;
using Journey.Communication.Requests;
using Journey.Communication.Responses;
using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Journey.Infrastructure;
using Journey.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Journey.Application.UseCases.Activities.Register
{
    public class RegisterActivityUseCase
    {
        public ResponseActivityJson Execute(Guid tripId, RequestRegisterActivityJson request)
        {
            var dbContext = new JourneyDbContext();

            var trip = dbContext
                .Trips
                .FirstOrDefault(trip => trip.Id == tripId);

            validate(trip, request);

            var entity = new Activity { Name = request.Name, Date = request.Date, TripId = tripId };

            dbContext.Activities.Add(entity);
            dbContext.SaveChanges();

            return new ResponseActivityJson
            {
                Id = entity.Id,
                Name = entity.Name,
                Date = entity.Date,
                Status = (Communication.Enums.ActivityStatus)entity.Status
            };
        }

        private void validate(Trip? trip, RequestRegisterActivityJson request)
        {
            var validator = new RegisterActivityValidator();
            var result = validator.Validate(request);

            if (trip is null)
            {
                throw new NotFoundException(ResourceErrorMessages.TRIP_NOT_FOUND);
            }

            if (!(request.Date >= trip.StartDate && request.Date <= trip.EndDate))
            {
                result.Errors.Add(
                    new ValidationFailure(
                        "Date",
                        ResourceErrorMessages.DATE_NOT_WITHIN_TRAVEL_PERIOD
                    )
                );
            }

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
