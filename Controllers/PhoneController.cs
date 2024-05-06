using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneManagement.Repository;
using PhoneManagement.Service;
using IAuthenticationService = PhoneManagement.Service.IAuthenticationService;

namespace PhoneManagement.Controllers {
    [ApiController]
    public class PhoneController : ControllerBase {
        private readonly IPhoneRepository _phoneRepository;
        private IAuthenticationService _authenticationService;

        public PhoneController(IServiceProvider serviceProvider) {
            _phoneRepository = serviceProvider.GetRequiredService<IPhoneRepository>();
            _authenticationService = serviceProvider.GetRequiredService<IAuthenticationService>();
        }

        private static List<Phone> phones = new List<Phone> {
            new Phone {
                Id = 1,
                Name = "Iphone 15 Promax",
                Brand = "Iphone",
                Price = 10000000,
            }
        };

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("/api/[controller]/get-all-phones")]
        public IActionResult getAllPhones() {
            var phones = _phoneRepository.getAllPhones();
            return Ok(phones);
        }

        [HttpPost]
        [Route("/api/[controller]/login")]
        public IActionResult login([FromBody] RequestLoginModel model) {
            try {
                return Ok(_authenticationService.Authenticator(model));
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("/api/[controller]/register")]
        public IActionResult register([FromBody] RequestRegisterModel model) {
            try {
                var check = _authenticationService.Register(model);
                return Ok(check == 1 ? "Register in process" : "Account or Email is existed! Register fail");
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("/api/[controller]/verify-email")]
        public IActionResult verifyEmail([FromBody] RequestVerifyEmailModel model) {
            try {
                var check = _authenticationService.VerifyEmail(model);
                return Ok(check == 1 ? "Register success" : "Register fail");
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("/api/[controller]/forget-password")]
        public IActionResult forgetPassword([FromBody] RequestForgetPasswordModel model) {
            try {
                var check = _authenticationService.ForgetPassword(model);
                return Ok(check == 1 ? "Reset password OTP is sent to your email" : "Not found any user");
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("/api/[controller]/reset-password")]
        public IActionResult resetPassword([FromBody] RequestResetPasswordModel model) {
            try {
                var check = _authenticationService.ResetPassword(model);
                return Ok(check == 1 ? "Reset password success" : "Your information is wrong! Reset password fail");
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("/api/[controller]/add-phone")]
        public IActionResult addPhone([FromBody] PhoneModel phone) {
            var entity = new Phone {
                Id = phone.Id,
                Name = phone.Name,
                Brand = phone.Brand,
                Price = phone.Price,
                IsActive = true,
                UserId = phone.UserId,
            };
            var isExistID = _phoneRepository.getPhoneById(phone.Id);
            if (isExistID != null) {
                return StatusCode(200, "PhoneID is existed");
            }
            var save = _phoneRepository.addNewPhone(entity);
            return Ok(save == 1 ? "Add new phone success" : "Add new phone fail");
        }

        [HttpPut]
        [Route("/api/[controller]/update-phone")]
        public IActionResult updatePhone([FromBody] PhoneModel phone) {
            var isExistID = _phoneRepository.getPhoneById(phone.Id);
            if (isExistID == null) {
                return StatusCode(404, "Not found any ID");
            }
            isExistID.Name = phone.Name;
            isExistID.Brand = phone.Brand;
            isExistID.Price = phone.Price;

            var save = _phoneRepository.updatePhone(isExistID, phone.Id);
            return Ok(save == 1 ? "Update phone success" : "Update phone fail");
        }

        [HttpDelete]
        [Route("/api/[controller]/delete-phone/{id}")]
        public IActionResult deletePhone(int id) {
            var isExist = _phoneRepository.getPhoneById(id);
            if (isExist == null) {
                return StatusCode(404, "Not found any ID");
            }
            var save = _phoneRepository.deletePhoneById(id);
            return Ok(save == 1 ? "Delete phone success" : "Delete phone fail");
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("/api/[controller]/get-phone-by-id/{id}")]
        public IActionResult getPhoneById(int id) {
            var isExist = phones.Where(x => x.Id == id).FirstOrDefault();
            if (isExist == null) {
                return StatusCode(404, "Not found any ID");
            }
            return Ok(isExist);
        }
    }
}
