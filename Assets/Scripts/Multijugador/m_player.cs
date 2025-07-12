using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class m_player : Photon.MonoBehaviour
{
    //Cosas De Emotes
    public Text MSMP1;
    public Text MSMP2;

    public Button EmoteP1;
    public Button EmoteP2;

    public GameObject TEm_p1;
    public GameObject TEm_p2;

    public Text[] AllEmotes_p1 = new Text[4];
    public Text[] AllEmotes_p2 = new Text[4];

    GameObject[] Inf;
    ///Termina Cosas de Emotes

    public Image Avatar1;
    public Image Avatar2;
    public Sprite[] Avatres;

    public Text FrameName1;
    public Text FrameName2;

    public GameObject PlayerPanel;
    public GameObject EndGamePanel;
    public Image EnergyBar;
    public Text txt_ShipsInGame;
    public Text txt_Resources;
    public Text txt_Time;
    public Text txt_FPS;
    public GameObject Alerts;
    public GameObject LostOtherPLayerCanvas;
    public GameObject TimePanel;

    WaitForSeconds Delay3s = new WaitForSeconds(3f);

    void Start()
    {
        //Dejar Solo activa tu UI
        Inf = GameObject.FindGameObjectsWithTag("HUDs");
        for (int i = 0; i < Inf.Length; i++)
        {
            if (Inf[i].gameObject.GetPhotonView().isMine == false)
            {
                m_player other = Inf[i].GetComponent<m_player>();
                other.PlayerPanel.SetActive(false);
                other.EndGamePanel.SetActive(false);
            }
        }
        //Aapgar Emotes del otro jugador
        if (PhotonNetwork.player.ID == 2)
        {
            EmoteP2.enabled = true;
        } else if (PhotonNetwork.player.ID == 1)
        {
            EmoteP1.enabled = true;
        }
        //Cosas Propias
        if (photonView.isMine)
        {
            scr_MNGame.GM.OtherPLayerOutCanvas = LostOtherPLayerCanvas;

            if (scr_MNGame.GM.IsHorda)
                TimePanel.SetActive(false);
            else
            {
                string OtherNamePlayer = "";
                for (int i=0; i< PhotonNetwork.playerList.Length; i++)
                {
                    if (PhotonNetwork.playerList[i].NickName != scr_StatsPlayer.Name)
                    {
                        OtherNamePlayer = PhotonNetwork.playerList[i].NickName;
                        break;
                    }    
                }
                if (PhotonNetwork.player.ID == 1)
                {
                    FrameName1.text = scr_StatsPlayer.Name;
                    FrameName2.text = OtherNamePlayer;
                } else
                {
                    FrameName2.text = scr_StatsPlayer.Name;
                    FrameName1.text = OtherNamePlayer;
                }
            }

            scr_MNGame.GM.MyPlayer = this;
            scr_MNGame.GM.Screens = transform.GetChild(2).gameObject;

            scr_MNGame.GM.txt_ShipsInGame = txt_ShipsInGame;
            scr_MNGame.GM.txt_Time = txt_Time;
            scr_MNGame.GM.txt_Resources = txt_Resources;
            scr_MNGame.GM.fpsText = txt_FPS;
            scr_MNGame.GM.EnergyBar = EnergyBar;


            //cargar Emotes Correspondientes
            if (PhotonNetwork.player.ID == 1)
            {
                scr_MNGame.GM.IDinGame = 0;
                scr_MNGame.GM.TeamInGame = 0;
                for (int i=0; i<4; i++)
                    AllEmotes_p1[i].text = scr_StatsPlayer.Emotes[i];
            }
            if (PhotonNetwork.player.ID == 2)
            {
                scr_MNGame.GM.IDinGame = 1;
                scr_MNGame.GM.TeamInGame = 1;
                for (int i = 0; i < 4; i++)
                    AllEmotes_p2[i].text = scr_StatsPlayer.Emotes[i];
            }
            //Cargar avatares de forma local por UI
            CargarAvatares();
            //
        }
    }

    public void ShowAlert(string alert)
    {
        Alerts.GetComponent<Text>().text = alert;
        Alerts.GetComponent<Animator>().Play("ShowWarning");
    }
    //Cosas Para los Emotes
    //Activar EL enivo de Emotes // Despliegue de los 4 campos
    public void AcEmotes_P1()
    {
        TEm_p1.gameObject.SetActive(!TEm_p1.gameObject.activeSelf);
    }
    public void AcEmotes_P2()
    {
        TEm_p2.gameObject.SetActive(!TEm_p2.gameObject.activeSelf);
    }

    public void MensajesEmotes(int id)
    {
        switch(id)
        {
            case 0:
                {
                    if (PhotonNetwork.player.ID == 1)
                    {
                        AcEmotes_P1();
                        photonView.RPC("EnviarMensajeP1", PhotonTargets.AllBufferedViaServer, scr_StatsPlayer.Emotes[id]);
                    }
                    if (PhotonNetwork.player.ID == 2)
                    {
                        AcEmotes_P2();
                        photonView.RPC("EnviarMensajeP2", PhotonTargets.AllBufferedViaServer, scr_StatsPlayer.Emotes[id]);
                    }
                }
                break;
            case 1:
                {
                    if (PhotonNetwork.player.ID == 1)
                    {
                        AcEmotes_P1();
                        photonView.RPC("EnviarMensajeP1", PhotonTargets.AllBufferedViaServer, scr_StatsPlayer.Emotes[id]);
                    }
                    if (PhotonNetwork.player.ID == 2)
                    {
                        AcEmotes_P2();
                        photonView.RPC("EnviarMensajeP2", PhotonTargets.AllBufferedViaServer, scr_StatsPlayer.Emotes[id]);
                    }
                }
                break;
            case 2:
                {
                    if(PhotonNetwork.player.ID == 1)
                    {
                        AcEmotes_P1();
                        photonView.RPC("EnviarMensajeP1", PhotonTargets.AllBufferedViaServer, scr_StatsPlayer.Emotes[id]);
                    }
                    if (PhotonNetwork.player.ID == 2)
                    {
                        AcEmotes_P2();
                        photonView.RPC("EnviarMensajeP2", PhotonTargets.AllBufferedViaServer, scr_StatsPlayer.Emotes[id]);
                    }
                }
                break;
            case 3:
                {
                    if (PhotonNetwork.player.ID == 1)
                    {
                        AcEmotes_P1();
                        photonView.RPC("EnviarMensajeP1", PhotonTargets.AllBufferedViaServer, scr_StatsPlayer.Emotes[id]);
                    }
                    if (PhotonNetwork.player.ID == 2)
                    {
                        AcEmotes_P2();
                        photonView.RPC("EnviarMensajeP2", PhotonTargets.AllBufferedViaServer, scr_StatsPlayer.Emotes[id]);
                    }
                }
                break;
            default:
                print("Error 404");
                break;
        }
    }
    [PunRPC]
    void EnviarMensajeP1(string _Mensaje)
    {
        MSMP1.transform.parent.gameObject.SetActive(true);
        MSMP1.text = _Mensaje;
    }
    [PunRPC]
    void EnviarMensajeP2(string _Mensaje)
    {
        MSMP2.transform.parent.gameObject.SetActive(true);
        MSMP2.text = _Mensaje;
    }
    IEnumerator TerminarP1()
    {
        yield return Delay3s;
        MSMP1.text = "";
        MSMP1.transform.parent.gameObject.SetActive(false);
    }
    IEnumerator TerminarP2()
    {
        yield return Delay3s;
        MSMP2.text = "";
        MSMP2.transform.parent.gameObject.SetActive(false);
    }
    //Termina Cosas de Emotes

    //Inicia Cosas de avatares
    void CargarAvatares()
    {
        foreach (PhotonPlayer p in PhotonNetwork.playerList)
        {
            if (p.ID == 1)
            {
                object ida;
                p.CustomProperties.TryGetValue("idAvatar", out ida);
                Avatar1.sprite = Avatres[(int)ida];
            }
            if (p.ID == 2)
            {
                object ida;
                p.CustomProperties.TryGetValue("idAvatar", out ida);
                Avatar2.sprite = Avatres[(int)ida];
            }
        }
    }
    //Terminar por usuario
    public void EndGameUser()
    {
        scr_MNGame.GM.UserEndGame = true;
    }

    public void EmergentEndGameUser()
    {
        scr_MNGame.GM.TERMINARPARTIDA = true;
        scr_MNGame.GM.UserEndGame = true;
    }

    //Inician spawns

    public GameObject CreateShipNET(string _name, float _x, float _y, int _team, float  hp, float scale, float skin, string fromsquad)
    {
        object[] udata = new object[6];
        udata[0] = _team;
        udata[1] = _name;
        udata[2] = skin;
        udata[3] = fromsquad;
        udata[4] = hp;
        udata[5] = scale;
        GameObject new_ship = PhotonNetwork.Instantiate("Units/BasePrefabs/U_Unit_Base", new Vector2(_x, _y), Quaternion.identity, 0, udata);
        return new_ship;
    }

    public void CreateStationNET(string _name, float _x, float _y, int _team, float skin)
    {
        object[] udata = new object[4];
        udata[0] = _team;
        udata[1] = _name;
        udata[2] = skin;
        udata[3] = "none";
        PhotonNetwork.Instantiate("Units/BasePrefabs/U_Unit_Base", new Vector2(_x, _y), Quaternion.identity, 0, udata);
    }

    public void CreateSkillNET(string _name, float _x, float _y, int _team)
    {
        object[] udata = new object[4];
        udata[0] = _team;
        udata[1] = _name;
        udata[2] = -1f;
        udata[3] = "none";
        PhotonNetwork.Instantiate("Units/BasePrefabs/U_Skill_Base", new Vector2(_x, _y), Quaternion.identity, 0, udata);
    }
}
