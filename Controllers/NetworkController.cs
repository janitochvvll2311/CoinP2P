using System.Net.WebSockets;
using CoinP2P.Models.Network;
using Microsoft.AspNetCore.Mvc;

namespace CoinP2P.Controllers;

public class NetworkController : Controller
{

    public Node Node { get; }

    public NetworkController(Node node)
    {
        Node = node;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task ConnectMe()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var host = new CoinP2P.Models.Network.Host
            {
                Remote = $"{HttpContext.Connection.RemoteIpAddress}:{HttpContext.Connection.RemotePort}",
                Socket = socket
            };
            await Node.Poll<Message>(host);
        }
    }

    [HttpGet]
    public async Task ConnectTo(string target)
    {
        var socket = new ClientWebSocket();
        var uri = new Uri($"wss://{target}/ConnectMe");
        await socket.ConnectAsync(uri, CancellationToken.None);
        var host = new CoinP2P.Models.Network.Host
        {
            Remote = target,
            Socket = socket
        };
        await Node.Poll<Message>(host);
    }

}