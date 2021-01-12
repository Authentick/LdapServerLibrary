# LDAP Server Library Sample

This sample implements a simple LDAP server using the LDAP Server Library.

## Code pointers

- `Program.cs`
  - Startup of the LDAP server and registration of the event listener, and the TLS certificate used for STARTTLS
- `LdapEventListener.cs`
  - Contains the callbacks executed by the LDAP Server Library.
- `SearchExpressionBuilder.cs`
  - Builds the LINQ queries for the search expressions passed by the server.
- `UserDatabase.cs`
  - Contains the UserDatabase that the custom logic in `LdapEventListener.cs` is listening for.
- `ConsoleLogger.cs`
  - Logs exceptions from the LDAP server.

## Running the server

```bash
dotnet run
```

The server will then listen on port 3389.
