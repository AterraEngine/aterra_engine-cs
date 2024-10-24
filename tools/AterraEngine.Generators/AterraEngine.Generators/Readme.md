# AterraEngine.Generators

This project contains Source Code Generators for the AterraEngine and its various sub-projects.

## Content

- **[InjectableServicesGenerator.cs](InjectableServicesGenerator.cs)**:
    - **Purpose**: This source generator automatically generates extension methods for registering services into the
      dependency injection container.
    - **How it works**:
        - **Attribute-Matched Classes**: It scans for classes annotated with a custom attribute
          `InjectableServiceAttribute<TService>`.
        - **Service Registration**: For each annotated class, it creates a registration method (e.g.,
          `services.AddSingleton<,>`).
        - **Generated Output**: The generated file `InjectableServicesExtensions.g.cs` contains an extension method
          `RegisterServicesFrom<YourAssemblyName>` which registers all the discovered services when invoked.
    - **Testing**:
        - Verification of the presence of the `InjectableServiceAttribute`.
        - Correct parsing of the generic type and lifetime parameters.
        - Generation of the expected service registration code.
