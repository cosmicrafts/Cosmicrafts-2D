using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Linq;

public class scr_ItemStore : MonoBehaviour {

    //Basic data Item
    public int Cost = 999;
    public int Amount = 1;
    public int Type = 0;
    public int OrbType = 0;
    public float Xpbuff = 1f;
    public int HoursExpire = 0;
    public int DaysExpire = 0;
    [Range(0, 1)]
    public float Discount = 0f; 
    public bool WithRealMoney = false;

    public bool DestroyAtBuy = false;

    [HideInInspector]
    public bool IsPack = false;
    [HideInInspector]
    public bool IsRefaction = false;

    [HideInInspector]
    public int Id_Daily = -1;

    //Info of Element(s)
    [HideInInspector]
    public string[] IdItem = new string[1] {""};
    [HideInInspector]
    public string[] UnitType = new string[1] { "" };
    [HideInInspector]
    public scr_UnitProgress[] ForUnit = new scr_UnitProgress[1] { null };
    [HideInInspector]
    public float[] HueSkin = new float[1] { -1f };
    [HideInInspector]
    public int[] LvlRarity = new int[1] { 0};

    public string NameItem = "";

    public Text t_Cost;
    public Text t_Amount;
    public Text t_Name;
    public Text t_Discount;
    public Text t_Accion;
    public GameObject LineOff;
    public GameObject AllSold;
    public GameObject Refaction;

    public Image Icon;

    public scr_Store Store;

    public bool AutoUpdate;

    // Use this for initialization
    void Start () {
        if (AutoUpdate)
            UpdateUIData();

        CheckDiscount();
    }

    public void UpdateUIData()
    {
        if (NameItem.Length>0)
        {
            if (NameItem.Substring(0, 4) == "txt_")
                NameItem = scr_Lang.GetText(NameItem);
        }

        if (t_Name!=null)
        {
            t_Name.text = NameItem;
        } 
        if (t_Amount!=null)
        {
            if (Amount==1)
            {
                t_Amount.text = "";
            } else
            {
                t_Amount.text = "x" + Amount.ToString();
            }
            
        }
        if (t_Cost != null)
        {
            if (WithRealMoney)
                t_Cost.text = Store.PriceNeutrinosPacks[Store.Region_Coin][Cost];
            else
                t_Cost.text = Cost.ToString();
        }
    }

    public void CheckDiscount()
    {
        if (Discount <= 0f)
        {
            Discount = 0f;
            t_Accion.text = scr_Lang.GetText("txt_mn_action22");
        }
        else
        {
            LineOff.SetActive(true);
            t_Discount.gameObject.SetActive(true);
            t_Discount.text = "-" + ((int)(Discount * 100f)).ToString() + "%";
        }
    }

    public void ReEvaluatePackCost()
    {
        int pp_unit = Mathf.CeilToInt(Cost / Amount); //COSTO DE CADA UNIDAD POR SEPARADO

        if (Type == 3)
        {
            for (int i = 0; i < Amount; i++)
            {
                string US = ForUnit[i] + ":" + IdItem[i];
                if (scr_StatsPlayer.MySkins.Contains(US))
                {
                    Cost -= pp_unit;
                }
            }
        }
        else if (Type == 2)
        {
            for (int i = 0; i < Amount; i++)
            {
                if (!scr_StatsPlayer.UnitsNotAv.Contains(IdItem[i]))
                {
                    Cost -= pp_unit;
                }
            }
        }
    }

    public void CheckClone(int _type, string[] _items)
    {
        if (_type==Type && DestroyAtBuy)
        {
            if (Enumerable.SequenceEqual(IdItem, _items))
                CheckDestroy();
        }
    }

    public void CheckDestroyForNewItem(string[] newskins, scr_UnitProgress[] forUnits)
    {
        for (int i=0; i<newskins.Length; i++)
        {
            if (forUnits[i] == null)
                continue;
            if (forUnits[i].Name == ForUnit[0].Name && newskins[i] == IdItem[0])
                CheckDestroy();
        }
    }

    public void CheckDestroyForNewItem(string[] newUnits)
    {
        for (int i = 0; i < newUnits.Length; i++)
        {
            if (newUnits[i] == IdItem[0])
                CheckDestroy();
        }
    }

    public void UpdateToType(int _type, string _idname, string _forunit, int _amount, int _price, int _discount)
    {
        switch(_type)
        {
            case 0: //ORB
                {
                    NameItem = "txt_mn_info29";
                    Cost = _price;
                    Discount = _discount / 100f;
                    Amount = _amount;
                    Icon.sprite = Store.Ico_Orb;
                    UpdateUIData();
                    CheckDiscount();
                }
                break;
            case 1: //XP buff
                {

                }
                break;
            case 2: //CARD
                {
                    UpdateToUnit(_price, _idname, _amount);
                    Discount = _discount / 100f;
                    UpdateUIData();
                    CheckDiscount();
                }
                break;
            case 3: //SKIN
                {
                    string skin = _forunit + _idname;
                    if (scr_StatsPlayer.MySkins.Contains(skin))
                    {
                        Destroy(gameObject);
                        return;
                    }
                    scr_UnitProgress unit = null;
                    for (int i=0; i<scr_StatsPlayer.MyUnits.Count; i++)
                    {
                        if (scr_StatsPlayer.MyUnits[i].Name == _forunit)
                        {
                            unit = scr_StatsPlayer.MyUnits[i];
                            break;
                        }
                    }
                    if (unit == null)
                        return;

                    XmlNode _Skin = scr_GetStats.LoadSkin(_forunit, _idname);

                    string s_hue = _Skin.Attributes["Hue"].InnerText;
                    string s_rarity = _Skin.Attributes["Rarity"].InnerText;

                    float hue = 0;
                    float.TryParse(s_hue, out hue);

                    int rarity = 0;
                    int.TryParse(s_rarity, out rarity);

                    UpdateToSkin(_price, unit, _idname, _Skin.InnerText, hue, rarity);
                    Discount = _discount / 100f;
                    UpdateUIData();
                    CheckDiscount();
                }
                break;
        }
    }

    public void UpdateToUnit(int realcost, string idname, int amount)
    {
        IdItem = new string[1] { "" };
        UnitType = new string[1] { "" };
        LvlRarity = new int[1] { 0 };

        IdItem[0] = idname;
        Cost = realcost;
        t_Cost.text = Cost.ToString();
        Amount = amount;
        DestroyAtBuy = false;
        IsRefaction = false;
        if (Refaction)
            Refaction.SetActive(false);

        if (!scr_StatsPlayer.UnitsNotAv.Contains(idname))
        {
            if (Refaction)
                Refaction.SetActive(true);
            IsRefaction = true;
        }

        UnitType[0] = scr_GetStats.GetTypeUnit(idname);
        string realname = scr_GetStats.GetPropUnit(idname, "Name");
        string rarity = scr_GetStats.GetPropUnit(idname, "Rarity");
        int.TryParse(rarity, out LvlRarity[0]);

        NameItem = realname;
        t_Name.text = NameItem;
        if (t_Amount)
            t_Amount.text = "+" + amount.ToString() + " " + scr_Lang.GetText("txt_mn_info115");

        string iconPath = "Units/Iconos/" + scr_GetStats.GetPropUnit(idname, "Icon");
        Sprite ico = Resources.Load<Sprite>(iconPath);
        if (ico != null && Icon)
            Icon.sprite = ico;
        else if (Icon)
        {
            Debug.LogWarning("Unit icon not found for unit '" + idname + "'. Icon path: " + iconPath + ". Using fallback icon.");
        }
    }

    public void UpdateToUnitsPack(int realcost, string namePack, Sprite _icon, string[] idnames)
    {
        IsPack = true;
        NameItem = namePack;
        Cost = realcost;
        t_Cost.text = Cost.ToString();
        Amount = idnames.Length;
        t_Name.text = NameItem;
        DestroyAtBuy = true;

        IdItem = new string[Amount];
        UnitType = new string[Amount];
        LvlRarity = new int[Amount];

        for (int i=0; i<Amount; i++)
        {
            IdItem[i] = idnames[i];
            UnitType[i] = scr_GetStats.GetTypeUnit(idnames[i]);
            string rarity = scr_GetStats.GetPropUnit(idnames[i], "Rarity");
            int.TryParse(rarity, out LvlRarity[i]);
            UnitType[i] = scr_GetStats.GetTypeUnit(idnames[i]);

        }

        Icon.sprite = _icon;

    }

    public void UpdateToSkin(int realcost, scr_UnitProgress forunit,  string idname, string _name, float hue, int rarity)
    {
        IdItem = new string[1] { "" };
        UnitType = new string[1] { "" };
        ForUnit = new scr_UnitProgress[1] { null };
        HueSkin = new float[1] { -1f };
        LvlRarity = new int[1] { 0 };

        IdItem[0] = idname;
        NameItem = _name;
        Cost = realcost;
        t_Cost.text = Cost.ToString();
        Amount = 1;
        t_Name.text = NameItem;
        LvlRarity[0] = rarity;
        HueSkin[0] = hue;
        ForUnit[0] = forunit;
        DestroyAtBuy = true;

        UnitType[0] = scr_GetStats.GetTypeUnit(forunit.Name);

        if (UnitType[0] == "Station")
        {
            Sprite[] sall = Resources.LoadAll<Sprite>("Units/Sprites/Stations/" + scr_GetStats.GetPropUnit(forunit.Name, "Sprite"));
            if (sall != null)
                Icon.sprite= sall[0];
        }
        else if (UnitType[0] == "Ship")
        {
            Sprite[] sall = Resources.LoadAll<Sprite>("Units/Sprites/Ships/" + scr_GetStats.GetPropUnit(forunit.Name, "Sprite"));
            if (sall != null && sall.Length > 0)
                Icon.sprite = sall[0];
        }
        else
        {
            Sprite sall = Resources.Load<Sprite>("Units/Sprites/Previews/Squads/" + forunit.Name + "_Prev");
            if (sall != null)
                Icon.sprite = sall;
        }

        //HueSprite.enabled = true;
        //Icon.color = Color.HSVToRGB(hue/360,1,1);

    }

    public bool UpdateToSkinsPack(int realcost, string namepack, Sprite _icon, string[] idnamesskins, scr_UnitProgress[] forunits)
    {
        IsPack = true;
        NameItem = namepack;
        Cost = realcost;
        t_Cost.text = Cost.ToString();
        Amount = idnamesskins.Length;
        t_Name.text = NameItem;
        Icon.sprite = _icon;
        DestroyAtBuy = true;

        IdItem = new string[Amount];
        UnitType = new string[Amount];
        ForUnit = new scr_UnitProgress[Amount];
        HueSkin = new float[Amount];
        LvlRarity = new int[Amount];

        int pp_unit = Mathf.CeilToInt(Cost / Amount); //COSTO DE CADA UNIDAD POR SEPARADO

        int elements = 0;

        for (int i=0; i<Amount; i++)
        {
            string US = forunits[i].Name + ":" + idnamesskins[i];
            if (scr_StatsPlayer.MySkins.Contains(US))
            {
                Cost -= pp_unit;

                IdItem[i] = UnitType[i] = "null";
                ForUnit[i] = null;
                LvlRarity[i] = -1;
                HueSkin[i] = -1;

            } else
            {
                elements++;
                IdItem[i] = idnamesskins[i];
                ForUnit[i] = forunits[i];
                UnitType[i] = scr_GetStats.GetTypeUnit(forunits[i].Name);

                XmlNode skin = scr_GetStats.LoadSkin(forunits[i].Name, IdItem[i]);

                string _rarity = skin.Attributes["Rarity"].InnerText;
                string _hue = skin.Attributes["Hue"].InnerText;

                int.TryParse(_rarity, out LvlRarity[i]);
                float.TryParse(_hue, out HueSkin[i]);
            }
        }

        if (elements <= 1)
        {
            return false;
        }

        //HueSprite.enabled = true;
        //Icon.color = Color.HSVToRGB(hue/360,1,1);

        return true;
    }

    public void Buy()
    {
        int Final_Cost = Cost - (Mathf.FloorToInt((float)Cost * Discount));
        switch (Type)
        {
            case 0: //BUY A ORB
                {
                    scr_StatsPlayer.Orbes[OrbType] += Amount;
                    scr_BDUpdate.f_BuyOrbes(scr_StatsPlayer.Neutrinos-Final_Cost);
                }
                break; 
            case 1: //BUY A XP BUFF
                {
                    scr_StatsPlayer.XpBuff = Xpbuff;
                    System.DateTime xpire = System.DateTime.Now;
                    xpire.AddDays(DaysExpire);
                    xpire.AddHours(HoursExpire);
                    scr_StatsPlayer.XpBuffExpire = xpire;
                    scr_BDUpdate.f_BuyXpBuff(scr_StatsPlayer.Neutrinos - Final_Cost);
                }
                break; //BUY A CARD
            case 2:
                {
                    if (!IsPack)
                    {
                        scr_UnitProgress _exist = scr_StatsPlayer.FindUnitProgress(IdItem[0]);
                        if (_exist!=null)
                        {
                            _exist.Reinf+= Amount;
                            scr_BDUpdate.f_BuyRefUnit(_exist, scr_StatsPlayer.Neutrinos - Final_Cost);
                        } else
                        {
                            scr_BDUpdate.f_BuyNewUnit(scr_StatsPlayer.id, IdItem[0], Amount-1, scr_StatsPlayer.Neutrinos - Final_Cost);
                        }
                        
                    } else
                    {
                        string units_list_new = "";
                        List<scr_UnitProgress> update_units = new List<scr_UnitProgress>();
                        for (int i=0; i<Amount; i++)
                        {
                            scr_UnitProgress _exist = scr_StatsPlayer.FindUnitProgress(IdItem[i]);
                            if (_exist == null)
                            {
                                if (units_list_new != "")
                                    units_list_new += "|";
                                units_list_new += IdItem[i];
                            } else
                            {
                                _exist.Reinf += Amount;
                                update_units.Add(_exist);
                            }
                        }
                        scr_BDUpdate.f_UpdateListUnitsProgress(update_units);
                        scr_BDUpdate.f_BuyNewPackUnits(scr_StatsPlayer.id, units_list_new, scr_StatsPlayer.Neutrinos - Final_Cost);
                    }
                    
                }
                break;
            case 3: //BUY A SKIN
                {
                    if (!IsPack)
                    {
                        scr_BDUpdate.f_BuySkin(ForUnit[0], IdItem[0], scr_StatsPlayer.Neutrinos - Final_Cost);
                    } else
                    {
                        scr_BDUpdate.f_BuySkinsPack(ForUnit, IdItem, scr_StatsPlayer.Neutrinos - Final_Cost);
                    }
                }
                break;
            case 4: //BUY QUANTUMS
                {
                    //MICRO TRANSACTION PROCESS
                }
                break;
        }

    }

    public void CheckDestroy()
    {
        if (Type==2)//Set to refaction card
        {
            if (Refaction)
                Refaction.SetActive(true);
            IsRefaction = true;
        }
        if (Id_Daily!=-1)
        {
            Scr_Database.MyData.Store_CreateDaily[Id_Daily] = false;
            Scr_Database.SaveDatFile();
        }

        if (DestroyAtBuy)
        {
            Canvas.ForceUpdateCanvases();

            if (transform.parent.GetComponent<GridLayoutGroup>())
            {
                transform.parent.GetComponent<GridLayoutGroup>().enabled = false;
                transform.parent.GetComponent<GridLayoutGroup>().enabled = true;
            }

            if (transform.parent.GetComponent<VerticalLayoutGroup>())
            {
                transform.parent.GetComponent<VerticalLayoutGroup>().enabled = false;
                transform.parent.GetComponent<VerticalLayoutGroup>().enabled = true;
            }

            if (transform.parent.GetComponent<HorizontalLayoutGroup>())
            {
                transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false;
                transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true;
            }

            if (transform.parent.childCount==1)
            {
                AllSold.SetActive(true);
            }

            Destroy(gameObject);
        }
    }

}
