using System;
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

                string errorMessage = "No Error";
                bool hasErrors = false;

                settings.ValidationEventHandler += (sender, e) =>
                {
                    hasErrors = true;
                    if (e.Severity == XmlSeverityType.Error)
                    {
                        errorMessage = $"Validation Error: {e.Message}";
                    }
                    else
                    {
                        errorMessage = $"Validation Warning: {e.Message}";
                    }
                };

                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read()) { }
                }

                return hasErrors ? errorMessage : "No errors are found.";
            }
            catch (Exception ex)
            {
                return $"Exception during validation: {ex.Message}";
            }
        }

        // XML to JSON Conversion
        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlUrl);

                // Convert to JSON with proper formatting
                string jsonText = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented, true);

                // Verify the JSON can be deserialized back to XML
                JsonConvert.DeserializeXmlNode(jsonText);

                return jsonText;
            }
            catch (Exception ex)
            {
                return $"Exception during XML to JSON conversion: {ex.Message}";
            }
        }
    }
}
