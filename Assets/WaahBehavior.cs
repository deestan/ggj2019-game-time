using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaahBehavior : MonoBehaviour
{
    Vector3 center;
    float prog;

    // Start is called before the first frame update
    void Start()
    {
        center = new Vector3(0, 0.4f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        prog += Time.deltaTime * 5f;
        while (prog > 2 * Mathf.PI)
            prog -= 2 * Mathf.PI;
        Vector3 d = new Vector3(Mathf.Cos(prog) * 0.2f, Mathf.Sin(prog) * 0.1f, 0);
        transform.localPosition = center + d;
    }
}
