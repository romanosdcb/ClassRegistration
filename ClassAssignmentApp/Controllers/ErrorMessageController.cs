using ClassAssignmentApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClassAssignmentApp.Controllers
{
    public class ErrorMessageController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7008/api");
        private readonly HttpClient _client;

        public ErrorMessageController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index(string ErrorMessage, string? AddressDescription)
        {
            ErrorMessageModel ErrorModel = new ErrorMessageModel();
            ErrorModel.ErrorMessage = ErrorMessage;

            if (AddressDescription != null)
            {
                ErrorModel.AddressDescription = AddressDescription;
            }
            else
            {
                ErrorModel.AddressDescription = "";
            }

            return View(ErrorModel);
        }
    }
}
