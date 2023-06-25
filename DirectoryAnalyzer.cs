using Aspose.Html;
using Aspose.Html.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask_ContractSystems
{
    internal class DirectoryAnalyzer
    {
        private DirectoryInfo _dirInfo;
        private Dictionary<string, int> _typeFrequency = new Dictionary<string, int>();
        private Dictionary<string, int> _typeSize = new Dictionary<string, int>();

        public string MainDirName { get {  return _dirInfo.Name; } }
        public DirectoryInfo MainDirInfo { get { return _dirInfo; } }
        public Dictionary<string, int> TypeFrequency { get { return _typeFrequency; } }
        public Dictionary<string, int> TypeSize { get { return _typeSize; } }

        public DirectoryAnalyzer(string path)
        {
            _dirInfo = new DirectoryInfo(path);
        }

        public float RecursiveDirScan(DirectoryInfo dInfo, HTMLDocCreator creator, Element parentElement, float fileSize)
        {
            var nestedUl = creator.AddElement("ul", parentElement);
            creator.SetElementClassName(nestedUl, "nested");

            FileInfo[] files = dInfo.GetFiles();
            foreach (var f in files)
            {
                string type = GetContentType(f.Name);

                if (!_typeFrequency.ContainsKey(type))
                {
                    _typeFrequency.Add(type, 0);
                }

                if (!_typeSize.ContainsKey(type))
                {
                    _typeSize.Add(type, 0);
                }

                _typeFrequency[type]++;
                _typeSize[type] += (int)f.Length;
                fileSize += f.Length;

                string label = f.Name + ", mime type - " + type + " (" + f.Length + " байт)";
                creator.AddElementWithText("li", nestedUl, label);
            }

            DirectoryInfo[] dirs = dInfo.GetDirectories();
            foreach (var d in dirs)
            {
                var li = creator.AddElement("li", nestedUl);
                var span = creator.AddElement("span", li);
                creator.SetElementClassName(span, "caret");
                creator.AddTextToSpan(span, d.Name);

                float totalSize = RecursiveDirScan(d, creator, li, 0);
                fileSize += totalSize;

                string label = " (" + totalSize.ToString() + " байт)";
                creator.AddTextToSpan(span, label);
            }

            return fileSize;
        }

        private string GetContentType(string fileName)
        {
            string contentType = "application/octetstream";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (registryKey != null && registryKey.GetValue("Content Type") != null)
                contentType = registryKey.GetValue("Content Type").ToString();
            return contentType;
        }
    }
}
