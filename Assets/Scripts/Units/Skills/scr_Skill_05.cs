using UnityEngine;

public class scr_Skill_05 : Photon.MonoBehaviour {

    public LineRenderer lineRenderer;

    public scr_Skill MySS;

    public GameObject Parent;

    public BoxCollider2D LaserColider;

    float DelayDmg = 0f;

    void InitSkill() //Laser Gun
    {
        if (!MySS.ImClone)            
            LaserColider.enabled = true;

        if (MySS.i_Team == 0)
        {
            Parent.transform.position = scr_MNGame.GM.Station0.transform.position;
        }
        else
        {
            Parent.transform.position = scr_MNGame.GM.Station1.transform.position;
        }

        if (MySS.ImClone)
            return;

        Vector2 sv = Parent.GetComponent<scr_Skill>().StartPosition;

        if (scr_MNGame.GM.b_InNetwork)
        {
            photonView.RPC("SyncLaserRPC", PhotonTargets.AllBuffered, sv.x, sv.y);
        } else
        {   
            SetDirLaser(sv.x,sv.y);
        }
    }

    private void Update()
    {
        if (DelayDmg > 0f)
            DelayDmg -= Time.deltaTime;
        else
            DelayDmg = 0.5f;
    }

    [PunRPC]
    public void SyncLaserRPC(float _x, float _y)
    {
        SetDirLaser(_x,_y);
    }

    void EndSkill()
    {

    }

    void EndAnimation()
    {

    }

    void SetDirLaser(float _x, float _y)
    {
        Vector2 FinalLaser = new Vector2(_x, _y);
        _x = _x - Parent.transform.position.x;
        _y = _y - Parent.transform.position.y;

        float angle = Mathf.Atan2(_y, _x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        //Vector3 direction = (Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.right);

        FinalLaser = Parent.transform.InverseTransformPoint(FinalLaser);
        FinalLaser *= 12f;
        FinalLaser = Parent.transform.TransformPoint(FinalLaser);

        lineRenderer.SetPosition(0, Parent.transform.position);
        lineRenderer.SetPosition(1, FinalLaser);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ship") || other.CompareTag("Station"))
        {
            scr_Unit scr_other = other.gameObject.GetComponent<scr_Unit>();
            if (!MySS.IsMyTeam(scr_other.i_Team))
            {
                if (DelayDmg > 0f)
                {
                    scr_other.LEA = MySS;
                    scr_other.AddDamage(Mathf.Ceil(MySS.f_power * 0.5f), true);
                }
                if (other.CompareTag("Ship"))
                {
                    other.transform.Translate(new Vector3(0f, Time.deltaTime * 1f, 0f));
                }        
            }
        }
    }

}
