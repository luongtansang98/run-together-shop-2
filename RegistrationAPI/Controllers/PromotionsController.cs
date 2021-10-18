using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationAPI.DTO;
using RegistrationAPI.Models;
using RegistrationAPI.Utilities.Paging;

namespace RegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly AuthenticationContext _context;
        public PromotionsController(AuthenticationContext context)
        {
            _context = context;
        }

		[HttpPost]
		[Route("create")]
		public IActionResult CreateProduct(PromotionDTO promotion)
		{
			try
			{
				if (promotion == null)
				{
					return BadRequest("User object is null");
				}

				if (!ModelState.IsValid)
				{
					return BadRequest("Invalid model object");
				}
				var entity = new Promotion()
				{
					PromotionTypeId = promotion.PromotionTypeId,
					Code = promotion.Code,
					Name = promotion.Name,
					Description = promotion.Description,
					Value = promotion.Value,
					StartTime = promotion.StartTime,
					EndTime = promotion.EndTime,
					CanApplyForAll = promotion.CanApplyForAll,
					IsDisable = promotion.IsDisable,
				};

				_context.Add(entity);
				_context.SaveChanges();

				return StatusCode(201, entity);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}

		[Route("GetList")]
		[HttpPost]
		public IActionResult GetAllPromotions(SearchModel searchModel)
		{
			try
			{
				var promotions = _context.Promotions.Include(n => n.PromotionType).Select(n => new PromotionDTO()
				{
					Id = n.Id,
					Name = n.Name,
					Code = n.Code,
					Description = n.Description,
					PromotionTypeId = n.PromotionTypeId,
					PromotionTypeName = n.PromotionType.Name,
					Value = n.Value,
					StartTime = n.StartTime,
					EndTime = n.EndTime,
					CanApplyForAll = n.CanApplyForAll,
					IsDisable = n.IsDisable
				}).Where(n => n.Name.Contains(searchModel.CodeOrName));

				if (searchModel.PromotionTypeId.HasValue && searchModel.PromotionTypeId.Value > 0)
				{
					promotions = promotions.Where(n => n.PromotionTypeId == searchModel.PromotionTypeId.Value);
				}

				if (searchModel.IsDisable.HasValue)
				{
					promotions = promotions.Where(n => n.IsDisable == searchModel.IsDisable);
				}

				if (searchModel.CanApplyForAll.HasValue)
				{
					promotions = promotions.Where(n => n.CanApplyForAll == searchModel.CanApplyForAll);
				}

				if (searchModel.StartTime.HasValue)
				{
					if (searchModel.EndTime.HasValue)
					{
						promotions = promotions.Where(n => n.StartTime <= searchModel.StartTime.Value && searchModel.EndTime.Value <= n.EndTime);
					}
					else
					{
						promotions = promotions.Where(n => n.StartTime <= searchModel.StartTime.Value);
					}	
				}

				var results = PagingMethod.GetPaged(promotions, searchModel.Page, 10);

				return Ok(results);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
		[HttpGet("{promotionId}")]
		public IActionResult GetPromotionById(int promotionId)
		{
			var promotion = _context.Promotions.Include(n => n.PromotionType).FirstOrDefault(n => n.Id == promotionId);
			if (promotion == null)
				return BadRequest("Mã khuyến mãi này không tồn tại");
			var result = new PromotionDTO()
			{
				Id = promotion.Id,
				Code = promotion.Code,
				Name = promotion.Name,
				Description = promotion.Description,
				PromotionTypeId = promotion.PromotionTypeId,
				PromotionTypeName = promotion.PromotionType.Name,
				Value = promotion.Value,
				StartTime = promotion.StartTime,
				EndTime = promotion.EndTime,
				CanApplyForAll = promotion.CanApplyForAll,
				IsDisable = promotion.IsDisable,
			};
			return Ok(result);
		}

		[HttpGet("delete/{promotionId}")]
		public IActionResult DeletePromotionById(int promotionId)
		{
			var promotion = _context.Promotions.Include(n => n.PromotionType).FirstOrDefault(n => n.Id == promotionId);
			if (promotion == null)
				return BadRequest("Mã khuyến mãi này không tồn tại");

			_context.Remove(promotion);
			_context.SaveChanges();

			return Ok();
		}

		[HttpGet("get-promotion-selection")]
		public IActionResult GetPromotionSelection()
		{
			var promotions = _context.Promotions.Include(n => n.PromotionType).Select(n => new PromotionDTO()
			{
				Id = n.Id,
				Name = n.Name,
				Code = n.Code,
				Description = n.Description,
				PromotionTypeId = n.PromotionTypeId,
				PromotionTypeName = n.PromotionType.Name,
				Value = n.Value,
				StartTime = n.StartTime,
				EndTime = n.EndTime,
				CanApplyForAll = n.CanApplyForAll,
				IsDisable = n.IsDisable
			});

			return Ok(promotions);
		}
		[HttpPost]
		[Route("update")]
		public IActionResult UpdateCart(PromotionDTO promotion)
		{
			try
			{
				if (promotion == null)
				{
					return BadRequest("Mã khuyên mãi không tồn tại");
				}

				var promotionEntity = _context.Promotions.Where(c => c.Id == promotion.Id).FirstOrDefault();
				if (promotionEntity != null)
				{
					promotionEntity.Name = promotion.Name;
					promotionEntity.Description = promotion.Description;
					promotionEntity.PromotionTypeId = promotion.PromotionTypeId;
					promotionEntity.Value = promotion.Value;
					promotionEntity.StartTime = promotion.StartTime;
					promotionEntity.EndTime = promotion.EndTime;
					promotionEntity.CanApplyForAll = promotion.CanApplyForAll;
					promotionEntity.IsDisable = promotion.IsDisable;

					_context.Update(promotionEntity);
				}
				else
				{
					return BadRequest("Mã khuyên mãi không tồn tại");
				}

				_context.SaveChanges();

				return StatusCode(201);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}

		[HttpGet("generatePromotionCode")]
		public IActionResult generatePromotionCode()
		{
			var code = this.RandomString(10);
			
			return Ok(code);
		}
		public class SearchModel
		{
			public int Page { get; set; }
			public string CodeOrName { get; set; }
			public int? PromotionTypeId { get; set; }
			public bool? IsDisable { get; set; }
			public bool? CanApplyForAll { get; set; }
			public DateTime? StartTime { get; set; }
			public DateTime? EndTime { get; set; }
		}

		public string RandomString(int length)
		{
			Random random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}