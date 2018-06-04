using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fb_InstaWpf.Helper
{
   public class FileOperation
    {
        public static string UserName { get; set; }
        public static string Password { get; set; }

       public static void AddIntoTextFile(string fileName, string id)
       {
           File.AppendAllText(fileName, id + Environment.NewLine);
       }
    }
}
