using Natsurainko.Toolkits.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tcp.NET.Client.Models;
using Tcp.NET.Client;
using OMMS.Core.Models;
using PHS.Networking.Enums;
using Newtonsoft.Json.Linq;

namespace OMMS.Core;

public class OMMSCentralClient :IDisposable
{
    public TcpNETClient TcpClient { get; private set; }

    public string DateTimeKey { get; private set; }

    private ICryptoTransform EncryptorCryptoTransform;

    private ICryptoTransform DecryptorCryptoTransform;

    private ClientConnectionParameters ConnectionParameters;

    private bool Sending = false;

    private bool disposedValue;

    public OMMSCentralClient(ClientConnectionParameters parameters)
    {
        CreateCryptoTransforms();

        ConnectionParameters = parameters;
        TcpClient = new TcpNETClient(new ParamsTcpClient(
            host: parameters.IpAddress,
            port: parameters.Port,
            endOfLineCharacters: "\n",
            isSSL: false));
    }

    public async Task Connect()
    {
        await TcpClient.ConnectAsync();

        var connectionKey = (long.Parse(DateTimeKey) ^ ConnectionParameters.LoginCode)
            .ToString().ConvertToBase64().ConvertToBase64();

        var responsePackage = await SendAndGetReply<JObject>(RequestPackage.Ping(connectionKey));
        RefreshCryptoTransforms(responsePackage.Content["key"].ToString());
    }

    public async Task Disconnect()
    {
        await SendAndGetReply<JObject>(new("END"));
        await TcpClient.DisconnectAsync();
    }

    public async Task<ResponsePackage<TClass>> SendAndGetReply<TClass>(RequestPackage request)
    {
        ResponsePackage<TClass> responsePackage = default;
        bool completed = false;

        while (Sending)
            await Task.Delay(100);

        void Callback(object sender, Tcp.NET.Client.Events.Args.TcpMessageClientEventArgs args)
        {
            if (args.MessageEventType.Equals(MessageEventType.Receive))
            {
                responsePackage = JsonConvert.DeserializeObject<ResponsePackage<TClass>>(DecryptString(args.Bytes));
                completed = true;
                TcpClient.MessageEvent -= Callback;
                Sending = false;
            }
        }

        TcpClient.MessageEvent += Callback;
        await TcpClient.SendAsync(EncryptString(JsonConvert.SerializeObject(request)));

        Sending = true;

        while (!completed)
            await Task.Delay(100);

        return responsePackage;
    }

    private void CreateCryptoTransforms()
    {
        DateTimeKey = DateTime.Now.ToString("yyyyMMddhhmm");

        var encryptKey = DateTimeKey.ConvertToBase64().ConvertToBase64();
        encryptKey += new string('0', 32 - encryptKey.Length);

        using var rijndaelManaged = new RijndaelManaged
        {
            Key = Encoding.UTF8.GetBytes(encryptKey),
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };

        EncryptorCryptoTransform = rijndaelManaged.CreateEncryptor();
        DecryptorCryptoTransform = rijndaelManaged.CreateDecryptor();
    }

    private void RefreshCryptoTransforms(string key)
    {
        EncryptorCryptoTransform.Dispose();
        DecryptorCryptoTransform.Dispose();

        using var rijndaelManaged = new RijndaelManaged
        {
            Key = Encoding.UTF8.GetBytes(key),
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };

        EncryptorCryptoTransform = rijndaelManaged.CreateEncryptor();
        DecryptorCryptoTransform = rijndaelManaged.CreateDecryptor();
    }

    private string EncryptString(string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        var encrypted = EncryptorCryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);

        return Convert.ToBase64String(encrypted, 0, encrypted.Length);
    }

    private string DecryptString(byte[] data)
    {
        var bytes = Convert.FromBase64String(Encoding.UTF8.GetString(data));
        var encrypted = DecryptorCryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);

        return Encoding.UTF8.GetString(encrypted);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {

            }

            TcpClient.Dispose();
            EncryptorCryptoTransform.Dispose();
            DecryptorCryptoTransform.Dispose();

            ConnectionParameters = null;
            DateTimeKey = null;

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
