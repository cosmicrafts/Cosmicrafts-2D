using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_UIInfoUnit : MonoBehaviour {

    [HideInInspector]
    public string Unit;

    int iu = -1;

    public Text NameUnit;
    public Text Level;
    public Text Type;
    public Text Rarity;

    public Text Reinf;

    public Slider ReinfBar;

    public Text Cost;
    public Text DMG;
    public Text HP;
    public Text Armor;
    public Text Range;
    public Text Speed;
    public Text CastDelay;
    public Text Dps;
    public Text DURATION;

    public Text NShips;

    public Text Description;

    public Image Image;

    public Sprite DefaultIcon;

    public GameObject btn_Upgrade;
    public GameObject btn_Skins;

    public scr_Skins Skins;

    public scr_UIPlayerEditor UIPE;

    string[] Dic_Rarity = new string[5] { "txt_mn_info13_unit", "txt_mn_info14_unit", "txt_mn_info15_unit", "txt_mn_info16_unit", "txt_mn_info13_unit" };

    Color[] Dic_Rarity_Colors = new Color[5] {Color.white, Color.cyan, Color.green, new Color(0.9f, 0, 1), Color.white };

    Dictionary<string, string> Dic_Type = new Dictionary<string, string>(){
        { "Ship", "txt_mn_info17_unit"},
        { "Station", "txt_mn_info18_unit"},
        { "Skill", "txt_mn_info19_unit"},
        { "Squad", "txt_mn_info52_unit"}
    };

    // Update is called once per frame
    void UpdateInfo () {

        string S_DPS = scr_Lang.GetText("txt_mn_info54_unit");
        string S_RANGE = scr_Lang.GetText("txt_mn_info58_unit");
        string S_SPEED = scr_Lang.GetText("txt_mn_info62_unit");
        string S_ARMOR = scr_Lang.GetText("txt_mn_info69_unit");
        string S_CASTD = scr_Lang.GetText("txt_mn_info53_unit");

        float dmg = 0f;
        float hp = 0f;
        float castdelay = 0f;

        string TypeUnit = scr_GetStats.GetTypeUnit(Unit);

        if (TypeUnit == "Squad")
        {
            string snships = scr_GetStats.GetPropUnit(Unit, "Ships");
            string[] nships = snships.Split(',');
            for (int i=0; i<nships.Length; i++)
            {
                float _dmg = 0f;
                float.TryParse(scr_GetStats.GetPropUnit(nships[i], "Attack"), out _dmg);
                float _hp = 0f;
                float.TryParse(scr_GetStats.GetPropUnit(nships[i],"Hp"), out _hp);
                dmg += _dmg;
                hp += _hp;
            }
            NShips.text = nships.Length.ToString();
        } else
        {
            float dps = 0f;
            float atkspeed = 0;
            float armor = 0;
            float maxspeed = 0f;
            float range = 0f;
            float duration = 0f;
            int n_cannons = 0;

            if (TypeUnit != "Skill")
            {
                float.TryParse(scr_GetStats.GetPropUnit(Unit, "Hp"), out hp);
                float.TryParse(scr_GetStats.GetPropUnit(Unit, "SpeedAttack"), out atkspeed);
                float.TryParse(scr_GetStats.GetPropUnit(Unit, "Armor"), out armor);
                int.TryParse(scr_GetStats.GetPropUnit(Unit, "Ncannons"), out n_cannons);
            }
            
            float.TryParse(scr_GetStats.GetPropUnit(Unit, "Attack"), out dmg);
            float.TryParse(scr_GetStats.GetPropUnit(Unit, "Speed"), out maxspeed);
            float.TryParse(scr_GetStats.GetPropUnit(Unit, "RangeView"), out range);
            float.TryParse(scr_GetStats.GetPropUnit(Unit, "Duration"), out duration);

            if (atkspeed != 0f)
                dps = (dmg / atkspeed)* n_cannons;
            if (dps > 20 && dps <= 60) { S_DPS = scr_Lang.GetText("txt_mn_info55_unit"); }
            if (dps > 60 && dps <= 90) { S_DPS = scr_Lang.GetText("txt_mn_info56_unit"); }
            if (dps > 90) { S_DPS = scr_Lang.GetText("txt_mn_info57_unit"); }

            if (range > 1.0f && range <= 1.5f) { S_RANGE = scr_Lang.GetText("txt_mn_info59_unit"); }
            if (range > 1.5f && range <= 3f) { S_RANGE = scr_Lang.GetText("txt_mn_info55_unit"); }
            if (range > 3f && range <= 6.0f) { S_RANGE = scr_Lang.GetText("txt_mn_info60_unit"); }
            if (range > 6.0f) { S_RANGE = scr_Lang.GetText("txt_mn_info61_unit"); }

            if (maxspeed > 0.0f && maxspeed < 0.5f) { S_SPEED = scr_Lang.GetText("txt_mn_info63_unit"); }
            if (maxspeed >= 0.5f && maxspeed < 0.75f) { S_SPEED = scr_Lang.GetText("txt_mn_info64_unit"); ; }
            if (maxspeed >= 0.75f && maxspeed < 1.0f) { S_SPEED = scr_Lang.GetText("txt_mn_info65_unit"); }
            if (maxspeed >= 1.0f && maxspeed < 1.5f) { S_SPEED = scr_Lang.GetText("txt_mn_info66_unit"); }
            if (maxspeed >= 1.5f && maxspeed < 3.0f) { S_SPEED = scr_Lang.GetText("txt_mn_info67_unit"); }
            if (maxspeed >= 3.0f) { S_SPEED = scr_Lang.GetText("txt_mn_info68_unit"); }

            if (armor > 0f && armor <= 4f) { S_ARMOR = scr_Lang.GetText("txt_mn_info70_unit"); }
            if (armor > 4f && armor <= 8f) { S_ARMOR = scr_Lang.GetText("txt_mn_info71_unit"); }
            if (armor > 8f && armor <= 16f) { S_ARMOR = scr_Lang.GetText("txt_mn_info72_unit"); }
            if (armor > 16f) { S_ARMOR = scr_Lang.GetText("txt_mn_info73_unit"); }

            if (Armor != null) { Armor.text = armor.ToString("0.0") +" ("+S_ARMOR+")"; }
            if (Range != null) { Range.text = range.ToString("0.0") + " (" + S_RANGE+")"; }
            if (Speed != null) { Speed.text = maxspeed.ToString("0.0") + " (" + S_SPEED+")"; }
            if (Dps != null) { Dps.text = dps.ToString("0.0") + " (" + S_DPS + ")"; }
            if (DURATION != null) {
                if (duration>0.1f)
                    DURATION.text = duration.ToString("0.0") + " S.";
                else
                    DURATION.text = scr_Lang.GetText("txt_mn_info53_unit");
            }
            
        }

        string iconPath = "Units/Iconos/" + scr_GetStats.GetPropUnit(Unit, "Icon");
        Sprite ico = Resources.Load<Sprite>(iconPath);
        if (ico!=null)
        {
            Image.sprite = ico;
        } else
        {
            Debug.LogWarning("Unit icon not found for unit '" + Unit + "'. Icon path: " + iconPath + ". Using fallback icon.");
            Image.sprite = DefaultIcon;
        }

        float.TryParse(scr_GetStats.GetPropUnit(Unit, "CastDelay"), out castdelay);

        if (castdelay>0f)
            CastDelay.text = castdelay.ToString("0.0")+" S.";
        else
            CastDelay.text = S_CASTD;

        iu = scr_StatsPlayer.FindIdUnit(Unit);
        int index_rarity = 0;
        int.TryParse(scr_GetStats.GetPropUnit(Unit, "Rarity"), out index_rarity);
        Rarity.text = scr_Lang.GetText(Dic_Rarity[index_rarity]) ;
        Rarity.color = Dic_Rarity_Colors[index_rarity];
        Cost.text = scr_GetStats.GetPropUnit(Unit, "Cost");
        NameUnit.text = scr_GetStats.GetPropUnit(Unit, "Name");
        if (Dic_Type.ContainsKey(TypeUnit))
            Type.text = scr_Lang.GetText(Dic_Type[TypeUnit]);
        if (iu!=-1)
        {
            scr_UnitProgress UP = scr_StatsPlayer.MyUnits[iu];
            Level.text =scr_Lang.GetText("txt_mn_info2_unit") +" "+ UP.Level.ToString();
            ReinfBar.maxValue = scr_StatsPlayer.GetNextReinfLevel(UP.Level, UP.Rarity);
            ReinfBar.value = UP.Reinf;
            Reinf.text = UP.Reinf.ToString() + "/" + scr_StatsPlayer.GetNextReinfLevel(UP.Level, UP.Rarity);

            int maxlvl = 1;
            string sml = scr_GetStats.GetPropUnit(Unit, "MaxLevel");
            int.TryParse(sml, out maxlvl);
            if (scr_StatsPlayer.MyUnits[iu].Level >= maxlvl)
            {
                btn_Upgrade.SetActive(false);
            } else
            {
                btn_Upgrade.SetActive(true);
            }
            if (btn_Skins)
                btn_Skins.SetActive(true);
        } else
        {
            Reinf.text = "("+scr_Lang.GetText("txt_mn_info79")+")";
            ReinfBar.maxValue = 1;
            ReinfBar.value = 0;
            Level.text = "1";
            if (btn_Skins)
                btn_Skins.SetActive(false);
            btn_Upgrade.SetActive(false);
        }

        Description.text = scr_GetStats.GetDescriptionUnit(Unit, scr_Lang.language);

        if (DMG != null) { DMG.text = dmg.ToString(); }
        if (HP != null) { HP.text = hp.ToString(); }
    }

    public void SetInfo(string unit)
    {
        Unit = unit;
        UpdateInfo();
    }

    public void ChangeSkin()
    {
        if (!Skins.gameObject.activeSelf)
        {
            Skins.gameObject.SetActive(true);
            Skins.ShowUnitSkins(Unit);
        }
    }

    public void UpgradeUnit()
    {
        if (iu == -1 || scr_StatsPlayer.MyUnits[iu].Reinf<=0)
            return;

        scr_UnitProgress UP = scr_StatsPlayer.MyUnits[iu];
        int maxlvl = 1;
        int reinf_requ = scr_StatsPlayer.GetNextReinfLevel(UP.Level, UP.Rarity);
        string sml = scr_GetStats.GetPropUnit(Unit, "MaxLevel");
        int.TryParse(sml, out maxlvl);
        if (UP.Level<maxlvl && UP.Reinf>=reinf_requ)
        {
            UIPE.UpgradeUnitReinf(iu);
            ReinfBar.maxValue = scr_StatsPlayer.GetNextReinfLevel(UP.Level, UP.Rarity);
            ReinfBar.value = UP.Reinf;
            Reinf.text = UP.Reinf.ToString() + "/" + scr_StatsPlayer.GetNextReinfLevel(UP.Level, UP.Rarity);
            Level.text = scr_Lang.GetText("txt_mn_info2_unit") + " "+ UP.Level.ToString();

            if (UP.Level >= maxlvl)
            {
                btn_Upgrade.SetActive(false);
            }
        }
    }

}
