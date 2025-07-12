using UnityEngine;
using UnityEngine.UI;

public class scr_UIdmg : MonoBehaviour {

    float vspeed = 0f;
    public float hspeed = 0f;

    public bool gravity = true;

    public Text txt_dmg;
    public int dmg;

    float scale = 2f;

    // Use this for initialization
    void Start () {
        Destroy(gameObject, 0.5f);
        scale = 3f;
        if (dmg>0)
        {
            txt_dmg.text = dmg.ToString();
            scale = 1f + (dmg * 0.01f);
            if (scale > 4f) { scale = 4f; }
        }
        hspeed = Random.Range(-scale, scale);
        vspeed = Random.Range(scale * 1.5f, scale * 2.5f);
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector2(hspeed,vspeed)*Time.deltaTime);

        scale -= Time.deltaTime*2f;

        transform.localScale = new Vector2(scale, scale);

        if (gravity)
            vspeed -= 0.098f;
	}
}
