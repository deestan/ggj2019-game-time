using UnityEngine;

class ChildBGoToBed : MonoBehaviour
{
    private Objective objective;
    private Game game;
    private float timeTotal = 15.0f;
    private float timeLeft;

    void Start()
    {
        timeLeft = timeTotal;
        game = FindObjectOfType<Game>();
        objective = GetComponent<Objective>();
        objective.SetTitle("Captain go to bed");
    }

    void Update()
    {
        if (objective.IsCompleted())
            return;
        if (IsInPosition() && game.clock.BedTime() && game.childB.GetComponent<Child>().GetDidBrush() && !game.childB.GetComponent<Child>().IsUnhappy())
            timeLeft -= Time.deltaTime;
        else
            timeLeft += Time.deltaTime * 2f;
        if (timeLeft > timeTotal)
            timeLeft = timeTotal;
        objective.SetCompletion((timeTotal - timeLeft) / timeTotal);
        if (timeLeft <= 0)
        {
            objective.Complete();
            game.childB.GetComponent<Child>().WentToSleep();
            game.ObjectiveCompleted();
        }
    }


    private bool IsInPosition()
    {
        if (game.childB.IsMoving())
            return false;
        if (!game.childB.GetGridPos().Equals(game.childB.GetComponent<Child>().bedPosition))
            return false;
        return true;
    }
}
