namespace Data.EntityModels
{
    public class Branches
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? CityId { get; set; }
        public City City { get; set; }
        public string CityName { get; set; }

        public string Adress { get; set; }
        public string PhoneNumber { get; set; }

        public bool Active { get; set; } 
    }
}
