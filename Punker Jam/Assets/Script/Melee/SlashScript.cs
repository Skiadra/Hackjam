using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class SlashScript : MonoBehaviour
{

    float xMulti;
    float yMulti;
    float x;
    ParticleSystem ps;

    void Start()
    {
        x = gameObject.transform.localPosition.x;
        ps = gameObject.GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        xMulti = Movement.move.currentFacingTime;
        yMulti = Movement.move.upSlash;

        if (yMulti > 0)
        {
            if (!ps.isPlaying)
            {
                ps.transform.localRotation = Quaternion.Euler(0, 0, 270);
            }
        }
        else if (xMulti < 0)
        {
            if (!ps.isPlaying)
            {
                ps.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (xMulti > 0)
        {
            if (!ps.isPlaying)
            {
                ps.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }

        if (!ps.isPlaying)
            gameObject.transform.localPosition = new Vector2(x * xMulti * (1 - yMulti), 1.3f * yMulti);
    }
}
