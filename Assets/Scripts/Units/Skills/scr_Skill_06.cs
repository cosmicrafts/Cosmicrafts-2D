using UnityEngine;

public class scr_Skill_06 : MonoBehaviour {

    public scr_Skill MySS; //Simulate

    void InitSkill()
    {
        if (MySS.ImClone)
            return;

        if (MySS.i_Team == 0)
        {
            MySS.gameObject.transform.position = scr_MNGame.GM.Station0.transform.position;
        }
        else
        {
            MySS.gameObject.transform.position = scr_MNGame.GM.Station1.transform.position;
        }

        if (MySS.IsMyTeam(scr_MNGame.GM.TeamInGame))
        {
            scr_MNGame.GM.AddResources(MySS.f_power);
        }
    }

    void EndSkill()
    {

    }

    void EndAnimation()
    {
        MySS.DestroyUnit();
    }
}
