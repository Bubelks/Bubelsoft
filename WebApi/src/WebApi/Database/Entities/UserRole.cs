namespace WebApi.Database.Entities
{
    public class UserRole
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int BuildingId { get; set; }
        public int CompanyId { get; set; }
        public BuildingCompany Building { get; set; }

        public Role Role { get; set; }
    }

    public enum Role
    {
        Admin,
        Reporter
    }
}