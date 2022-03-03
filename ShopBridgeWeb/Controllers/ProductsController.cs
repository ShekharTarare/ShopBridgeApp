using Microsoft.AspNetCore.Mvc;
using ShopBridgeWeb.Models;
using ShopBridgeWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeWeb.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
       
        public IActionResult Index()
        {
            return View(new Product() { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Product obj = new Product();
            if(id == null)
            {
                //This will be true for insert and Create
                return View(obj);
            }
            //This will be for update
            obj = await _productRepository.GetAsync(StaticDetails.ProductAPI, id.GetValueOrDefault());
            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Product obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    await _productRepository.CreateAsync(StaticDetails.ProductAPI, obj);
                }
                else
                {
                    await _productRepository.UpdateAsync(StaticDetails.ProductAPI + obj.Id, obj);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(obj);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _productRepository.DeleteAsync(StaticDetails.ProductAPI, id);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }

        public async Task<IActionResult> GetAllProducts()
        {
            return Json(new { data = await _productRepository.GetAllAsync(StaticDetails.ProductAPI) });
        }
    }
}
