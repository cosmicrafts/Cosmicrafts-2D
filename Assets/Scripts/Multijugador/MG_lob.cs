using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MG_lob : Photon.MonoBehaviour
{
    int Counter;

    public Slider Charge_Bar;
    public Image AvatarP1;
    public Image AvatarP2;
    public Text Status;
    public Text NameP1;
    public Text NameP2;

    public Sprite[] Avatres;

    public Text Advice;

    WaitForSeconds Delay1s = new WaitForSeconds(1f);

    [HideInInspector]
    public int Players_Ready = 0;

    void Start ()
    {
        Counter = 5;
        int id_tip = Random.Range(1, 11);
        Advice.text = scr_Lang.GetText("txt_game_tips_" + id_tip.ToString());
        AvatarP1.sprite = Avatres[scr_StatsPlayer.IDAvatar];
        NameP1.text = scr_StatsPlayer.Name;
        if (scr_MNGame.GM.b_InNetwork)
        {
            photonView.RPC("AddPlayerEnterRPC", PhotonTargets.AllBuffered);
            foreach (PhotonPlayer p in PhotonNetwork.playerList)
            {
                object ida;
                if (p.NickName!= scr_StatsPlayer.Name)
                {
                    p.CustomProperties.TryGetValue("idAvatar", out ida);
                    AvatarP2.sprite = Avatres[(int)ida];
                    NameP2.text = p.NickName;
                    break;
                }
            }
            if (PhotonNetwork.isMasterClient)
                StartCoroutine(TimerNetwork());
        } else
        {
            StartCoroutine(Timer());
        }
        if (scr_MNGame.GM.ConnectionDelay)
            Destroy(scr_MNGame.GM.ConnectionDelay);
    }

    IEnumerator Timer()
    {
        while(Counter>0)
        {
            Charge_Bar.value = 5 - Counter;
            yield return Delay1s;
            Counter--;
            if (Counter==0)
            {
                scr_MNGame.GM.StartGame();
                Destroy(gameObject);
            }
        }
    }

    IEnumerator TimerNetwork()
    {
        yield return new WaitUntil(() => Players_Ready>1);
        while (Counter>-1)
        {
            Counter--;
            if (Counter == -1)
            {
                Destroy(gameObject);
            }
            else
            {
                Charge_Bar.value = 5 - Counter;
                if (Counter == 0)
                {
                    photonView.RPC("StartGameRPC", PhotonTargets.OthersBuffered);
                    scr_MNGame.GM.StartGame();
                }
                else
                {
                    photonView.RPC("SetCounterRPC", PhotonTargets.OthersBuffered, Counter);
                }
            }
            yield return Delay1s;
        }
    }

    [PunRPC]
    public void SetCounterRPC(int time)
    {
        Counter = time;
        Charge_Bar.value = 5 - Counter;
    }

    [PunRPC]
    public void AddPlayerEnterRPC()
    {
        Players_Ready++;
    }

    [PunRPC]
    public void StartGameRPC()
    {
        scr_MNGame.GM.StartGame();
        Destroy(gameObject);
    }
}
