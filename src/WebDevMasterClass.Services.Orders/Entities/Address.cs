namespace WebDevMasterClass.Services.Orders.Entities;

public abstract class Address
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Address() { } // For EF Core
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    protected Address(string name, string street1, string? street2,
                    string postalCode, string city, string country)
    {
        Name = name;
        Street1 = street1;
        Street2 = street2;
        PostalCode = postalCode;
        City = city;
        Country = country;
    }

    public string Name { get; private set; }
    public string Street1 { get; private set; }
    public string? Street2 { get; private set; }
    public string PostalCode { get; private set; }
    public string City { get; private set; }
    public string Country { get; private set; }
}
public class BillingAddress : Address
{
    private BillingAddress() { } // For EF Core
    private BillingAddress(string name, string street1, string? street2,
                        string postalCode, string city, string country)
        : base(name, street1, street2, postalCode, city, country)
    {
    }

    public static BillingAddress Create(string name, string street1, string? street2,
                        string postalCode, string city, string country)
        => new BillingAddress(name, street1, street2, postalCode, city, country);
}
public class DeliveryAddress : Address
{
    private DeliveryAddress() { } // For EF Core
    private DeliveryAddress(string name, string street1, string? street2,
                        string postalCode, string city, string country)
        : base(name, street1, street2, postalCode, city, country)
    {
    }

    public static DeliveryAddress Create(string name, string street1, string? street2,
                        string postalCode, string city, string country)
        => new DeliveryAddress(name, street1, street2, postalCode, city, country);
}
