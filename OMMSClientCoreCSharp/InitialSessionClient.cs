using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMMSClientCoreCSharp;
using OMMSClientCoreCSharp.Data;
using OMMSClientCoreCSharp.Exceptions;
using OMMSClientCoreCSharp.Utils;

namespace OMMSClientCoreCSharp
{
    namespace Session
    {
        public class InitialSessionClient
        {
            private EncryptedConnector _encryptedConnector = null;
            private bool IsConnected = false;
            private InitialSessionClient(EncryptedConnector encryptedConnector)
            {
                this._encryptedConnector = encryptedConnector;
            }

            public static SessionClient Connect(String ip, int port, int code)
            {
                var currentTime = DateTime.Now;
                String key = currentTime.ToString("yyyyMMddhhmm");
                key = Util.Base64Encode(Util.Base64Encode(key));
                EncryptedConnector encryptedConnector = new EncryptedConnector(key);
                long timeCode = Int64.Parse(currentTime.ToString("yyyyMMddhhmm"));
                String connCode = (timeCode ^ code).ToString();
                connCode = Util.Base64Encode(connCode);
                connCode = Util.Base64Encode(connCode);

                String content =
                    Util.ToJson(new InitRequest("PING", Util.version).WithContentKeyPair("token", connCode));
                encryptedConnector.Connect(ip, port);
                encryptedConnector.Send(content);
                String line = encryptedConnector.ReadLine();
                Response response = Response.deserialize(line);

                if (Equals(response.code, "OK"))
                {
                    String newKey = response.getContent("key");
                    EncryptedConnector connector = new EncryptedConnector(newKey, encryptedConnector.ClientSocket);
                    return SessionClient.CreateSessionClient(connector);
                }

                throw new CannotConnectToServerException($"Server Returned Status {response.code}");
            }
        }
    }
    
}
