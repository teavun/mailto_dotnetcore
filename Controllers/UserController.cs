using System;

namespace mailto
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly EmailSettings _emailSettings;
        private readonly IEmailService _emailService;

        // https://gist.githubusercontent.com/dozieogbo/2e6623ecb6fd1d124fe0830ad55d5bb3/raw/cd8e4f4eb294e405a70556dff2be1b373959a1d9/UserController.cs
        public UserController(IEmailService emailService, IOptionsMonitor<EmailSettings> optionsMonitor)
        {
            _emailService = emailService;
            _emailSettings = optionsMonitor.CurrentValue; ;
        }
        
        // random generated guids
        Guid user = Guid.Parse("52a9c443-b398-461d-9b88-33205b51de61");
        Guid confirmationToken = Guid.Parse("cd258bab-18b3-4435-9e8b-00c55ad8cb94");

        [HttpGet("register")]
        public async Task<IActionResult> Get()
        {
            string to = "xxx-xxx-xxx@gmail.com";

            string confirmationLink = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host +
             Url.Action("ConfirmEmail", "User", new { user = this.user, confirmationToken = this.confirmationToken });

            var mailContent = $"<a href=\"{confirmationLink}\"> CONFIRM </a>";

            _emailService.Send(_emailSettings.SmtpUser, to, "title", mailContent);

            return Ok("confirmation mail has been sent");
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(Guid user, Guid confirmationToken)
        {
            if (user == this.user && confirmationToken == this.confirmationToken)
            {
                return Ok("Successfull confirm, you can login system");
            }

            return BadRequest("Confirmation Error");
        }

    }

}