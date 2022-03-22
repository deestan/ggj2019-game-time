using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushBehavior : MonoBehaviour
{
    Vector3 center;
    float prog;

    // Start is called before the first frame update
    void Start()
    {
        center = new Vector3(0, 0.75f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        prog += Time.deltaTime * 5;
        while (prog > 2)
            prog -= 2;
        Vector3 d = new Vector3(prog < 1 ? -0.2f : 0.2f, 0, 0);
        transform.localPosition = center + d;
    }
}
