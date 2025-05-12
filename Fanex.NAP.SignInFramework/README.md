# Fanex.NAP.SignIn.Framework

Authentication and sign-in framework for Fanex NAP applications. This package provides controllers, models, and utilities for user authentication, password management, and health checks.

## Installation

Install the package via NuGet:

```bash
Install-Package Fanex.NAP.SignIn.Framework
```

Or add it to your `.csproj` file:

```xml
<PackageReference Include="Fanex.NAP.SignIn.Framework" Version="1.0.0" />
```

## Features

- **Authentication Controllers**: Ready-to-use controllers for user login and authentication
- **Password Management**: Utilities for password updates and security
- **Health Checks**: Built-in health check functionality
- **Cryptography**: Security utilities for data encryption and hashing
- **View Models**: Pre-built view models for common authentication scenarios
- **CSS Assets**: Included styling for login pages

## Usage

### Controllers

The package includes several controllers that you can inherit from or use as reference:

- `AuthController`: Main authentication controller
- `UserLoginController`: User login functionality
- `HealthCheckController`: Health monitoring

### Models

Available models include:

- `RecheckInModel`: For user re-authentication
- `UpdatePasswordModel`: For password updates

### View Models

- `IndexViewModel`: Main index view model
- `MaintenanceViewModel`: Maintenance page view model

### CSS Assets

The package includes CSS files and images for styling login pages. These are available in the `content/css` and `content/img` folders when the package is installed.

## Dependencies

This package depends on:

- Fanex.NAP.Client.Framework (1.0.0.1)
- Microsoft.AspNet.Mvc (5.2.9)
- Newtonsoft.Json (13.0.3)
- ONEbook.UM (3.1.1)
- ONEbook.UM.AspNetMVC (3.1.3)
- ONEBook.UM.NETFramework (3.1.5)
- System.ComponentModel.Annotations (5.0.0)
- System.Configuration.ConfigurationManager (5.0.0)
- System.Security.AccessControl (5.0.0)
- System.Security.Permissions (5.0.0)
- System.Security.Principal.Windows (5.0.0)

## Target Framework

This package targets .NET Framework 4.7.2.

## License

This package is licensed under the MIT License.

## Contributing

For issues and contributions, please visit the project repository.

## Version History

- **1.0.0**: Initial release with authentication controllers, models, and utilities 