using UnityEngine;

public class scr_AreaSelect : MonoBehaviour {

    float _x = 0f;
    float _y = 0f;

    bool one_selected = false;

    public SpriteRenderer MySprite;

    public BoxCollider2D Collider;
	
	// Update is called once per frame
	void Update () {

        if (Collider.enabled)
            return;

        if (!MySprite.enabled)
        {
            if (transform.localScale.magnitude > 2f)
            {
                MySprite.enabled = true;
                scr_MNGame.GM.In_Ships_Selection = true;
            }
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _x = mouse.x - transform.position.x;
            _y = transform.position.y - mouse.y;
            transform.localScale = new Vector2(_x, _y);
        } else
        {
            if (MySprite.enabled)
                Collider.enabled = true;
            Destroy(gameObject,0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ship"))
        {
            scr_Unit _unit = other.GetComponent<scr_Unit>();
            if (_unit.IsMyTeam(scr_MNGame.GM.TeamInGame))
            {
                if (!one_selected)
                {
                    scr_MNGame.GM.ClearSelected();
                    one_selected = true;
                }
                scr_MNGame.GM.AddSelection(_unit);
                _unit.RedySelect = true;
            }
        }
    }

    private void OnDestroy()
    {
        if (scr_MNGame.GM)
            scr_MNGame.GM.In_Ships_Selection = false;
    }
}
