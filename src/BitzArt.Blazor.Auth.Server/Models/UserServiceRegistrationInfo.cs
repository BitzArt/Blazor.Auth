namespace BitzArt.Blazor.Auth.Server;

internal record UserServiceRegistrationInfo(Type ImplementationType, ICollection<Type>? AdditionalTypes = null);
