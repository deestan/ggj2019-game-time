using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepBehavior : MonoBehaviour
{
    Vector3 center;
    float prog;

    // Start is called before the first frame update
    void Start()
    {
        center = new Vector3(0.1f, 0.6f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        prog += Time.deltaTime;
        while (prog > 0.5f)
        {
            prog -= 0.5f;
            Vector2 d = Random.insideUnitCircle;
            d.Scale(new Vector2(0.3f, 0.2f));
            transform.localPosition = center + new Vector3(d.x, d.y);
        }
    }
}
