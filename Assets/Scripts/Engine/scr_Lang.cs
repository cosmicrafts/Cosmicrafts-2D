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

        TextAsset textAsset = (TextAsset)Resources.Load("XML/UIStrings");
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(textAsset.text);

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
        return UIS[key].ToString();
    }

    public static string GetTitleInfo(int _id)
    {
        return Titles[_id];
    }

    public static string GetTitleName(int _id)
    {
        return Titles[_id].Split('|')[0];
    }

    public static string GetTitleDescription(int _id)
    {
        return Titles[_id].Split('|')[1];
    }
}
