using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_CardsCntrl : MonoBehaviour {

    public scr_Card[] Cards = new scr_Card[4];

    public scr_Card NextCard;

    public scr_Tutorial GameTutorial;

    int NCards;

    public bool is_placing_unit = false;

    public List<string> GameDeck;

    public Dictionary<string,scr_StatsUnit> UnitsToUse = new Dictionary<string, scr_StatsUnit>();

    public Dictionary<string, Vector2[]> Formations = new Dictionary<string, Vector2[]>();

    public Dictionary<string, float> Skins;

    public Dictionary<string, scr_StatsCard> StatsCards = new Dictionary<string, scr_StatsCard>();

    [HideInInspector]
    public string[] PCButtonsL = new string[4] { "q", "w", "e", "r" };
    [HideInInspector]
    public string[] PCButtonsN = new string[4] { "1", "2", "3", "4" };

    [HideInInspector]
    public int PlacingByKey = -1;

    [HideInInspector]
    public bool RedyCards = false;

    //[HideInInspector]
    public bool CursorOnCards = false;


    void Start()
    {
        GameDeck = scr_StatsPlayer.PlayerDeck[scr_StatsPlayer.idc];

        if (scr_StatsPlayer.Tutorial)
        {
            GameDeck = new List<string>();
            GameDeck.Add("U_Ship_04");
            GameDeck.Add("U_Ship_09");
            GameDeck.Add("U_Station_06");
            GameDeck.Add("U_Squad_08");
            GameDeck.Add("U_Station_04");
            GameDeck.Add("U_Skill_02");
            GameDeck.Add("U_Squad_02");
            GameDeck.Add("U_Skill_04");
        } else if (scr_MNGame.GM.DeckForThisGame.Count==8)
        {
            GameDeck = scr_MNGame.GM.DeckForThisGame;
            Debug.Log("Deck Seteado Predefinido");
        }

        scr_MNGame.GM.CntrlCards = this;

        StartCoroutine(LoadXMLBaseStatsForDeck());
    }

    public void SetCursorOnCards(bool _coc)
    {
        CursorOnCards = _coc;
    }

    IEnumerator LoadXMLBaseStatsForDeck()
    {
        //Load Skins
        Skins = new Dictionary<string, float>();
        for (int i = 0; i < GameDeck.Count; i++)
        {
            if (GameDeck[i].Contains("Skill"))
                continue;
            for (int j=0; j<scr_StatsPlayer.MyUnits.Count; j++)
            {
                if (scr_StatsPlayer.MyUnits[j].Name == GameDeck[i])
                {
                    float _hue = scr_GetStats.GetHueSkin(GameDeck[i], scr_StatsPlayer.MyUnits[j].CurrentSkin);
                    Skins.Add(GameDeck[i],_hue);
                    break;
                } else if (j== scr_StatsPlayer.MyUnits.Count-1)
                {
                    Skins.Add(GameDeck[i],-1f);
                }
            }
        }

        yield return null;
        //Definimos stats de cartas

        for (int i = 0; i < GameDeck.Count; i++)
        {
            InitCardStats(GameDeck[i]);
        }

        InitControl();

        yield return null;

        //Leemos unidades para el juego

        List<string> UnitsForGame = new List<string>();

        for (int i = 0; i<8; i++)
        {
            if (GameDeck[i].Contains("Squad"))
            {
                string _allunits = scr_GetStats.GetPropUnit(GameDeck[i],"Ships");
                string[] _units = _allunits.Split(',');
                for (int j=0; j<_units.Length; j++)
                {
                    if (!UnitsForGame.Contains(_units[j]))
                    {
                        UnitsForGame.Add(_units[j]);
                    }
                }
                //Add New Formation
                string[] Positions = scr_MNGame.GM.GetPositionsFromSquad(GameDeck[i]);

                if (Positions.Length > 1)
                {
                    Vector2[] _formation = new Vector2[Positions.Length];
                    //FORMACIONES UNICAS
                    for (int j = 0; j < Positions.Length; j++)
                    {
                        int _x = 0;
                        int _y = 0;
                        int.TryParse(Positions[j].Split(',')[0], out _x);
                        int.TryParse(Positions[j].Split(',')[1], out _y);
                        _formation[j] = new Vector2(_x, _y);
                    }
                    if (!Formations.ContainsKey(GameDeck[i]))
                        Formations.Add(GameDeck[i], _formation);
                }
                else
                {//FORMACIONES DEFAULT
                    Vector2[] _formation = new Vector2[Positions.Length];
                    int d = 0;
                    if (Positions[0] == "pyramid")
                        d = -1;
                    else if (Positions[0] == "mouth")
                        d = 1;

                    if (scr_MNGame.GM.TeamInGame == 1)
                        d *= -1;

                    float f = 0f;
                    float t = 0.5f;
                    float deltaF = 6f / Positions.Length;
                    for (int j = 0; j < Positions.Length; j++)
                    {
                        _formation[j] = new Vector2(Mathf.Abs(t) * d, f);
                        f += deltaF * Mathf.Sign(f);
                        f *= -1;
                        t += 0.5f;
                    }
                    if (!Formations.ContainsKey(GameDeck[i]))
                        Formations.Add(GameDeck[i], _formation);
                }

            } else
            {
                UnitsForGame.Add(GameDeck[i]);
            }
            
        }

        if (!scr_MNGame.GM.b_InNetwork)
        {
            scr_IA _ia = scr_MNGame.GM.IA; 
            if (_ia)
            {
                yield return new WaitUntil(() => _ia.MyDeck.Count>0);

                for (int i = 0; i < _ia.MyDeck.Count; i++)
                {
                    if (_ia.MyDeck[i].Contains("Squad"))
                    {
                        string _allunits = scr_GetStats.GetPropUnit(_ia.MyDeck[i], "Ships");
                        string[] _units = _allunits.Split(',');
                        for (int j = 0; j < _units.Length; j++)
                        {
                            if (!UnitsForGame.Contains(_units[j]))
                            {
                                UnitsForGame.Add(_units[j]);
                            }
                        }
                    } else if (!UnitsForGame.Contains(_ia.MyDeck[i]))
                    {
                        UnitsForGame.Add(_ia.MyDeck[i]);
                    }
                }
            }
            
        }

        yield return null;

        UnitsForGame.Add("Station_Central_Command");
        UnitsForGame.Add("Station_Inhibitor");
        UnitsForGame.Add("Station_Satelite");
        UnitsForGame.Add("U_Station_06");
        UnitsForGame.Add("U_Station_07");
        UnitsForGame.Add("U_Ship_15");

        //Creamos stats para el juego

        for (int i=0; i< UnitsForGame.Count; i++)
        {
            //Generamos stats Base
            scr_StatsUnit _unittouse;
            _unittouse = scr_MNGame.GM.LoadStats(UnitsForGame[i]);
            if (Skins.ContainsKey(UnitsForGame[i]))
                _unittouse.Skin = Skins[UnitsForGame[i]];

            _unittouse.IdUnit = UnitsForGame[i];

            if (!UnitsToUse.ContainsKey(UnitsForGame[i]))
                UnitsToUse.Add(UnitsForGame[i],_unittouse);
        }

        RedyCards = true;
    }

    void InitControl()
    {
        if (!scr_StatsPlayer.Tutorial)
            Order_Random(12);

        NCards = GameDeck.Count;
        if (NCards > 4)
        {
            NextCard.Set_Ship(GameDeck[4]);
            NextCard.IsNext = true;
            NCards = 4;
        }
        for (int i = 0; i < NCards; i++)
        {
            Cards[i].index = i;
            Cards[i].Set_Ship(GameDeck[i]);
        }
    
    }

    void InitCardStats(string id_unit)
    {
        scr_StatsCard sc = new scr_StatsCard();

        sc.MyUnit = id_unit;
        sc.AutoSpawn = false;
        sc.DragPreview = null;
        sc.Type = scr_GetStats.GetTypeUnit(id_unit);
        if (sc.Type == "Station")
        {
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "Size"), out sc.Size);
            Sprite[] sall = Resources.LoadAll<Sprite>("Units/Sprites/Stations/" + scr_GetStats.GetPropUnit(id_unit, "Sprite"));
            if (sall != null)
                sc.DragPreview = sall[0];
        }
        else if (sc.Type == "Skill")
        {
            sc.Size = 0.6f;
            Sprite sall = Resources.Load<Sprite>("Units/Sprites/Previews/Skills/" + id_unit + "_Prev");
            if (sall != null)
                sc.DragPreview = sall;
        }
        else if (sc.Type == "Ship")
        {
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "Size"), out sc.Size);
            Sprite[] sall = Resources.LoadAll<Sprite>("Units/Sprites/Previews/Ships/" + scr_GetStats.GetPropUnit(id_unit, "Sprite"));
            if (sall != null && sall.Length > 0)
                sc.DragPreview = sall[0];
        }
        else
        {
            sc.Size = 0.6f;
            Sprite sall = Resources.Load<Sprite>("Units/Sprites/Previews/Squads/" + id_unit + "_Prev");
            if (sall != null)
                sc.DragPreview = sall;
        }

        sc.Icon = Resources.Load<Sprite>("Units/Iconos/" + scr_GetStats.GetPropUnit(id_unit, "Icon"));

        int.TryParse(scr_GetStats.GetPropUnit(id_unit, "Poblation"), out sc.i_Pcost);
        int.TryParse(scr_GetStats.GetPropUnit(id_unit, "Cost"), out sc.i_Cost);

        if (sc.Size < 1f)
            sc.Size = 1f;

        if (Skins.ContainsKey(id_unit))
        {
            sc.SkinHue = Skins[id_unit];
        }

        StatsCards.Add(id_unit, sc);
    }

    public void UseCard(string _unit, int _id)
    {
        if (scr_MNGame.GM.FreezeDeck)
            return;

        if (scr_MNGame.GM.b_RepeatCard)
        {
            scr_MNGame.GM.b_RepeatCard = false;
        }
        else
        {
            GameDeck.Remove(_unit);
            GameDeck.Add(_unit);
            
            
            Cards[_id].Set_Ship(NextCard.Get_Ship());
            NextCard.Set_Ship(GameDeck[4]);
        }
        if (GameTutorial)
        {
            if (GameTutorial.WhaitForSpawn) { GameTutorial.AddProgress(); }
        }
    }

    public void SwitchCard(int _index)
    {
        string unit = Cards[_index].Get_Ship();
        GameDeck.Remove(unit);
        GameDeck.Add(unit);

        Cards[_index].Set_Ship(NextCard.Get_Ship());
        NextCard.Set_Ship(GameDeck[4]);
    }

    void Order_Random(int iterations) //varajea las cartas con un numerod e iteraciones
    {
        if (GameDeck.Count > 1)
            for (int i = 0; i < iterations; i++)
            {
                int a = Random.Range(0, GameDeck.Count - 1);
                int b = Random.Range(0, GameDeck.Count - 1);
                string temp = GameDeck[a];
                GameDeck[a] = GameDeck[b];
                GameDeck[b] = temp;
            }
        GameDeck.Reverse();
    }

}
