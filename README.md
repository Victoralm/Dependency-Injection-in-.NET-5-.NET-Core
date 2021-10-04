# [Dependency Injection in .NET 5 (.NET Core)](https://www.udemy.com/course/dependency-injection-in-net-5-net-core)

- [Course Original Repository](https://github.com/bhrugen/WazeCredit)

## Dependency Injection ##

- ASP .Net Core is designed from scratch to support Dependency Injection
  - A technique for achieving Inversion of Control between classes and their dependencies

- NET Core injects objects of dependency classes through constructor or method
  by using built-in IOC container
  - IOC - Inversion of Control Container: A framework for implementing automatic
    dependency injection
  - Manages object creation and its lifetime, also injects dependencies to their class
  - Creates an object of the specified class and also injects all of the
    dependency objects through constructor or method at run-time and dispose it
    at appropriate time. This is done so that the devs do not have to create and
    manage the objects manually

- Dependency Injection (DI) is a pattern that can help developers decouple the
  different pieces of their applications

- In ASP.NET Core, both framework services and application services can be
  injected into your classes, rather than being tightly coupled
