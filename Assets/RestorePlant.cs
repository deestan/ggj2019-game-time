using UnityEngine;

class RestorePlant : MonoBehaviour
{
    private Objective objective;
    private Game game;
    private float timeTotal = 1.0f;
    private float timeLeft;
    private Plant plant;
    private Vector3Int plantGridPos;

    void Start()
    {
        timeLeft = timeTotal;
        game = FindObjectOfType<Game>();
        objective = GetComponent<Objective>();
        objective.CanComplete();
        Cat cat = FindObjectOfType<Cat>();
        foreach (Plant p in cat.plants)
        {
            if (Mathf.Abs((p.transform.position - cat.transform.position).magnitude) < 1f)
                plant = p;
        }
        if (plant.IsTrashcan)
            objective.SetTitle("Pick up trashcan");
        else
            objective.SetTitle("Rescue plant");
        plantGridPos = plant.gridPos;
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
            plant.Restore();
            game.ObjectiveCompleted();
        }
    }


    private bool IsInPosition()
    {
        if (game.player.IsMoving())
            return false;
        if (Mathf.Abs((plant.transform.position - game.player.transform.position).magnitude) > .5)
            return false;
        return true;
    }
}
