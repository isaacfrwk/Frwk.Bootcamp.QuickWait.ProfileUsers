namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities
{
    public class Address
    {
        public Guid UserId { get; set; }
        public string Street { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Number { get; set; }
        public string CEP { get; set; }

        public User User { get; set; }
    }
}
