FROM mcr.microsoft.com/devcontainers/base:ubuntu-22.04

ENV NODE_MAJOR=20
ENV DOTNET_ROOT=/usr/share/dotnet
ENV PATH=$PATH:$DOTNET_ROOT:~/.dotnet/tools
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_USE_POLLING_FILE_WATCHER=true

RUN apt-get update; \
    apt-get install -y ca-certificates curl gnupg; \
    mkdir -p /etc/apt/keyrings; \
    curl -fsSL https://deb.nodesource.com/gpgkey/nodesource-repo.gpg.key | gpg --dearmor -o /etc/apt/keyrings/nodesource.gpg; \
    echo "deb [signed-by=/etc/apt/keyrings/nodesource.gpg] https://deb.nodesource.com/node_$NODE_MAJOR.x nodistro main" | tee /etc/apt/sources.list.d/nodesource.list

RUN apt-get update; \
    apt-get install -y nodejs python3 libc++1 libatomic1 gnupg2

RUN curl -sSL https://dot.net/v1/dotnet-install.sh | bash -s -- --channel 8.0 --install-dir /usr/share/dotnet/

RUN curl -qL https://www.npmjs.com/install.sh | sh

RUN /usr/share/dotnet/dotnet tool install --global PowerShell
RUN /usr/share/dotnet/dotnet workload install wasm-tools