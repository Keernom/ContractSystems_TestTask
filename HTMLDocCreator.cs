using Aspose.Html;
using Aspose.Html.Dom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestTask_ContractSystems
{
    public class HTMLDocCreator
    {
        private const string FileName = "myDoc";
        private HTMLDocument _htmlDoc;
        private string _programDir;

        public HTMLDocument HtmlDoc { get { return _htmlDoc; } }

        public HTMLDocCreator(string path)
        {
            _htmlDoc = new HTMLDocument();
            _programDir = path;

            AddCssLink();
        } 

        public Element AddElement(string element, Element parentElement)
        {
            var e = _htmlDoc.CreateElement(element);
            parentElement.AppendChild(e);
            return e;
        }

        public Element AddElementWithText(string element, Element parentElement, string text)
        {
            var e = _htmlDoc.CreateElement(element);
            var span = _htmlDoc.CreateElement("span");
            AddTextToSpan(span, text);
            e.AppendChild(span);

            parentElement.AppendChild(e);

            return e;
        }

        public Element AddElementWithText(string element, Element parentElement, string text, string spanClassName)
        {
            var e = _htmlDoc.CreateElement(element);
            var span = _htmlDoc.CreateElement("span");
            span.ClassName = spanClassName;

            AddTextToSpan(span, text);
            e.AppendChild(span);

            parentElement.AppendChild(e);

            return e;
        }

        public void AddTextToSpan(Element span, string text)
        {
            var label = _htmlDoc.CreateTextNode(text);
            span.AppendChild(label);
        }

        public void SetElementId(Element element, string id)
        {
            element.Id = id;
        }

        public void SetElementClassName(Element element, string className)
        {
            element.ClassName = className;
        }

        public void SaveDocument()
        {
            AddJsScript();

            string path = _programDir + $"\\{FileName}.html";
            _htmlDoc.Save(path);

            Console.WriteLine("\n Готово! Ваш документ сохранен в папке: \n" + path);
        }

        private void AddCssLink()
        {
            var style = _htmlDoc.CreateElement("style");
            style.TextContent = File.ReadAllText(_programDir + "\\htmlConfigs\\style.css");

            var head = _htmlDoc.GetElementsByTagName("head").First();
            head.AppendChild(style);
        }

        private void AddJsScript()
        {
            var script = _htmlDoc.CreateElement("script");
            script.TextContent = File.ReadAllText(_programDir + "\\htmlConfigs\\script.js");
            _htmlDoc.Body.AppendChild(script);
        }
    }
}
