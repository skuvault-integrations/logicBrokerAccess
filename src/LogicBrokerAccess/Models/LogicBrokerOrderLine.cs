namespace LogicBrokerAccess.Models
{
	public class LogicBrokerOrderLine
	{
		public int Quanity { get; set; }
		public LogicBrokerItemIdentifier ItemIdentifier { get; set; }
		public decimal Price { get; set; }
		public LogicBrokerItemDiscount[] Discounts { get; set; }
		public LogicBrokerItemTax[] Taxes { get; set; }
		public decimal Weight { get; set; }
	}

	public class LogicBrokerItemIdentifier
	{
		public string SupplierSKU { get; set; }
	}

	public class LogicBrokerItemDiscount
	{
		public decimal DiscountAmount { get; set; }
	}

	public class LogicBrokerItemTax
	{
		public decimal TaxAmount { get; set; }
	}
}
