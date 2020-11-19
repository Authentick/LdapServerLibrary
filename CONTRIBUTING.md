# Contributing to the LDAP Server Library

We generally appreciate any contributions to the LDAP Server Library. Please note, that whilst this aims to be a general-purpose library, this library was ultimately created to implement an LDAP Server in the [Gatekeeper application](https://github.com/GetGatekeeper/Server).

## Setup Dev Environment

The repository includes a dev container that ships all the required dependencies to set this up. Either use [GitHub Codespaces](https://github.com/codespaces) or [Visual Studio Code Remote Containers](https://code.visualstudio.com/docs/remote/containers#_quick-start-open-a-git-repository-or-github-pr-in-an-isolated-container-volume).

## Using Wireshark to analyze the traffic

The sample application provided in the "Sample" folder can easily be intercepted with tcpdump:

```
cd Sample/ && dotnet run
ldapsearch -W -H ldap://localhost -b "dc=ldap,dc=net" -D "cn=Manager,dc=ldap,dc=net" "uid=test"
tcpdump -i lo -w output.dump port 389
```

Once done, download "output.dump" and open it in Wireshark. This will give you a good overview of the behaviour.
