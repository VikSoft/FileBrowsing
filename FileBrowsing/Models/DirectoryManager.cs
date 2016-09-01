using System;
using System.Collections.Generic;
using System.IO;

namespace FileBrowsing.Models
{
    public static class DirectoryManager
    {
        public static IEnumerable<DirItem> GetDrives()
        {
            List<DirItem> dirItems = new List<DirItem>();
            foreach (var item in DriveInfo.GetDrives())
                if (item.DriveType == DriveType.Fixed)
                    dirItems.Add(new DirItem(item.Name, item.Name));
            return dirItems.ToArray();
        }

        public static IEnumerable<DirItem> GetDirItems(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            List<DirItem> dirItems = new List<DirItem>();
            string up = dir.Parent?.FullName ?? "";
            dirItems.Add(new DirItem("..", up));
            try {
                foreach (var item in dir.GetDirectories())
                    dirItems.Add(new DirItem(item.Name, item.FullName));
                foreach (var item in dir.GetFiles())
                    dirItems.Add(new DirItem(item.Name, item.FullName, false));
            } catch (Exception) {/*Ignore the folders we can't access*/}
            return dirItems.ToArray();
        }

        public static string GetNextFolder(string root, string last)
        {
            if (last.Length == 0) return root;
            string rootParent = new DirectoryInfo(root).Parent?.FullName ?? "";
            DirectoryInfo tmp = new DirectoryInfo(last).Parent;
            DirectoryInfo[] subTmp;

            while ((subTmp = tmp?.GetDirectories()) != null &&
                subTmp[subTmp.Length - 1].FullName.Equals(last) &&
                !tmp.FullName.Equals(rootParent)
                )
            {
                last = tmp.FullName;
                tmp = tmp.Parent;
                if (tmp == null) return "";
            }

            if (tmp.FullName.Equals(rootParent)) return "";

            for (int i = 0; i < subTmp.Length - 1; ++i)
            {
                if (subTmp[i].FullName.Equals(last))
                {
                    return subTmp[i + 1].FullName;
                }
            }
            return "";
        }
        public static bool CountFiles(string path, ref FileCounter counter)
        {
            if (path.Length == 0) return true;

            DirectoryInfo dir = new DirectoryInfo(path);
            try
            {
                foreach (var item in dir.GetFiles())
                {
                    if (item.Length <= 10 << 20)
                        counter.Small++;
                    else if (item.Length >= 100 << 20)
                        counter.Large++;
                    else if (item.Length > 10 << 20 && item.Length <= 50 << 20)
                        counter.Medium++;
                }
                foreach (var item in dir.GetDirectories())
                {
                    CountFiles(item.FullName, ref counter);
                    if (counter.IsEnough() && !counter.IsFirst()) return true;
                }
            }
            catch (Exception) {/*Ignore the folders we can't access*/}

            counter.LastPath = path;
            return false;
        }
    }
}