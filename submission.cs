using System;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json

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
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            // Verify invalid XML
            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            // Convert to JSON
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
                    errors.AppendLine($"{e.Message}");
                };

                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read()) { }
                }

                return hasErrors ? errors.ToString().Trim() : "No Error";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // XML to JSON Conversion
        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                // Loads XML
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlUrl);

                // Converts to JSON with attributes prefixed with '_'
                string json = JsonConvert.SerializeXmlNode(
                    doc,
                    Newtonsoft.Json.Formatting.Indented,
                    true 
                );

                // Validates the JSON structure
                if (string.IsNullOrEmpty(json) || !json.Contains("\"Hotel\":"))
                {
                    throw new InvalidOperationException("Invalid JSON: Missing Hotel array");
                }

                // Verifies round-trip conversion 
                var xmlNode = JsonConvert.DeserializeXmlNode(json);
                if (xmlNode == null)
                {
                    throw new InvalidOperationException("JSON could not be deserialized back to XML");
                }

                // Returns the JSON
                return json;
            }
            catch (Exception ex)
            {
                // Return error message if conversion fails
                return $"Conversion error: {ex.Message}";
            }
        }
    }
}
