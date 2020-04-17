using System.Collections.Generic;

namespace LogicBrokerAccess.Models
{
	public class OrderLine
	{
		public string SupplierSku { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal Weight { get; set; }
		public IEnumerable< LogicBrokerItemDiscount > Discounts { get; set; }
		public IEnumerable< LogicBrokerItemTax > Taxes { get; set; }
	}
}