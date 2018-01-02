using System;

namespace WebApi.Domain.Models
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

        public Company(string name, string nip, string phoneNumber, string eMail, string city, string postCode, string street, string placeNumber): this(name)
        {
            Nip = nip;
            PhoneNumber = phoneNumber;
            EMail = eMail;
            City = city;
            PostCode = postCode;
            Street = street;
            PlaceNumber = placeNumber;
        }

        public CompanyId Id { get; private set; }

        public string Name { get; }
        public string Nip { get; }
        public string PhoneNumber { get; }
        public string EMail { get; }
        public string City { get; }
        public string PostCode { get; }
        public string Street { get; }
        public string PlaceNumber { get; }

        public void SetId(CompanyId id)
        {
            if (Id.Value != 0)
                throw new InvalidOperationException("Id is already set");
            Id = id;
        }
    }

    public struct CompanyId
    {
        public CompanyId(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}