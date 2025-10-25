using NetBeta.Net;

Server server = new("192.168.1.127", 25565);
await server.StartServer();