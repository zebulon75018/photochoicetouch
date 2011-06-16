using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SurfaceApplicationMaurice.Utility
{
    public class FileScanner
    {
        List<FileInfo> listFileInfo = null;
        public bool isEmpty = true;
        public FileScanner()
        {
            listFileInfo = new List<FileInfo>();
        }

        public void AddDirectory(string Directory)
        {
            isEmpty = false;
            DirectoryInfo dir = new DirectoryInfo(Directory);
            FileInfo[] resultdir = dir.GetFiles("*.jpg");
            foreach (FileInfo fi in resultdir)
            {
                listFileInfo.Add(fi);
            }
        }

        public List<FileInfo> getFiles(DateTime d)
        {
            List<FileInfo> result = new List<FileInfo>();
            foreach (FileInfo fi in listFileInfo)
            {
                if (fi.CreationTime.DayOfYear == d.DayOfYear)
                {
                    result.Add(fi);
                }
            }
            return result;
        }

    }
}
