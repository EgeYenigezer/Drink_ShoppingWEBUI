using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using ShoppingWEBUI.Core.DTO;
using ShoppingWEBUI.Core.Result;
using ShoppingWEBUI.Core.ViewModel;
using ShoppingWEBUI.Helper.SessionHelper;

namespace ShoppingWEBUI.WebUI.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("/User/Anasayfa")]
        public async Task<IActionResult> Index()
        {
            var url = "http://localhost:5183/Products";
            var client = new RestClient(url);
            var request = new RestRequest(url, Method.Get);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + SessionManager.LoggedUser.Token);
            RestResponse restResponse = await client.ExecuteAsync(request);

            var responseObject = JsonConvert.DeserializeObject<ApiResult<List<ProductDTO>>>(restResponse.Content);

            var products = responseObject.Data;


            return View(products);
        }


        //public async Task<List<ProductDTO>> GetProductList()
        //{
        //    var url = "http://localhost:5183/Products";
        //    var client = new RestClient(url);
        //    var request = new RestRequest(url, Method.Get);
        //    request.AddHeader("Content-Type", "application/json");
        //    request.AddHeader("Authorization", "Bearer " + SessionManager.LoggedUser.Token);
        //    RestResponse restResponse = await client.ExecuteAsync(request);

        //    var responseObject = JsonConvert.DeserializeObject<ApiResult<List<ProductDTO>>>(restResponse.Content);

        //    var products = responseObject.Data;

        //    return products;
        //}
    }
}
