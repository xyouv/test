using Microsoft.AspNetCore.Mvc;
using PhoneManagement.Service;

namespace PhoneManagement.Controllers {
    public class EmailController : ControllerBase {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService) {
            _emailService = emailService;
        }

        [HttpPost]
        [Route("/api/[controller]/send-email")]
        public IActionResult sendEmail([FromBody] SendMailModel model) {
            try {
                _emailService.sendMail(model);
                return Ok("Send mail success");
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}
