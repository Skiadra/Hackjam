using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class SlashScript : MonoBehaviour
{

    float x;
    float y;
    int a;

    void Start()
    {
        x = gameObject.transform.localPosition.x;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)) a = 1;
        else a = 0;
        Debug.Log(gameObject.transform.localPosition.x);
        gameObject.transform.localPosition = new Vector2(x * Movement.move.currentFacingTime * (1 - a), 1 * a);
    }
}
