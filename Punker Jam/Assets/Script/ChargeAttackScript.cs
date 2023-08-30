using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttackScript : MonoBehaviour
{
    // Update is called once per frame
    int changeRotation = 0;
    float x;

    void Start()
    {
        x = gameObject.transform.localPosition.x;
    }
    void Update()
    {
        gameObject.transform.localPosition = new Vector2(x * Movement.move.currentFacingTime, 0);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.localPosition = new Vector3(0, 0, 0);
            transform.localRotation = Quaternion.Euler(270, 90, 0);
        }
        else if (transform.localPosition.x < 0) transform.localRotation = Quaternion.Euler(0, 270, 0);
        else if (transform.localPosition.x > 0) transform.localRotation = Quaternion.Euler(0, 90, 0);
    }
}
