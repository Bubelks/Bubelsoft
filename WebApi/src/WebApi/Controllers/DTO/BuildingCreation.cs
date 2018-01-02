namespace WebApi.Controllers.DTO
{
    public class BuildingCreation
    {
        public Company Company { get; set; }

        public string Name { get; set; }
    }

    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NIP { get; set; }
        public string PhoneNumber { get; set; }
        public string EMail { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; }
        public string PlaceNumber { get; set; }
    }
}