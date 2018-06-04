using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Fb_InstaWpf.Model
{
    public class ConstantClass
    {
     
        public static string JsonFileFolder = "JsonFile";

        public static string ConnectrionString { get; set; }
        public static string DBDirectory = "DBFbInstaComment";
        public static string DBFileName = "FbInstaCommentDb.s3db";
        
    }
}
