﻿using BAL.Interfaces;
using DAL.DataContext;
using DAL.DataModels;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using BAL.Repository;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Web.Optimization;
using Microsoft.Extensions.FileProviders.Physical;

namespace HalloDoc_Project.Controllers
{
    public class GuestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _config;
        private readonly IRequestRepo _patient_Request;
        private readonly IJwtToken _jwtToken;
        private readonly IResetPasswordService _resetPasswordService;
        private readonly IEmailService _emailService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAgreement _agreement;
        private readonly INotyfService _notyf;
        public GuestController(ApplicationDbContext context, IWebHostEnvironment environment, IConfiguration config, IRequestRepo request, IJwtToken token, IResetPasswordService resetPasswordService, IEmailService emailService, IPasswordHasher passwordHasher, IAgreement agreement, INotyfService notyf)
        {
            _context = context;
            _environment = environment;
            _config = config;
            _patient_Request = request;
            _jwtToken = token;
            _resetPasswordService = resetPasswordService;
            _emailService = emailService;
            _passwordHasher = passwordHasher;
            _agreement = agreement;
            _notyf = notyf;
        }

        //Only ResetPassword is not taken into the three tier architecture otherwise everything from GuestController is in three tier architecture.
        public IActionResult Agree(int Requestid)
        {
            _agreement.AgreementAccepted(Requestid);
            return RedirectToAction("login_page", "Guest");
        }
        public IActionResult CancelAgreement(int Requestid, string Notes)
        {
            _agreement.AgreementRejected(Requestid, Notes);
            return RedirectToAction("login_page", "Guest");
        }
        public IActionResult ReviewAgreement(int ReqId)
        {
            var user = _context.Requestclients.FirstOrDefault(x => x.Requestid == ReqId);
            if (user != null)
            {
                ReviewAgreementViewModel reviewmodel = new()
                {
                    reqID = ReqId,
                    PatientName = user.Firstname + " " + user.Lastname
                };
                return View(reviewmodel);
            }
            return RedirectToAction("submit_request_page");
        }
        [HttpPost]
        public JsonResult CheckEmail(string email)
        {
            bool emailExists = _context.Users.Any(u => u.Email == email);
            return Json(new { exists = emailExists });
        }

        public IActionResult PatientAccountSetupPage(string token)
        {
            try
            {
                string emailid = _resetPasswordService.ValidateToken(token);
                Aspnetuser? user = _context.Aspnetusers.FirstOrDefault(user => user.Email == emailid);

                PatientSetupViewModel PatientData = new()
                {
                    UserName = user.Email
                };
                if (user != null)
                {
                    return View(PatientData);
                }
                else
                {
                    return RedirectToAction("submit_request_page", "Guest");
                }
            }
            catch (Exception ex)
            {
                return Content("Exception :- " + ex);
            }
        }
        [HttpPost]
        public IActionResult PatientAccountSetupPage(PatientSetupViewModel patientData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Aspnetuser user = _context.Aspnetusers.FirstOrDefault(user => user.Email == patientData.UserName);
                    if (patientData.Password == patientData.ConfirmPassword)
                    {
                        user.Passwordhash = _passwordHasher.GenerateSHA256(patientData.Password);
                    }
                    _context.Aspnetusers.Update(user);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
            }
            return RedirectToAction("login_page", "Guest");
        }

        #region GUEST VIEWS
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult submit_request_page()
        {
            return View();
        }
        public IActionResult patient_submit_request_screen()
        {
            return View();
        }
        public IActionResult SessionExpired()
        {
            return View();
        }
        public IActionResult TermsAndConditions()
        {
            return View();
        }
        #endregion

        #region BUSINESS REQUEST
        [HttpGet]
        public IActionResult Business_Info()

        {
            BusinessModel model = new BusinessModel();
            model.Regions = _context.Regions.ToList();
            return View(model);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Business_Info(BusinessModel bm)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var jwtToken = _resetPasswordService.GenerateJWTTokenForPassword(bm.PatientEmail);
                    var SetupLink = Url.Action("PatientAccountSetupPage", "Guest", new { token = jwtToken }, Request.Scheme);
                    _patient_Request.BRequest(bm, SetupLink ?? " ");
                    return RedirectToAction("Business_Info", "Guest");
                }
                bm.Regions = _context.Regions.ToList();
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
            }
            return View(bm);
        }
        #endregion

        #region CONCIERGE REQUEST
        [HttpGet]
        public IActionResult Concierge_info()
        {
            ConciergeModel model = new ConciergeModel();
            model.Regions = _context.Regions.ToList();
            return View(model);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Concierge_info(ConciergeModel cm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var jwtToken = _resetPasswordService.GenerateJWTTokenForPassword(cm.PatientEmail);
                    var SetupLink = Url.Action("PatientAccountSetupPage", "Guest", new { token = jwtToken }, Request.Scheme);
                    _patient_Request.CRequest(cm, SetupLink ?? " ");
                    return RedirectToAction("Concierge_info", "Guest");
                }
                cm.Regions = _context.Regions.ToList();
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
            }

            return View(cm);
        }
        #endregion

        #region FAMILY FRIEND REQUEST
        public IActionResult Friend_family()
        {
            FamilyFriendModel familyFriendModel = new FamilyFriendModel();
            familyFriendModel.PatientRegions = _context.Regions.ToList();
            return View(familyFriendModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Friend_family(FamilyFriendModel fmfr)
        {
            string path = _environment.WebRootPath;
            try
            {
                if (ModelState.IsValid)
                {
                    var uniqueid = Guid.NewGuid().ToString();
                    var jwtToken = _resetPasswordService.GenerateJWTTokenForPassword(fmfr.PatientModel.Email);
                    var SetupLink = Url.Action("PatientAccountSetupPage", "Guest", new { token = jwtToken }, Request.Scheme);
                    _patient_Request.FRequest(fmfr, uniqueid, path, SetupLink ?? " ");
                    return RedirectToAction("Friend_Family", "Guest");

                }
                fmfr.PatientRegions = _context.Regions.ToList();
                _notyf.Success("Request Created Successfully");
                _notyf.Custom("Account Set-up details have been sent to the user email");

            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
            }
            return View(fmfr);
        }
        #endregion

        #region PATIENT REQUEST 
        [HttpGet]
        public IActionResult create_patient_request()
        {
            PatientModel patient = new()
            {
                Regions = _context.Regions.ToList()
            };
            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult create_patient_request(PatientModel pm)
        {
            string path = _environment.WebRootPath;
            if (ModelState.IsValid)
            {
                var uniqueid = Guid.NewGuid().ToString();
                _patient_Request.PRequest(pm, uniqueid, path);
                return RedirectToAction("create_patient_request", "Guest");
            }

            pm.Regions = _context.Regions.ToList();
            return View(pm);
        }
        #endregion

        #region LOGIN PAGE METHODS

        #region Login
        public IActionResult login_page()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult login_page(LoginViewModel demouser)
        {
            if (ModelState.IsValid)
            {
                var password = _passwordHasher.GenerateSHA256(demouser.Password);
                Aspnetuser v = _context.Aspnetusers.FirstOrDefault(dt => dt.Username == demouser.Username && dt.Passwordhash == password);
                if (v != null)
                {
                    if (v.Role == "Patient")
                    {
                        HttpContext.Session.SetString("Email", v.Email);
                        var token = _jwtToken.generateJwtToken(v.Email, "Patient");
                        Response.Cookies.Append("jwt", token);

                        _notyf.Success("Logged In Successfully");
                        return RedirectToAction("PatientDashboard", "Home");
                    }
                    else if (v.Role == "Admin")
                    {
                        Admin admin = _context.Admins.FirstOrDefault(u => u.Email == v.Email);

                        HttpContext.Session.SetString("Email", v.Email);
                        var token = _jwtToken.generateJwtToken(v.Email, "Admin");
                        Response.Cookies.Append("jwt", token);

                        TempData["UserName"] = string.Concat(admin.Firstname, " ", admin.Lastname ?? " ");
                        _notyf.Success("Logged In Successfully");
                        return RedirectToAction("AdminDashboard", "Admin");
                    }
                    else if (v.Role == "Physician")
                    {
                        Physician physician = _context.Physicians.FirstOrDefault(phy => phy.Aspnetuserid == v.Id);
                        if (physician != null)
                        {
                            HttpContext.Session.SetString("AspnetuserId", physician.Aspnetuserid);
                            var token = _jwtToken.generateJwtToken(physician.Aspnetuserid, "Physician");
                            Response.Cookies.Append("jwt", token);

                            TempData["UserName"] = physician.Firstname + " " + physician.Lastname ?? " ";
                            _notyf.Success("Logged In Successfully");
                            return RedirectToAction("ProviderDashboard", "Provider");
                        }
                    }
                }
                else
                {
                    _notyf.Error("Invalid Credentials");
                }
            }
            return View();
        }
        #endregion

        #region ForgotPassword
        public IActionResult forgot_password_page()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult forgot_password_page(ForgotPasswordViewModel fvm)
        {
            if (ModelState.IsValid)
            {
                var jwtToken = _resetPasswordService.GenerateJWTTokenForPassword(fvm.email);
                var resetLink = Url.Action("ResetPassword", "Guest", new { token = jwtToken }, Request.Scheme);
                _emailService.SendEmailForPasswordReset(fvm, resetLink);
                return RedirectToAction("login_page", "Guest");
            }
            return View();
        }
        #endregion

        #region Reset Password
        [HttpGet]
        public ActionResult ResetPassword(string token)
        {
            // 4. In the MVC controller, create an action method to handle the password reset request
            try
            {
                string emailid = _resetPasswordService.ValidateToken(token);
                ResetPasswordViewModel rpvm = new ResetPasswordViewModel()
                {
                    email = emailid
                };
                return View(rpvm);
            }
            catch (Exception ex)
            {
                return Content("Invalid token");
            }
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel rpvm)
        {
            Aspnetuser aspnetuser = _context.Aspnetusers.FirstOrDefault(u => u.Email == rpvm.email);
            if (rpvm.password == rpvm.confirmpassword)
            {
                aspnetuser.Passwordhash = _passwordHasher.GenerateSHA256(rpvm.password);
                aspnetuser.Modifieddate = DateTime.Now;
                _context.Aspnetusers.Update(aspnetuser);
                _context.SaveChanges();
                return View("login_page");
            }
            else
            {
                return View("Error");
            }
        }
        #endregion

        #endregion
    }
}
