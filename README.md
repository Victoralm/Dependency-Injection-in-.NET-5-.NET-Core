# [Dependency Injection in .NET 5 (.NET Core)](https://www.udemy.com/course/dependency-injection-in-net-5-net-core)

- [Course Original Repository](https://github.com/bhrugen/WazeCredit)

## Dependency Injection ##

- ASP .Net Core is designed from scratch to support **Dependency Injection**
  - A technique for achieving **Inversion of Control** between classes and their dependencies

- NET Core injects objects of dependency classes through constructor or method
  by using built-in **IoC container**
  - **IOC** - **Inversion of Control Container**: A framework for implementing automatic
    dependency injection
  - Manages object creation and its lifetime, also injects dependencies to their class
  - Creates an object of the specified class and also injects all of the
    dependency objects through constructor or method at run-time and dispose it
    at appropriate time. This is done so that the devs do not have to create and
    manage the objects manually

- **Dependency Injection** (**DI**) is a pattern that can help developers decouple the
  different pieces of their applications

- In ASP .NET Core, both framework services and application services can be
  injected into your classes, rather than being tightly coupled

- **DI** is an integral part of ASP .NET Core (.NET 5)

- **Dependency Injection** is a form of **IoC** (**Inversion of Control**)
  - **Inversion of Control** design principle suggest that the inversion of various
    types of controls in object orientated design to achieve loose coupling
    between the application classes

- **DI** is the fifth principle of **S.O.L.I.D.**
    - Five basic principles of object orientated programming which states that a
      class should depend on abstraction and not upon the implementations
    - According to the principles, a class should concentrate on fulfilling its
      responsabilities and not on creating objects that it requires to fulfill
      those responsabilities. And that's where **DI** comes into play, providing the
      class with the required objects

- Built-in **IoC Container**
- The built-in container is represented by `IServiceProvider`
  - `IServiceProvider` implementation supports constructor injection
  - By default, the types of classes that are managed by **IoC Container** is called
    **Services**. There are basically two types of **Services**
- Types of **Services** in ASP .NET Core (.NET 5)
  - **Framework Services**: Services that are part of ASP .NET Core framework
    itself. Ex: `IApplicationBuilder`, `IHostingEnvironment`, `IFactoryLogger`...
  - **Application Services**: Custom Services made by the developers for the application

## Service Lifetime ##

### Singleton ###

- Same instance for the life of the application (unless restarted)
- Syntax to register:
  ```cshapr
  services.AddSingleton<>
  ```
- Should be used very carefully
- Singleton service sends same instance for the life of the application
- E.g. If you click on all vill or link on a website, whenever an instance is
  required it will send same object. It will change only when application
  restarts
- It can be used for services that are expensive to instantiate.
- Memory wil be allocated just once. So, Garbage Collector will have less things
  to do

### Scoped ###

- Same instance for one scope (one request in most cases)
- Syntax to register:
  ```cshapr
  services.AddScoped<>
  ```
- Not ideal for multi-threading
- Scoped services sends a new instance for each request
- E.g: If you click on a view or a link for that page load, if an instance is
  requested 10 times, it will send the same object
- An Entity Framework `Context` is a good candidate

### Transient ###

- Different instance every time the service is injected (or requested)
- Syntax to register:
  ```cshapr
  services.AddTransient<>
  ```
- Always try to register a service as a Transient if unsure
- Transient services sends a new instance every time it is requested
- E.g: If you click on a view or a link for that page load, if an instance is
  requested 10 times, it will send 10 different objects
- Works best for light weight and stateless services

## Types of Injection ##

- Constructor Injection
- Action Injection
  - Injection of dependency classes as parameters of IActionResult methods
  - Useful if only a given method needs some dependencies, so they doesn't need
    to be injected on the constructor of the class
  - Example:
    ```csharp
    public IActionResult AllConfigSettings(
            [FromServices] IOptions<StripeSettings> stripeOptions,
            [FromServices] IOptions<WazeForecastSettings> wazeForecastOptions,
            [FromServices] IOptions<TwilioSettings> twilioOptions,
            [FromServices] IOptions<SendGridSettings> sendGridOptions
            )
        {
            List<string> messages = new List<string>();
            messages.Add($"Waze config - Forecast Tracker: {wazeForecastOptions.Value.ForecastTrackerEnabled}");
            messages.Add($"Stripe config - Secret Key: {stripeOptions.Value.SecretKey}");
            messages.Add($"Stripe config - Publishable Key: {stripeOptions.Value.PublishableKey}");
            messages.Add($"Twilio config - Phone Number: {twilioOptions.Value.PhoneNumber}");
            messages.Add($"Twilio config - AuthToken: {twilioOptions.Value.AuthToken}");
            messages.Add($"Twilio config - Account Sid: {twilioOptions.Value.AccountSid}");
            messages.Add($"SendGrid config - Send GridKey: {sendGridOptions.Value.SendGridKey}");

            return View(messages);
        }
    ```
- View Injection
  - Injection of dependencies into View Pages
  - Example:
    ```csharp
    ...
    @using Microsoft.Extensions.Options
    @inject IOptions<WazeCredit.Utility.AppSettingsClasses.WazeForecastSettings> wazeForecastSettings
    ...
    <p>Current Market Prediction Status: @(wazeForecastSettings.Value.ForecastTrackerEnabled ? "Online" : "Offline")</p>
    ```
- Middleware Injection
  - Allows injections on its constructor and methods, even that it works differently
  - When injected on the constructor, the object remains the same. Since
    middlewares are registered the first time the application runs, and it
    remains the same object. Injections on its constructor also remains the same
    while it exists (even Transient Services).

## Registering Services ##

Alternative ways of registering a Service on Startup class:
```csharp
// Transient and Scoped
// With abstraction (interface)
services.AddTransient<Desired_Interface>(Desired_Implementation());
services.AddTransient(typeof(Desired_Interface), typeof(Desired_Implementation));
// Without abstraction (interface)
services.AddTransient<Desired_Class>();
services.AddTransient(typeof(Desired_Implementation));

//Singleton
// With abstraction (interface)
services.AddSingleton<Desired_Interface>(new Desired_Implementation());
// Without abstraction (interface)
services.AddSingleton<Desired_Class>();
services.AddSingleton(new Desired_Implementation());
```
