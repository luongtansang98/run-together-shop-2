using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.Models
{
	public class Order:BaseEntity
	{
		public Order()
		{
			this.OrderDetails = new List<OrderDetail>();
		}
		public string Code { get; set; }
		public DateTime OrderDate { get; set; }
		public int CustomerId { get; set; }
		[ForeignKey("CustomerId")]
		public Customer Customer { get; set; }
		public string Note { get; set; }
		public string ShipName { get; set; }
		public string ShipAddress { get; set; }
		public string ShipEmail { get; set; }
		public string ShipPhone { get; set; }
		public OrderStatus Status { get; set; } = OrderStatus.InProcess;
		public DeliveryType DeliveryType { get; set; } = DeliveryType.Standard;
		public PaymentType PaymentType { get; set; } = PaymentType.Cash;
		public List<OrderDetail> OrderDetails { get; set; }
	}
	public enum OrderStatus
	{
		InProcess = 0,
		Shipping = 1,
		Success = 2,
		Canceled = 3
	}
	public enum DeliveryType
	{
		Standard = 0,
		Fast = 1
	}
	public enum PaymentType
	{
		Cash = 0,
		InternetBanking = 1
	}
}
