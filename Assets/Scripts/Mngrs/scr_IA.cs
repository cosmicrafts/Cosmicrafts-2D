using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scr_IA : Photon.MonoBehaviour {

    float Resources = 0f;

    [HideInInspector]
    public List<string> MyDeck;

    public List<scr_Unit> MyStations = new List<scr_Unit>();
    [HideInInspector]
    public List<scr_Unit> MyShips = new List<scr_Unit>();

    int CardIndexSpawn = 0;
    bool repeat_card = false;
    bool CantSpawn = false;

    [HideInInspector]
    public int MaxUnits = 15;

    [HideInInspector]
    public List<GameObject> Enemys = new List<GameObject>();

    public Text[] EmotesPlayer = new Text[4];
    string[] EmotsIA;

    bool EndGame = false;

    public GameObject LobyBot;
    public GameObject BotFrame;

    public Image FrameAvatar;

    public Text FrameName;
    public Text PlayerFrameName;

    public GameObject TitleIA;
    public GameObject Emotes;
    public Button BtnEmoteP1;
    public GameObject EmotesP1;

    public GameObject MsgEmote1;
    public GameObject MsgEmote2;

    float time_em1 = 0f;
    float time_em2 = 0f;

    [HideInInspector]
    public string MyName = "Bot";

    string[] BotsNames = new string[10] { "spacelegend", "galacticking", "gunmaster", "shipcommander",
        "mrgalaxy", "gamerwarrior", "odysseycap", "magicnoob", "starlord", "supercosmos"};

    int[] idbot = new int[10] {9,10,11,12,13,14,15,16,17,18 };

    int indexbot = 0;
    int BotLevel = 1;

    WaitForSeconds DelayIA = new WaitForSeconds(1f);

    int DelayTimes = 8;

    bool ImPlayerBot = false;

    private void Awake()
    {
        BotLevel = scr_StatsPlayer.Level;
        ImPlayerBot = m_quickManager.InBotRoom;
        PlayerFrameName.text = scr_StatsPlayer.Name;
        FrameName.text = "IA";
        if (ImPlayerBot)
        {
            //Cosas De Bot
            indexbot = Random.Range(0, BotsNames.Length);
            MyName = BotsNames[indexbot];
            TitleIA.SetActive(false);
            Emotes.SetActive(true);
            LobyBot = Instantiate(LobyBot);
            BotFrame.SetActive(true);
            MG_lob _loby = LobyBot.GetComponent<MG_lob>();
            Sprite MyAvatar = _loby.Avatres[Random.Range(0, _loby.Avatres.Length)];
            _loby.AvatarP2.sprite = MyAvatar;
            _loby.NameP2.text = MyName;
            FrameAvatar.sprite = MyAvatar;
            for (int i = 0; i < EmotesPlayer.Length; i++)
            {
                EmotesPlayer[i].text = scr_StatsPlayer.Emotes[i];
            }
            FrameName.text = MyName;
            BtnEmoteP1.enabled = true;
            //DelayTimes = 8;
        }

    }

    // Use this for initialization
    void Start () {

        MyDeck = new List<string>();

        List<string> Ships = new List<string>();
        List<string> others = new List<string>();
        int refresh = 5;

        for (int i = 0; i < scr_StatsPlayer.PlayerAvUnits[0].Count; i++)
        {
            string u = scr_StatsPlayer.PlayerAvUnits[0][i];
            if (u.Contains("Squad") || u.Contains("Ship"))
                Ships.Add(u);
            else
                others.Add(u);

            refresh--;
            if (refresh == 0)
            {
                refresh = 5;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            int idu = Random.Range(0, Ships.Count);
            string su = Ships[idu];
            Ships.RemoveAt(idu);
            MyDeck.Add(su);
            idu = Random.Range(0, others.Count);
            su = others[idu];
            others.RemoveAt(idu);
            MyDeck.Add(su);
        }
        //yield return DelayUpdate;
        Order_Random(12);

        if (!scr_StatsPlayer.Tutorial && !scr_StatsPlayer.Practice)
            StartCoroutine(IALoop());

        EmotsIA = new string[4] { "", "", "", "" };

        string[] elost = new string[4] { "GG", ":(", "Nice Play", "You'll pay for it!" };
        string[] ewin = new string[4] { "Yhea!", "Surprise!", "Buajajaja", "Bye" };
        string[] esad = new string[4] { "D:", ":v", ":(", "I'll kill you!" };
        string[] eangry = new string[4] { "You'll pay for it!", "D:", ":v", "Don't Kill me please" };

        EmotsIA[0] = esad[Random.Range(0, 4)];
        EmotsIA[1] = eangry[Random.Range(0, 4)];
        EmotsIA[2] = ewin[Random.Range(0, 4)];
        EmotsIA[3] = elost[Random.Range(0, 4)];

        Resources = 3f;
        MyStations.Add(scr_MNGame.GM.Station1);
    }
	
	// Update is called once per frame
	void Update () {

        if (ImPlayerBot)
        {
            if (time_em1 > 0f)
            {
                time_em1 -= Time.deltaTime;
                if (time_em1 <= 0)
                {
                    BtnEmoteP1.enabled = true;
                    MsgEmote1.SetActive(false);
                }
            }

            if (time_em2 > 0f)
            {
                time_em2 -= Time.deltaTime;
                if (time_em2 <= 0)
                    MsgEmote2.SetActive(false);
            }
        }

        if (scr_MNGame.GM.UserEndGame && EndGame)
        {
            SceneManager.LoadScene("Menu-Interfas");
            return;
        }

        if (scr_MNGame.GM.b_EndGame)
        {

            if (!EndGame)
            {
                EndGame = true;
                if (!scr_StatsPlayer.Tutorial && !scr_StatsPlayer.Practice)
                    scr_MNGame.GM.CheckWiner();
                else
                {
                    scr_StatsPlayer.FirstBattle = true;
                    scr_MNGame.GM.EndTutorial();
                }
            }
        }

        if (EndGame)
            return;

        if (Resources<10f)
        {
            Resources += Time.deltaTime*0.5f;
        } else
        {
            Resources = 10f;
        }
        
    }

    void f_CantSpawn()
    {
        DelayTimes = 0;
        CantSpawn = true;
        CardIndexSpawn++;
        if (CardIndexSpawn >= 4)
            CardIndexSpawn = 0;
    }

    IEnumerator IALoop()
    {
        while(!EndGame)
        {
            for (int i=0; i< DelayTimes; i++)
                yield return DelayIA;

            //Select card
            if (!CantSpawn)
            {
                if (MyStations.Count <= 0)
                {
                    CardIndexSpawn = GetSpecificTypeFromDeck("Station");
                } else if (MyShips.Count <= 0)
                {
                    CardIndexSpawn = GetSpecificTypeFromDeck("Ship");
                } else
                {
                    CardIndexSpawn = Random.Range(0, 4);
                }
            } else
            {
                yield return null;
            }

            int cost = 0;
            int poblation = 0;
            bool OkPoblation = true;
            int.TryParse(scr_GetStats.GetPropUnit(MyDeck[CardIndexSpawn], "Cost"), out cost);
                
            string unittype = scr_GetStats.GetTypeUnit(MyDeck[CardIndexSpawn]);

            if (unittype != "Skill")
            {
                int.TryParse(scr_GetStats.GetPropUnit(MyDeck[CardIndexSpawn], "Poblation"), out poblation);
                if (poblation + MyStations.Count - 5 >= MaxUnits)
                    OkPoblation = false;
            }

            if (Resources>=cost && OkPoblation)
            {
                Resources -= cost;
            } else
            {
                f_CantSpawn();
                continue;
            }

            CantSpawn = false;
            //Check Card
            string card = scr_GetStats.GetTypeUnit(MyDeck[CardIndexSpawn]);
            if (card == "Skill") //Spawn Skill
            {
                int IdSkill = 1;
                int.TryParse(MyDeck[CardIndexSpawn].Substring(8), out IdSkill);
                Vector2 position = new Vector2();
                switch (IdSkill)
                {
                    case 1: //Inspire
                    case 4: //ForceShield
                    case 7: //Inmortal Phase
                    case 9: //Solar Mirror
                    case 12: //Ghost Bomb
                    case 14: //Heal
                        {
                            if (MyShips.Count > 0)
                            {
                                int ix = Random.Range(0, MyShips.Count);
                                scr_Unit ship = MyShips[ix];
                                if (ship != null)
                                {
                                    position = ship.transform.position;
                                    SpawnCard(CardIndexSpawn, position);
                                }
                                else
                                {
                                    MyShips.RemoveAt(ix);
                                    f_CantSpawn();
                                    continue;
                                }
                            }
                            else
                            {
                                f_CantSpawn();
                                continue;
                            }
                        }
                        break;
                    case 2: //Ping
                    case 3: //Hack
                    case 8: //BlackHole
                    case 10: //Contamination
                    case 11: //Black Nebula
                    case 13: //Thunder
                    case 5: //Lazer
                        {
                            if (Enemys.Count>0)
                            {
                                int index_enemy = Random.Range(0, Enemys.Count);
                                if (Enemys[index_enemy] == null)
                                {
                                    Enemys.RemoveAt(index_enemy);
                                    f_CantSpawn();
                                    continue;
                                }
                                else
                                {
                                    position = Enemys[index_enemy].transform.position;
                                    SpawnCard(CardIndexSpawn, position);
                                }
                            } else
                            {
                                f_CantSpawn();
                                continue;
                            }
                            
                        }
                        break;
                    default://Energize
                        {
                            SpawnCard(CardIndexSpawn, position);
                        }
                        break;
                }
            }
            else
            { //Spawn Unit o Station
                float Range_spawn = 0f;
                float.TryParse(scr_GetStats.GetPropUnit(MyDeck[CardIndexSpawn], "SpawnArea"), out Range_spawn);
                Vector2 position = new Vector2();
                OrderStationtFar();
                int max_index = 3;
                if (max_index > MyStations.Count)
                    max_index = MyStations.Count;
                scr_Unit origin = MyStations[Random.Range(0,max_index)];
                if (origin == null)
                {
                    f_CantSpawn();
                    continue;
                } else
                {
                    position = origin.transform.position + new Vector3(-Range_spawn, 0f);
                    SpawnCard(CardIndexSpawn, position);
                }
            }
            
            DelayTimes = 3 - (BotLevel / 10);
            if (DelayTimes <= 0) { DelayTimes = 1; }
            
        }
    
    }

    public void ShowEmotePlayer(Text msg)
    {
        MsgEmote1.SetActive(true);
        Text ts = MsgEmote1.transform.GetChild(0).GetComponent<Text>();
        ts.text = msg.text;
        time_em1 = 3f;
        EmotesP1.SetActive(false);
    }

    public void ShowEmoteIA(string msg)
    {
        if (MsgEmote2.activeSelf || !ImPlayerBot)
            return;

        MsgEmote2.SetActive(true);
        Text ts = MsgEmote2.transform.GetChild(0).GetComponent<Text>();
        ts.text = msg;
        time_em2 = 3f;
    }

    public void ShowEmoteIA(int idmsg)
    {
        if (MsgEmote2.activeSelf || !ImPlayerBot)
            return;

        MsgEmote2.SetActive(true);
        Text ts = MsgEmote2.transform.GetChild(0).GetComponent<Text>();
        ts.text = EmotsIA[idmsg];
        time_em2 = 3f;
    }

    public void RemoveUnit(scr_Unit remove)
    {
        MyStations.Remove(remove);
        if (Random.Range(0,4)==0)
        {
            ShowEmoteIA(EmotsIA[Random.Range(0,2)]);
        }
    }

    void Order_Random(int iterations) //varajea las cartas con un numero de iteraciones
    {
        if (MyDeck.Count > 1)
            for (int i = 0; i < iterations; i++)
            {
                int a = Random.Range(0, MyDeck.Count - 1);
                int b = Random.Range(0, MyDeck.Count - 1);
                string temp = MyDeck[a];
                MyDeck[a] = MyDeck[b];
                MyDeck[b] = temp;
            }
        MyDeck.Reverse();
    }

    int GetSpecificTypeFromDeck(string _type)
    {
        int result = 0;
        for (int i=0; i< 4; i++)
        {
            if (MyDeck[i].Contains(_type))
            {
                result = i;
                break;
            }
        }
        return result;
    }

    void OrderStationtFar()
    {
        float _max_x = 19f;

        for(int i=0; i<MyStations.Count; i++)
        {
            if (MyStations[i]!=null)
            {
                MyStations[i].Priority_distance = _max_x - MyStations[i].transform.position.x;
            } else
            {
                MyStations.RemoveAt(i);
            }
        }

        MyStations.Sort(
                           delegate (scr_Unit u1, scr_Unit u2)
                           {
                               return u2.Priority_distance.CompareTo(u1.Priority_distance);
                           }
                       );

    }

    void SpawnCard(int index, Vector2 position)
    {

        float x, y;
        x = position.x;
        y = position.y;

        if (y > -8) { y = -8; }
        if (y < -18) { y = -18f; }

        GameObject _unit = scr_MNGame.GM.CreateUnitOff(MyDeck[index], new Vector2(x, y), 1, -1f);

        if (!MyDeck[index].Contains("Skill"))
        {
            scr_Unit _scr_unit = _unit.GetComponent<scr_Unit>();

            if (_unit.CompareTag("Ship"))
                MyShips.Add(_scr_unit);
            else
                MyStations.Add(_scr_unit);
        }

        UseCard(MyDeck[index]);
    }

    public void UseCard(string _unit)
    {
        if (repeat_card)
        {
            repeat_card = false;
        }
        else
        {
            string temp = _unit;

            MyDeck.Remove(_unit);
            MyDeck.Add(temp);
        }
    }

    public string GetDeckToString()
    {
        string str_deck = "";
        bool start = true;
        for (int i = 0; i < MyDeck.Count; i++)
        {
            if (start)
            {
                start = false;
            }
            else { str_deck += "-"; }
            str_deck += MyDeck[i];
        }
        return str_deck;
    }

    public int GetIdBot()
    {
        return idbot[indexbot];
    }

    public void EndGameUser()
    {
        scr_MNGame.GM.UserEndGame = true;
    }

}
