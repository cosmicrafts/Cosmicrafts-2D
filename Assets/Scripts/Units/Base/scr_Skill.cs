using UnityEngine;

public class scr_Skill : scr_BaseStats {

    public int IdSkill = 1;
    public float RangeSkill = 0f;
    public GameObject skills;
    GameObject SkillSelected;

    [HideInInspector]
    public Vector2 StartPosition;

    public float f_range = 0f;
    public float f_power = 0f;
    public float f_boost = 0f;

    private void Start()
    {
        InitStatsData();

        int.TryParse(s_IdName.Substring(8), out IdSkill);
        f_range = NS.Range_View;
        f_power = NS.Power;
        f_boost = NS.Boost;

        SkillSelected = skills.transform.GetChild(IdSkill - 1).gameObject;

        if (ImClone)
            return;

        StartPosition = transform.position;
        MyAnimator.SetFloat("CastSpeed", f_CastDelay);
    }

    private void Update()
    {
        UpdateStats();
    }

    public void EndAnimation()
    {
        SkillSelected.SendMessage("EndAnimation");
    }

    public void EndCast()
    {
        if (ImClone)
            return;
        else if (scr_MNGame.GM.b_InNetwork)
            photonView.RPC("EndCastRPC", PhotonTargets.Others, IdSkill, f_range);

        UnitRedy = true;
        SkillSelected.SetActive(true);
        SkillSelected.SendMessage("InitSkill");
        MyAnimator.Play(s_IdName);
    }

    [PunRPC]
    public void EndCastRPC(int _idskill, float _range)
    {
        f_range = _range;
        SkillSelected = skills.transform.GetChild(IdSkill - 1).gameObject;
        SkillSelected.SetActive(true);
        SkillSelected.SendMessage("InitSkill");
        MyAnimator.Play(s_IdName);
    }

    public void DestroyUnit()
    {
        if (ImClone)
            return;

        SkillSelected.SendMessage("EndSkill");

        IsEnable = false;
        if (scr_MNGame.GM.b_InNetwork)
            PhotonNetwork.Destroy(gameObject);
        else
            Destroy(gameObject);
    }

}
