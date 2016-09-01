using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileBrowsing.Models
{
    public enum ActionType
    {
        NULL = 0,
        GetDrives = 1,
        GetItems = 2,
        CountFiles = 3
    };

    public class DirAction
    {
        public DirAction(string path)
        {
            Path = path;
        }

        public void SetType()
        {
            if (Type == ActionType.NULL)
                Type = (Path.Length > 0 ? ActionType.GetItems : ActionType.GetDrives);
        }

        public string Path { get; }
        public ActionType Type { get; set; }
    }
}