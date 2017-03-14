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

            DocumentHelper.CreateTemplateFile(TemplateType.ContractTemplate, documentFileName);


            using (WordprocessingDocument myDoc = WordprocessingDocument.Open(documentFileName, isEditable: true))
            {
                var document = myDoc.MainDocumentPart.Document;

                Body body = document.Body;
                
                DocumentHelper.SetTemplateTextName(body, "[ContractName]", description.Name);

                DocumentHelper.SetTemplateTextName(body, "[ContractDescription]", description.Desription);

                var table = body.OfType<Table>().FirstOrDefault();

                DocumentHelper.PopulateTable(description.ContractProperties, table);

                // Save changes to the main document part. 
                document.Save();
            }
        }
    }
}
