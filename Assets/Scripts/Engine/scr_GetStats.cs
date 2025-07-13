using System.Collections;
using System.Xml;
using UnityEngine;

public class scr_GetStats : MonoBehaviour {

    public static XmlDocument XmlUnits;

    public static XmlNode DefaultUnit;

    private void Awake()
    {
        LoadXmlShips();
    }

    // Use this for initialization
    public static void LoadXmlShips() {
        TextAsset textAsset = (TextAsset)Resources.Load("XML/DefUnits");
        XmlUnits = new XmlDocument();
        XmlUnits.LoadXml(textAsset.text);
        DefaultUnit = XmlUnits.SelectSingleNode("/Units/Unit[@IdName='DefaultUnit']");
    }

    public static string GetPropUnit(string Name, string Prop)
    {
        string result = "null";
        XmlNode node = XmlUnits.SelectSingleNode("/Units/Unit[@IdName='" + Name + "']/" + Prop);
        if (node != null)
            result = node.InnerText;
        else
        {
            result = DefaultUnit.SelectSingleNode(Prop).InnerText;
            if (Prop == "Icon")
            {
                Debug.LogWarning("Unit '" + Name + "' not found in XML, using default icon: " + result);
            }
        }
        return result;
    }

    public static string GetDescriptionUnit(string Name, string lang)
    {
        string result = "0";
        XmlNode node = XmlUnits.SelectSingleNode("/Units/Unit[@IdName='" + Name + "']/Description[@lan='" + lang + "']");
        if (node != null)
            result = node.InnerText;
        else
            result = DefaultUnit.SelectSingleNode("Description[@lan='" + lang + "']").InnerText;
        return result;
    }

    public static string GetTypeUnit(string Name)
    {
        return XmlUnits.SelectSingleNode("/Units/Unit[@IdName='" + Name + "']").Attributes["Type"].InnerText;
    }

    public static string[] GetSkins(string Name)
    {
        XmlNodeList ListSkins = XmlUnits.SelectNodes("/Units/Unit[@IdName='" + Name + "']/Skins/Skin");
        string[] unit_skins = new string[ListSkins.Count];
        for (int i=0; i<ListSkins.Count; i++)
        {
            unit_skins[i] = ListSkins[i].Attributes["IdSkin"].InnerText;
        }

        return unit_skins;
    }

    public static XmlNode LoadSkin(string NameUnit, string NameSkin)
    {
        return XmlUnits.SelectSingleNode("/Units/Unit[@IdName='" + NameUnit + "']/Skins/Skin[@IdSkin='" + NameSkin + "']");
    }

    public static float GetHueSkin(string NameUnit, string NameSkin)
    {
        XmlNode _skin = XmlUnits.SelectSingleNode("/Units/Unit[@IdName='" + NameUnit + "']/Skins/Skin[@IdSkin='" + NameSkin + "']");
        float _hue = 0;
        float.TryParse(_skin.Attributes["Hue"].InnerText, out _hue);
        return _hue;
    }

    public static XmlNodeList LoadSkins(string NameUnit)
    {
        return XmlUnits.SelectNodes("/Units/Unit[@IdName='" + NameUnit + "']/Skins/Skin");
    }


}
