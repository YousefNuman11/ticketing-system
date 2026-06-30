namespace TicketingSystem.Repository.Search
{
    public interface IUserSearchService
    {
        void IndexUser(Guid id, string fullName, string email, string? address, string role);
        void DeleteUser(Guid id);
        List<Guid> Search(string queryText, string? roleFilter = null, int maxResult = 200);
    }
}
