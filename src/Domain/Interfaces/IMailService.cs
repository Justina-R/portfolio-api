namespace Domain.Interfaces;

public interface IMailService
{
    Task SendFirstContact(string name, string email, string message);

}