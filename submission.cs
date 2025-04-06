using System;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    public class Program
    {
        // URLs to my GitHub
        public static string xmlURL = "https://raw.githubusercontent.com/ChaseZellers/czellers.github.io/main/Hotels.xml";
        public static string xmlErrorURL = "https://raw.githubusercontent.com/ChaseZellers/czellers.github.io/main/HotelsErrors.xml";
        public static string xsdURL = "https://raw.githubusercontent.com/ChaseZellers/czellers.github.io/main/Hotels.xsd";

        public static void Main(string[] args)
        {
            // Verify valid XML
            Console.WriteLine("Validating Hotels.xml...");
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            // Verify invalid XML
            Console.WriteLine("\nValidating HotelsErrors.xml...");
            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            // Convert to JSON
            Console.WriteLine("\nConverting Hotels.xml to JSON...");
            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // XML Validation against XSD
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add(null, XmlReader.Create(xsdUrl));
                settings.ValidationType = ValidationType.Schema;

                StringBuilder errors = new StringBuilder();
                bool hasErrors = false;

                settings.ValidationEventHandler += (sender, e) =>
                {
                    hasErrors = true;
                    errors.AppendLine($"{e.Severity}: {e.Message}");
                };

                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read()) { }
                }

                return hasErrors ? errors.ToString().Trim() : "No errors are found.";
            }
            catch (Exception ex)
            {
                return $"Validation exception: {ex.Message}";
            }
        }

        // XML to JSON Conversion
        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlUrl);

                // Special handling to ensure proper JSON structure
                string json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented, true);

                // Verify the conversion
                JsonConvert.DeserializeXmlNode(json);
                return json;
            }
            catch (Exception ex)
            {
                return $"Conversion error: {ex.Message}";
            }
        }
    }
}
