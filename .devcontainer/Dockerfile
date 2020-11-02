FROM mcr.microsoft.com/dotnet/sdk:5.0

RUN apt-get update
RUN apt-get install -y ldap-utils netcat lsof zsh
RUN sh -c "$(curl -fsSL https://raw.githubusercontent.com/ohmyzsh/ohmyzsh/master/tools/install.sh)" "" --unattended

