using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class scr_Conection : Photon.MonoBehaviour {

    public GameObject MsgDis;
    public GameObject WarningOffline;
    public ChatNewGui Chat;
    public Text StateCon;

    WaitForSeconds timetry = new WaitForSeconds(1);

    IEnumerator Recconect()
    {
        while (true)
        {
            yield return timetry;
            ConnectToPhoton();
        }      
    }

    void OnConnectedToPhoton()
    {
        StopAllCoroutines();
        if (MsgDis != null)
            MsgDis.SetActive(false);
        WarningOffline.SetActive(false);
        Chat.Connect();
        scr_StatsPlayer.Offline = false;
        if (scr_MNGame.GM != null)
            scr_MNGame.GM.ShowAlert(scr_Lang.GetText("txt_stats_info25_log"));
    }

    public void AlertDisconnect()
    {
        if (MsgDis != null)
            MsgDis.SetActive(true);
    }

    void OnDisconnectedFromPhoton()
    {
        if (FindObjectOfType<scr_UIPlayerEditor>()!=null)
            AlertDisconnect();

        scr_StatsPlayer.Offline = true;
        WarningOffline.SetActive(true);
        if (scr_MNGame.GM != null)
            scr_MNGame.GM.ShowAlert(scr_Lang.GetText("txt_stats_info24_log"));
        StartCoroutine(Recconect());
    }

    public void OfflineMode()
    {
        MsgDis.SetActive(false);
    }

    void OnGUI()
    {
        StateCon.text = "Ping:" + PhotonNetwork.GetPing().ToString() + "("+ PhotonNetwork.connectionState.ToString()+")";
    }

    public static bool CheckInternetConnection()
    {
        bool result = true;

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            result = false;
            Debug.Log("Error. Check internet connection!");
        }

        return result;
    }

    public static bool ConnectToPhoton()
    {
        return PhotonNetwork.ConnectUsingSettings("v1.0");
    }

}
