using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Core.Ifx.Documentation.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Core.Ifx.Documentation
{
    public class DocumentHelper
    {
        public static void SetTemplateTextName(Body body, string templateText, string templateValue)
        {
            var contractNameText = GetTextElementWithTextOf(body, templateText);

            if (contractNameText == null)
            {
                throw new TemplateMissingException($"The template field {templateText}");
            }
            contractNameText.Text = templateValue;
        }

        public static Text GetTextElementWithTextOf(Body body, string templateText)
        {
            var listOfParagraphs = body.ChildElements.OfType<Paragraph>().ToList();

            var listOfRuns = listOfParagraphs.SelectMany(paragraph => paragraph.ChildElements).OfType<Run>().ToList();

            var listOfText = listOfRuns.SelectMany(run => run.ChildElements).OfType<Text>();

            var descriptionText =
                listOfText.FirstOrDefault(
                    text => text.Text.Equals(templateText, StringComparison.InvariantCultureIgnoreCase));

            return descriptionText;
        }

        public static void CreateTemplateFile(TemplateType templateType, string documentFileName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            string resoucePath = null;

            switch (templateType)
            {
                case TemplateType.ContractTemplate:
                    resoucePath = Resources.CoreIfxDocumentationContractTemplateDocx;
                    break;
                case TemplateType.ServiceTemplate:
                    resoucePath = Resources.CoreIfxDocumentationServiceTemplateDocx;
                    break;
                case TemplateType.MethodTemplate:
                    resoucePath = Resources.CoreIfxDocumentationMethodTemplateDocx;
                    break;
            }

            Debug.Assert(resoucePath != null, "Resource path should not be null.");

            using (var resourceStream = assembly.GetManifestResourceStream(resoucePath))
            {
                var directory = Path.GetDirectoryName(documentFileName);

                Directory.CreateDirectory(directory);

                using (
                    FileStream fileStream = new FileStream(documentFileName, FileMode.Create, FileAccess.Write,
                        FileShare.Read, 4096))
                {
                    resourceStream.CopyTo(fileStream);
                }
            }
        }

        private static Style CreateHeaderOneStyle()
        {
            Style style = new Style()
            {
                Type = StyleValues.Paragraph,
                StyleId = "Heading1",
                CustomStyle = true
            };
            var styleName1 = new StyleName() { Val = "Heading1" };
            style.Append(styleName1);
            var styleRunProperties1 = new StyleRunProperties();
            styleRunProperties1.Append(new RunFonts() { Ascii = "Calibri Light (Headings)" });
            styleRunProperties1.Append(new FontSize() { Val = "32" }); // Sizes are in half-points. Oy!
            style.Append(styleRunProperties1);
            return style;
        }

        private static Paragraph CreateContractDescription(string contractDescription,
            ParagraphStyleId paragraphStyleId = null)
        {
            //Create paragraph 
            Paragraph paragraph = new Paragraph();

            if (paragraphStyleId != null)
                paragraph.ParagraphProperties = new ParagraphProperties(paragraphStyleId);

            Run run_paragraph = new Run();
            // we want to put that text into the output document 
            Text text_paragraph = new Text(contractDescription);
            //Append elements appropriately. 
            run_paragraph.Append(text_paragraph);
            paragraph.Append(run_paragraph);
            return paragraph;
        }

        public static void PopulateTable(IEnumerable<ContractProperty> contractProperties, Table table)
        {
            foreach (var prop in contractProperties)
            {
                TableRow row = new TableRow();

                string typeName;

                if (prop.DataType.IsConstructedGenericType)
                {
                    var genericArgs = prop.DataType.GetGenericArguments().Select(gType => gType.Name);

                    var namesOfGenericParams = string.Join(", ", genericArgs);

                    typeName = string.Format("{0}<{1}>", prop.DataType.Name, namesOfGenericParams);
                }
                else
                {
                    typeName = prop.DataType.Name;
                }


                row.Append(CreateCell(prop.Name), CreateCell(typeName), CreateCell(prop.Desription));
                table.Append(row);
            }
        }

        private static TableCell CreateCell(string text)
        {
            TableCell tableCell = new TableCell();

            tableCell.Append(new Paragraph(new Run(new Text(text))));

            return tableCell;

        }

        public static OpenXmlElement GetMethodTemplate()
        {
            var tempFile = Path.GetTempFileName();

            DocumentHelper.CreateTemplateFile(TemplateType.MethodTemplate, tempFile);

            using (WordprocessingDocument myDoc = WordprocessingDocument.Open(tempFile, isEditable: true))
            {
                var document = myDoc.MainDocumentPart.Document;

                return document.Body;
            }
        }
    }
}