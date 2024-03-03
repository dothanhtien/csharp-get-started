# Get Started With C# - ASP.NET Core

### Tools / Frameworks
#### .NET 7.0.9
#### VS code [(extensions)](#vs-code-extensions)

### dotnet-ef commands
- Install:
`dotnet tool install --global dotnet-ef --version 7.0.9`
- Migrate:
`dotnet ef migrates add FirstMigration -o Data/Migrations`
- Drop Database:
`dotnet ef database drop`
- Update Database:
`dotnet ef database update`

### dotnet commands:
- Build:
`dotnet build`
- Watch:
`dotnet watch`

### Specify the following details in `appsetting.json` to use Image functionality with Cloudinary:
```
"CloudinarySettings": {
  "CloudName": "",
  "ApiKey": "",
  "ApiSecret": ""
}
```

### VS Code Extensions:
![VS Code Extensions Image](/vscode-extensions.png)
