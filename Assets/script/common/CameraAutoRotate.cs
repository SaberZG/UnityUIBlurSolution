using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAutoRotate : MonoBehaviour
{
    float speed = 1.0f;
    Vector2 cur_maincam_angel;
    Vector2 angel_delta;
    void Awake()
    {
        cur_maincam_angel = transform.eulerAngles;
        angel_delta = new Vector2(0, 1.0f) * speed;
    }
    // Update is called once per frame
    void Update()
    {
        cur_maincam_angel += angel_delta;
        transform.rotation = Quaternion.Euler(cur_maincam_angel);
    }
}
