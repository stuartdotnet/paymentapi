# Build and run

## Visual Studio
Hit Ctrl F5 :)

## Visual Studio Code
Hit Ctrl F5 :)

## Terminal
Navigate to project directory PaymentAPI/PaymentAPI in a console and run

`dotnet restore`

`dotnet build`

`dotnet run`

Nagivate to http://localhost:5000

Swagger will describe the endpoints.

# Testing

If you aren't using an IDE with built-in testing features, execute the following command at a command prompt in the PaymentAPI.UnitTests or PaymentAPI.IntegrationTests folder:

`dotnet test`

### Testing notes
Some issues getting the integration tests to run, solved by using AddDbContextPool instead of AddDbContext in the APIWebApplicationFactory ConfigureWebHost, that took some problem solving :D
I'd be keen to add some database cleanup between tests too, I've made them robust enough to work in any order by working on different records but they'd be easy to break if someone added one and didn't realise that record was already being modified by another test.

# Continuous Integration
Built using https://github.com/stuartdotnet/paymentapi/blob/master/azure-pipelines.yml

TODO run integration tests

TODO build release pipeline to an Azure Web App

# Self Publishing
From PaymentAPI/PaymentAPI run

`dotnet publish -o <your local directory>`

`dotnet PaymentAPI.dll`

# Configuration
Set your machine's local Environment setting by creating a setting called ASPNETCORE_ENVIRONMENT and setting it to Development or Production

# Notes

- Simple repo multi tier architecture as such a small application, but I've tried to separate concerns as well as possible, and keep the services separate. For example, the balance and the payments come from different services, and are only put together at the Application level, making this easier if these domains need to be split into different projects for a microservice transition.
- Different error handling for different environments

# TODO

- SSL
- No authentication yet, hence the need to pass account id instead of using logged in user. Would use IdentityServer4
- Integration tests, clean up data between them
- More testing scenarios, cover more unhappy paths
- Production config, keyvault