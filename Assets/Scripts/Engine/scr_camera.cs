using System.Collections;
using UnityEngine;

public class scr_camera : MonoBehaviour {

    Camera MyCamera;

    Vector3 so, se;
    //float sw, sh;

    //Vector3 sp;

    //private Vector3 fp;   
    //private Vector3 lp;   
    //private float dragDistance;  

    float orthoZoomSpeed = 0.1f;
    //float moveSpeed = 4f;

    public float maxsize = 17f;
    public float minsize = 7f;

    //Transform PCamera;

    WaitForSeconds DelayShakeCamera = new WaitForSeconds(0.05f);

    // Use this for initialization
    void Start () {
        //PCamera = transform.parent;
        //sp = PCamera.position;
        MyCamera = GetComponent<Camera>();
        //dragDistance = Screen.height * 15 / 100;
        orthoZoomSpeed = scr_StatsPlayer.Op_CameraSen;
        //moveSpeed = scr_StatsPlayer.Op_CameraMov;
        MyCamera.aspect = 1.7778f;
        //MyCamera.aspect
    }

    /*
    // Update is called once per frame
    void Update () {

        
#if UNITY_IPHONE 
        ZoomMobile();
        SwipeMobile();
#endif
#if UNITY_ANDROID 
        ZoomMobile();
        SwipeMobile();
#endif

#if UNITY_STANDALONE

        

        //Debug.Log(MyCamera.aspect);

        sw = Screen.width;
        sh = Screen.height;

        if (Input.GetAxis("Mouse ScrollWheel")<0) // forward
        {
             MyCamera.orthographicSize+=10f * orthoZoomSpeed;
        }
        if (Input.GetAxis("Mouse ScrollWheel") >0) // back
        {
            MyCamera.orthographicSize-= 10f * orthoZoomSpeed;
        }

        so = Camera.main.ScreenToWorldPoint(new Vector3(0 ,0, 0));

        se = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        if (Input.mousePosition.x<sw*0.1f && Input.mousePosition.y > sh * 0.27f && so.x > -3)
        {
            PCamera.Translate(new Vector2(-moveSpeed, 0f) * Time.deltaTime);
        }

        if (Input.mousePosition.x > sw- (sw * 0.1f) && Input.mousePosition.y > sh * 0.27f && se.x<56)
        {
            PCamera.Translate(new Vector2(moveSpeed, 0f) * Time.deltaTime);
        }

        if (Input.mousePosition.y < sh * 0.1f && so.y > -31f)
        {
            PCamera.Translate(new Vector2(0f, -moveSpeed) * Time.deltaTime);
        }

        if (Input.mousePosition.y > sh -(sh* 0.1f) && se.y < 2)
        {
            PCamera.Translate(new Vector2(0f, moveSpeed) * Time.deltaTime);
        }

#endif

        CheckCameraPosition();
        }
        */


void ZoomMobile()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touchOnePrevPos = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            float deltaMagnitudediff = prevTouchDeltaMag - touchDeltaMag;

            MyCamera.orthographicSize += deltaMagnitudediff * orthoZoomSpeed;

        }
    }

    void SwipeMobile()
    {
        /*
        if (Input.touchCount == 1 && !scr_MNGame.GM.b_DragUnit) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            if (touch.position.y > sh * 0.27f && se.x < 56)
                            {
                                PCamera.Translate(new Vector2(-moveSpeed, 0f) * Time.deltaTime);
                            }
                        }
                        else
                        {   //Left swipe
                            if (touch.position.y > sh * 0.27f && so.x > -3)
                            {
                                PCamera.Translate(new Vector2(moveSpeed, 0f) * Time.deltaTime);
                            }
                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            if (so.y > -31f)
                            {
                                PCamera.Translate(new Vector2(0f, moveSpeed) * Time.deltaTime);
                            }
                        }
                        else
                        {   //Down swipe
                            if (se.y < 2)
                            {
                                PCamera.Translate(new Vector2(0f, -moveSpeed) * Time.deltaTime);
                            }
                        }
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list
                //End Slide
            }
        }
        */
    }

    void CheckCameraPosition()
    {
        /*
        if (MyCamera.orthographicSize < minsize)
        {
            MyCamera.orthographicSize = minsize;
        }
        if (MyCamera.orthographicSize >= maxsize)
        {
            MyCamera.orthographicSize = maxsize;
            PCamera.position = sp;
        } else
        {
            while (so.x <= -4)
            {
                PCamera.Translate(new Vector2(1f, 0f));
                so = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            }
            while (se.x >= 57)
            {
                PCamera.Translate(new Vector2(-1f, 0f));
                se = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            }
            while (so.y <= -32f)
            {
                PCamera.Translate(new Vector2(0f, 1f));
                so = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            }
            while (se.y >= 3)
            {
                PCamera.Translate(new Vector2(0f, -1f));
                se = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            }
        }
        */

    }

    IEnumerator Shake()
    {
        while(scr_MNGame.GM.f_CamShakeTime > 0f)
        {
            yield return DelayShakeCamera;
            float intensity = scr_MNGame.GM.f_CamShakeIntensity * scr_StatsPlayer.Op_ShakeCamera;
            transform.localPosition = new Vector2(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));
        }
        transform.localPosition = Vector2.zero;
    }

}
