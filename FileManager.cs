using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdKR
{
    public interface IFileManager
    {
        string[] OpenFile(string path);
        void SaveInitialData(string data, string path);
    }
    public class FileManager : IFileManager
    {
        public string[] OpenFile(string path)
        {
            return File.ReadAllLines(path).Where(y => y != "").ToArray();
        }

        public void SaveInitialData(string data, string path)
        {

            File.WriteAllText(path, data);
        }
    }
}
