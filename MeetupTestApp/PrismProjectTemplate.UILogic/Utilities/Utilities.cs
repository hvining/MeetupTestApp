using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MeetupTestClient.UILogic
{
    public class Utilities
    {
        public static async Task<IEnumerable<byte>> ParseResponse(WebResponse response)
        {
            List<byte> bytes = new List<byte>();
            using (var stream = response.GetResponseStream())
            {
                int read = 0;
                byte[] buffer = new byte[1024 * 1024];
                while ((read = await stream.ReadAsync(buffer, 0, 1024 * 1024)) > 0)
                {
                    bytes.AddRange(buffer.Take(read));
                }
            }

            return bytes;
        }
    }
}
