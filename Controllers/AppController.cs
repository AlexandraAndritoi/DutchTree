using DutchTree.ViewModels;
using DutchTree.Services;
using Microsoft.AspNetCore.Mvc;
using DutchTree.Data;
using Microsoft.AspNetCore.Authorization;

namespace DutchTree.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService mailService;
        private readonly IDutchRepository dutchRepository;

        public AppController(IMailService mailService, IDutchRepository dutchRepository)
        {
            this.mailService = mailService;
            this.dutchRepository = dutchRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Send the email
                mailService.SendMessage("alexandra@alexandra", model.Subject, $"From: {model.Name} - Message: {model.Message}");
                ViewBag.UserMessage = "Mail Sent";
                ModelState.Clear();
            }
            return View();
        }

        [HttpGet("about")]
        public IActionResult About()
        {
            ViewBag.Title = "About Us";

            return View();
        }

        [HttpGet("shop")]
        public IActionResult Shop()
        {
            return View();
        }
    }
}
