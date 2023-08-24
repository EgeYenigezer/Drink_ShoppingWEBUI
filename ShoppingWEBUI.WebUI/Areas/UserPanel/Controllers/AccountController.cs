using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using ShoppingWEBUI.Core.DTO;
using ShoppingWEBUI.Core.Result;
using ShoppingWEBUI.Helper.SessionHelper;
using System.Net;

namespace ShoppingWEBUI.WebUI.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    public class AccountController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("/UserAccount/Login")]
        public IActionResult Index()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            return View();
        }
        [HttpPost("/Account/UserLogin")]
        public async Task<IActionResult> UserLogin(LoginDTO loginDTO)
        {
            var url = "http://localhost:5183/Login";
            var client = new RestClient(url);
            var request = new RestRequest(url, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            var body = JsonConvert.SerializeObject(loginDTO);
            request.AddBody(body, "application/json");
            RestResponse restResponse = await client.ExecuteAsync(request);

            var responseObject = JsonConvert.DeserializeObject<ApiResult<LoginDTO>>(restResponse.Content);

            if (restResponse.StatusCode == HttpStatusCode.NotFound && responseObject?.Data == null)
            {
                ViewBag.LoginError = "Buraya Bir Data Geliyor";
                ViewData["LoginError"] = "Kullanıcı Adı Veya Şifre Yanlış";
                TempData["LoginError"] = "Buraya Başka Bir Data Geliyor";
                return View("Index");
            }
            else if (restResponse.StatusCode != HttpStatusCode.OK)
            {
                ViewData["LoginError"] = "Hata Oluştu";
                return View("Index");
            }

            

            SessionManager.LoggedUser = responseObject.Data;

            return RedirectToAction("Index", "Home");
        }
    }
}
