using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
		[Display(Name = "Đang xử lý")]
		[Description("The order is handling")]
		InProcess = 0,
		[Display(Name = "Đang giao hàng")]
		Shipping = 1,
		[Display(Name = "Đơn đã giao")]
		Success = 2,
		[Display(Name = "Đã hủy")]
		Canceled = 3
	}
	public enum DeliveryType
	{
		[Display(Name = "Tiêu chuẩn")]
		Standard = 0,
		[Display(Name = "Siêu tốc")]
		Fast = 1
	}
	public enum PaymentType
	{
		[Display(Name = "Tiền mặt")]
		Cash = 0,
		[Display(Name = "Ngân hàng")]
		InternetBanking = 1
	}
}
