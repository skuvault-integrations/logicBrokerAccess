using LogicBrokerAccess.Models;
using NUnit.Framework;
using System.Collections.Generic;
using FluentAssertions;
using System.Linq;
using System;
using LogicBrokerAccess.Shared;

namespace LogicBrokerAccessTests
{
	[ TestFixture ]
	public class OrderMapperTests
	{
		[ Test ]
		public void ToSvOrder()
		{
			var orderNumber = "TEST12345";
			var documentDate = DateTime.UtcNow;
			var orderTaxAmount = 12.3m;
			var orderLines = new []
			{
				new LogicBrokerOrderLine()
			};
			var taxes = new []
			{
				new LogicBrokerOrderTax
				{
					TaxAmount = orderTaxAmount
				}
			};
			var logicBrokerIdentifier = new LogicBrokerIdentifier
			{
				LogicbrokerKey = "123123KLAE"
			};
			var shippingCarrier = "FDEG";
			var shippingClass = "FDE-2";
			var logicBrokerShipmentInfos = new []
			{
				new LogicBrokerShipmentInfo
				{
					CarrierCode = shippingCarrier,
					ClassCode = shippingClass
				}
			};
			var totalAmount = 12.30m;
			var statusCode = "123";
			var order = new LogicBrokerOrder
			{
				OrderNumber = orderNumber,
				DocumentDate = documentDate.ToStringUtcIso8601(),
				OrderLines = orderLines,
				Taxes = taxes,
				Identifier = logicBrokerIdentifier,
				ShipmentInfos = logicBrokerShipmentInfos,
				TotalAmount = totalAmount,
				StatusCode = statusCode
			};

			var svOrder = order.ToSvOrder();

			svOrder.OrderNumber.Should().Be( orderNumber );
			svOrder.DocumentDate.Should().Be( documentDate );
			svOrder.LogicBrokerKey.Should().Be( logicBrokerIdentifier.LogicbrokerKey );
			svOrder.OrderLines.Count().Should().Be( orderLines.Count() );
			svOrder.Taxes.Should().BeEquivalentTo( taxes.ToList() );
			svOrder.ShippingCarrier.Should().Be( shippingCarrier );
			svOrder.ShippingClass.Should().Be( shippingClass );
			svOrder.TotalAmount.Should().Be( totalAmount );
			svOrder.StatusCode.Should().Be( statusCode.ToSvOrderStatusCode() );
		}

		[ Test ]
		public void ToSvOrder_ShipToAddress()
		{
			var companyName = "ACME";
			var firstName = "Bubba";
			var lastName = "Gump";
			var address1 = "123 Some St";
			var address2 = "Apt 2";
			var city = "Mayberry";
			var state = "AZ";
			var country = "US";
			var zip = "12345";
			var phone = "111-222-3333";
			var email = "bob@bob.bob";
			var shipToAddress = new LogicBrokerShipToAddress
			{
				CompanyName = companyName,
				FirstName = firstName,
				LastName = lastName,
				Address1 = address1,
				Address2 = address2,
				City = city,
				State = state,
				Country = country,
				Zip = zip,
				Phone = phone,
				Email = email
			};
			var order = new LogicBrokerOrder
			{
				ShipToAddress = shipToAddress
			};

			var svOrder = order.ToSvOrder();

			svOrder.ShipToAddress.CompanyName.Should().Be( companyName );
			svOrder.ShipToAddress.FirstName.Should().Be( firstName );
			svOrder.ShipToAddress.LastName.Should().Be( lastName );
			svOrder.ShipToAddress.Address1.Should().Be( address1 );
			svOrder.ShipToAddress.Address2.Should().Be( address2 );
			svOrder.ShipToAddress.City.Should().Be( city );
			svOrder.ShipToAddress.State.Should().Be( state );
			svOrder.ShipToAddress.Country.Should().Be( country );
			svOrder.ShipToAddress.Zip.Should().Be( zip );
			svOrder.ShipToAddress.Phone.Should().Be( phone );
			svOrder.ShipToAddress.Email.Should().Be( email );
		}

		[ Test ]
		public void ToSvOrderLine()
		{
			var testSku = "testSku1";
			var quantity = 12;
			var price = 2.3m;
			var weight = 4.5m;
			var discountAmount = 1.23m;
			var taxAmount = 0.32m;
			var orderLine = new LogicBrokerOrderLine
			{
				Discounts = new List< LogicBrokerItemDiscount >
				{
					new LogicBrokerItemDiscount
					{
						DiscountAmount = discountAmount
					}
				}.ToArray(),
				ItemIdentifier = new LogicBrokerItemIdentifier
				{
					SupplierSKU = testSku
				}, 
				Price = price,
				Quanity = quantity,
				Taxes = new List< LogicBrokerItemTax >
				{ 
					new LogicBrokerItemTax
					{
						TaxAmount = taxAmount
					}
				}.ToArray(),
				Weight = weight
			};

			var svOrderLine = orderLine.ToSvOrderLine();

			svOrderLine.SupplierSku.Should().Be( testSku );
			svOrderLine.Quantity.Should().Be( quantity );
			svOrderLine.Price.Should().Be( price );
			svOrderLine.Weight.Should().Be( weight );
			svOrderLine.Discounts.First().DiscountAmount.Should().Be( discountAmount );
			svOrderLine.Taxes.First().TaxAmount.Should().Be( taxAmount );
		}

		[ Test ]
		public void ToSvOrderStatusCode()
		{
			var orderStatusCode = "150";

			var svStatusCode = orderStatusCode.ToSvOrderStatusCode();

			svStatusCode.Should().Be( LogicBrokerOrderStatusEnum.ReadyToAcknowledge );
		}
	}
}
