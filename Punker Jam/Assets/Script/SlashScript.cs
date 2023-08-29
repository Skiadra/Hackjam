using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class SlashScript : MonoBehaviour
{

    float x;

    void Start()
    {
        x = gameObject.transform.localPosition.x;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameObject.transform.localPosition.x);
        gameObject.transform.localPosition = new Vector2(x * Movement.move.facing, 0);
    }
}
