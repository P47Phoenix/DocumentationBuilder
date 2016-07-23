using Core.Ifx.Documentation.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Ifx.Documentation.Services
{
    public class ContractDocumentationWriter : IDocumentationWriter<ContractDescription>
    {
        public void WriteDocumenation(ContractDescription description, string m_outputDirectory)
        {
            var fileName = Path.ChangeExtension(description.Name, "doc");

            var documentFileName = Path.Combine(m_outputDirectory, fileName);

            CreateTemplateFile(documentFileName);


            using (WordprocessingDocument myDoc = WordprocessingDocument.Open(documentFileName, isEditable: true))
            {
                // Add a new main document part. 
                // MainDocumentPart mainPart = myDoc.AddMainDocumentPart();

                var document = myDoc.MainDocumentPart.Document;
                //Create Document tree for simple document. 
                //mainPart.Document = new Document();
                //Create Body (this element contains
                //other elements that we want to include 
                Body body = document.Body;


                SetContractName(body, description.Name);


                SetContractDescription(body, description.Desription);


                var table = body.OfType<Table>().FirstOrDefault();

                PopulateTable(description.ContractProperties, table);

                // Save changes to the main document part. 
                document.Save();
            }
        }

        private static void SetContractDescription(Body body, string contractDescriptionValue)
        {
            var contractDescription = "ContractDescription";

            var descriptionText = GetTextElementWithTextOf(body, contractDescription);

            descriptionText.Text = contractDescriptionValue;
        }

        private static void SetContractName(Body body, string contractNameValue)
        {
            var contractName = "ContractName";

            var contractNameText = GetTextElementWithTextOf(body, contractName);

            contractNameText.Text = contractNameValue;
        }

        private static Text GetTextElementWithTextOf(Body body, string contractdescription)
        {
            var listOfParagraphs = body.ChildElements.OfType<Paragraph>().ToList();

            var listOfRuns = listOfParagraphs.SelectMany(paragraph => paragraph.ChildElements).OfType<Run>().ToList();

            var listOfText = listOfRuns.SelectMany(run => run.ChildElements).OfType<Text>();

            var descriptionText = listOfText.FirstOrDefault(text => text.Text.Equals(contractdescription, StringComparison.InvariantCultureIgnoreCase));

            return descriptionText;
        }

        private static void CreateTemplateFile(string documentFileName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var resourceStream = assembly.GetManifestResourceStream("Core.Ifx.Documentation.template.docx"))
            {
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
            StyleName styleName1 = new StyleName() { Val = "Heading1" };
            style.Append(styleName1);
            StyleRunProperties styleRunProperties1 = new StyleRunProperties();
            styleRunProperties1.Append(new RunFonts() { Ascii = "Calibri Light (Headings)" });
            styleRunProperties1.Append(new FontSize() { Val = "32" });  // Sizes are in half-points. Oy!
            style.Append(styleRunProperties1);
            return style;
        }

        private static Paragraph CreateContractDescription(string contractDescription, ParagraphStyleId paragraphStyleId = null)
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

        private static void PopulateTable(IEnumerable<ContractProperty> contractProperties, Table table)
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
    }
}
