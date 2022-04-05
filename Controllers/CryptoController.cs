using CoinP2P.Helpers;
using CoinP2P.Models.Crypto;
using Microsoft.AspNetCore.Mvc;

namespace CoinP2P.Controllers;

public class CryptoController : Controller
{

    public IActionResult Index()
    {
        var site = $"{HttpContext.Request.Host}{HttpContext.Request.Path}";
        return View(new
        {
            Site = site
        });
    }

    public IActionResult Generate()
    {
        KeyPair keys = default;
        keys.Generate();
        return Ok(new
        {
            Private = keys.Private.GetBase64String(),
            Public = keys.Public.GetBase64String()
        });
    }

    public class EncryptModel
    {
        public string? Message { get; set; }
        public byte[]? Key { get; set; }
    }

    [HttpPost]
    public IActionResult Encrypt([FromBody] EncryptModel model)
    {
        var encrypted = model.Message!.GetBytes().Encrypt(model.Key!);
        return Ok(new
        {
            Data = encrypted.GetBase64String()
        });
    }

    public class DecryptModel
    {
        public byte[]? Data { get; set; }
        public byte[]? Key { get; set; }
    }

    [HttpPost]
    public IActionResult Decrypt([FromBody] DecryptModel model)
    {
        var decrypted = model.Data!.Decrypt(model.Key!);
        var message = decrypted.GetString();
        return Ok(new
        {
            Message = message
        });
    }

    public class SignModel
    {
        public string? Message { get; set; }
        public byte[]? Key { get; set; }
    }

    [HttpPost]
    public IActionResult Sign([FromBody] SignModel model)
    {
        var signature = model.Message!.GetBytes().SignData(model.Key!);
        return Ok(new
        {
            Value = signature.GetBase64String()
        });
    }

    public class VerifyModel
    {
        public string? Message { get; set; }
        public byte[]? Signature { get; set; }
        public byte[]? Key { get; set; }
    }

    [HttpPost]
    public IActionResult Verify([FromBody] VerifyModel model)
    {
        var valid = model.Message!.GetBytes()!.VerifyData(model.Signature!, model.Key!);
        return Ok(new
        {
            Value = valid
        });
    }

}