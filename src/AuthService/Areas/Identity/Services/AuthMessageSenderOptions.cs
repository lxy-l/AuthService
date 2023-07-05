namespace AuthService.Areas.Identity.Services;

public class AuthMessageSenderOptions
{
    public string? SendGridKey { get; set; }

    public string? Email { get; set; }
}