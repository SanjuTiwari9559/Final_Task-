

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Task_Cat_ProMvc.Models;
using Task_Cat_ProMvc.Models.Data;
using Task_Cat_ProMvc.Models.viewModel;
using Task_Cat_ProMvc.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Task_Cat_ProMvc.Controllers
{
    public class CategoryController : Controller
    {

        //Uri baseUrl = new Uri("https://localhost:7220/api/Category");
        private readonly ICategoryMvc _categoryService;
        private readonly HttpClient _client;
        //private readonly Cat_ProductDbContext cat_ProductDbContext;
        // private readonly bool _useApi=true ; 
        // private readonly bool _useMvc=true ;
        // private readonly bool apiaction;


        private readonly HttpClient client;


        public CategoryController(ICategoryMvc category)
        {
            this._categoryService = category;

            this.client = new HttpClient();



        }



        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List(int pageNo = 1, int PageSize = 20)
        {
            //var APIorMVC= cat_ProductDbContext.CallApi.Where(x => x.IsActive == true);
            //  if(APIorMVC.Any())
            // {
            //var response = await client.GetAsync($"{baseUrl}?pageNo={pageNo}&pageSize={PageSize}");
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = await response.Content.ReadAsStringAsync();
            //    var newcategory = JsonConvert.DeserializeObject<List<Category>>(jsonData);
            //    return View(newcategory);
            //}
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync(pageNo, PageSize);
                return View(categories);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return View(ex.Message);
            }

        }



        //var categories = await newcategory.GetAllCategoriesAsync(pageNo, PageSize);
        //HttpResponseMessage responce = client.GetAsync(client.BaseAddress + "/Category/GetCategoriesAsync").Result;
        //if (responce.IsSuccessStatusCode)
        //{
        //    string Data = responce.Content.ReadAsStringAsync().Result;

        //    return View(JsonConvert.DeserializeObject<List<Product>>(Data));
        //}
        // return View(categories);





        [HttpGet]
        [Route("Add")]
        public async Task<IActionResult> Add()
        {
            return View();
        }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(addCategoryRequest addCategoryRequest)
        {
            if (!ModelState.IsValid) return View(addCategoryRequest);

            try
            {
                var category = new Category
                {
                    Name = addCategoryRequest.Name,
                    IsActive = addCategoryRequest.IsActive,
                };
                await _categoryService.AddCategoryAsync(category);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return View(ex.Message);
            }
        }
        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                if (category == null) return NotFound();

                var editRequest = new editCategoryRequest
                {
                    Id = id,
                    IsActive = category.IsActive,
                    Name = category.Name,
                };
                return View(editRequest);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return View(ex.Message);
            }
        }
        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit(editCategoryRequest editCategoryRequest)
        {
            if (!ModelState.IsValid) return View(editCategoryRequest);

            try
            {
                var category = new Category
                {
                    Id = editCategoryRequest.Id,
                    Name = editCategoryRequest.Name,
                    IsActive = editCategoryRequest.IsActive,
                };
                var result = await _categoryService.UpdateCategoryAsync(editCategoryRequest.Id, category);
                if (!result) return NotFound();

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return View(ex.Message);
            }
        }



        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteCategory deleteViewModel)
        {
            try
            {
                var result = await _categoryService.DaleteCategoryAsyn(deleteViewModel.Id);
                if (!result) return NotFound();

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return View(ex.Message);
            }
        }
    }
}
      
    

