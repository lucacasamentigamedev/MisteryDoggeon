using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Aiv.Tiled
{
    internal class Base64Loader
    {
        public Stream Data { get; private set; }

        public Base64Loader(XElement xData)
        {
            if ((string)xData.Attribute("encoding") == "base64")
            {
                byte[] rawData = Convert.FromBase64String((string)xData.Value);
                Data = new MemoryStream(rawData, false);

                var compression = (string)xData.Attribute("compression");
                if (compression == "gzip")
                {
                    Data = new GZipStream(Data, CompressionMode.Decompress);
                }
                else if (compression == "zlib")
                {
                    // data
                    // Strip 2-byte header and 4-byte checksum
                    var bodyLength = rawData.Length - 6;
                    byte[] bodyData = new byte[bodyLength];
                    Array.Copy(rawData, 2, bodyData, 0, bodyLength);

                    var bodyStream = new MemoryStream(bodyData, false);
                    Data = new DeflateStream(bodyStream, CompressionMode.Decompress);
                }
            }
        }
    }
}
