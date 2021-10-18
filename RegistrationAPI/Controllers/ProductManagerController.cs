using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
	public class ProductManagerController : ControllerBase
	{
		private readonly AuthenticationContext _context;
		public ProductManagerController(AuthenticationContext context)
		{
			_context = context;
		}
		[Route("Upload")]
		[HttpPost, DisableRequestSizeLimit]
		public IActionResult Upload()
		{
			try
			{
				var file = Request.Form.Files[0];
				var folderName = Path.Combine("Resources", "Images", "Employees");
				var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
				if (file.Length > 0)
				{
					var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;

					fileName = fileName.Trim('"');
					var fullPath = Path.Combine(pathToSave, fileName);
					var dbPath = Path.Combine(folderName, fileName);
					using (var stream = new FileStream(fullPath, FileMode.Create))
					{
						file.CopyTo(stream);
					}
					return Ok(new { dbPath });
				}
				else
				{
					return BadRequest();
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
		[Route("UploadMultipleFiles")]
		[HttpPost, DisableRequestSizeLimit]
		public IActionResult UploadMultipleFiles()
		{
			try
			{
				var files = Request.Form.Files;
				var folderName = Path.Combine("Resources", "Images", "Products");
				var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

				if (files.Any(f => f.Length == 0))
				{
					return BadRequest();
				}
				string path = "";
				foreach (var file in files)
				{
					var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
					var fullPath = Path.Combine(pathToSave, fileName);
					var dbPath = Path.Combine(folderName, fileName); //you can add this path to a list and then return all dbPaths to the client if require
					path = dbPath;
					using (var stream = new FileStream(fullPath, FileMode.Create))
					{
						file.CopyTo(stream);
					}
				}
				return Ok(new { path });
				//return Ok(path);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error");
			}
		}
		[HttpPost]
		[Route("create")]
		public IActionResult CreateProduct(ProductDTO product)
		{
			try
			{
				if (product == null)
				{
					return BadRequest("User object is null");
				}

				if (!ModelState.IsValid)
				{
					return BadRequest("Invalid model object");
				}
				var entity = new Product()
				{
					CategoryId = product.CategoryId,
					ColorId = product.ColorId,
					Description = product.Description,
					Name = product.Name,
					Code = product.Code,
					PriceExport = product.PriceExport,
					PriceImport = product.PriceImport,
					ProductGroupId = product.ProductGroupId,
				};

				int i = 1;
				var images = this.RemoveDuplicates(product.ImagesList);
				entity.ImagesList = images.Select(n => new ImageProduct()
				{
					ImagePath = n.ImagePath,
					Order = i++,
				}).ToList();

				if (product.PromotionList.Count > 0)
				{
					var promotion = _context.Promotions.FirstOrDefault(n => n.Id == product.PromotionList.First());
					if(promotion == null)
					{
						return BadRequest("Mã khuyến mãi không tồn tại");
					}
					var promotionInsert = new PromotionProduct { Product = entity, Promotion = promotion };
					_context.PromotionProducts.Add(promotionInsert);
				}
				_context.Add(entity);
				_context.SaveChanges();

				return StatusCode(201);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
		public class SearchModel
		{
			public int Page { get; set; }
			public string CodeOrNameProduct { get; set; }
			public int? GroupProductId { get; set; }
			public int? CategoryId { get; set; }
			public int? ProductGroupId { get; set; }
			public bool IsClientSide { get; set; }
			public int? FilterId { get; set; }
		}
		List<ImageProductDTO> RemoveDuplicates(List<ImageProductDTO> items)
		{
			List<ImageProductDTO> results = new List<ImageProductDTO>();
			for (int i = 0; i < items.Count; i++)
			{
				// Assume not duplicate.
				bool duplicate = false;
				for (int z = 0; z < i; z++)
				{
					if (items[z].ImagePath == items[i].ImagePath)
					{
						// This is a duplicate.
						duplicate = true;
						break;
					}
				}
				// If not duplicate, add to result.
				if (!duplicate)
				{
					results.Add(items[i]);
				}
			}
			return results;
		}
		[Route("GetList")]
		[HttpPost]
		public async Task<IActionResult> GetAllProductsAsync(SearchModel searchModel)
		{
			try
			{
				const string sqlQuery = "select p.id, p.Name, p.Code,p.Description,p.ProductGroupId,p.ColorId, " +
											"p.CategoryId,p.PriceExport,p.PriceImport,COUNT(ipp.ProductId) as CountImage, " +
											"MIN(ipp.ImagePath) as FirstImagePath, pp.PromotionId,prom.Value, prom.PromotionTypeId " +
											"from Products p " +
											"left join PromotionProducts pp on p.Id = pp.ProductId " +
											"left join Promotions prom on pp.PromotionId = prom.Id " +
											"left join ImageProduct ipp on p.id = ipp.ProductId " +
											"group by p.id, p.Name, p.Code, p.Description, p.ProductGroupId, p.ColorId, p.CategoryId, p.PriceExport, p.PriceImport, pp.PromotionId, prom.Value, prom.PromotionTypeId";

				var products = _context.ProductQueries.FromSql(sqlQuery);

				//var products = _context.Products.Include(n => n.ImagesList).Select(n => new ProductDTO()
				//{
				//	Id = n.Id,
				//	Name = n.Name,
				//	Code = n.Code,
				//	Description = n.Description,
				//	ProductGroupId = n.ProductGroupId,
				//	ColorId = n.ColorId,
				//	CategoryId = n.CategoryId,
				//	PriceExport = n.PriceExport,
				//	PriceImport = n.PriceImport,
				//	CountImage = n.ImagesList.Count(),
				//	FirseImagePath = n.ImagesList.Any() ? n.ImagesList.Select(i => i.ImagePath).FirstOrDefault() : null
				//}).Where(n => n.Name.Contains(searchModel.CodeOrNameProduct));

				if(!string.IsNullOrEmpty(searchModel.CodeOrNameProduct))
				{
					products = products.Where(n => n.Name.Contains(searchModel.CodeOrNameProduct));
				}
				if (searchModel.ProductGroupId.HasValue && searchModel.ProductGroupId.Value > 0)
				{
					products = products.Where(n => n.ProductGroupId == searchModel.ProductGroupId.Value);
				}
				//  && (searchModel.PositionId.HasValue && searchModel.PositionId.Value > 0 ? n.PositionId == searchModel.PositionId : true));
				if (searchModel.FilterId == 4)
					products = products.OrderBy(n => n.PriceExport);
				if (searchModel.FilterId == 5)
					products = products.OrderByDescending(n => n.PriceExport);

				var results = PagingMethod.GetPaged(products, searchModel.Page, 10, searchModel.IsClientSide);

				foreach (var item in results.Results)
				{
					if (item.PromotionId.HasValue && item.PromotionId.Value > 0)
					{
						if (item.PromotionTypeId.HasValue && item.PromotionTypeId.Value == 1)
						{
							//discount by %
							var discount = item.PriceExport - (item.PriceExport * double.Parse(item.Value) / 100);
							item.PriceWithDiscount = discount;
						}
						else
						{
							//discount by money
							var discount = item.PriceExport - double.Parse(item.Value);
							item.PriceWithDiscount = discount;
						}
					}
				}
				return Ok(results);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
		[HttpGet("{productId}")]
		public IActionResult GetProductById(int productId)
		{
			var product = _context.Products.Include(n => n.ImagesList).FirstOrDefault(n => n.Id == productId);
			if (product == null)
				return BadRequest("Sản phẩm không tồn tại");
			var result = new ProductDTO()
			{
				Id = product.Id,
				Name = product.Name,
				ImagesList = product.ImagesList.Select(n => new ImageProductDTO()
				{
					Id = n.Id,
					ImagePath = n.ImagePath.Replace('\\', '/'),
					Order = n.Order,
					ProductId = n.ProductId
				}).ToList(),
				PriceExport = product.PriceExport,
				PriceImport = product.PriceImport,
				ProductGroupId = product.ProductGroupId,
				ColorId = product.ColorId,
				CategoryId = product.CategoryId,
				Code = product.Code,
				CountImage = product.ImagesList.Count(),
				Description = product.Description,
				Sizes = this.generateProductSizes(product.Id)
			};
			var promotionProduct = _context.PromotionProducts.Include(n => n.Promotion).Where(n => n.ProductId == productId).ToList();
			if(promotionProduct.Count() > 0)
			{
				double discountTotal = 0;
				foreach(var item in promotionProduct)
				{
					if(item.Promotion.PromotionTypeId == 1)
					{
						//discount by %
						var discount =(result.PriceExport * double.Parse(item.Promotion.Value) / 100);
						discountTotal += discount;
					} 
					else
					{
						//discount by money
						var discount = double.Parse(item.Promotion.Value);
						discountTotal += discount;
					}
					
				}
				result.PriceWithDiscount = result.PriceExport - discountTotal;
			}
			return  Ok(result);
		}
		[HttpGet("imagesProduct/{imagesOfProductId}")]
		public IActionResult GetImagesOfProductById(int imagesOfProductId)
		{
			var product = _context.Products.Include(n => n.ImagesList).FirstOrDefault(n => n.Id == imagesOfProductId);
			if (product == null)
				return BadRequest("Sản phẩm không tồn tại");
			var result = product.ImagesList.Select(n => new ImageProductDTO()
			{
				Id = n.Id,
				ImagePath = n.ImagePath.Replace('\\', '/'),
				Order = n.Order,
				ProductId = n.ProductId
			}).ToList();
			return Ok(result);
		}
		public List<ProductSizeDTO> generateProductSizes(int productId)
		{
			var lstResult = new List<ProductSizeDTO>();
			for(int i = 0 ; i < 5; i++ )
			{
				var obj = new ProductSizeDTO(40 + i, i , productId, (i == 0 ? true : false));
				lstResult.Add(obj);
			}
			return lstResult;
		}
	}
}