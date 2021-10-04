# [Dependency Injection in .NET 5 (.NET Core)](https://www.udemy.com/course/dependency-injection-in-net-5-net-core)

- [Course Original Repository](https://github.com/bhrugen/WazeCredit)

## Dependency Injection ##

- ASP .Net Core is designed from scratch to support **Dependency Injection**
  - A technique for achieving **Inversion of Control** between classes and their dependencies

- NET Core injects objects of dependency classes through constructor or method
  by using built-in **IoC container**
  - **IOC** - **Inversion of Control** Container: A framework for implementing automatic
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
