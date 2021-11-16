﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Bartz24.Docs
{
    public class HTMLPage
    {
        public string Name { get; }
        public string TemplateFileName { get; }

        public List<HTMLElement> HTMLElements { get; set; } = new List<HTMLElement>();

        public HTMLPage(string name, string templateFileName)
        {
            Name = name;
            TemplateFileName = templateFileName;
        }

        protected HtmlDocument GetMainDocument(string mainFolder)
        {
            HtmlDocument doc = new HtmlDocument();
            if (!String.IsNullOrEmpty(TemplateFileName))
            {
                doc.LoadHtml(File.ReadAllText(mainFolder + "/" + TemplateFileName));
            }

            return doc;
        }

        public void Generate(string fileName, string mainFolder)
        {
            HtmlDocument doc = GetMainDocument(mainFolder);
            GenerateContent(doc);
            doc.Save(fileName);            
        }

        protected virtual void GenerateContent(HtmlDocument doc)
        {
            HtmlNode title = doc.GetHead().SelectSingleNode("//title");
            title.InnerHtml = HtmlDocument.HtmlEncode(Name);

            HtmlNode contentNode = doc.DocumentNode.SelectSingleNode(".//*[contains(concat(\" \",normalize-space(@class),\" \"),\"tm-content\")]");
            HTMLElements.ForEach(e => e.Generate().ForEach(n => contentNode.AppendChild(n)));
        }
    }
}
