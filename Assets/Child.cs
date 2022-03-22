using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour
{
    public Vector3Int bedPosition;
    private Agent agent;
    public Transform player;
    public bool isA;
    public GameObject brushText;
    public GameObject sleepText;
    public GameObject unhappyText;

    private Game game;
    private bool isUnhappy;
    private float patience = 0;
    private float reactTimeout = 0;
    private bool catHuntMode = true;
    private bool isAsleep = false;

    private bool didBrushTeeth = false;
    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<Game>();
        agent = GetComponent<Agent>();
    }

    public void Unhappy()
    {
        if (isAsleep)
            return;
        isUnhappy = true;
        unhappyText.SetActive(true);
        agent.SetTurbo(true);
    }

    public void WentToSleep()
    {
        sleepText.SetActive(true);
        isAsleep = true;
    }

    public bool GetDidBrush()
    {
        return didBrushTeeth;
    }

    public bool IsUnhappy()
    {
        return isUnhappy;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAsleep)
            return;
        if (reactTimeout > 0)
        {
            reactTimeout -= Time.deltaTime;
            return;
        }
        if (Mathf.Abs((transform.position - player.position).magnitude) < 1.2)
        {
            patience = Random.Range(4.0f, 9.0f);
            agent.Target(nextGoal());
            reactTimeout = 1f;
            catHuntMode = false;
            isUnhappy = false;
            unhappyText.SetActive(false);
            agent.SetTurbo(false);
        }
        if (isUnhappy)
            return;
        if (agent.IsMoving())
            patience -= Time.deltaTime;
        else
            patience -= Time.deltaTime * 0.3f;
        if (!agent.IsMoving())
        {
            if (patience < 0)
            {
                agent.SetTurbo(true);
                catHuntMode = true;
            }
            if (catHuntMode) {
                Vector3Int catPos = FindObjectOfType<Cat>().GetComponent<Agent>().GetGridPos();
                agent.Target(catPos);
                reactTimeout = 1f;
            }
        }
    }

    public void DidBrush()
    {
        didBrushTeeth = true;
        patience = 0;
        if (!game.clock.BedTime())
            return;
        if (isA)
            FindObjectOfType<ChildAGoToBed>().GetComponent<Objective>().CanComplete();
        else
            FindObjectOfType<ChildBGoToBed>().GetComponent<Objective>().CanComplete();
    }

    public void DidBecomeBedtime()
    {
        if (!didBrushTeeth)
            return;
        DidBrush();
    }

    public Vector3Int nextGoal()
    {
        if (!didBrushTeeth)
            return FindObjectOfType<Game>().brushTeethPosition;
        if (game.clock.BedTime())
            return bedPosition;
        if (isA)
        {
            return game.tvPosition;
        } else
        {
            return game.tvPosition2;
        }
    }
}
