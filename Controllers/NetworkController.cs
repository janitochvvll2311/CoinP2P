using System.Net.Sockets;
using System.Net.WebSockets;
using CoinP2P.Helpers;
using CoinP2P.Models;
using CoinP2P.Models.Crypto;
using CoinP2P.Models.Network;
using Microsoft.AspNetCore.Mvc;

namespace CoinP2P.Controllers;

public class NetworkController : Controller
{

    public static Func<ChatMessage, bool> Validator = (m) =>
    {
        var remote = m.Remote!.GetBase64Array();
        var message = m.Message!.GetBytes();
        var signature = m.Signature!.GetBase64Array();
        var valid = remote.Concat(message).ToArray().ComputeHash().VerifyHash(signature, remote);
        return valid;
    };

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

    public async Task Listen(P2PHost host)
    {
        try
        {
            await Node.Poll<ChatMessage>(host, (m) =>
            {
                if (!Validator(m))
                {
                    Node.Log.Add($"{host.Remote}: Usurpation alert! (Address: {m.Remote!.GetBase64Array().ComputeHash().GetHexString()})");
                    return false;
                }
                return true;
            });
        }
        catch (Exception ex)
        {
            Node.Log.Add($"--- ERROR --- : {ex.Message}");
        }
        host.Socket!.Dispose();
    }

    [HttpGet]
    public async Task<IActionResult> ConnectMe()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var host = new P2PHost
            {
                Remote = $"{HttpContext.Connection.RemoteIpAddress}:{HttpContext.Connection.RemotePort}",
                Socket = socket
            };
            _ = Listen(host);
            return Ok(new
            {
                Message = "Connection OPEN"
            });
        }
        return BadRequest(new
        {
            Message = "Connection ERROR"
        });
    }

    [HttpPost]
    public async Task<IActionResult> ConnectTo([FromBody] string remote)
    {
        var socket = new ClientWebSocket();
        var uri = new Uri($"wss://{remote}/ConnectMe");
        await socket.ConnectAsync(uri, CancellationToken.None);
        if (socket.State == WebSocketState.Open)
        {
            var host = new P2PHost
            {
                Remote = remote,
                Socket = socket
            };
            _ = Listen(host);
            return Ok(new
            {
                Message = "Connection OPEN"
            });
        }
        return BadRequest(new
        {
            Message = "Connection ERROR"
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

    public IActionResult Generate()
    {
        KeyPair keys = default;
        keys.Generate();
        return Ok(new
        {
            Private = keys.Private.GetBase64String(),
            Public = keys.Public.GetBase64String(),
        });
    }

    public class SignModel
    {
        public byte[]? Remote { get; set; }
        public string? Message { get; set; }
        public byte[]? Key { get; set; }
    }

    [HttpPost]
    public IActionResult Sign([FromBody] SignModel model)
    {
        var signature = model.Remote!.Concat(model.Message!.GetBytes()).ToArray().ComputeHash().SignHash(model.Key!);
        return Ok(new
        {
            Value = signature.GetBase64String()
        });
    }

}