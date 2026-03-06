namespace EventInvitationApp.Interface
{
    public interface IEventsService
    {
        // Define method signatures for event-related operations
        Task<IEnumerable<Events>> GetAllEventsAsync();
        Task<Events> GetEventByIdAsync(int id);
        Task CreateEventAsync(Events newEvent);
        Task UpdateEventAsync(Events updatedEvent);
        public Task DeleteEventAsync(int id);
    }
}
