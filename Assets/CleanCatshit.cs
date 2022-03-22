using UnityEngine;

class CleanCatshit : MonoBehaviour
{
    private Objective objective;
    private Game game;
    private float timeTotal = 10.0f;
    private float timeLeft;

    void Start()
    {
        timeLeft = timeTotal;
        game = FindObjectOfType<Game>();
        objective = GetComponent<Objective>();
        objective.SetTitle("Empty cat toilet");
        objective.CanComplete();
    }

    void Update()
    {
        if (objective.IsCompleted())
            return;
        if (IsInPosition())
            timeLeft -= Time.deltaTime;
        else
            timeLeft += Time.deltaTime * 0.001f;
        if (timeLeft > timeTotal)
            timeLeft = timeTotal;
        objective.SetCompletion((timeTotal - timeLeft) / timeTotal);
        if (timeLeft <= 0)
        {
            objective.Complete();
            game.ObjectiveCompleted();
        }
    }


    private bool IsInPosition()
    {
        if (game.player.IsMoving())
            return false;
        if (!game.player.GetGridPos().Equals(game.catToiletPosition))
            return false;
        return true;
    }
}
