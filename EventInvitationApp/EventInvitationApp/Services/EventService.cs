using EventInvitationApp.Interface;
using EventInvitationApp.Models;

namespace EventInvitationApp.Services
{
    public class EventService : IEventsService
    {
        public async Task<Event> Add(RSVP model)
        {
            //opensession
            //transaction
            RSVP rsvp = new RSVP
            {
                GuestName = model.GuestName,
                IsAttending = Guid.NewGuid(),
                EventId = 1
            };

            transaction.Commit(); //here is yg dia akan masuk dlm db

            return model;
        }
    }
}
