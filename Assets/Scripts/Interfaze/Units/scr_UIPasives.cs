using UnityEngine;
using UnityEngine.UI;

public class scr_UIPasives : MonoBehaviour {

    public GameObject EffectsParent;

    public GameObject Noeffects;

    float f_HpRegenDrones = 0f;

    float f_AftershockDMG = 0f;

    float f_Critical = 0f;

    float f_Vampiric = 0f;

    int i_Bank = 0;

    float f_RadioAttack = 0f;

    float f_BerserkMaxDMG = 0f;

    float f_Energize = 0f;

    bool b_Charge = false;

    float f_ChargeDMG = 0f;

    float f_Stack = 0f;

    float f_TSpawnUnit = 0f;

    string s_UnitSpawn = "none";

    bool b_DirectShoot = false;

    public void LoadPasivesUnit(scr_UIInfoUnit info_unit)
    {
        if (info_unit.Unit.Length<=0)
        {
            gameObject.SetActive(false);
            return;
        }

        int.TryParse(scr_GetStats.GetPropUnit(info_unit.Unit, "Banck"), out i_Bank);
        float.TryParse(scr_GetStats.GetPropUnit(info_unit.Unit, "Energize"), out f_Energize);
        float.TryParse(scr_GetStats.GetPropUnit(info_unit.Unit, "HpRegen"), out f_HpRegenDrones);
        float.TryParse(scr_GetStats.GetPropUnit(info_unit.Unit, "AttackRadio"), out f_RadioAttack);
        float.TryParse(scr_GetStats.GetPropUnit(info_unit.Unit, "Critical"), out f_Critical);
        float.TryParse(scr_GetStats.GetPropUnit(info_unit.Unit, "Vampiric"), out f_Vampiric);
        float.TryParse(scr_GetStats.GetPropUnit(info_unit.Unit, "BerserkMaxDMG"), out f_BerserkMaxDMG);
        float.TryParse(scr_GetStats.GetPropUnit(info_unit.Unit, "Overheating"), out f_Stack);
        float.TryParse(scr_GetStats.GetPropUnit(info_unit.Unit, "ChargeDMG"), out f_ChargeDMG);
        bool.TryParse(scr_GetStats.GetPropUnit(info_unit.Unit, "DirectShoot"), out b_DirectShoot);
        s_UnitSpawn = scr_GetStats.GetPropUnit(info_unit.Unit, "SpawnUnit");
        float.TryParse(scr_GetStats.GetPropUnit(info_unit.Unit, "SpawnUnitTime"), out f_TSpawnUnit);
        float.TryParse(scr_GetStats.GetPropUnit(info_unit.Unit, "AfterShockDMG"), out f_AftershockDMG);

        bool noef = true;

        //Pasive effects

        if (i_Bank > 0)
        {
            GameObject Item = EffectsParent.transform.GetChild(0).gameObject;
            Item.SetActive(true);
            Item.transform.GetChild(0).GetComponent<Text>().text = i_Bank.ToString() + " em";
            noef = false;
        }

        if (f_Energize > 0f)
        {
            GameObject Item = EffectsParent.transform.GetChild(1).gameObject;
            Item.SetActive(true);
            Item.transform.GetChild(0).GetComponent<Text>().text = "+" + f_Energize.ToString("N0") + " e";
            noef = false;
        }

        if (f_HpRegenDrones > 0f)
        {
            GameObject Item = EffectsParent.transform.GetChild(2).gameObject;
            Item.SetActive(true);
            Item.transform.GetChild(0).GetComponent<Text>().text = f_HpRegenDrones.ToString("N0");
            noef = false;
        }

        if (f_RadioAttack > 0f)
        {
            GameObject Item = EffectsParent.transform.GetChild(3).gameObject;
            Item.SetActive(true);
            Item.transform.GetChild(0).GetComponent<Text>().text = f_RadioAttack.ToString("N0");
            noef = false;
        }

        if (f_Critical > 0f)
        {
            GameObject Item = EffectsParent.transform.GetChild(4).gameObject;
            Item.SetActive(true);
            Item.transform.GetChild(0).GetComponent<Text>().text = (f_Critical * 100f).ToString("N0") + "%";
            noef = false;
        }

        if (f_Vampiric > 0f)
        {
            GameObject Item = EffectsParent.transform.GetChild(5).gameObject;
            Item.SetActive(true);
            Item.transform.GetChild(0).GetComponent<Text>().text = (f_Vampiric * 100f).ToString("N0") + "%";
            noef = false;
        }

        if (f_BerserkMaxDMG > 0f)
        {
            GameObject Item = EffectsParent.transform.GetChild(6).gameObject;
            Item.SetActive(true);
            Item.transform.GetChild(0).GetComponent<Text>().text = f_BerserkMaxDMG.ToString("N0") + " dm";
            noef = false;
        }

        if (f_Stack > 0f)
        {
            GameObject Item = EffectsParent.transform.GetChild(7).gameObject;
            Item.SetActive(true);
            Item.transform.GetChild(0).GetComponent<Text>().text = "+" + f_Stack.ToString("N0") + " d";
            noef = false;
        }

        if (b_Charge)
        {
            GameObject Item = EffectsParent.transform.GetChild(8).gameObject;
            Item.SetActive(true);
            Item.transform.GetChild(0).GetComponent<Text>().text = f_ChargeDMG.ToString("N0") + " d";
            noef = false;
        }

        if (b_DirectShoot)
        {
            GameObject Item = EffectsParent.transform.GetChild(9).gameObject;
            Item.SetActive(true);
            noef = false;
        }

        if (s_UnitSpawn != "none")
        {
            GameObject Item = EffectsParent.transform.GetChild(10).gameObject;
            Item.SetActive(true);
            Item.transform.GetChild(0).GetComponent<Text>().text = s_UnitSpawn;
            Item.transform.GetChild(1).GetComponent<Text>().text = f_TSpawnUnit.ToString("N1") + " s";
            noef = false;
        }

        if (f_AftershockDMG > 0.1f)
        {
            GameObject Item = EffectsParent.transform.GetChild(11).gameObject;
            Item.SetActive(true);
            Item.transform.GetChild(0).GetComponent<Text>().text = f_AftershockDMG.ToString("N0") + " d";
            noef = false;
        }

        Noeffects.SetActive(noef);
    }

    public void HideIcons()
    {
        Noeffects.SetActive(true);
        for (int i=0; i<EffectsParent.transform.childCount; i++)
        {
            EffectsParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
