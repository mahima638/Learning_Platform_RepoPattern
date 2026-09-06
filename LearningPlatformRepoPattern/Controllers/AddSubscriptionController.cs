using LearningPlatformRepoPattern.Models;
using LearningPlatformRepoPattern.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace LearningPlatformRepoPattern.Controllers
{
    public class AddSubscriptionController : Controller
    {
        // Razorpay API Credentials
        // IMPORTANT: Move these to appsettings.json / User Secrets.
        private readonly string keyId = "rzp_test_Kl7588Yie2yJTV";
        private readonly string razorpayKeySecret = "6dN9Nqs7M6HPFMlL45AhaTgp";

        private readonly SubscriptionService _subscriptionService;
        private readonly IWebHostEnvironment environment;

        public AddSubscriptionController(SubscriptionService subscriptionService, IWebHostEnvironment environment)
        {
            _subscriptionService = subscriptionService; this.environment = environment;
        }

        // GET: Home
        public IActionResult Index()
{
    ViewBag.MasterCourses = new SelectList(
        _subscriptionService.GetActiveMasterCourses(),
        "Id",
        "CourseName"
    );

    var data = _subscriptionService.GetSubscriptions();

    return View(data);
}

        // Get Sub Courses based on Master Course
        public IActionResult GetSubCourses(int mid)
        {
            var data = _subscriptionService .GetSubCourses(mid) .Select(x => new { sid = x.Id, SubCourseName = x.SubCourseName }).ToList();
            return Json(data);
        }

        // GET: Modal
        public IActionResult Modal(int? mid)
        {
            var masterCourses = _subscriptionService.GetActiveMasterCourses();

            ViewBag.MasterCourses = new SelectList( masterCourses, "mid", "CourseName",  mid   );
            return View();
        }

        // POST: Modal
        [HttpPost]
        public async Task<IActionResult> Modal(
            Subscriptions s,
            IFormFile ThumbnailFile)
        {
            var selectedSubCourses =
                Request.Form["sid"].ToArray();

            // Check duplicate subscription type
            bool alreadyExists = _subscriptionService.GetSubscriptions().Any(x => x.sub_type == s.sub_type);

            if (alreadyExists)
            {
                ModelState.AddModelError( "sub_type", "Subscription type already exists.");
            }

            if (ModelState.IsValid)
            {
                
                // Save Thumbnail
                if (ThumbnailFile != null &&
                    ThumbnailFile.Length > 0)
                {
                    string folderPath = Path.Combine( environment.WebRootPath, "Uploads", "Subscriptions"
                    );

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fileName =
                        Path.GetFileName(
                            ThumbnailFile.FileName);

                    string filePath =
                        Path.Combine(
                            folderPath,
                            fileName);

                    using (var stream =
                           new FileStream(
                               filePath,
                               FileMode.Create))
                    {
                        await ThumbnailFile.CopyToAsync(stream);
                    }

                    s.subThumbnail =
                        "/Uploads/Subscriptions/" +
                        fileName;
                }

                // Save Subscription
                await _subscriptionService.AddSubscriptionAsync(s);

                // =========================
                // Save Selected Sub Courses
                // =========================

                if (selectedSubCourses != null)
                {
                    foreach (string sid in selectedSubCourses)
                    {
                        SubscriptionSubCourse obj =
                            new SubscriptionSubCourse();

                        obj.sub_id = s.sub_id;
                        obj.sid = Convert.ToInt32(sid);

                        await _subscriptionService
                            .AddSubscriptionSubCourseAsync(obj);
                    }
                }

                TempData["InsertMessage"] =
                    "Data Inserted";

                return RedirectToAction("Index");
            }


            // Validation Failed
            var masterCourses = _subscriptionService.GetActiveMasterCourses();
            ViewBag.MasterCourses = new SelectList( masterCourses, "mid", "CourseName", s.mid );
            ViewBag.OpenModal = true;
            var data =_subscriptionService.GetSubscriptions();
            return View("Index", data);
        }

        // GET: Edit
        public IActionResult Edit(int id)
        {
            var row = _subscriptionService.GetSubscriptionById(id);

            if (row == null)
            {
                return NotFound();
            }

            var masterCourses = _subscriptionService .GetActiveMasterCourses();
            ViewBag.MasterCourses = masterCourses .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.CourseName,
                    Selected = x.Id == row.mid
                }) .ToList();
            return View(row);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( Subscriptions s, IFormFile ThumbnailFile)
        {
            var selectedSubCourses = Request.Form["sid"].ToArray();

            if (ModelState.IsValid)
            {
                // Get existing subscription
                var existingSubscription = _subscriptionService.GetSubscriptionById(s.sub_id);
                if (existingSubscription == null)
                {
                    return NotFound();
                }
                // Update Subscription Details
                existingSubscription.sub_type = s.sub_type;
                existingSubscription.mid = s.mid;
                existingSubscription.sub_amount = s.sub_amount;
                existingSubscription.subStatus = s.subStatus;
                // Save New Thumbnail
                if (ThumbnailFile != null &&
                    ThumbnailFile.Length > 0)
                {
                    string folderPath = Path.Combine(environment.WebRootPath, "Uploads", "Subscriptions" );

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fileName = Path.GetFileName( ThumbnailFile.FileName);
                    string filePath = Path.Combine( folderPath, fileName);
                    using (var stream = new FileStream( filePath, FileMode.Create))
                    {
                        await ThumbnailFile.CopyToAsync(stream);
                    }

                    existingSubscription.subThumbnail = "/Uploads/Subscriptions/" + fileName;
                }

                await _subscriptionService .UpdateSubscriptionAsync( existingSubscription);  
                await _subscriptionService .RemoveSubscriptionSubCoursesAsync(  s.sub_id);

                if (selectedSubCourses != null)
                {
                    foreach (string sid in selectedSubCourses)
                    {
                        SubscriptionSubCourse obj =  new SubscriptionSubCourse();
                        obj.sub_id = s.sub_id;
                        obj.sid = Convert.ToInt32(sid);

                        await _subscriptionService  .AddSubscriptionSubCourseAsync(obj);
                    }
                }
                TempData["UpdateMessage"] = "Data Updated";
                return RedirectToAction("Index");
            }
            ViewBag.MasterCourses = new SelectList(  _subscriptionService.GetActiveMasterCourses(), "mid", "CourseName", s.mid);
            return View(s);
        }

        // GET: Delete
        public IActionResult Delete(int id)
        {
            var subIdRow = _subscriptionService.GetSubscriptionById(id);

            if (subIdRow == null)
            {
                return NotFound();
            }
            return View(subIdRow);
        }

        // POST: Delete
        [HttpPost]
        public async Task<IActionResult> Delete( Subscriptions s)
        {
            if (ModelState.IsValid)
            {
                await _subscriptionService .DeleteSubscriptionAsync(s.sub_id);
                TempData["DeleteMessage"] = "Data Deleted";
                return RedirectToAction("Index");
            }
            return View(s);
        }

        // User Subscriptions
        public IActionResult UserSubscriptions()
        {
            var subscriptions = _subscriptionService.GetSubscriptions() .Where(x => x.subStatus == "Active") .ToList();
            return View(subscriptions);
        }

        // GET: BuySubscription
        public IActionResult BuySubscription(int id)
        {
            // Login will be integrated later
            /*
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(
                HttpContext.Session.GetString("UserId"));
            */

            // Get subscription
            var subscription =
                _subscriptionService.GetSubscriptionById(id);

            if (subscription == null)
            {
                return NotFound();
            }

            // Razorpay client
            RazorpayClient razorpayClient =
                new RazorpayClient(
                    keyId,
                    razorpayKeySecret
                );

            // Amount in rupees
            double amount =
                subscription.sub_amount;

            // Razorpay amount must be in paise
            int amountInPaise =
                Convert.ToInt32(amount * 100);

            Dictionary<string, object> options =
                new Dictionary<string, object>();

            options.Add(
                "amount",
                amountInPaise);

            options.Add(
                "currency",
                "INR");

            options.Add(
                "receipt",
                "subscription_" +
                subscription.sub_id);

            options.Add(
                "payment_capture",
                1);

            // Create Razorpay order
            Razorpay.Api.Order order =
                razorpayClient.Order.Create(options);

            string orderId =
                order["id"].ToString();

            // Send data to View
            ViewBag.RazorpayKey = keyId;
            ViewBag.OrderId = orderId;
            ViewBag.Amount = amountInPaise;
            ViewBag.SubscriptionName =
                subscription.sub_type;

            return View(subscription);
        }

        // POST: PaymentSuccess
        [HttpPost]
        public IActionResult PaymentSuccess(
            string razorpay_payment_id,
            string razorpay_order_id,
            string razorpay_signature,
            int subscription_id)
        {
            try
            {
                // Create signature payload
                string payload =
                    razorpay_order_id +
                    "|" +
                    razorpay_payment_id;

                // Generate HMAC SHA256
                using (var hmac =
                       new HMACSHA256(
                           Encoding.UTF8.GetBytes(
                               razorpayKeySecret)))
                {
                    byte[] hash =
                        hmac.ComputeHash(
                            Encoding.UTF8.GetBytes(
                                payload));

                    string generatedSignature =
                        BitConverter
                            .ToString(hash)
                            .Replace("-", "")
                            .ToLower();

                    // Compare signatures
                    bool isValid =
                        generatedSignature ==
                        razorpay_signature;

                    if (isValid)
                    {
                        return Json(new
                        {
                            success = true
                        });
                    }

                    return Json(new
                    {
                        success = false
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // GET: PaymentSuccess
        public IActionResult PaymentSuccess()
        {
            return View();
        }
    }
}