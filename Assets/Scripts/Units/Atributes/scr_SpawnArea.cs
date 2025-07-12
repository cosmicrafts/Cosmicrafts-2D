using UnityEngine;

public class scr_SpawnArea : MonoBehaviour {

    public float f_RadioArea = 0f;

    public scr_Unit MyUS;

    public SpriteRenderer spr_ApawnArea;

    public CircleCollider2D Triger_Area;

    // Use this for initialization
    void Start () {
        f_RadioArea = MyUS.NS.SpawnRange;
        
        transform.localScale = new Vector3(f_RadioArea, f_RadioArea, 1f);
    }

    public void EnableSpawnArea(bool enable)
    {
        spr_ApawnArea.enabled = enable;
        Triger_Area.enabled = enable;
    }

}
