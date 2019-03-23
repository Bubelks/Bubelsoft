using System;

namespace BubelSoft.Core.Domain.Models
{
    public class Company
    {
        public Company(CompanyId id, string name, string nip, string phoneNumber, string eMail, string city, string postCode, string street, string placeNumber)
            : this(name, nip, phoneNumber, eMail, city, postCode, street, placeNumber)
        {
            Id = id;
        }

        public Company(CompanyId id, string name): this(name)
        {
            Id = id;
        }

        public Company(string name)
        {
            Name = name;
        }

        public Company(string name, string nip, string phoneNumber, string email, string city, string postCode, string street, string placeNumber): this(name)
        {
            Nip = nip;
            PhoneNumber = phoneNumber;
            Email = email;
            City = city;
            PostCode = postCode;
            Street = street;
            PlaceNumber = placeNumber;
        }

        public CompanyId Id { get; private set; }

        public string Name { get; private set; }
        public string Nip { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string City { get; private set; }
        public string PostCode { get; private set; }
        public string Street { get; private set; }
        public string PlaceNumber { get; private set; }

        public void SetId(CompanyId id)
        {
            if (Id.Value != 0)
                throw new InvalidOperationException("Id is already set");
            Id = id;
        }

        public void Update(string name, string nip, string phoneNumber, string email, string city, string postCode,
            string street, string placeNumber)
        {
            Name = name;
            Nip = nip;
            PhoneNumber = phoneNumber;
            Email = email;
            City = city;
            PostCode = postCode;
            Street = street;
            PlaceNumber = placeNumber;
        }
    }
}