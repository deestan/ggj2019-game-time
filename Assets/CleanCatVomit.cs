using UnityEngine;

class CleanCatVomit : MonoBehaviour
{
    private Objective objective;
    private Game game;
    private float timeTotal = 5.0f;
    private float timeLeft;
    private Transform vomit;
    private Vector3Int vomitGridPos;

    void Start()
    {
        timeLeft = timeTotal;
        game = FindObjectOfType<Game>();
        objective = GetComponent<Objective>();
        objective.SetTitle("Clean cat vomit");
        objective.CanComplete();
        Transform cat = FindObjectOfType<Cat>().transform;
        vomit = Instantiate(game.vomitPrototype, game.spriteLayer, true);
        vomit.position = FindObjectOfType<Cat>().transform.position;
        vomitGridPos = FindObjectOfType<Cat>().GetComponent<Agent>().GetGridPos();
    }

    void Update()
    {
        if (objective.IsCompleted())
            return;
        if (IsInPosition())
            timeLeft -= Time.deltaTime;
        else
            timeLeft += Time.deltaTime * 0.05f;
        if (timeLeft > timeTotal)
            timeLeft = timeTotal;
        objective.SetCompletion((timeTotal - timeLeft) / timeTotal);
        if (timeLeft <= 0)
        {
            objective.Complete();
            Destroy(vomit.gameObject);
            game.ObjectiveCompleted();
        }
    }


    private bool IsInPosition()
    {
        if (game.player.IsMoving())
            return false;
        if (Mathf.Abs((vomit.position - game.player.transform.position).magnitude) > 1.5)
            return false;
        return true;
    }
}
