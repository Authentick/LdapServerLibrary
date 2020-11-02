FROM mcr.microsoft.com/dotnet/sdk:5.0

LABEL org.opencontainers.image.source https://github.com/lukasreschke/LdapServer

RUN apt-get update
RUN apt-get install -y ldap-utils netcat lsof zsh tcpdump
RUN sh -c "$(curl -fsSL https://raw.githubusercontent.com/ohmyzsh/ohmyzsh/master/tools/install.sh)" "" --unattended

