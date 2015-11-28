# Folke.Identity.Elm
ASP.NET Identity provider for [Elm](https://github.com/folkelib/Folke.Elm).

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
