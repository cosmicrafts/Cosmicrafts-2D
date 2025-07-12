using UnityEngine;

public class scr_Skill_07 : Photon.MonoBehaviour {

    public scr_Skill MySS; //Inmortal
    public CircleCollider2D Range;

    scr_Unit Affected;

    void InitSkill()
    {
        if (MySS.ImClone)
            return;

        Range.enabled = true;
        Range.radius = MySS.f_range;
    }

    void EndSkill()
    {
        if (MySS.ImClone)
            return;

        if (Affected)
        {
            Affected.SetInmortal(false);
        }
            
    }

    void EndAnimation()
    {
        Range.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Affected)
            return;

        if (other.CompareTag("Ship") || other.CompareTag("Station"))
        {
            scr_Unit otherscr = other.GetComponent<scr_Unit>();
            if (otherscr.IsMyTeam(MySS.i_Team))
            {
                Affected = otherscr;
                Affected.SetInmortal(true);
                Range.enabled = false;
            }
        }

    }
}
