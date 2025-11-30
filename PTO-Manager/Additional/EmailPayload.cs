namespace PTO_Manager.Additional;

public class EmailPayload
{
    public List<String> To { get; set; }
    public string Subject { get; set; }
    public string TemplateName { get; set; }
    public Dictionary<string, string> Placeholders { get; set; }
}