using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OMMSClientCoreCSharp
{
    namespace Utils
    {
        public class EncryptedConnector
        {
            public Socket ClientSocket { get; set; }

            private readonly ICryptoTransform encryptorCryptoTransform = null;
            private readonly ICryptoTransform decryptorCryptoTransform = null;

            private readonly byte[] key;

            public EncryptedConnector(String key)
            {
                String k = key;
                ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                k = k + new String('0', 32 - k.Length);
                this.key = Encoding.UTF8.GetBytes(k);
                var rijndaelManaged = new RijndaelManaged()
                {
                    Key = Encoding.UTF8.GetBytes(k),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                encryptorCryptoTransform = rijndaelManaged.CreateEncryptor();
                decryptorCryptoTransform = rijndaelManaged.CreateDecryptor();
            }

            public EncryptedConnector(String key, Socket socket)
            {
                String k = key;
                ClientSocket = socket;
                k = k + new String('0', 32 - k.Length);
                this.key = Encoding.UTF8.GetBytes(k);
                var rijndaelManaged = new RijndaelManaged()
                {
                    Key = Encoding.UTF8.GetBytes(k),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                encryptorCryptoTransform = rijndaelManaged.CreateEncryptor();
                decryptorCryptoTransform = rijndaelManaged.CreateDecryptor();
            }

            public EncryptedConnector Connect(String ip, int port)
            {
                return this.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            }

            public EncryptedConnector Connect(IPEndPoint endPoint)
            {
                ClientSocket.Connect(endPoint);
                return this;
            }

            public int Send(String content)
            {
                return Println(content);
            }

            public int Println(String content)
            {
                byte[] bytes = EncryptECB(Encoding.UTF8.GetBytes(content), key);
                String str = Encoding.UTF8.GetString(bytes) + "\n";
                byte[] c = Encoding.UTF8.GetBytes(str);
                return ClientSocket.Send(c);
            }

            public String ReadLine()
            {
                byte[] buffer = new Byte[1];
                int length = 0;

                while (buffer.Last() != '\n')
                {
                    byte[] cbuffer = new byte[1];
                    ClientSocket.Receive(cbuffer, 0, 1, SocketFlags.None);

                    length++;
                    byte[] bytes = new byte[length + 1];
                    bytes[bytes.Length - 1] = cbuffer[0];

                    buffer = bytes;
                }

                buffer = DecryptECB(Convert.FromBase64String(Encoding.UTF8.GetString(buffer)), key);
                String text = Encoding.UTF8.GetString(buffer);
                //MessageBox.Show(text);
                return text;
            }

            private byte[] EncryptECB(byte[] data, byte[] key)
            {
                var encrypted = encryptorCryptoTransform.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetBytes(Convert.ToBase64String(encrypted, 0, encrypted.Length));
            }

            private byte[] DecryptECB(byte[] data, byte[] key)
            {
                var decrypted = decryptorCryptoTransform.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetBytes(Convert.ToBase64String(decrypted, 0, decrypted.Length));
            }

        }
    }
}
