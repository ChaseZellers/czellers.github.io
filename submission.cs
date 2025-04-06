using System
using System.Xml;
using System.Xml.Schema;

namespace ConsoleApp1
{
    public class Program
    {
        // URLs to my github
        public static string hotelsXmlUrl = "https://raw.githubusercontent.com/ChaseZellers/czellers.github.io/main/Hotels.xml";
        public static string hotelsXsdUrl = "https://raw.githubusercontent.com/ChaseZellers/czellers.github.io/main/Hotels.xsd";
        public static string hotelsErrorsXmlUrl = "https://raw.githubusercontent.com/ChaseZellers/czellers.github.io/main/HotelsErrors.xml";

        public static void Main(string[] args)
        {
            Console.WriteLine("Validating Hotels.xml...");
            ValidateXmlFromUrl(hotelsXmlUrl, hotelsXsdUrl);

            Console.WriteLine("\nValidating HotelsErrors.xml...");
            ValidateXmlFromUrl(hotelsErrorsXmlUrl, hotelsXsdUrl);
        }

        // Method that will validate the XML from a URL using a remote XSD file
        public static void ValidateXmlFromUrl(string xmlUrl, string xsdUrl)
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();

                // Schema from the XSD URL
                settings.Schemas.Add(null, XmlReader.Create(xsdUrl));
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallback);

                // Read the XML document from the given URL
                XmlReader reader = XmlReader.Create(xmlUrl, settings);

                while (reader.Read()) { } // Force the reader to validate the entire XML

                Console.WriteLine("Validation completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error validating XML: " + ex.Message);
            }
        }

        // Callback for validation errors and warnings
        public static void ValidationCallback(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
                Console.WriteLine("WARNING: " + e.Message);
            else
                Console.WriteLine("ERROR: " + e.Message);
        }
    }
}
