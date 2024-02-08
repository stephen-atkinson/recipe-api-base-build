# recipes-api-base-build

### Entity Framework

- Database Provider - SQLite
- Database Name - recipes.db

#### CLI Commands
- Add Migration - dotnet ef migrations add <Name> --project Recipes.Core --startup-project Recipes.Api -o Infrastructure/Database/Migrations
- Create/Update Database - dotnet ef database update --project Recipes.Core --startup-project Recipes.Api