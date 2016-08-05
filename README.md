# Folke.Identity.Elm
ASP.NET Identity provider for [Elm](https://github.com/folkelib/Folke.Elm).

[![Build status](https://ci.appveyor.com/api/projects/status/55p13jsseuln3bj4?svg=true)](https://ci.appveyor.com/project/acastaner/folke-identity-elm)

## Usage

If `TUser` is your user class, in `ConfigureServices`:

```cs
services.AddElmIdentity<TUser>();
```

To create the tables:

```cs
var session = app.ApplicationServices.GetService<IFolkeConnection>();
session.UpdateStringIdentityUserSchema<TUser>();
session.UpdateStringIdentityRoleSchema();
```
