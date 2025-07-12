using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class m_quickManager : Photon.MonoBehaviour {

    public static bool InBotRoom =  false;

    AsyncOperation ProgresBotLevel;

    public GameObject QueuePanel;

    public scr_UIPlayerEditor PE;

    WaitForSeconds Delay1s = new WaitForSeconds(1f);

    bool EntryRoom = false;

    int ModoDeJuego = 0;
    int LevelMatch = 0;
    int MaxLevelDiference = 0;
    int JoinAtemps = 0;

    string[] GameModes;

    void Start ()
    {
        InBotRoom = false;
        GameModes = new string[2] { "League", "Practice"};
    }


    void Update ()
    {
        if (!EntryRoom && PhotonNetwork.inRoom)
        { 
            //cambiar para modo dev a 1
            //
            if (PhotonNetwork.room.PlayerCount >= 2)
            {
                EntryRoom = true;
                StopCoroutine("TimeOutToBot");
                QueuePanel.transform.GetChild(2).GetComponent<Button>().interactable = false;
                QueuePanel.transform.GetChild(1).GetComponent<Text>().text = scr_Lang.GetText("txt_mn_action3");
                PhotonNetwork.room.IsOpen = false;
                PhotonNetwork.room.IsVisible = false;
                switch(ModoDeJuego)
                {
                    case 0:
                        {
                            Debug.Log("Cargo Mapa (League)");
                            PhotonNetwork.LoadLevel("Mapa_Base_");
                        }
                        break;
                }
                
            }
        }
    }

    public void StartSearchMatch(int GameType)
    {
        if (scr_StatsPlayer.Offline)
        {
            PE.ShowWarningNoConnection();
            return;
        }

        if (PhotonNetwork.inRoom)
            return;

        PhotonNetwork.autoJoinLobby = false;
        ModoDeJuego = GameType;
        JoinAtemps = 5;
        MaxLevelDiference = 1;

        SetLevelMatch();
        StopAllCoroutines();

        Debug.Log("P1 Buscar partida"+GameModes[ModoDeJuego]);
        
        PE.ChangeStatePlayer(5);
        QueuePanel.SetActive(true);
        JoinRandomMatch();
    }

    void JoinRandomMatch()
    {
        //Hashtable expectedCustomRoomProperties = new Hashtable() { { GameModes[ModoDeJuego], LevelMatch } };
        Hashtable expectedCustomRoomProperties = new Hashtable() { { GameModes[ModoDeJuego], 0 } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 2);
    }

    void SetLevelMatch()
    {
        LevelMatch = scr_StatsPlayer.Level / 25;
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Se creara partida");

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;

        System.DateTime theTime = System.DateTime.Now;
        string datetime = theTime.ToString("yyyy-MM-dd\\THH:mm:ss\\Z");

        roomOptions.CustomRoomPropertiesForLobby = new string[] { GameModes[ModoDeJuego] };
        roomOptions.CustomRoomProperties = new Hashtable() { { GameModes[ModoDeJuego], 0 } };
        PhotonNetwork.CreateRoom(GameModes[ModoDeJuego] + "_" + datetime, roomOptions, TypedLobby.Default);
        Debug.Log("Partida " + GameModes[ModoDeJuego] + " creada!");

        if (ModoDeJuego == 0)
        {
            //Debug.Log("Partidas Bot deshabilitadas");
            StartCoroutine(TimeOutToBot());
        }

        /*
        if (JoinAtemps>0)
        {
            Debug.Log("Intentando unirse a partida");
            JoinAtemps--;
            LevelMatch++;
            if (LevelMatch> MaxLevelDiference)
            {
                MaxLevelDiference++;
                SetLevelMatch();
            }
            StopCoroutine("DelayJoinRandomMatch");
            StartCoroutine(DelayJoinRandomMatch());
        } else
        {
            Debug.Log("Se creara partida");

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            roomOptions.MaxPlayers = 2;

            System.DateTime theTime = System.DateTime.Now;
            string datetime = theTime.ToString("yyyy-MM-dd\\THH:mm:ss\\Z");

            roomOptions.CustomRoomPropertiesForLobby = new string[] { GameModes[ModoDeJuego] };
            roomOptions.CustomRoomProperties = new Hashtable() { { GameModes[ModoDeJuego], LevelMatch } };
            PhotonNetwork.CreateRoom(GameModes[ModoDeJuego] + "_" + datetime, roomOptions, TypedLobby.Default);
            Debug.Log("Partida " + GameModes[ModoDeJuego] + " creada!");

            if (ModoDeJuego == 0)
            {
                //Debug.Log("Partidas Bot deshabilitadas");
                StartCoroutine(TimeOutToBot());
            }
        }
        */
    }

    public void CancelQuick()
    {
        Debug.Log("Busqueda cancelada");
        PE.ChangeStatePlayer(scr_StatsPlayer.LastState);
        StopAllCoroutines();
        PhotonNetwork.LeaveRoom();
        QueuePanel.SetActive(false);
    }

    IEnumerator TimeOutToBot()
    {
        WaitForSeconds timout = new WaitForSeconds(Random.Range(5.5f, 7.5f));
        yield return timout;
        InBotRoom = true;
        PhotonNetwork.LeaveRoom();
        QueuePanel.transform.GetChild(2).GetComponent<Button>().interactable = false;
        QueuePanel.transform.GetChild(1).GetComponent<Text>().text = scr_Lang.GetText("txt_mn_action3");
        ProgresBotLevel = SceneManager.LoadSceneAsync("Mapa_vsIA");
        ProgresBotLevel.allowSceneActivation = true;
        Debug.Log("No hay jugadores, entrando a bot");
        while (!ProgresBotLevel.isDone)
        {
            yield return Delay1s;
        }
    }

    IEnumerator DelayJoinRandomMatch()
    {
        yield return Delay1s;
        JoinRandomMatch();
    }

    void OnJoinedRoom()
    {
        if (PhotonNetwork.inRoom)
        {
            Debug.Log("En room de juego");
        }
    }
    void OnLeftRoom()
    {
        EntryRoom = false;
        Debug.Log("Sali del room");
    }
}
