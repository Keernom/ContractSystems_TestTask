using System.Reflection;

namespace TestTask_ContractSystems
{
    public class Program
    {
        static void Main(string[] args)
        {
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            HTMLDocCreator docCreator = new HTMLDocCreator(path);
            DirectoryAnalyzer dirAnalyzer = new DirectoryAnalyzer(path);

            Console.WriteLine("Начинается сканирование директории...");
            DirectoryTreeGenerate(docCreator, dirAnalyzer);

            Statistics stats = new Statistics(dirAnalyzer.TypeFrequency);

            Console.WriteLine("Собирается статистическая информация...");
            FilesCountGenerate(docCreator, dirAnalyzer.TypeFrequency, stats.FilesCount);
            FilesSizeGenerate(docCreator, dirAnalyzer.TypeFrequency, dirAnalyzer.TypeSize);

            Console.WriteLine("Сохраняется документ...");
            docCreator.SaveDocument();

            Console.ReadLine();
        }

        private static void FilesSizeGenerate(HTMLDocCreator docCreator, Dictionary<string, int> typeFrequency,
            Dictionary<string, int> typeSize)
        {
            docCreator.AddElement("hr", docCreator.HtmlDoc.Body);
            docCreator.AddElementWithText("h2", docCreator.HtmlDoc.Body, "Средний размер для каждого MimeType:");
            var sizeUl = docCreator.AddElement("ul", docCreator.HtmlDoc.Body);

            foreach (var e in typeSize)
            {
                string text = e.Key + " - " + typeSize[e.Key] / typeFrequency[e.Key] + " байт";
                docCreator.AddElementWithText("li", sizeUl, text);
            }
        }

        private static void FilesCountGenerate(HTMLDocCreator docCreator, 
            Dictionary<string, int> typeFrequency ,int filesCount)
        {
            docCreator.AddElement("hr", docCreator.HtmlDoc.Body);
            docCreator.AddElementWithText("h2", docCreator.HtmlDoc.Body, "Частота появления каждого MimeType:");
            var freqUl = docCreator.AddElement("ul", docCreator.HtmlDoc.Body);

            foreach (var e in typeFrequency)
            {
                double percentFreq = Math.Round((double)typeFrequency[e.Key] / filesCount * 100, 3);

                string text = e.Key + " (" + typeFrequency[e.Key] + "шт; " + percentFreq + "%)";
                docCreator.AddElementWithText("li", freqUl, text);
            }
        }

        private static void DirectoryTreeGenerate(HTMLDocCreator docCreator, DirectoryAnalyzer dirAnalyzer)
        {
            docCreator.AddElementWithText("h2", docCreator.HtmlDoc.Body, "Результат сканирования директории:");

            var dirUl = docCreator.AddElement("ul", docCreator.HtmlDoc.Body);
            docCreator.SetElementId(dirUl, "myUl");
            var mainLi = docCreator.AddElement("l1", dirUl);
            var span = docCreator.AddElement("span", mainLi);
            docCreator.AddTextToSpan(span, dirAnalyzer.MainDirInfo.Name);

            docCreator.SetElementClassName(span, "caret");

            float totalSize = dirAnalyzer.RecursiveDirScan(dirAnalyzer.MainDirInfo, docCreator, mainLi, 0);

            docCreator.AddTextToSpan(span, " (" + totalSize.ToString() + " байт)");
        }
    }
}