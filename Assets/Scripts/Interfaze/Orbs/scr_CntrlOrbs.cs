using UnityEngine;

public class scr_CntrlOrbs : MonoBehaviour {

    public scr_UIPlayerEditor PE;

    public scr_StarOrb DStarsOrb;

    public GameObject NoStarsMssage;

    [HideInInspector]
    public scr_StarOrb SelectedOrb;

    //bool ok_init = false;

    // Use this for initialization
    void Start () {
        DStarsOrb.InitOrbe(scr_StatsPlayer.OpeningOrbs[0],100);
    }
	
    public void GetOrb(scr_StarOrb _orb)
    {
        if (scr_StatsPlayer.Offline)
        {
            return;
        }

        SelectedOrb = _orb;

        if (!SelectedOrb.RedyToOpen)
        {
            NoStarsMssage.SetActive(true);
            return;
        }

        _orb.RestartOrb();
        PE.OpenStarsOrb(_orb.MyData.Type);
        scr_BDUpdate.f_SetStarsOrbs(scr_StatsPlayer.id, false);

    }

    public void PayToOpenOrb()
    {
        /*
        if (scr_StatsPlayer.Offline)
        {
            return;
        }

        scr_StatsPlayer.Neutrinos -= SelectedOrb.GetCostToOpen();
        SelectedOrb.MyData.Created = new System.DateTime(2000,1,1,1,1,1);
        scr_BDUpdate.f_SetTimeOrbs(scr_StatsPlayer.id, false);
        scr_BDUpdate.f_SetNeutrinos(scr_StatsPlayer.id, scr_StatsPlayer.Neutrinos);
        PayToOpenMssage.SetActive(false);
        PE.UpdateCoins();
        */
    }

    private void OnEnable()
    {
        /*
        if (ok_init)
            return;

        VicOrb.gameObject.SetActive(false);
        VicEmpty.SetActive(false);

        bool isempty = true;

        for (int i=2; i<scr_StatsPlayer.OpeningOrbs.Length; i++)
        {
            if (scr_StatsPlayer.OpeningOrbs[i].Type>=0)
            {
                scr_StarOrb _orb = Instantiate(VicOrb, VicContent.transform).GetComponent<scr_StarOrb>();
                _orb.gameObject.SetActive(true);
                _orb.InitOrbe(scr_StatsPlayer.OpeningOrbs[i]);
                isempty = false;
            }
        }

        if (isempty)
            VicEmpty.SetActive(true);

        ok_init = true;
        */
    }
}
