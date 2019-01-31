# Mailman
The easiest way to send email from .NET Core<br/> 
A wrapper around the MailKit SMTP client (https://github.com/jstedfast/MailKit)

## Getting Started
These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Installing via NuGet

The easiest way to install Mailman is via [NuGet](https://www.nuget.org/packages/Mailman/).

In Visual Studio's [Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console),
enter the following command:

    Install-Package MailKit

## Using Mailman

### Configuration

Add settings to appsettings.json
```
"Email": {
    "FromName": "<fromname>",
    "FromAddress": "<fromaddress>",

    "LocalDomain": "<localdomain>",

    "MailServerAddress": "<mailserveraddress>",
    "MailServerPort": "<mailserverport>",

    "UserId": "<userid>",
    "UserPassword": "<userpasword>"
  }
```

### Registration

Register Mailman inside of startup.cs

```csharp
public void ConfigureServices(IServiceCollection services)
{
  ...
  // Register email service 
  services.AddMailman(Configuration.GetSection("Email"));
  ...
}
```

### Using Mailman

Inject Mailman service into controller and send emails

```csharp
public class HomeController : Controller
{
    private readonly IEmailService _emailService;

    public HomeController(IEmailService emailService)
    {
        _emailService = emailService;
    }
    
    [HttpGet]
    public async Task Index()
    {
        await _emailService.SendEmailAsync(recipient, subject, message);
    }
 }
```

## Contributing

Please read [CONTRIBUTING.md](https://gist.github.com/PurpleBooth/b24679402957c63ec426) for details on our code of conduct, and the process for submitting pull requests to us.

## Authors

* **Dan Huenecke**

See also the list of [contributors](https://github.com/your/project/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

Inspiration: https://www.codeproject.com/Articles/1166364/%2FArticles%2F1166364%2FSend-email-with-Net-Core-using-Dependency-Injectio
