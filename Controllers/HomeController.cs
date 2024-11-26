using Infinicare_Ojash_Devkota.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Infinicare_Ojash_Devkota.Services;
using Infinicare_Ojash_Devkota.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Infinicare_Ojash_Devkota.Controllers {
	public class HomeController : Controller {
		private readonly ILogger<HomeController> _logger;
		private readonly UserServices _userServices;
		private readonly TransactionServices _transactionServices;

		public HomeController(
			ILogger<HomeController> logger, UserServices userServices, TransactionServices transactionServices) {
			_logger = logger;
			_userServices = userServices;	
			_transactionServices = transactionServices;
		}

		public IActionResult Index() {
			if (!User.Identity.IsAuthenticated) {
				return RedirectToAction("Signup", "Home");
			}
			return View();
		}

		[HttpGet("/Signup")]
		public IActionResult Signup() {
			if (User.Identity.IsAuthenticated) {
				return RedirectToAction("Index", "Home");
			}
			return View();
		}


		[HttpPost("Signup")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Signup(UserInfoModel userInfoModel) {
			TempData["Error"] = null;
			if (_userServices.CheckIfUserExists(userInfoModel.UserCredentials.UserName)) {
				TempData["Error"] = "Username already exists!";
			}
			
			if (_userServices.RegisterUser(userInfoModel)) {
				var claims = new List<Claim> {
					new Claim(ClaimTypes.Name, userInfoModel.UserCredentials.UserName)
				};
                				
				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var authProperties = new AuthenticationProperties() {
					IsPersistent = true,
					ExpiresUtc = DateTimeOffset.UtcNow.AddDays(3),
				};
				
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);	
				return RedirectToAction("Index", "Home");
			}
			else {
				TempData["Error"] = "Something went wrong!";
			}	
			return View(userInfoModel);
		}
		
		[HttpGet("/Login")]
		public IActionResult Login() {
			if (User.Identity.IsAuthenticated) {
				return RedirectToAction("Index", "Home");
			}
			return View();
		}

		[HttpPost("/Login")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(UserCredentials userCredentials) {
			if (User.Identity.IsAuthenticated) {
				return RedirectToAction("Index", "Home");
			}
			
			if (_userServices.ValidateUserCredentials(userCredentials)) {
				var claims = new List<Claim> {
					new Claim(ClaimTypes.Name, userCredentials.UserName)
				};
				
				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var authProperties = new AuthenticationProperties() {
					IsPersistent = true,
					ExpiresUtc = DateTimeOffset.UtcNow.AddDays(3),
				};
				
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
				return RedirectToAction("Index", "Home");
			};
			ModelState.AddModelError("", "Invalid username or password");
			
			return View();
		}

		[HttpGet("/Transaction")]
		public IActionResult Transaction() {
			if (!User.Identity.IsAuthenticated) {
				return RedirectToAction("Signup", "Home");	
			}
			return View();
		}

		[HttpPost("/Transaction")]
		[ValidateAntiForgeryToken]
		public IActionResult Transaction(TransactionDetails transactionDetails) {
			_userServices.RetrieveUserDetails(User.Identity.Name);
			transactionDetails.TransactionDetailsId = new Guid();
			transactionDetails.TransferAmountNPR = transactionDetails.TransferAmountMYR  * transactionDetails.TransferAmountMYR;
			transactionDetails.TransferDate = DateTime.Now;
			if (_transactionServices.registerTransaction(transactionDetails)) {
				TempData["Success"] = true;
			};
			TempData["Error"] = "Something Went Wrong";
			return View();
		}
		
		public IActionResult Privacy() {
			return View();
		}
		

		[HttpGet("/Report")]
		public IActionResult TransactionReport(DateTime? startDate=null, DateTime? endDate=null) {
			if (!User.Identity.IsAuthenticated) {
				return RedirectToAction("Signup", "Home");
			}
			var report = new List<TransactionDetails>();
			if (startDate == null) {
				report = _transactionServices.readTransactions();
			}
			else if (endDate == null && startDate != null) {
				report = _transactionServices.readTransactions(startDate!.Value);
			}
			else {
				report = _transactionServices.readTransactions(startDate!.Value, endDate!.Value);
			}
			return View(report);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
