using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace SaveLoadSystem {
    /// <summary>
    /// Compresses data to reduce load times
    /// </summary>
    public static class GZIP {
        public static byte[] Compress(string text) {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            using var output = new MemoryStream();
            using (var gzip = new GZipStream(output, CompressionMode.Compress)) {
                gzip.Write(bytes, 0, bytes.Length);
            }
            return output.ToArray();
        }

        public static string Decompress(byte[] compressed) {
            using var input = new MemoryStream(compressed);
            using var gzip = new GZipStream(input, CompressionMode.Decompress);
            using var output = new MemoryStream();
            gzip.CopyTo(output);
            return Encoding.UTF8.GetString(output.ToArray());
        }
    }

}
