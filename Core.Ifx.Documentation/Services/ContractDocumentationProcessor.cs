using Core.Ifx.Documentation.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Core.Ifx.Documentation.Services
{
    public class ContractDocumentationProcessor : IDocumentationProcessor
    {
        private readonly XDocument m_assemblyDocumentation;
        private readonly string m_outputDirectory;
        private readonly List<Type> m_typesInNamespaces;
        private readonly ITypeParser<ContractDescription> m_typePaser;

        public ContractDocumentationProcessor(
            XDocument assemblyDocumentation,
            List<Type> typesInNamespaces,
            string outputDirectory,
            ITypeParser<ContractDescription> typePaser)
        {
            this.m_assemblyDocumentation = assemblyDocumentation;
            this.m_typesInNamespaces = typesInNamespaces;
            this.m_outputDirectory = outputDirectory;
            this.m_typePaser = typePaser;
        }

        public void CreateDocumentation()
        {
            List<ContractDescription> contractDescriptions = m_typePaser.Parse(m_assemblyDocumentation, m_typesInNamespaces);

            foreach (ContractDescription contractDescription in contractDescriptions)
            {
                WriteDocumenation(contractDescription, m_outputDirectory);
            }
        }

        private void WriteDocumenation(ContractDescription contractDescription, string m_outputDirectory)
        {
            var fileName = Path.ChangeExtension(contractDescription.Name, "doc");

            var documentFileName = Path.Combine(m_outputDirectory, fileName);

            using (WordprocessingDocument myDoc = WordprocessingDocument.Create(documentFileName, WordprocessingDocumentType.Document))
            {
                // Add a new main document part. 
                MainDocumentPart mainPart = myDoc.AddMainDocumentPart();
                //Create Document tree for simple document. 
                mainPart.Document = new Document();
                //Create Body (this element contains
                //other elements that we want to include 
                Body body = new Body();

                Paragraph header = CreateContractDescription(contractDescription.Name);

                body.Append(header);

                Paragraph description = CreateContractDescription(contractDescription.Desription);

                body.Append(description);

                Table table = CreatePropertyTable(contractDescription.ContractProperties);

                body.Append(table);

                mainPart.Document.Append(body);

                // Save changes to the main document part. 
                mainPart.Document.Save();
            }
        }

        private static Paragraph CreateContractDescription(string contractDescription, ParagraphStyleId paragraphStyleId = null)
        {
            //Create paragraph 
            Paragraph paragraph = new Paragraph();

            if (paragraphStyleId != null)
                paragraph.ParagraphProperties.ParagraphStyleId = paragraphStyleId;

            Run run_paragraph = new Run();
            // we want to put that text into the output document 
            Text text_paragraph = new Text(contractDescription);
            //Append elements appropriately. 
            run_paragraph.Append(text_paragraph);
            paragraph.Append(run_paragraph);
            return paragraph;
        }

        private static Table CreatePropertyTable(IEnumerable<ContractProperty> contractProperties)
        {
            Table table = new Table();

            TableProperties tableProperties = new TableProperties();

            TableStyle tableStyle = new TableStyle();
            
            

            tableProperties.TableStyle = tableStyle;
            
            TableGrid tableGrid = new TableGrid(new GridColumn(), new GridColumn(), new GridColumn());
            
            table.Append(tableGrid);
            
            TableRow tableRow = new TableRow();

            tableRow.Append(CreateCell("Property"), CreateCell("Type"), CreateCell("Description"));

            table.Append(tableRow);

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

            return table;
        }

        private static TableCell CreateCell(string text)
        {
            TableCell tableCell = new TableCell();
            
            tableCell.Append(new Paragraph(new Run(new Text(text))));

            return tableCell;

        }
    }
}
