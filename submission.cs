using System
using System.Xml;
using System.Xml.Schema;

namespace ConsoleApp1
{
    public class Program
    {
        // URLs to my github
        public static string hotelsXmlUrl = "https://raw.githubusercontent.com/ChaseZellers/czellers.github.io/main/Hotels.xml";
        public static string hotelsErrorsXmlUrl = "https://raw.githubusercontent.com/ChaseZellers/czellers.github.io/main/HotelsErrors.xml";
        public static string hotelsXsdUrl = "https://raw.githubusercontent.com/ChaseZellers/czellers.github.io/main/Hotels.xsd";
        

        public static void Main(string[] args)
        {
            Console.WriteLine("Validating Hotels.xml...");
            string result = Verification(hotelsXmlUrl, hotelsXsdUrl);
            Console.WriteLine(result);

            Console.WriteLine("\nValidating HotelsErrors.xml...");
            result = Verification(hotelsErrorsXmlUrl, hotelsXsdUrl);
            Console.WriteLine(result);

            Console.WriteLine("\nConverting Hotels.xml to JSON...");
            result = Xml2Json(hotelsXmlUrl);
            Console.WriteLine(result);
        }

        // Method that will validate the XML from a URL using a remote XSD file
        public static void Verification(string hotelsXmlUrl, string hotelsXsdUrl)
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add(null, XmlReader.Create(xsdUrl));
                settings.ValidationType = ValidationType.Schema;
                string errorMessage = "No Error";

                settings.ValidationEventHandler += (sender, e) =>
                {
                    if (e.Severity == XmlSeverityType.Error)
                        errorMessage = "ERROR: " + e.Message;
                    else
                        errorMessage = "WARNING: " + e.Message;
                };

                XmlReader reader = XmlReader.Create(hotelsXmlUrl, settings);
                while (reader.Read()) { }

                return errorMessage;
            }
            catch (Exception ex)
            {
                return "Exception during validation: " + ex.Message;
            }
        }

        // Callback for validation errors and warnings
        public static string Xml2Json(string hotelsXmlUrl)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(hotelsXmlUrl);

                // Force JSON attributes with "_"
                string jsonText = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented, true);

                return jsonText;
            }
            catch (Exception ex)
            {
                return "Exception during XML to JSON conversion: " + ex.Message;
            }
        }
    }
}
