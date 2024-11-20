namespace WebDevMasterClass.Web.Models;

public class AddOrderModel
{
    public required Item[] Items { get; set; }
    public required Address DeliveryAddress { get; set; }
    public required Address BillingAddress { get; set; }

    public class Item
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class Address
    {
        public required string Name { get; set; }
        public required string Street1 { get; set; }
        public string? Street2 { get; set; }
        public required string PostalCode { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
    }
}
