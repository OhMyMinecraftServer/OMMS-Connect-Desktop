using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using OMMSClientCoreCSharp.Data;
using OMMSClientCoreCSharp.Utils;

namespace OMMSClientCoreCSharp
{
    public class SessionClient
    {
        private EncryptedConnector connector;

        private SessionClient(EncryptedConnector connector)
        {
            this.connector = connector;
        }

        public static SessionClient CreateSessionClient(EncryptedConnector encryptedConnector)
        {
            return new SessionClient(encryptedConnector);
        }

        public Response Send(Request request)
        {
            String content = Util.ToJson(request);
            connector.Println(content);
            return Util.FromJson<Response>(connector.ReadLine());
        }

        public void Close()
        {
            Response response = Send(new Request("END"));
            if (Equals(response.code, "OK"))
            {
                connector.ClientSocket.Close();
            }
        }

    }
}
