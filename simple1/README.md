# nats.io Simple test

## Start a docker with a NATS running

```powershell
PS> docker run -p 4444:4444 nats -p 4444
```

## Start 2 chatter clients

```powershell
PS> cd ".\chatter"
PS> # start new powershell to start server in
PS> saps pwsh
---------------------
PS> dotnet run -n TEST1 -p 4444
---------------------
PS> # start new powershell to start worker in
PS> saps pwsh
---------------------
PS> dotnet run -n TEST2 -p 4444
---------------------
```

The chatter clients will prompt with `INP>` just write your chat-text at the prompt.

If the text is started with `[NAME]:[TEXT]` then the chat text is sent to [NAME] only.

If no [NAME] is given - text is sent to all chat-clients.