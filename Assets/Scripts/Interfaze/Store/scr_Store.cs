using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class scr_Store : MonoBehaviour {

    public GameObject[] Pages = new GameObject[5];

    [HideInInspector]
    bool[] DataOkLoad = new bool[5] { true, true, true, false, false };

    //Carts To buy ellemnts
    public GameObject ctb_content;
    public GameObject ctb_item;
    public GameObject ctb_allsold;
    public InputField ctb_Input_Name;
    public Dropdown ctb_filter_type;
    public HorizontalLayoutGroup ctb_cont_lay;
    List<scr_ItemStore> ctb_Items;

    //Skins To buy elements
    public GameObject stb_content;
    public GameObject stb_item;
    public GameObject stb_allsold;
    public Dropdown stb_filter_type;
    public InputField stb_Input_Name;
    public GridLayoutGroup stb_cont_lay;
    List<scr_ItemStore> stb_Items;

    //Packs Deck Cards
    public GameObject cpk_content;
    public GameObject cpk_item;
    public GameObject cpk_allsold;

    //Packs Skins
    public GameObject spk_content;
    public GameObject spk_item;
    public GameObject spk_allsold;

    //Promo Items
    public GameObject ptb_content;
    public GameObject ptb_item;
    public GameObject ptb_allsold;

    //New Items
    public GameObject ntb_content;
    public GameObject ntb_item;
    public GameObject ntb_allsold;

    public GameObject CardsSection;

    public Sprite Ico_VirtualMoney;
    public Sprite Ico_Orb;
    public Sprite Ico_Reinf;

    public scr_UIPlayerEditor PEditor;
    [HideInInspector]
    public scr_ItemStore IndexItem;

    [HideInInspector]
    public GameObject CurrentPage;

    public GameObject ConfirmPay;
    public GameObject CantPay;

    public Text PurchaseStatus;
    public GameObject LogoPurchaseStatus;
    public GameObject FinishPurchase;
    public GameObject OptionsPurchase;
    public GameObject InfoPurchase;
    public Text NamePurchase;
    public Text CostPurchase;
    public Image IconPurchase;
    public Image IconCoinPurchase;

    public Sprite NoIcon;

    string[] UnitTypes = new string[5] {"All","Ship","Station","Skill","Squad"};
    int[] CostUnits = new int[4] {25,100,250,1000 };

    public static bool WaitForDBResponse = false;

    float TimeOutPurchase = 0f;

    WaitForSeconds Delay1s = new WaitForSeconds(1);

    WaitForFixedUpdate DelayUpdate = new WaitForFixedUpdate();

    [HideInInspector]
    public Dictionary<string, string[]> PriceNeutrinosPacks = new Dictionary<string, string[]>();

    string[] N_price_MXN = new string[5] {"$19.00 MXN", "$39.00 MXN", "$79.00 MXN", "$129.00 MXN", "$150.00 MXN" };
    string[] N_price_USD = new string[5] { "$0.99 USD", "$1.99 USD", "$3.99 USD", "$6.99 USD", "$7.99 USD" };

    [HideInInspector]
    public string Region_Coin = "MXN";

    // Use this for initialization
    void Start () {

        if (Scr_Database.GetCurrentCultureInfo().Name != "es-ES")
            Region_Coin = "USD";

        PriceNeutrinosPacks.Add("MXN", N_price_MXN);
        PriceNeutrinosPacks.Add("USD", N_price_USD);

        CurrentPage = Pages[0];

        stb_Items = new List<scr_ItemStore>();
        ctb_Items = new List<scr_ItemStore>();

        StartCoroutine(ShowStoreNews());
    }
	
	// Update is called once per frame
	void Update () {
        if (TimeOutPurchase > 0)
            TimeOutPurchase -= Time.deltaTime;

    }

    public void CTB_Filter()
    {
        string search = ctb_Input_Name.text.ToLower();
        string filter = UnitTypes[ctb_filter_type.value];

        for (int i=0; i<ctb_Items.Count; i++)
        {
            if ((ctb_Items[i].NameItem.ToLower().Contains(search) || search=="") 
                && (ctb_Items[i].UnitType[0] == filter || ctb_filter_type.value==0))
                ctb_Items[i].gameObject.SetActive(true);
            else
                ctb_Items[i].gameObject.SetActive(false);
        }

        CTB_UpdateCanvas();
    }

    public void STB_Filter()
    {
        string search = stb_Input_Name.text.ToLower();
        string filter = UnitTypes[stb_filter_type.value];

        for (int i = 0; i < stb_Items.Count; i++)
        {
            if ((stb_Items[i].NameItem.ToLower().Contains(search) || search == "")
                && (stb_Items[i].UnitType[0] == filter || stb_filter_type.value == 0))
                stb_Items[i].gameObject.SetActive(true);
            else
                stb_Items[i].gameObject.SetActive(false);
        }

        STB_UpdateCanvas();
    }

    void CTB_UpdateCanvas()
    {
        Canvas.ForceUpdateCanvases();
        ctb_cont_lay.enabled = false;
        ctb_cont_lay.enabled = true;
    }

    void STB_UpdateCanvas()
    {
        Canvas.ForceUpdateCanvases();
        stb_cont_lay.enabled = false;
        stb_cont_lay.enabled = true;
    }

    void AddPackUnitsItems()
    {
        /*
        scr_ItemStore newpack = Instantiate(cpk_item, cpk_content.transform).GetComponent<scr_ItemStore>();

        string[] s_newpack = new string[4] { "U_Skill_01", "U_Skill_03", "U_Skill_04", "U_Skill_06" };

        if (!newpack.UpdateToUnitsPack(950, "Starter Skills", NoIcon, s_newpack))
            Destroy(newpack.gameObject);

        newpack = Instantiate(cpk_item, cpk_content.transform).GetComponent<scr_ItemStore>();

        s_newpack = new string[3] { "U_Squad_01", "U_Squad_03", "U_Squad_04", };

        if (!newpack.UpdateToUnitsPack(950, "Starter Squads", NoIcon, s_newpack))
            Destroy(newpack.gameObject);

        if (cpk_content.transform.childCount == 0)
            cpk_allsold.SetActive(true);
        */
    }

    void AddDailyBundleUnits()
    {

        int _type_daily = 0;
        if (scr_StatsPlayer.Level > 25)
            _type_daily = 1;
        if (scr_StatsPlayer.Level > 50)
            _type_daily = 2;
        if (scr_StatsPlayer.Level > 100)
            _type_daily = 3;
        if (scr_StatsPlayer.Level > 250)
            _type_daily = 4;

        string[] _units = new string[4] {
            scr_StatsPlayer.GetRandomUnitFromRarity(0),
            scr_StatsPlayer.GetRandomUnitFromRarity(1),
            scr_StatsPlayer.GetRandomUnitFromRarity(2),
            scr_StatsPlayer.GetRandomUnitFromRarity(3)
        };

        bool[] _create = new bool[4] { true, true, true, true };

        bool OldBundel = !scr_Missions.New_Day;

        if (Scr_Database.MyData.Store_UnitsBundle[0] == "")
            OldBundel = false;

        if (OldBundel)
        {
            _units = Scr_Database.MyData.Store_UnitsBundle;
            for (int i = 0; i < 4; i++)
                _create[i] = Scr_Database.MyData.Store_CreateDaily[i];
        } else
        {
            for (int i=0; i<4; i++)
            {
                Scr_Database.MyData.Store_UnitsBundle[i] = _units[i];
                Scr_Database.MyData.Store_CreateDaily[i] = _create[i];
            }
            Scr_Database.SaveDatFile();
        }

        switch (_type_daily)
        {
            case 0:
                {
                    //Base Rarity
                    if (_create[0])
                        CreateBundleUnit(_units[0], 16, 0, 0.5f).Id_Daily = 0;
                    //Rare Rarity
                    if (_create[1])
                        CreateBundleUnit(_units[1], 8, 1, 0.5f).Id_Daily = 1;
                }
                break;
            case 1:
                {
                    //Base Rarity
                    if (_create[0])
                        CreateBundleUnit(_units[0], 32, 0, 0.5f).Id_Daily = 0;
                    //Rare Rarity
                    if (_create[1])
                        CreateBundleUnit(_units[1], 16, 1, 0.5f).Id_Daily = 1;
                    //Epic Rarity
                    if (_create[2])
                        CreateBundleUnit(_units[2], 2, 2, 0.5f).Id_Daily = 2;
                }
                break;
            case 2:
                {
                    //Base Rarity
                    if (_create[0])
                        CreateBundleUnit(_units[0], 64, 0, 0.5f).Id_Daily = 0;
                    //Rare Rarity
                    if (_create[1])
                        CreateBundleUnit(_units[1], 34, 1, 0.5f).Id_Daily = 1;
                    //Epic Rarity
                    if (_create[2])
                        CreateBundleUnit(_units[2], 4, 2, 0.5f).Id_Daily = 2;
                    //Leg Rarity
                    if (_create[3])
                        CreateBundleUnit(_units[3], 1, 3, 0.5f).Id_Daily = 3;
                }
                break;
            case 3:
                {
                    //Base Rarity
                    if (_create[0])
                        CreateBundleUnit(_units[0], 128, 0, 0.5f).Id_Daily = 0;
                    //Rare Rarity
                    if (_create[1])
                        CreateBundleUnit(_units[1], 64, 1, 0.5f).Id_Daily = 1;
                    //Epic Rarity
                    if (_create[2])
                        CreateBundleUnit(_units[2], 8, 2, 0.5f).Id_Daily = 2;
                    //Leg Rarity
                    if (_create[3])
                        CreateBundleUnit(_units[3], 2, 3, 0.5f).Id_Daily = 3;
                }
                break;
            case 4:
                {
                    //Base Rarity
                    if (_create[0])
                        CreateBundleUnit(_units[0], 256, 0, 0.5f).Id_Daily = 0;
                    //Rare Rarity
                    if (_create[1])
                        CreateBundleUnit(_units[1], 128, 1, 0.5f).Id_Daily = 1;
                    //Epic Rarity
                    if (_create[2])
                        CreateBundleUnit(_units[2], 16, 2, 0.5f).Id_Daily = 2;
                    //Leg Rarity
                    if (_create[3])
                        CreateBundleUnit(_units[3], 4, 3, 0.5f).Id_Daily = 3;
                }
                break;
        }

        if (cpk_content.transform.childCount==1)
        {
            cpk_allsold.SetActive(true);
        }

        cpk_item.SetActive(false);

    }

    scr_ItemStore CreateBundleUnit(string _unit, int _amount, int rarity, float discount)
    {
        scr_ItemStore newpack = Instantiate(cpk_item, cpk_content.transform).GetComponent<scr_ItemStore>();
        int cost = CostUnits[rarity] * _amount;
        if (discount > 0f)
        {
            newpack.Discount = discount;
            newpack.t_Accion.text = cost.ToString();
        }
        newpack.UpdateToUnit(cost, _unit, _amount);
        newpack.DestroyAtBuy = true;
        return newpack;
    }

    scr_ItemStore CreateBundleUnit(string _unit, int _amount, int rarity)
    {
        scr_ItemStore newpack = Instantiate(cpk_item, cpk_content.transform).GetComponent<scr_ItemStore>();
        int cost = CostUnits[rarity] * _amount;
        newpack.UpdateToUnit(cost, _unit, _amount);
        return newpack;
    }

    void AddSkinsDailyOffer()
    {
        spk_item.SetActive(true);

        string[] _skins = new string[4] {"","","",""};

        List<scr_ItemStore> offers = new List<scr_ItemStore>();

        if (stb_Items.Count<=4)
        {
            offers = stb_Items;
        } else
        {
            offers.Add(stb_Items[Random.Range(0, stb_Items.Count)]);

            for (int i=0; i<3; i++)
            {
                int start_index = Random.Range(0, stb_Items.Count);
                int atemps = 0;
                while (offers.Contains(stb_Items[start_index]) && atemps < stb_Items.Count)
                {
                    start_index++;
                    atemps++;
                }
                if (atemps<stb_Items.Count)
                    offers.Add(stb_Items[start_index]);
            }
        }

        bool OldBundel = !scr_Missions.New_Day;

        if (Scr_Database.MyData.Store_SkinsPromo[0] == "")
            OldBundel = false;

        if (OldBundel)
        {
            _skins = Scr_Database.MyData.Store_SkinsPromo;
            offers.Clear();
            for (int a=0; a<4; a++)
            {
                scr_ItemStore _skin = stb_Items.Find(i => i.ForUnit[0].Name == _skins[a]);
                if (_skin != null)
                    offers.Add(_skin);
            }
        }
        else
        {
            for (int i = 0; i < offers.Count; i++)
            {
                _skins[i] = offers[i].ForUnit[0].Name;
                Scr_Database.MyData.Store_UnitsBundle[i] = _skins[i];
            }
            Scr_Database.SaveDatFile();
        }

        for (int i=0; i< offers.Count; i++)
        {
            scr_ItemStore newpack = Instantiate(offers[i].gameObject, spk_content.transform).GetComponent<scr_ItemStore>();
            newpack.Discount = 0.5f;
            newpack.t_Accion.text = offers[i].Cost.ToString();
            newpack.t_Cost.text = (offers[i].Cost / 2).ToString();
        }

        if (spk_content.transform.childCount==1)
        {
            spk_allsold.SetActive(true);
        }

        spk_item.SetActive(false);
    }

    void AddPackSkinsItems()
    {
        scr_ItemStore newpack = Instantiate(spk_item, spk_content.transform).GetComponent<scr_ItemStore>();

        string[] s_newpack = new string[4] { "Gold", "Gold", "Gold", "Gold" };
        scr_UnitProgress[] s_forunits = new scr_UnitProgress[4] { scr_StatsPlayer.MyUnits[0], scr_StatsPlayer.MyUnits[1], scr_StatsPlayer.MyUnits[2], scr_StatsPlayer.MyUnits[3] };

        if (!newpack.UpdateToSkinsPack(950, "Golden Skins", NoIcon, s_newpack, s_forunits))
            Destroy(newpack.gameObject);

        if (cpk_content.transform.childCount == 0)
            spk_allsold.SetActive(true);

        spk_item.SetActive(false);
    }

    IEnumerator LoadUnitsToBuy()
    {
        AddDailyBundleUnits();

        DataOkLoad[3] = true;

        cpk_item.SetActive(false);
        ctb_item.SetActive(false);

        //Load Units To Buy
        if (scr_StatsPlayer.AllUnits.Count>0)
        {
            for (int i=0; i< scr_StatsPlayer.AllUnits.Count; i++)
            {
                string idname = scr_StatsPlayer.AllUnits[i];
                scr_ItemStore Item = Instantiate(ctb_item, ctb_content.transform).GetComponent<scr_ItemStore>();
                Item.gameObject.SetActive(true);
                ctb_Items.Add(Item);
                int rarity = 0;
                int.TryParse (scr_GetStats.GetPropUnit(idname, "Rarity"),out rarity);
                int cost = CostUnits[rarity];
                Item.UpdateToUnit(cost, idname,1);
                yield return DelayUpdate;
            }
        } else
        {
            ctb_filter_type.gameObject.SetActive(false);
            ctb_Input_Name.gameObject.SetActive(false);
            ctb_content.transform.parent.parent.gameObject.SetActive(false);
            ctb_allsold.SetActive(true);
        }

        CTB_UpdateCanvas();

        if (CurrentPage == Pages[3])
            Pages[3].SetActive(true);
    }

    IEnumerator LoadSkinsToBuy()
    {
        spk_item.SetActive(false);
        //Load Skins To Buy
        DataOkLoad[4] = true;

        if (scr_StatsPlayer.MyUnits.Count > 0)
        {
            for (int i = 0; i < scr_StatsPlayer.MyUnits.Count; i++)
            {
                XmlNodeList Skins = scr_GetStats.LoadSkins(scr_StatsPlayer.MyUnits[i].Name);
                for (int j = 1; j < Skins.Count; j++)
                {
                    string idnameskin = Skins[j].Attributes["IdSkin"].InnerText;

                    string US = scr_StatsPlayer.MyUnits[i].Name + ":" + idnameskin;
                    if (scr_StatsPlayer.MySkins.Contains(US))
                        continue;

                    scr_ItemStore Item = Instantiate(stb_item, stb_content.transform).GetComponent<scr_ItemStore>();
                    stb_Items.Add(Item);

                    string rarity = Skins[j].Attributes["Rarity"].InnerText;
                    string hue = Skins[j].Attributes["Hue"].InnerText;

                    int _rarity = 0;
                    int.TryParse(rarity, out _rarity);

                    int _cost = 2500+(2500*_rarity);

                    int _hue = 0;
                    int.TryParse(hue, out _hue);

                    Item.UpdateToSkin(_cost, scr_StatsPlayer.MyUnits[i], idnameskin, Skins[j].InnerText, _hue, _rarity);
                }
                yield return DelayUpdate;
            }
        }
        else
        {
            stb_item.SetActive(false);
            stb_filter_type.gameObject.SetActive(false);
            stb_Input_Name.gameObject.SetActive(false);
            stb_content.transform.parent.parent.gameObject.SetActive(false);
            stb_allsold.SetActive(true);
        }

        stb_item.SetActive(false);
        AddSkinsDailyOffer();

        STB_UpdateCanvas();

        if (CurrentPage == Pages[4])
            Pages[4].SetActive(true);
    }

    public void ChangePage(GameObject Page)
    {
        if (CurrentPage == Page)
            return;

        CurrentPage = Page;

        int idpage = 0;

        for (int i = 0; i < Pages.Length; i++)
        {
            if (Page == Pages[i])
                idpage = i;
            Pages[i].SetActive(false);
        }

        Page.SetActive(true);

        switch (idpage)
        {
            case 3: //Units Page
                {
                    if (!DataOkLoad[idpage])
                    {
                        StartCoroutine(LoadUnitsToBuy());
                    }
                }
                break;
            case 4: //SkinsPage
                {
                    if (!DataOkLoad[idpage])
                    {
                        StartCoroutine(LoadSkinsToBuy());
                    }
                }
                break;
        }
        
        
    }

    public void WantBuyItem(scr_ItemStore item_store)
    {
        if (ConfirmPay.activeSelf || CantPay.activeSelf)
            return;

        if (scr_StatsPlayer.Offline || !scr_Conection.CheckInternetConnection())
        {
            PEditor.ShowWarningNoConnection();
            return;
        }

        IndexItem = item_store;

        PurchaseStatus.text = scr_Lang.GetText("txt_mn_action24")+"\n";
        LogoPurchaseStatus.SetActive(false);
        FinishPurchase.SetActive(false);
        OptionsPurchase.SetActive(true);
        InfoPurchase.SetActive(true);

        NamePurchase.text = IndexItem.NameItem;
        if (IndexItem.Amount > 1)
            NamePurchase.text += " x" + IndexItem.Amount.ToString();
        CostPurchase.text = IndexItem.t_Cost.text;
        IconPurchase.sprite = IndexItem.Icon.sprite;

        int coin = scr_StatsPlayer.Neutrinos;

        if (coin >= IndexItem.Cost)
        {
            ConfirmPay.SetActive(true);
        }
        else
        {
            CantPay.SetActive(true);
        }
    }

    public void ConformItemToBuy(bool buy)
    {
        if (scr_StatsPlayer.Offline)
        {
            PEditor.ShowWarningNoConnection();
            ConfirmPay.SetActive(false);
            return;
        }

        if (!buy)
        {
            ConfirmPay.SetActive(false);
            return;
        }

        PEditor.B_Back.SetActive(false);
        WaitForDBResponse = false;

        StartCoroutine(WaitToCompleteBuy());

        IndexItem.Buy();

        PurchaseStatus.text = scr_Lang.GetText("txt_mn_info80");
        OptionsPurchase.SetActive(false);
        InfoPurchase.SetActive(false);
        LogoPurchaseStatus.SetActive(true);
    }

    IEnumerator WaitToCompleteBuy()
    {
        TimeOutPurchase = 5f;

        yield return new WaitUntil(() => (WaitForDBResponse || TimeOutPurchase<=0f));

        if (TimeOutPurchase<=0f) //Error en la compra
        {
            PurchaseStatus.text = scr_Lang.GetText("txt_mn_info82");
        }
        else // Exito en la compra
        {
            PEditor.AddProgressAchivInEditor(9, 1);

            TimeOutPurchase = 0f;
            //Cobramos Dinero
            int Cost = IndexItem.Cost - (Mathf.FloorToInt((float)IndexItem.Cost * IndexItem.Discount));
            scr_StatsPlayer.Neutrinos -= Cost;
            PEditor.UpdateCoins();

            //Re evaluamos precios e items si compramos un paquete
            scr_ItemStore[] items = FindObjectsOfType<scr_ItemStore>();
            for (int i=0; i<items.Length; i++)
            {
                if (items[i] == IndexItem)
                    continue;

                items[i].CheckClone(IndexItem.Type, IndexItem.IdItem);

                if (items[i] == null)
                    continue;

                if (items[i].IsPack)
                    items[i].ReEvaluatePackCost();
                else
                {
                    if (IndexItem.IsPack)
                    {
                        if (IndexItem.Type == 2)
                            items[i].CheckDestroyForNewItem(IndexItem.IdItem);
                        else if (IndexItem.Type == 3)
                            items[i].CheckDestroyForNewItem(IndexItem.IdItem, IndexItem.ForUnit);
                    }
                }
                yield return DelayUpdate;
            }
            //Destruimos Item
            IndexItem.CheckDestroy();
            IndexItem = null;
        }

        PurchaseStatus.text = scr_Lang.GetText("txt_mn_info81");
        FinishPurchase.SetActive(true);
        LogoPurchaseStatus.SetActive(false);
        WaitForDBResponse = false;
        PEditor.B_Back.SetActive(true);

    }

    public void AddUnitSkins(string unit_name)
    {
        stb_item.SetActive(true);

        scr_UnitProgress newunit = null;

        for (int i= scr_StatsPlayer.MyUnits.Count-1; i>=0 ; i--)
        {
            if (scr_StatsPlayer.MyUnits[i].Name == unit_name)
            {
                newunit = scr_StatsPlayer.MyUnits[i];
                break;
            }
        }

        if (newunit == null)
            return;

        XmlNodeList Skins = scr_GetStats.LoadSkins(unit_name);
        for (int j = 1; j < Skins.Count; j++)
        {
            
            string idnameskin = Skins[j].Attributes["IdSkin"].InnerText;

            scr_ItemStore Item = Instantiate(stb_item, stb_content.transform).GetComponent<scr_ItemStore>();
            stb_Items.Add(Item);
            string cost = Skins[j].Attributes["RealCost"].InnerText;

            string rarity = Skins[j].Attributes["Rarity"].InnerText;
            string hue = Skins[j].Attributes["Hue"].InnerText;

            int _cost = 99999;
            int.TryParse(cost, out _cost);

            int _rarity = 1;
            int.TryParse(rarity, out _rarity);

            int _hue = 0;
            int.TryParse(hue, out _hue);

            Item.UpdateToSkin(_cost, newunit, idnameskin, Skins[j].InnerText, _hue, _rarity);
            
        }

        stb_item.SetActive(false);
        STB_UpdateCanvas();
    }


    IEnumerator ShowStoreNews()
    {
        //Checamos si fue exitoso
        ptb_item.SetActive(false);
        ntb_item.SetActive(false);
        while (PEditor.NewsItemsData == "")
        {
            yield return Delay1s;
        }
        ptb_item.SetActive(true);
        ntb_item.SetActive(true);

        string[] ItemsJSon = PEditor.NewsItemsData.Split('|');
        scr_NewItemStore[] NewItems = new scr_NewItemStore[ItemsJSon.Length];

        for (int i = 0; i < ItemsJSon.Length; i++)
        {
            NewItems[i] = new scr_NewItemStore();
            JsonUtility.FromJsonOverwrite(ItemsJSon[i], NewItems[i]);

            if ((NewItems[i].Type == 2 && !scr_StatsPlayer.UnitsNotAv.Contains(NewItems[i].IdName)) ||
                (NewItems[i].Type == 3 && scr_StatsPlayer.MySkins.Contains(NewItems[i].ForUnit+":"+ NewItems[i].IdName)))
            {
                continue;
            }

            if (NewItems[i].IsNew == 0)
            {
                scr_ItemStore _item = Instantiate(ptb_item, ptb_content.transform).GetComponent<scr_ItemStore>();
                _item.UpdateToType(NewItems[i].Type, NewItems[i].IdName, NewItems[i].ForUnit, NewItems[i].Ammount, NewItems[i].Price, NewItems[i].Discount);
            }
            else
            {
                scr_ItemStore _item = Instantiate(ntb_item, ntb_content.transform).GetComponent<scr_ItemStore>();
                _item.UpdateToType(NewItems[i].Type, NewItems[i].IdName, NewItems[i].ForUnit, NewItems[i].Ammount, NewItems[i].Price, NewItems[i].Discount);
            }
        }

        if (ptb_content.transform.childCount == 1)
            ptb_allsold.SetActive(true);

        if (ntb_content.transform.childCount == 1)
            ntb_allsold.SetActive(true);

        Destroy(ptb_item);
        Destroy(ntb_item);

        PEditor.NewsItemsData = "";

    }

    private void OnEnable()
    {
        while (PEditor.NewSkinsToAdd.Count > 0)
        {
            string _uint = PEditor.NewSkinsToAdd[0];
            AddUnitSkins(_uint);
            PEditor.NewSkinsToAdd.RemoveAt(0);
        }
    }
}
