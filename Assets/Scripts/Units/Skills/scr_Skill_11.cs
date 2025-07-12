using UnityEngine;

public class scr_Skill_11 : MonoBehaviour {

    public scr_Skill MySS; //BlackNebula

    public CircleCollider2D Range;

    void InitSkill()
    {
        if (MySS.ImClone)
            return;

        Range.enabled = true;
        Range.radius = MySS.f_range;
    }

    void EndSkill()
    {

    }

    void EndAnimation()
    {

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ship") || other.CompareTag("Station"))
        {
            scr_Unit otherscr = other.gameObject.GetComponent<scr_Unit>();
            if (!otherscr.IsMyTeam(MySS.i_Team))
            {
                if (otherscr.MyShooter)
                    otherscr.MyShooter.SetBlind(MySS.f_Duration);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ship") || other.CompareTag("Station"))
        {
            scr_Unit otherscr = other.gameObject.GetComponent<scr_Unit>();
            if (!otherscr.IsMyTeam(MySS.i_Team))
            {
                if (otherscr.MyShooter)
                    otherscr.MyShooter.SetBlind(0f);
            }
        }
    }
}
