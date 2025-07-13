using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_UIDeckEditor : MonoBehaviour {

    public GameObject InfoShip;
    public GameObject InfoStation;
    public GameObject InfoSkill;
    public GameObject InfoSquad;

    public GameObject LevelUp;
    public Text InfoLevelUp;

    public Sprite NoIco;

    public GameObject EditorPanel;
    public GameObject LoadingPanel;

    [HideInInspector]
    public bool to_drop = false;
    [HideInInspector]
    public GameObject go_DragShip = null;
    [HideInInspector]
    public scr_CardColection go_selected = null;
    [HideInInspector]
    public scr_CardColection go_remplace = null;
    [HideInInspector]
    public int CardPosition = 0;
    [HideInInspector]
    public bool LoadingCards = false;
    bool OkInit = false;

    public GameObject go_card;
    public GameObject BoxCards;

    GridLayoutGroup ContGLG;

    List<scr_CardColection> Cards; //Cartas En el Juego para seleccionar
    List<string> CardsNames; //Nombre de Cartas En el Juego para seleccionar

    public scr_CardColection[] Slots = new scr_CardColection[8]; // Botones de naves seleccionadas

    public Text DeckName;

    public Text NewNameDeck;

    public Button[] Decks = new Button[3];

    public Text AvgEnergy;

    public Dropdown DD_OrderBy;
    public Dropdown DD_Filter;

    string[] NameTypes = new string[5] { "All","Ship","Station","Skill","Squad"};

    public scr_UIPlayerEditor UIEditor;

    // Use this for initialization
    void Start () {

        Cards = new List<scr_CardColection>();
        CardsNames = new List<string>();

        CardPosition = -1;

        ContGLG = BoxCards.GetComponent<GridLayoutGroup>();

        UpdateNameDeck();
        for (int i=0; i<3; i++)
        {
            Decks[i].GetComponentInChildren<Text>().text = scr_StatsPlayer.Name_deck[i];
        }
        Decks[scr_StatsPlayer.idc].GetComponentInChildren<Text>().color = Color.red;

        List<string> OptionsOrder = new List<string>();

        OptionsOrder.Add(scr_Lang.GetText("txt_mn_opts24"));
        OptionsOrder.Add(scr_Lang.GetText("txt_mn_opts23"));
        OptionsOrder.Add(scr_Lang.GetText("txt_mn_opts25"));
        OptionsOrder.Add(scr_Lang.GetText("txt_mn_opts26"));
        DD_OrderBy.ClearOptions();
        DD_OrderBy.AddOptions(OptionsOrder);

        List<string> OptionsFilter = new List<string>();

        OptionsFilter.Add(scr_Lang.GetText("txt_mn_opts2"));
        OptionsFilter.Add(scr_Lang.GetText("txt_mn_opts27"));
        OptionsFilter.Add(scr_Lang.GetText("txt_mn_opts28"));
        OptionsFilter.Add(scr_Lang.GetText("txt_mn_opts29"));
        OptionsFilter.Add(scr_Lang.GetText("txt_mn_opts30"));
        DD_Filter.ClearOptions();
        DD_Filter.AddOptions(OptionsFilter);

        //UIEditor.LoadAllXmlShips(true);
        OrderCardsbyXml();
    }

    void UpdateNameDeck()
    {
        DeckName.text = scr_Lang.GetText("txt_mn_info1_player") +" " + scr_StatsPlayer.Name_deck[scr_StatsPlayer.idc] + " (" + (scr_StatsPlayer.idc + 1).ToString() + ")";
        Decks[scr_StatsPlayer.idc].GetComponentInChildren<Text>().text = scr_StatsPlayer.Name_deck[scr_StatsPlayer.idc];
    }

    public void SetIndexDeck(int _deck)
    {
        int _prev = scr_StatsPlayer.idc;
        scr_StatsPlayer.idc = _deck;
        UpdateNameDeck();
        for (int i = 0; i < 3; i++)
        {
            Decks[i].GetComponentInChildren<Text>().color = Color.white;
        }
        Decks[scr_StatsPlayer.idc].GetComponentInChildren<Text>().color = Color.red;
        for (int i = 0; i < scr_StatsPlayer.PlayerDeck[_prev].Count; i++)
        {
            int _i = CardsNames.IndexOf(scr_StatsPlayer.PlayerDeck[_prev][i]);
            if (_i>=0 && _i< Cards.Count)
                Cards[_i].gameObject.SetActive(true);
        }
        for (int i=0; i< scr_StatsPlayer.PlayerDeck[_deck].Count; i++)
        {
            int _i = CardsNames.IndexOf(scr_StatsPlayer.PlayerDeck[_deck][i]);
            if (_i >= 0 && _i < Cards.Count)
                Cards[_i].gameObject.SetActive(false);
        }
        OrderSelectedCards();

    }

    public void SetNameDeck()
    {
        if (Scr_Database.CheckNOSPCH(NewNameDeck.text) && NewNameDeck.text.Length>0)
        {
            scr_StatsPlayer.Name_deck[scr_StatsPlayer.idc] = NewNameDeck.text;
            UpdateNameDeck();
        }
    }

    public void OrderCardsbyXml()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            Destroy(Cards[i]);
        }
        Cards.Clear();
        CardsNames.Clear();
        LoadingCards = true;
        LoadingPanel.SetActive(true);
        EditorPanel.SetActive(false);
        StartCoroutine(LoadAllCards());
        OrderSelectedCards();
    }

    public void AddCardToCollection(string idcard, bool hide)
    {
        //creamos carta y damos propiedades
        scr_CardColection newItem = Instantiate(go_card, BoxCards.transform).GetComponent<scr_CardColection>();
        newItem.SetUnit(idcard);
        Cards.Add(newItem);
        CardsNames.Add(idcard);
        if (hide)
            newItem.gameObject.SetActive(false);
    }

    IEnumerator LoadAllCards()
    {
        yield return null;
        int cardsperframe = 10;

        for (int i=0; i < scr_StatsPlayer.PlayerAvUnits[scr_StatsPlayer.idc].Count; i++)
        {
            string idnu = scr_StatsPlayer.PlayerAvUnits[scr_StatsPlayer.idc][i];
            //Creamos carta
            AddCardToCollection(idnu, false);
            cardsperframe--;
            if (cardsperframe <= 0) { yield return null; cardsperframe = 10; }
        }
        for (int i = 0; i < scr_StatsPlayer.PlayerDeck[scr_StatsPlayer.idc].Count; i++)
        {
            string idnu = scr_StatsPlayer.PlayerDeck[scr_StatsPlayer.idc][i];
            //Creamos carta
            
            AddCardToCollection(idnu, true);
            cardsperframe--;
            if (cardsperframe <= 0) { yield return null; cardsperframe = 10;}
        }

        LoadingCards = false;
        OrderCollectionCards();
        LoadingPanel.SetActive(false);
        EditorPanel.SetActive(true);
        OkInit = true;
    }

    void LoadNewCards()
    {
        if (UIEditor.NewCardsToDeck.Count <= 0)
            return;

        while (UIEditor.NewCardsToDeck.Count>0)
        {
            string idnu = UIEditor.NewCardsToDeck[0];
            UIEditor.NewCardsToDeck.RemoveAt(0);
            //Creamos carta
            if (OkInit)
                AddCardToCollection(idnu, false);
        }
    }

    public void SwitchCardsInUI(string tocollection, string todeck)
    {
        GameObject ToCollection = Cards[CardsNames.IndexOf(tocollection)].gameObject;
        GameObject ToDeck = Cards[CardsNames.IndexOf(todeck)].gameObject;
        ToCollection.SetActive(true);
        ToDeck.SetActive(false);
        int new_index_parent = ToDeck.transform.GetSiblingIndex();
        ToCollection.transform.SetSiblingIndex(new_index_parent);
        Canvas.ForceUpdateCanvases();
        ContGLG.enabled = false;
        ContGLG.enabled = true;
    }

    public void FilterName(string s_search)
    {
        int limit_name = s_search.Length;
        for (int i=0; i<Cards.Count; i++)
        {
            string nu = Cards[i].s_name;
            if (limit_name > nu.Length) { limit_name = nu.Length; }
            if (limit_name > 0 && nu.ToLower().Substring(0, limit_name) != s_search.ToLower())
            {
                Cards[i].gameObject.SetActive(false);
            } else
            {
                if (!scr_StatsPlayer.PlayerDeck[scr_StatsPlayer.idc].Contains(Cards[i].s_idname))
                    Cards[i].gameObject.SetActive(true);
            }
        }
        Canvas.ForceUpdateCanvases();
        ContGLG.enabled = false;
        ContGLG.enabled = true;
    }

    public void FilterType(int value)
    {
        string s_type = NameTypes[value];
        for (int i = 0; i < Cards.Count; i++)
        {
            string ti = Cards[i].s_type;
            if (ti == s_type || value == 0)
            {
                if (!scr_StatsPlayer.PlayerDeck[scr_StatsPlayer.idc].Contains(Cards[i].s_idname))
                    Cards[i].gameObject.SetActive(true);
            }
            else
            {
                Cards[i].gameObject.SetActive(false);
            }
        }
        Canvas.ForceUpdateCanvases();  // *
        ContGLG.enabled = false;
        ContGLG.enabled = true;
    }

    public void OrderSelectedCards()
    {
        // Safety check: ensure deck has exactly 8 cards
        if (scr_StatsPlayer.PlayerDeck[scr_StatsPlayer.idc].Count != 8)
        {
            Debug.LogError("Lost Cards in Deck - Expected 8, got " + scr_StatsPlayer.PlayerDeck[scr_StatsPlayer.idc].Count);
            
            // Fix the deck by adding missing cards or removing excess
            while (scr_StatsPlayer.PlayerDeck[scr_StatsPlayer.idc].Count < 8)
            {
                // Add a default unit if we have any available
                if (scr_StatsPlayer.PlayerAvUnits[scr_StatsPlayer.idc].Count > 0)
                {
                    string defaultUnit = scr_StatsPlayer.PlayerAvUnits[scr_StatsPlayer.idc][0];
                    scr_StatsPlayer.PlayerDeck[scr_StatsPlayer.idc].Add(defaultUnit);
                }
                else
                {
                    // Fallback to a hardcoded unit
                    scr_StatsPlayer.PlayerDeck[scr_StatsPlayer.idc].Add("U_Ship_01");
                }
            }
            
            // Remove excess cards if more than 8
            while (scr_StatsPlayer.PlayerDeck[scr_StatsPlayer.idc].Count > 8)
            {
                scr_StatsPlayer.PlayerDeck[scr_StatsPlayer.idc].RemoveAt(scr_StatsPlayer.PlayerDeck[scr_StatsPlayer.idc].Count - 1);
            }
        }

        int TotalCost = 0;

        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].Clear();
            
            // Safety check for array bounds
            if (i < scr_StatsPlayer.PlayerDeck[scr_StatsPlayer.idc].Count)
            {
                string idname = scr_StatsPlayer.PlayerDeck[scr_StatsPlayer.idc][i];
                Slots[i].SetUnit(idname);
                Slots[i].empty = false;
                Slots[i].UpdateInfo();
                int cost = 0;
                int.TryParse(scr_GetStats.GetPropUnit(idname, "Cost"), out cost);
                TotalCost += cost;
            }
            else
            {
                Slots[i].empty = true;
            }
        }
        
        float FACost = (TotalCost / 8f);

        AvgEnergy.text = FACost.ToString("0.0");
    }

    public void OrderCollectionCards()
    {
        switch (DD_OrderBy.value)
        {
            case 0: //Por Costo
                {
                    Cards.Sort(
                           delegate (scr_CardColection p1, scr_CardColection p2)
                           {
                               return p1.i_Cost.CompareTo(p2.i_Cost);
                           }
                       );
                    
                }
                break;
            case 1: //Por Alfabeto
                {
                    Cards.Sort(
                        delegate (scr_CardColection p1, scr_CardColection p2)
                        {
                            return p1.s_name.CompareTo(p2.s_name);
                        }
                    );
                }
                break;
            case 2: //Por Nivel
                {
                    Cards.Sort(
                        delegate (scr_CardColection p1, scr_CardColection p2)
                        {
                            return p1.i_Level.CompareTo(p2.i_Level);
                        }
                    );
                }
                break;
            case 3: //Por Rareza
                {
                    Cards.Sort(
                        delegate (scr_CardColection p1, scr_CardColection p2)
                        {
                            return p1.i_rarity.CompareTo(p2.i_rarity);
                        }
                    );
                }
                break;
        }
        //Ordenamos en Fisico
        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].gameObject.transform.SetSiblingIndex(i);
            CardsNames[i] = Cards[i].s_idname;
        }
    }

    public void ShowInfoCard(string idUnit)
    {
        if (!InfoShip.activeSelf && !InfoStation.activeSelf && !InfoSkill.activeSelf)
        {
            string type = scr_GetStats.GetTypeUnit(idUnit);

            if (type=="Ship")
            {
                InfoShip.SetActive(true);
                InfoShip.GetComponent<scr_UIInfoUnit>().SetInfo(idUnit);
            }
            if (type=="Station")
            {
                InfoStation.SetActive(true);
                InfoStation.GetComponent<scr_UIInfoUnit>().SetInfo(idUnit);
            }
            if (type=="Skill")
            {
                InfoSkill.SetActive(true);
                InfoSkill.GetComponent<scr_UIInfoUnit>().SetInfo(idUnit);
            }
            if (type == "Squad")
            {
                InfoSquad.SetActive(true);
                InfoSquad.GetComponent<scr_UIInfoUnit>().SetInfo(idUnit);
            }
        }    

    }

    void OnDisable()
    {
        scr_BDUpdate.f_UpdateDeck(scr_StatsPlayer.id);
        StopAllCoroutines();
        InfoShip.SetActive(false);
        InfoStation.SetActive(false);
        InfoSkill.SetActive(false);
        InfoSquad.SetActive(false);
    }

    private void OnEnable()
    {
        LoadNewCards();
    }

}
