using System.IO;
using System.Xml;

namespace MonkeyBot.Helpers
{
    public static class Inner
    {
        public static char GetPrefix()
        {
            string path = $"{Directory.GetCurrentDirectory()}\\botconfig.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList nodes = doc.DocumentElement.ChildNodes;
            XmlNode tokenNode = nodes[1];
            return tokenNode.InnerText.ToCharArray()[0];
        }

        public static string GetDiscordToken()
        {
            string path = $"{Directory.GetCurrentDirectory()}\\botconfig.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList nodes = doc.DocumentElement.ChildNodes;
            XmlNode tokenNode = nodes[0];
            return tokenNode.InnerText;
        }

        public static string GetHypixelKey()
        {
            string path = $"{Directory.GetCurrentDirectory()}\\botconfig.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList nodes = doc.DocumentElement.ChildNodes;
            XmlNode tokenNode = nodes[2];
            return tokenNode.InnerText;
        }
    }
}