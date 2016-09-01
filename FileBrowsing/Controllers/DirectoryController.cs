using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FileBrowsing.Models;

namespace FileBrowsing.Controllers
{
    public class DirectoryController : ApiController
    {
        // POST: api/Directory
        public dynamic Post([FromBody]DirAction action)
        {
            action.SetType();
            switch (action.Type)
            {
                case ActionType.GetDrives:
                    return DirectoryManager.GetDrives();
                case ActionType.GetItems:
                    return DirectoryManager.GetDirItems(action.Path);
                case ActionType.CountFiles:
                    {
                        string[] args = action.Path.Split('|');
                        FileCounter counter = new FileCounter(args[0], args[1]);
                        string start = DirectoryManager.GetNextFolder(counter.Path, counter.LastPath);
                        while (!DirectoryManager.CountFiles(start, ref counter))
                            start = DirectoryManager.GetNextFolder(counter.Path, counter.LastPath);
                        counter.Done = (start.Length == 0);
                        return counter;
                    }
            }
            return null;
        }
    }
}
