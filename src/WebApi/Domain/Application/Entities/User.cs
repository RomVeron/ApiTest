namespace Api.Test.Domain.Application.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Addres Address { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public Company Company { get; set; }
    }

    public class Addres
    {
        public string Street { get; set; }
        public string Suite { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public Geo Geo { get; set; }
    }

    public class Geo
    {
        public string Lat { get; set; }
        public string Lng { get; set; }
    }

    public class Company
    {
        public string Name { get; set; }

        public string CatchPhrase { get; set; }

        public string Bs { get; set; }
    }
}
