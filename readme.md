# Build and run

Navigate to project directory PaymentAPI in a console and run

`dotnet restore`

`dotnet build`

`dotnet run`

Nagivate to https://localhost:5000

Swagger will describe the endpoints.

#Notes

- Simple repo multi tier architecture as such a small application, but I've tried to separate concerns as well as possible, and keep the services separate. For example, the balance and the payments come from different services, and are only put together at the Application level, making this easier if these domains need to be split into different projects for a microservice transition.
- Different error handling for different environments

#TODO

- No authentication yet, hence the need to pass account id instead of using logged in user
- Fix the integration tests
- More testing scenarios, cover unhappy paths