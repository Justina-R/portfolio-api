using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Requests;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MailController : ControllerBase
{
    private readonly IMailService _mailService;

    public MailController(IMailService mailService)
    {
        _mailService = mailService;
    }

    [HttpPost("contactMe")]
    public async Task<ActionResult> ContactMe(MailRequest mailRequest)
    {

        await _mailService.SendFirstContact(mailRequest.Name, mailRequest.Email, mailRequest.Message);

        return Ok();
    }

}