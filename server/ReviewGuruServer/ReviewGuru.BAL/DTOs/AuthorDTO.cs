using ReviewGuru.BLL.Services.IServices;

namespace ReviewGuru.BLL.DTOs
{
    public record AuthorDTO : IEntity
    {
        public int Id { get; private init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
    }

}
