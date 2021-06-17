using RegistrationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.DTO
{
	public class OrderDTO
	{
		public int? Id { get; set; }
		public DateTime OrderDate { get; set; }
		public int CustomerId { get; set; }
		public CustomerDTO Customer { get; set; }
		public string Note { get; set; }
		public string ShipName { get; set; }
		public string ShipAddress { get; set; }
		public string ShipEmail { get; set; }
		public string ShipPhone { get; set; }
		public OrderStatus Status { get; set; } = OrderStatus.InProcess;
		public List<OrderDetailDTO> OrderDetails { get; set; }
		public double TotalPrice { get; set; }
		public DeliveryType DeliveryType { get; set; }
		public PaymentType PaymentType { get; set; }
	}
}
