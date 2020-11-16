# LDAP Server Library Sample

This sample implements a simple LDAP server using the LDAP Server Library.

## Code pointers

- `Program.cs`
  - Startup of the LDAP server and registration of the event listener
- `LdapEventListener.cs`
  - Contains the callbacks executed by the LDAP Server Library. This would implement most of your custom logic.
- `UserDatabase.cs`
  - Contains the UserDatabase that the custom logic in `LdapEventListener.cs` is listening for.

## Running the server

```
dotnet run
```

The server will then listen on port 389.
