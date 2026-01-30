using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Requests;
using Microsoft.AspNetCore.RateLimiting;

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


    [EnableRateLimiting("mailLimiter")]
    [HttpPost("contactMe")]
    public async Task<ActionResult> ContactMe(MailRequest mailRequest)
    {
        if (string.IsNullOrWhiteSpace(mailRequest.Message) || mailRequest.Message.Length < 10)
            return BadRequest("El mensaje es demasiado corto o inválido");

        if (string.IsNullOrWhiteSpace(mailRequest.Email) || !mailRequest.Email.Contains("@"))
            return BadRequest("Email inválido");

        if (string.IsNullOrWhiteSpace(mailRequest.Name))
            return BadRequest("Nombre inválido");

        await _mailService.SendFirstContact(mailRequest.Name, mailRequest.Email, mailRequest.Message);

        return Ok();
    }

}