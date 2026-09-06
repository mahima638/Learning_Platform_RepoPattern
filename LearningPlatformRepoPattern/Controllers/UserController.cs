using LearningPlatformRepoPattern.Models;
using LearningPlatformRepoPattern.Repository;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;

namespace LearningPlatformRepoPattern.Controllers
{
    public class UserController : Controller
    {
        IUserRepository up;
        IMasterCourseRepository mp;
        ISubCourseRepository sp;
        public UserController(IUserRepository up, IMasterCourseRepository mp,ISubCourseRepository sp)
        {
            this.up = up;
            this.mp = mp;
            this.sp = sp;

        }
        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = up.GetUserById(userId.Value);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }
        [HttpGet]
        public IActionResult EditProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = up.GetUserById(userId.Value);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new EditProfile
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserEmail = user.UserEmail
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult EditProfile(EditProfile model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = up.GetUserById(model.UserId);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            user.UserName = model.UserName;
            user.UserEmail = model.UserEmail;

            up.UpdateUser(user);

            return RedirectToAction("Profile");
        }
        public IActionResult AllCourses()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var courses = sp.GetAll();

            var masterCourses = mp.GetAll();

            var cart = HttpContext.Session.GetString("Cart");

            int cartCount = 0;

            if (!string.IsNullOrEmpty(cart))
            {
                cartCount = cart.Split(',').Length;
            }

            ViewBag.CartCount = cartCount;
            ViewBag.MasterCourses = masterCourses;

            return View(courses);
        }
        public IActionResult SubCourses(int mid)
        {
            var courses = sp.GetByMasterCourseId(mid);

            return View(courses);
        }
        public IActionResult BuyMasterCourse(int mid)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

           
            var courses = sp.GetByMasterCourseId(mid);

            if (courses == null || !courses.Any())
            {
                return RedirectToAction("AllCourses");
            }

         
            decimal totalAmount = courses.Sum(c => c.Amount);

            string keyId = "rzp_test_Kl7588Yie2yJTV";
            string keySecret = "6dN9Nqs7M6HPFMlL45AhaTgp";

            RazorpayClient razorpayClient =
                new RazorpayClient(keyId, keySecret);

            Dictionary<string, object> options =
                new Dictionary<string, object>();

            options.Add("amount", (int)(totalAmount * 100));
            options.Add("currency", "INR");
            options.Add("receipt", "master_" + mid + "_" + DateTime.Now.Ticks);
            options.Add("payment_capture", 1);

            Razorpay.Api.Order order =
                razorpayClient.Order.Create(options);

            string orderId = order["id"].ToString();

            ViewBag.KeyId = keyId;
            ViewBag.OrderId = orderId;
            ViewBag.Amount = (int)(totalAmount * 100);
            ViewBag.MasterCourseId = mid;
            ViewBag.UserId = userId.Value;

            return View("Payment");
        }
        [HttpPost]
        public IActionResult AddToCart(int sid)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = HttpContext.Session.GetString("Cart");

            if (string.IsNullOrEmpty(cart))
            {
                cart = sid.ToString();
            }
            else
            {
                var cartItems = cart.Split(',').ToList();

                if (!cartItems.Contains(sid.ToString()))
                {
                    cartItems.Add(sid.ToString());
                }

                cart = string.Join(",", cartItems);
            }

            HttpContext.Session.SetString("Cart", cart);

            return RedirectToAction("AllCourses");
        }
        public IActionResult Cart()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = HttpContext.Session.GetString("Cart");

            if (string.IsNullOrEmpty(cart))
            {
                return View(new List<SubCourse>());
            }

            var sidList = cart
                .Split(',')
                .Select(int.Parse)
                .ToList();

            var courses = sp.GetByIds(sidList);

            return View(courses);
        }

        [HttpPost]
        public IActionResult CreateOrder()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = HttpContext.Session.GetString("Cart");

            if (string.IsNullOrEmpty(cart))
            {
                return RedirectToAction("Cart");
            }

            var sidList = cart
                .Split(',')
                .Select(int.Parse)
                .ToList();

            var courses = sp.GetByIds(sidList);

            decimal totalAmount = courses.Sum(c => c.Amount);

            string keyId = "rzp_test_Kl7588Yie2yJTV";
            string keySecret = "6dN9Nqs7M6HPFMlL45AhaTgp";

            RazorpayClient razorpayClient = new RazorpayClient(keyId, keySecret);

            Dictionary<string, object> options = new Dictionary<string, object>();

            options.Add("amount", (int)(totalAmount * 100));
            options.Add("currency", "INR");
            options.Add("receipt", "order_" + DateTime.Now.Ticks);
            options.Add("payment_capture", 1);

            Razorpay.Api.Order order = razorpayClient.Order.Create(options);

            string orderId = order["id"].ToString();

            ViewBag.KeyId = keyId;
            ViewBag.OrderId = orderId;
            ViewBag.Amount = (int)(totalAmount * 100);
            ViewBag.UserId = userId.Value;

            return View("Payment");
        }
    }
}
