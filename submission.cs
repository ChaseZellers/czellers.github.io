using System;
using System.Xml;
using System.Xml.Schema;

class Program
{
    static bool validationSuccess = true;

    static void Main()
    {
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.Schemas.Add(null, "Hotels.xsd");
        settings.ValidationType = ValidationType.Schema;
        settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallback);

        Console.WriteLine("Validating Hotels.xml:");
        ValidateXML("Hotels.xml", settings);

        Console.WriteLine("\nValidating HotelsErrors.xml:");
        ValidateXML("HotelsErrors.xml", settings);
    }

    static void ValidateXML(string xmlFile, XmlReaderSettings settings)
    {
        validationSuccess = true;

        using (XmlReader reader = XmlReader.Create(xmlFile, settings))
        {
            try
            {
                while (reader.Read()) ;
            }
            catch (XmlException ex)
            {
                Console.WriteLine($"XML Exception: {ex.Message}");
                validationSuccess = false;
            }
        }

        if (validationSuccess)
        {
            Console.WriteLine($"{xmlFile} is valid.");
        }
        else
        {
            Console.WriteLine($"{xmlFile} is INVALID.");
        }
    }

    static void ValidationCallback(object sender, ValidationEventArgs args)
    {
        Console.WriteLine($"Validation {args.Severity}: {args.Message}");
        validationSuccess = false;
    }
}
