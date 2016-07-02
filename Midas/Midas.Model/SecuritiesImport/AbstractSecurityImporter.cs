using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Midas.Model.SecuritiesImport
{
    public abstract class AbstractSecurityImporter : ISecurityImporter
    {
        protected  readonly string _marketName ;

        protected AbstractSecurityImporter(string marketName)
        {
            _marketName = marketName;
        }
        public abstract  Task<IEnumerable<Security>> ImportSecuritiesAsync(string path);

        protected async Task<string> ReadTextAsync(string filePath)
        {
            using (FileStream sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            {
                StringBuilder builder = new StringBuilder();
                byte[] buffer = new byte[0x1000];
                int numRead;
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string text = Encoding.UTF8.GetString(buffer, 0, numRead);
                    builder.Append(text);
                }
                return builder.ToString();
            }

        }
    }
}