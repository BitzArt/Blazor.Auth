![Tests](https://github.com/BitzArt/Blazor.Auth/actions/workflows/Tests.yml/badge.svg)

[![NuGet version](https://img.shields.io/nuget/v/BitzArt.Blazor.Auth.svg)](https://www.nuget.org/packages/BitzArt.Blazor.Auth/)
[![NuGet downloads](https://img.shields.io/nuget/dt/BitzArt.Blazor.Auth.svg)](https://www.nuget.org/packages/BitzArt.Blazor.Auth/)

## Overview

**Blazor.Auth** is a developer-friendly JWT & Cookie authentication library for Blazor. Built for .NET 8+ and designed to make Blazor authentication less painful, more secure, and (dare we say) enjoyable.

> 🍪
> The package uses [Blazor.Cookies](https://github.com/BitzArt/Blazor.Cookies) for persisting user authentication state via browser cookies.

This package is for developers who want to:

- Add JWT authentication to Blazor apps (Server, WebAssembly, United)
- Use secure cookie authentication in .NET 8+ Blazor projects
- Implement authentication & authorization in public-facing Blazor sites, SaaS dashboards, or internal enterprise Blazor apps
- Implement custom login, logout, and token refresh flows
- Leverage Blazor's built-in authorization capabilities (such as `[AuthorizeView]` and `[CascadingAuthenticationState]`)

### Why Blazor.Auth?

Because you shouldn't have to fight your framework just to log users in. This library skips the boilerplate, handles Blazor's quirks (SSR, WASM, Server, you name it), and keeps your sanity intact. Built by developers who have already made all the mistakes—so you don't have to.

## Install via NuGet

**Server project:**
```sh
dotnet add package BitzArt.Blazor.Auth.Server
```

**Client project:**
```sh
dotnet add package BitzArt.Blazor.Auth.Client
```

## Quickstart

1. Add the NuGet package(s) to your Blazor project (see above).
2. Configure authentication in your `Program.cs` (see [docs](https://bitzart.github.io/Blazor.Auth)).
3. Use your favorite Blazor auth patterns.
4. Enjoy secure, cookie-based authentication without the headaches.

## Resources

Find guides, API docs, and examples in the [documentation](https://bitzart.github.io/Blazor.Auth).

[![documentation](https://img.shields.io/badge/documentation-512BD4?style=for-the-badge)](https://bitzart.github.io/Blazor.Auth)

## License

[![License](https://img.shields.io/badge/mit-%230072C6?style=for-the-badge)](https://github.com/BitzArt/Blazor.Auth/blob/main/LICENSE)

<!--
Keywords: Blazor authentication, JWT, cookie auth, .NET 8, ASP.NET Core, OpenID Connect, OAuth2, login, token, claims, identity, middleware, secure, session, refresh token, user management, authorization, Blazor WASM, Blazor Server, NuGet, C#, Microsoft, browser cookies, no localStorage, no sessionStorage, SSR, WASM, Server, Auto, developer-friendly, secure authentication, Blazor security, authentication library, Blazor identity, Blazor login, Blazor token, Blazor claims, Blazor middleware, Blazor authorization, Blazor user management
-->
