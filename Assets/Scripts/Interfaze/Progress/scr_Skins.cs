using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class scr_Skins : MonoBehaviour {

    string Unit;
    XmlNodeList AllSkins;

    public Image Preview;
    public GameObject Lock;
    public Text NameSkin;

    public GameObject btn_close;
    public GameObject btn_Select;
    public GameObject btn_Buy;

    List<string> MySkins;

    int Index = 0;

    public void NextSkin()
    {
        Index++;
        if (Index>=AllSkins.Count)
        {
            Index = 0;
        }
        UpdatePreview();
    }

    public void PrevSkin()
    {
        Index--;
        if (Index<0)
        {
            Index = AllSkins.Count - 1;
        }
        UpdatePreview();
    }

    void UpdatePreview()
    {
        XmlNode node = AllSkins[Index];
        NameSkin.text = node.InnerText;
        float hue = 0f;
        float.TryParse(node.Attributes["Hue"].InnerText,out hue);
        Lock.SetActive(!MySkins.Contains(node.Attributes["IdSkin"].InnerText));
        btn_Buy.SetActive(Lock.activeSelf);
        btn_Select.SetActive(!Lock.activeSelf);
    }

    public void ShowUnitSkins(string unit)
    {
        Unit = unit;
        string Skins = "Classic";
        for (int i=0; i< scr_StatsPlayer.MyUnits.Count; i++)
        {
            if (scr_StatsPlayer.MyUnits[i].Name == Unit)
            {
                Skins = scr_StatsPlayer.MyUnits[i].Skins;
                break;
            }
        }
        MySkins = new List<string>();
        if (Skins.Contains("|"))
        {
            string[] _skins = Skins.Split('|');
            for (int i=0; i<_skins.Length; i++)
            {
                MySkins.Add(_skins[i]);
            }
        } else
        {
            MySkins.Add("Classic");
        }
        AllSkins = scr_GetStats.LoadSkins(Unit);
        string type = scr_GetStats.GetTypeUnit(Unit);
        Sprite PrevUnit = null;

        if (type == "Station")
        {
            Sprite[] sall = Resources.LoadAll<Sprite>("Units/Sprites/Stations/" + scr_GetStats.GetPropUnit(Unit, "Sprite"));
            if (sall != null)
                PrevUnit = sall[0];
        }
        else if (type == "Ship")
        {
            Sprite[] sall = Resources.LoadAll<Sprite>("Units/Sprites/Ships/" + scr_GetStats.GetPropUnit(Unit, "Sprite"));
            if (sall != null && sall.Length > 0)
                PrevUnit = sall[0];
        }
        else
        {
            Sprite sall = Resources.Load<Sprite>("Units/Sprites/Previews/Squads/" + Unit + "_Prev");
            if (sall != null)
                PrevUnit = sall;
        }
        Preview.sprite = PrevUnit;
        NameSkin.text = AllSkins[0].InnerText;
        Index = 0;
        btn_close.SetActive(true);
        btn_Select.SetActive(true);
        btn_Buy.SetActive(false);
        Lock.SetActive(false);
    }

    public void SetUnitSkin()
    {
        XmlNode node = AllSkins[Index];
        scr_UnitProgress _unit = null;
        for (int i = 0; i < scr_StatsPlayer.MyUnits.Count; i++)
        {
            if (scr_StatsPlayer.MyUnits[i].Name == Unit)
            {
                _unit = scr_StatsPlayer.MyUnits[i];
                break;
            }
        }
        if (_unit!=null)
        {
            _unit.CurrentSkin = node.Attributes["IdSkin"].InnerText;
            scr_BDUpdate.f_UpdateUnit(_unit);
        }
    }

    public void BuySkin()
    {

    }
}
