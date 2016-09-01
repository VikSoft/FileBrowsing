using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileBrowsing.Models
{
    public class FileCounter
    {
        //Count at least 4k files per request
        private const long MaxCount = 1 << 12;
        public long Small { get; set; }
        public long Medium { get; set; }
        public long Large { get; set; }
        public bool Done { get; set; }

        public string Path { get; set; }
        public string LastPath { get; set; }

        public FileCounter(string path, string lastpath) : this(0, 0, 0)
        {
            Path = path;
            LastPath = lastpath;
        }

        public FileCounter(long s, long m, long l)
        {
            Small = s;
            Medium = m;
            Large = l;
            Done = false;
        }

        public bool IsEnough()
        {
            return MaxCount < Small + Medium + Large;
        }

        public bool IsFirst()
        {
            return LastPath == "";
        }
    }
}