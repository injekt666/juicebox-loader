using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace juiceboxes_loader.io
{
    public static class CreateDirectories
    {
        public static void Create()
        {
            Directory.CreateDirectory("C:\\configs");
            Directory.CreateDirectory("C:\\configs\\user1");
            Directory.CreateDirectory("C:\\configs\\user2");
            Directory.CreateDirectory("C:\\configs\\user3");
            Directory.CreateDirectory("C:\\configs\\user4");
            Directory.CreateDirectory("C:\\configs\\user3\\extract");
            Directory.CreateDirectory("C:\\configs\\user1\\extract");
            Directory.CreateDirectory("C:\\configs\\user2\\extract");
            Directory.CreateDirectory("C:\\configs\\user4\\extract");
        }
    }
}
