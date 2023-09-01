using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttackScript : MonoBehaviour
{
    // Update is called once per frame
    ParticleSystem ps;
    float x;

    void Start()
    {
        x = gameObject.transform.localPosition.x;
        ps = gameObject.GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if (Movement.move.upSlash > 0)
        {
            if (!ps.isPlaying)
            {
                transform.localPosition = new Vector3(0.9f, 7, 0);
                transform.localRotation = Quaternion.Euler(0, 0, 90);
                transform.localScale = new Vector3(1, 10, 1);
            }
        }
        else if (Movement.move.currentFacingTime < 0)
        {
            if (!ps.isPlaying)
            {
                transform.localPosition = new Vector3(-5.5f, 0, 0);
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                transform.localScale = new Vector3(10, 1, 1);
            }
        }
        else if (Movement.move.currentFacingTime > 0)
        {
            if (!ps.isPlaying)
            {
                transform.localPosition = new Vector3(5.5f, 0, 0);
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                transform.localScale = new Vector3(10, 1, 1);
            }
        }
    }
}
