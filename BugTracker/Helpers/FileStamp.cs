﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using BugTracker.Helpers;

namespace BugTracker.Helpers
{
    public class FileStamp
    {
        public static string MakeUnique(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            fileName = Path.GetFileNameWithoutExtension(fileName);
            fileName = Slugger.MakeSlug(fileName);
            fileName = $"{fileName}{DateTime.Now.Ticks}{extension}";
            return fileName;
        }
    }
}