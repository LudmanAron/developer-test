# Taxually technical test

This solution contains an [API endpoint](https://github.com/Taxually/developer-test/blob/main/Taxually.TechnicalTest/Taxually.TechnicalTest/Controllers/VatRegistrationController.cs) to register a company for a VAT number. Different approaches are required based on the country where the company is based:

- UK companies can register via an API
- French companies must upload a CSV file
- German companies must upload an XML document

We'd like you to refactor the existing solution with the following in mind:

- Readability
- Testability
- Adherance to SOLID principles

We'd also like you to add some tests to show us how you'd test your solution, although we aren't expecting exhaustive test coverage.

We don't expect you to implement the classes for making HTTP calls and putting messages on queues.

We'd like you to spend not more than a few hours on the exercise.

To develop and submit your solution please follow these steps:

1. Create a public repo in your own GitHub account and push the technical test there
2. Develop your solution and push your changes to your own public GitHub repo
3. Once you're happy with your solution send us a link to your repo

# In order to setup the Azure infrastructure these commands will need to be executed:
az login
az group create --name VatRegistrationRG --location eastus
az acr create --resource-group VatRegistrationRG --name VatRegACR --sku Basic
az acr create --resource-group VatRegistrationRG --name VatRegACR --sku Basic

az appservice plan create --name VatRegPlan --resource-group VatRegistrationRG --is-linux --sku B1
az webapp create --resource-group VatRegistrationRG --plan VatRegPlan --name VatRegistrationAPI --deployment-container-image-name VatRegACR.azurecr.io/vatregistrationapi:v1
az webapp config appsettings set --resource-group VatRegistrationRG --name VatRegistrationAPI --settings \
"ASPNETCORE_ENVIRONMENT=Production" \
"ApplicationInsights__InstrumentationKey=<Your-AppInsights-Key>" \
"ConnectionStrings__DefaultConnection=<Your-Azure-SQL-Connection-String>"

az webapp browse --resource-group VatRegistrationRG --name VatRegistrationAPI

# To set up the Azure Key Vault:
az keyvault create --name VatRegKeyVault --resource-group VatRegistrationRG --location eastus
az keyvault secret set --vault-name VatRegKeyVault --name "SqlServerPassword" --value "YourStrongPassword123!"
az webapp identity assign --name VatRegistrationAPI --resource-group VatRegistrationRG

# Get the managed identity's client ID
az webapp show --name VatRegistrationAPI --resource-group VatRegistrationRG --query identity.principalId --output tsv

# Grant Key Vault access to the Web App's managed identity
az keyvault set-policy --name VatRegKeyVault --object-id <PRINCIPAL_ID> --secret-permissions get list

