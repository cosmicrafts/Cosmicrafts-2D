using System.Collections;
using System.Xml;
using UnityEngine;

public class scr_Lang: MonoBehaviour {

    public static Hashtable UIS;
    public static string language;
    public static string[] Titles;


    void Start()
    {
        setLanguage();
    }

    public static void setLanguage()
    {
        language = "English";

        if (scr_StatsPlayer.Op_Leng == 1)
        {
            language = "Spanish";
        }
        
        Debug.Log("Language system initialized with language: " + language + " (Op_Leng: " + scr_StatsPlayer.Op_Leng + ")");

        TextAsset textAsset = (TextAsset)Resources.Load("XML/UIStrings");
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(textAsset.text);

        // Initialize MyTitles array if it doesn't exist (for dev bypass mode)
        if (scr_StatsPlayer.MyTitles == null)
        {
            scr_StatsPlayer.MyTitles = new bool[11];
        }

        Titles = new string[scr_StatsPlayer.MyTitles.Length];
        UIS = new Hashtable();

        //Load Strings

        XmlNodeList elements = xml.SelectNodes("/languages/"+ language + "/string");
        if (elements != null)
        {
            IEnumerator elemEnum = elements.GetEnumerator();
            while (elemEnum.MoveNext())
            {
                XmlElement xmlItem = (XmlElement)elemEnum.Current;
                UIS.Add(xmlItem.GetAttribute("name"), xmlItem.InnerText);
            }
            
            foreach(scr_UILang lg in FindObjectsOfType<scr_UILang>())
            {
                lg.SetMyText();
            }
        }
        else
        {
            Debug.LogError("The specified language does not exist: " + language);
        }

        //Load Titles

        elements = xml.SelectNodes("/languages/" + language + "/Title");
        if (elements != null)
        {
            IEnumerator elemEnum = elements.GetEnumerator();
            int index = 0;
            while (elemEnum.MoveNext())
            {
                XmlElement xmlItem = (XmlElement)elemEnum.Current;
                Titles[index]= xmlItem.GetAttribute("name")+"|"+xmlItem.InnerText;
                index++;
            }
        }
        else
        {
            Debug.LogError("The specified language does not exist: " + language);
        }
    }

    public static string GetText(string key)
    {
        if (UIS == null)
        {
            Debug.LogWarning("Language system not initialized, initializing now...");
            setLanguage();
        }
        
        if (UIS.ContainsKey(key))
        {
            string result = UIS[key].ToString();
            Debug.Log("GetText: " + key + " -> " + result);
            return result;
        }
        else
        {
            Debug.LogWarning("Language key not found: " + key + ", returning key as fallback");
            return key;
        }
    }

    public static string GetTitleInfo(int _id)
    {
        if (Titles == null || _id >= Titles.Length)
        {
            Debug.LogWarning("Titles not initialized or invalid ID: " + _id);
            return "Title " + _id;
        }
        return Titles[_id];
    }

    public static string GetTitleName(int _id)
    {
        if (Titles == null || _id >= Titles.Length)
        {
            Debug.LogWarning("Titles not initialized or invalid ID: " + _id);
            return "Title " + _id;
        }
        string[] parts = Titles[_id].Split('|');
        return parts.Length > 0 ? parts[0] : "Title " + _id;
    }

    public static string GetTitleDescription(int _id)
    {
        if (Titles == null || _id >= Titles.Length)
        {
            Debug.LogWarning("Titles not initialized or invalid ID: " + _id);
            return "Description for title " + _id;
        }
        string[] parts = Titles[_id].Split('|');
        return parts.Length > 1 ? parts[1] : "Description for title " + _id;
    }
}
