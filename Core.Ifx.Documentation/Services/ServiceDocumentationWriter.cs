using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Ifx.Documentation.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Core.Ifx.Documentation.Services
{
    public class ServiceDocumentationWriter : IDocumentationWriter<ServiceDescription>
    {
        public void WriteDocumenation(ServiceDescription description, string m_outputDirectory)
        {
            var fileName = Path.ChangeExtension(description.Name, "doc");

            var documentFileName = Path.Combine(m_outputDirectory, fileName);

            DocumentHelper.CreateTemplateFile(TemplateType.ServiceTemplate, documentFileName);

            using (WordprocessingDocument myDoc = WordprocessingDocument.Open(documentFileName, isEditable: true))
            {
                var document = myDoc.MainDocumentPart.Document;

                Body body = document.Body;

                DocumentHelper.SetTemplateTextName(body, "[ServiceContract]", description.Name);
                DocumentHelper.SetTemplateTextName(body, "[ServiceContractDescription]", description.Desription);

                OpenXmlElement methodTemplate = DocumentHelper.GetMethodTemplate();

                foreach (var serviceMethod in description.ServiceMethods)
                {
                    var template = methodTemplate;

                    foreach (var openXmlElement in template.ChildElements)
                    {
                        body.Append(openXmlElement.CloneNode(true));
                    }

                    DocumentHelper.SetTemplateTextName(body, "[MethodName]", serviceMethod.Name);
                    DocumentHelper.SetTemplateTextName(body, "[MethodDescription]", serviceMethod.Description);
                    DocumentHelper.SetTemplateTextName(body, "[MethodSignature]", serviceMethod.Signature);
                    DocumentHelper.SetTemplateTextName(body, "[MethodSample]", description.Sample);
                    DocumentHelper.SetTemplateTextName(body, "[MethodDiagrams]", description.Diagrams);

                }


            }
        }
    }
}
