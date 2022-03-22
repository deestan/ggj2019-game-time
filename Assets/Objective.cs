using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    private bool isCanComplete = false;
    private bool isCompleted;
    private string title;
    private float deathTime = 0;
    public Game game;
    private Transform backplate;
    private Transform completionPlate;

    public bool IsCompleted() {
        return isCompleted;
    }
    public void Complete()
    {
        isCompleted = true;
        completionPlate.GetComponent<RectTransform>().localScale = new Vector3(0, 1);
        backplate.GetComponent<Image>().color = new Color(0.9515274f, 0.9716981f, 0.4354308f);
        backplate.GetComponent<Image>().CrossFadeAlpha(0, 1, false);
        deathTime = Time.time + 1;
    }

    public void CanComplete()
    {
        isCanComplete = true;
        backplate.GetComponent<Image>().color = new Color(0.2369616f, 0.7075472f, 0.4835254f);
    }

    public bool IsCanComplete()
    {
        return isCanComplete;
    }

    void Start()
    {
        backplate = transform.Find("Backplate");
        completionPlate = transform.Find("CompletionPlate");
        backplate.GetComponent<Image>().color = Color.grey;
    }

    void Update()
    {
        if (deathTime == 0)
            return;
        if (Time.time < deathTime)
            return;
        deathTime = 0;
        game.RemoveObjective(this);
    }

    public void SetCompletion(float val)
    {
        if (isCompleted)
            return;
        if (val < 0)
            val = 0;
        if (val > 1.0f)
            val = 1.0f;
        completionPlate.GetComponent<RectTransform>().localScale = new Vector3(val, 1);
    }

    public void SetTitle(string t)
    {
        title = t;
        GetComponentInChildren<Text>().text = t;
    }

    public string GetTitle()
    {
        return title;
    }
}
