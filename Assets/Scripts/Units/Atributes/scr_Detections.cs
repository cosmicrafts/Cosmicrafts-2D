using UnityEngine;

public class scr_Detections : MonoBehaviour {

    public scr_Unit MyUS;
    public CircleCollider2D RangeView;

    float DelayNewTarget = 0f;

    private void Update()
    {
        if (DelayNewTarget>0f)
        {
            DelayNewTarget -= Time.deltaTime;
            if (DelayNewTarget <= 0f)
                RangeView.enabled = true;
        }
    }

    public void FindeNewTarget()
    {
        RangeView.enabled = false;
        DelayNewTarget = 0.1f;
    }

    public void UpdateCollider()
    {
        if (!MyUS.NS.CanMove && !MyUS.NS.CanShoot)
        {
            Destroy(gameObject);
            return;
        }

        if (MyUS.gameObject.CompareTag("Station") && MyUS.NS.DirectShoot)
            MyUS.NS.Range_View = 30;


        RangeView.radius = MyUS.f_size + MyUS.NS.Range_View;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (MyUS.IsDeath || !MyUS.IsEnable)
            return;

        GameObject g_other = other.gameObject;
        if (g_other.CompareTag("Ship") || g_other.CompareTag("Station"))
        {
            scr_Unit scr_other = g_other.gameObject.GetComponent<scr_Unit>();
            if (scr_other.IsDeath || !scr_other.IsEnable)
                return;

            if (!scr_other.IsMyTeam(MyUS.i_Team))
            {
                MyUS.SetNewTarget(scr_other, false);
            }
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        GameObject g_other = other.gameObject;
        if (MyUS.MyShooter.TargetShoot == g_other && MyUS.gameObject.CompareTag("Station"))
        {
            MyUS.SetNullTarget();
        }
    }
}
