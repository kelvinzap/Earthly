namespace Earthly.Authentication.Settings;

public class EmailSettings
{ 
    public string Server { get; set; } 
    public int Port { get; set; } 
    public string SenderEmail { get; set; } 
    public string Account { get; set; } 
    public string Password { get; set; }
}