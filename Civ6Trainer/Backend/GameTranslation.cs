using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace tctianchi.Civ6Trainer.Backend
{
    class GameTranslation
    {
        #region Singleton

        // singleton instance
        private static GameTranslation _instance = new GameTranslation();

        // get singleton instance
        public static GameTranslation Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        private Dictionary<string, string> localizationMap = new Dictionary<string, string>();

        public void Init()
        {
            localizationMap.Clear();

            // find all XML files
            var assembly = Assembly.GetExecutingAssembly();
            string[] paths = assembly.GetManifestResourceNames();
            foreach (var path in paths)
            {
                if (!path.StartsWith("tctianchi.Civ6Trainer.Asset.") || !path.EndsWith(".xml"))
                {
                    continue;
                }

                // Load the XML file
                XmlDocument xml = new XmlDocument();
                using (var stream = assembly.GetManifestResourceStream(path))
                {
                    xml.Load(stream);
                }

                // Read key-value pairs
                XmlNodeList nodeList = xml.SelectNodes("/GameData/LocalizedText/Replace");
                foreach (XmlNode node in nodeList)
                {
                    try
                    {
                        string key = node.Attributes["Tag"].Value;
                        string value = node.SelectSingleNode("Text").InnerText;
                        if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                        {
                            localizationMap[key] = value;
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }

        // return key if failed
        public string GetNameFromKey(string key)
        {
            string value;
            if (!localizationMap.TryGetValue(key, out value))
            {
                value = key;
            }
            return value;
        }
    }
}
