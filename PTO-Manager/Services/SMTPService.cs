using System.Net;
using System.Net.Mail;
using PTO_Manager.Additional;


namespace PTO_Manager.Services;
public interface ISMTPService
{
    Task IncomingRequestNotification(EmailPayload EmailAdatok);

    Task DecisionNotifyEmail(EmailPayload EmailAdatok);
    /*
    Task KerelemElfogadasErtesitokAsync(EmailDontesAdatok payload);
    Task KerelemElut_VisszavonErtesitokAsync(EmailDontesAdatok payload);
    */

}

public class SMTPService : ISMTPService
{
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;

    public SMTPService(IConfiguration config, IWebHostEnvironment env)
    {
        _config = config;
        _env = env;
    }

    
    public async Task IncomingRequestNotification(EmailPayload EmailAdatok)
    {
        string baseDir = AppContext.BaseDirectory;
        
        string TemplatePath = Path.Combine(baseDir,"Additional",EmailAdatok.TemplateName);
        
        if (!File.Exists(TemplatePath))
            throw new FileNotFoundException($"Email template not found at {TemplatePath}");
        
        string toHtmlContent = await File.ReadAllTextAsync(TemplatePath);
        
        foreach (var placeholder in EmailAdatok.Placeholders)
        {
            toHtmlContent = toHtmlContent.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value); 
        }
        
        using var client = new SmtpClient
        {
            Host = _config["Smtp:Host"],
            Port = int.Parse(_config["Smtp:Port"]),
            EnableSsl = false,
            UseDefaultCredentials = true
        };

       
        var toMail = new MailMessage
        {
            From = new MailAddress("noreply@example.com"),
            Subject = EmailAdatok.Subject,
            Body = toHtmlContent,
            IsBodyHtml = true
        };
        
        foreach (var tomai in EmailAdatok.To)
        {
            toMail.To.Add(tomai);
        }
        
        try
        {
            await client.SendMailAsync(toMail);
        }
        catch (Exception ex)
        {
            Console.WriteLine("SMTP ERROR: " + ex.ToString());
            throw;
        }
    
    }
    
    public async Task DecisionNotifyEmail(EmailPayload EmailAdatok)
    {
        string baseDir = AppContext.BaseDirectory;
        
        string TemplatePath = Path.Combine(baseDir,"Additional",EmailAdatok.TemplateName);
        
        if (!File.Exists(TemplatePath))
            throw new FileNotFoundException($"Email template not found at {TemplatePath}");
        
        string toHtmlContent = await File.ReadAllTextAsync(TemplatePath);
        
        foreach (var placeholder in EmailAdatok.Placeholders)
        {
            toHtmlContent = toHtmlContent.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value); 
        }
        
        using var client = new SmtpClient
        {
            Host = _config["Smtp:Host"],
            Port = int.Parse(_config["Smtp:Port"]),
            EnableSsl = false,
            UseDefaultCredentials = true
        };

       
        var toMail = new MailMessage
        {
            From = new MailAddress("noreply@example.com"),
            Subject = EmailAdatok.Subject,
            Body = toHtmlContent,
            IsBodyHtml = true
        };
        
        foreach (var tomai in EmailAdatok.To)
        {
            toMail.To.Add(tomai);
        }
        
        try
        {
            await client.SendMailAsync(toMail);
        }
        catch (Exception ex)
        {
            Console.WriteLine("SMTP ERROR: " + ex.ToString());
            throw;
        }
    
    }
    
    
  
}