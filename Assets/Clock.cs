using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public Text textThingy;
    int h = 19;
    int m = 00;
    float partialMinute = 0;
    bool stopped = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool BedTime()
    {
        return h >= 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopped)
            return;
        partialMinute += Time.deltaTime;
        while (partialMinute > 1f)
        {
            partialMinute -= 1f;
            m += 1;
            if (m > 59)
            {
                m = 0;
                h += 1;
                if (h == 20)
                {
                    foreach (Child c in FindObjectsOfType<Child>())
                    {
                        c.DidBecomeBedtime();
                    }
                }
            }
            UpdateTime();
        }
    }

    public int MinutesOfGame()
    {
        int ret = (23 - h) * 60;
        ret += (60 - m);
        if (ret < 0)
            ret = 0;
        return ret;
    }

    public void Stop()
    {
        stopped = true;
    }

    private void UpdateTime()
    {
        if (h > 23)
        {
            textThingy.text = "TIME ??!??";
            return;
        }
        textThingy.text = "TIME " + h + ":" + ((m < 10) ? "0" : "") + m;
    }
}
