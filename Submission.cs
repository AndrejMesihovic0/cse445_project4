using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;



/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Submission
    {
        public static string xmlURL = "https://andrejmesihovic0.github.io/cse445_project4/NationalParks.xml";
        public static string xmlErrorURL = "https://andrejmesihovic0.github.io/cse445_project4/NationalParksErrors.xml";
        public static string xsdURL = "https://andrejmesihovic0.github.io/cse445_project4/NationalParks.xsd";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);


            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);


            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            string errors = "";

            try
            {
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.Add("http://nationalparks.org", xsdUrl);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = schemaSet;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

                settings.ValidationEventHandler += delegate (object sender, ValidationEventArgs e)
                {
                    errors += e.Message + Environment.NewLine;
                };

                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read())
                    {
                    }
                }

                if (errors.Trim().Length == 0)
                {
                    return "No errors are found";
                }

                return errors.Trim();
            }
            catch (Exception ex)
            {
                if (errors.Trim().Length > 0)
                {
                    return (errors + ex.Message).Trim();
                }

                return ex.Message;
            }
        }

        public static string Xml2Json(string xmlUrl)
        {
            XmlDocument doc = new XmlDocument();
            string xmlContent = DownloadContent(xmlUrl);
            doc.LoadXml(xmlContent);

            string jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);

            return jsonText;
        }

        // Helper method to download content from URL
        private static string DownloadContent(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }

}