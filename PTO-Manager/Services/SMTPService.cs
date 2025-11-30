using System.Net;
using System.Net.Mail;
using PTO_Manager.Additional;


namespace PTO_Manager.Services;
public interface ISMTPService
{
    Task BeerkezoKerelemErtesitokAsync(EmailPayload EmailAdatok);
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

    
    public async Task BeerkezoKerelemErtesitokAsync(EmailPayload EmailAdatok)
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
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _config["Smtp:Username"],
                _config["Smtp:Password"])
        };

       
        var toMail = new MailMessage
        {
            From = new MailAddress(_config["Smtp:From"]!),
            Subject = EmailAdatok.Subject,
            Body = toHtmlContent,
            IsBodyHtml = true
        };
        
        
        foreach (var tomai in EmailAdatok.To)
        {
            toMail.To.Add(tomai);
        }

        
        await client.SendMailAsync(toMail);
    
    }
    
    
    
    /*
    
    public async Task KerelemElfogadasErtesitokAsync(EmailDontesAdatok payload)
    {
        string baseDir = AppContext.BaseDirectory;
        
        string toTemplatePath = Path.Combine(baseDir,"Additional",payload.ToTemplateElfogado);
        
        if (!File.Exists(TemplatePath))
            throw new FileNotFoundException($"Email template not found at {toTemplatePath}");
        
        
        string ElfogadoHtmlContent = await File.ReadAllTextAsync(TemplatePath);
        
        
        foreach (var placeholder in payload.Placeholders)
        {
            toElfogadoHtmlContent = toElfogadoHtmlContent.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value); 
        }
        
        using var client = new SmtpClient
        {
            Host = _config["Smtp:Host"],
            Port = int.Parse(_config["Smtp:Port"]),
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _config["Smtp:User"],
                _config["Smtp:Password"])
        };
        
        var UgyintezoTOMail = new MailMessage
        {
            From = new MailAddress(_config["Smtp:From"]!),
            Subject = payload.Subject,
            Body = toElfogadoHtmlContent,
            IsBodyHtml = true
        };
        
       
        UgyintezoTOMail.To.Add(payload.UgyintezoCime);
        
        await client.SendMailAsync(UgyintezoTOMail);
       
    }
    */
}