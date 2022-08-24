# Notes

Simple console chat program that uses the [nats](https://nats.io) message bus
for internal communication.

## Construction and Dependencies

```powershell
PS> dotnet new console -n chatter --output .
PS> dotnet add package NATS.Client
PS> dotnet add package CommandLineParser
```

## How to run

### Starting the NATS message bus

This uses the nats docker image located [here](https://hub.docker.com/_/nats)

The simplest use is to expose the default client port of nats (4222) as in:

```powershell
PS> docker run -p 4222:4222 nats
```

Alternatively, if you want to use a client port of 4444 - use the below command:

```powershell
PS> docker run -p 4444:4444 nats -p 4444
```

### Starting a chatter client

The chatter console application takes the following arguments:

* -n \<NAME\> name of the chatter client. Limit to max 8 characters, allowed is A-Z0-9. Illegal characters are silently removed.
* -p \<PORT\> the port of the nats messages bus. Defaults to 4222

This will start a default run, with a random assigned name:

```powershell
PS> dotnet run
```

This will start a run with the name: "TEST" and connecting to nats on port 4444:

```powershell
PS> dotnet run -n TEST -p 4444
```

### Communicating to other chatter clients

Can be done either directly to the named client by prefixing the messages with the name of the client.
In the below example, the target client is named "OTHER".

```powershell
PS> dotnet run -n TEST
Chatter:
Chatter TEST listening...
INP> OTHER:Hello from me
```

With no named prefix - the message will be sent on the "all" channel, that all clients also listens on.
