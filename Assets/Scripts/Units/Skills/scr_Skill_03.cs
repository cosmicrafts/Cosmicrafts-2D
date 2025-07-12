using UnityEngine;

public class scr_Skill_03 : Photon.MonoBehaviour {

    public scr_Skill MySS;
    public CircleCollider2D Range;
    scr_Unit Affected;
    //Rogue One

    void InitSkill()
    {
        if (MySS.ImClone)
            return;

        Range.enabled = true;
        Range.radius = MySS.f_range;
    }

    void EndAnimation()
    {
        Range.enabled = false;
        if (!Affected)
            MySS.DestroyUnit();
    }

    void EndSkill()
    {
        if (MySS.ImClone)
            return;

        if (Affected)
            Affected.EndRevelion();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Affected)
            return;

        if (other.CompareTag("Ship") || other.CompareTag("Station"))
        {
            scr_Unit otherscr = other.GetComponent<scr_Unit>();
            if (!otherscr.IsMyTeam(MySS.i_Team))
            {
                Affected = otherscr;
                otherscr.Revelion(MySS.i_Team);
                Range.enabled = false;
            }
        }

    }
}
