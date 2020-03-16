using LogicBrokerAccess.Shared;
using System;
using System.Linq;

namespace LogicBrokerAccess.Models
{
	public class LogicBrokerOrder
	{
		public string OrderNumber { get; set; }
		public string OrderDate { get; set; }
		public LogicBrokerOrderLine[] OrderLines { get; set; }
		public LogicBrokerOrderTax[] Taxes { get; set; }
		public LogicBrokerShipmentInfo[] ShipmentInfos { get; set; }
		public LogicBrokerIdentifier Identifier { get; set; }
		public LogicBrokerShipToAddress ShipToAddress { get; set; }
		public decimal TotalAmount { get; set; }
		public string StatusCode { get; set; }
	}

	public class LogicBrokerOrderTax
	{
		public decimal TaxAmount { get; set; }
	}

	public class LogicBrokerShipmentInfo
	{
		public string CarrierCode { get; set; }
		public string ClassCode { get; set; }
	}

	public class LogicBrokerShipToAddress
	{
		public string CompanyName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string Zip { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
	}

	public enum LogicBrokerOrderStatusEnum
	{
		Unknown,
		New = 0,
		Submitted = 100,
		ReadyToAcknowledge = 150,
		Acknowledged = 200,
		ReadyToFulfill = 400,
		ReadyToShip = 500,
		ReadyToInvoice = 600,
		Complete = 1000,
		Cancelled = 1100,
		Failed = 1200,
		Ignored = 1400
	}

	public class LogicBrokerGetOrdersResponse
	{
		public LogicBrokerOrder [] Records { get; set; }
		public int TotalPages { get; set; }
		public int CurrentPage { get; set; }
	}

	public class LogicBrokerPutOrdersStatusResponse
	{
		public string [] Records { get; set; }
		public int TotalRecords { get; set; }
	}

	public static class OrderExtensions
	{
		public static Order ToSvOrder( this LogicBrokerOrder logicBrokerOrder )
		{
			return new Order
			{
				OrderNumber = logicBrokerOrder.OrderNumber,
				OrderDate = logicBrokerOrder.OrderDate.ToDateTime(),
				LogicBrokerKey = logicBrokerOrder.Identifier?.LogicbrokerKey,
				OrderLines = logicBrokerOrder.OrderLines?.Select( l => l.ToSvOrderLine() ),
				Taxes = logicBrokerOrder.Taxes,
				ShippingCarrier = logicBrokerOrder.ShipmentInfos?.FirstOrDefault().CarrierCode,
				ShippingClass = logicBrokerOrder.ShipmentInfos?.FirstOrDefault().ClassCode,
				ShipToAddress = logicBrokerOrder.ShipToAddress,
				TotalAmount = logicBrokerOrder.TotalAmount,
				StatusCode = logicBrokerOrder.StatusCode != null ? logicBrokerOrder.StatusCode.ToSvOrderStatusCode() : LogicBrokerOrderStatusEnum.Unknown
			};
		}

		public static OrderLine ToSvOrderLine( this LogicBrokerOrderLine logicBrokerOrderLine )
		{
			return new OrderLine
			{
				SupplierSku = logicBrokerOrderLine.ItemIdentifier?.SupplierSKU,
				Quantity = logicBrokerOrderLine.Quanity,
				Price = logicBrokerOrderLine.Price,
				Weight = logicBrokerOrderLine.Weight,
				Discounts = logicBrokerOrderLine.Discounts,
				Taxes = logicBrokerOrderLine.Taxes
			};
		}

		public static LogicBrokerOrderStatusEnum ToSvOrderStatusCode( this string orderStatusCode )
		{
			return ( LogicBrokerOrderStatusEnum )Enum.Parse( typeof( LogicBrokerOrderStatusEnum ), orderStatusCode );
		}
	}
}