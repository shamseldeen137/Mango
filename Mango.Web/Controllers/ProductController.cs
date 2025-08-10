using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _ProductService;
        public ProductController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }


        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? list = new();

            ResponseDto? response = await _ProductService.GetAllProductsAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        } 
        public async Task<IActionResult> ProductEdit(int ProductId)
        {
            ResponseDto? response = await _ProductService.GetProductByIdAsync(ProductId);

            if (response != null && response.IsSuccess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _ProductService.CreateProductAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View( model);
        }


        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _ProductService.CreateProductAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ProductDelete(int ProductId)
        {
            ResponseDto? response = await _ProductService.GetProductByIdAsync(ProductId);

            if (response != null && response.IsSuccess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto ProductDto)
        {
            ResponseDto? response = await _ProductService.DeleteProductAsync(ProductDto.ProductId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(ProductDto);
        }

    }
}

