using System.Net.WebSockets;
using CoinP2P.Models;
using CoinP2P.Models.Network;
using Microsoft.AspNetCore.Mvc;

namespace CoinP2P.Controllers;

public class NetworkController : Controller
{

    public P2PNode Node { get; }

    public NetworkController(P2PNode node)
    {
        Node = node;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var site = $"{HttpContext.Request.Host}{HttpContext.Request.Path}";
        return View(new
        {
            Site = site
        });
    }

    [HttpGet]
    public async Task ConnectMe()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var host = new P2PHost
            {
                Remote = $"{HttpContext.Connection.RemoteIpAddress}:{HttpContext.Connection.RemotePort}",
                Socket = socket
            };
            await Node.Poll<ChatMessage>(host, (m) =>
            {
                return true;
            });
        }
    }

    [HttpPost]
    public async Task ConnectTo([FromBody] string remote)
    {
        using var socket = new ClientWebSocket();
        var uri = new Uri($"wss://{remote}/ConnectMe");
        await socket.ConnectAsync(uri, CancellationToken.None);
        var host = new P2PHost
        {
            Remote = remote,
            Socket = socket
        };
        await Node.Poll<ChatMessage>(host, (m) =>
        {
            return true;
        });
    }

    [HttpGet]
    public IActionResult Log()
    {
        return View(new
        {
            Hosts = Node.Hosts.Select(x => x.Remote),
            Nonces = Node.Nonces,
            Log = Node.Log
        });
    }

}