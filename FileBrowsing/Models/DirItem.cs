using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileBrowsing.Models
{
    public class DirItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsDir { get; }

        public DirItem(string name, string path) : this(name, path, true) { }
        public DirItem(string name, string path, bool isdir)
        {
            Name = name;
            Path = path;
            IsDir = isdir;
        }
    }
}