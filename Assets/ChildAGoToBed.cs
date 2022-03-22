using UnityEngine;

class ChildAGoToBed : MonoBehaviour
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
        objective.SetTitle("Stripes go to bed");
    }

    void Update()
    {
        if (objective.IsCompleted())
            return;
        if (IsInPosition() && game.clock.BedTime() && game.childA.GetComponent<Child>().GetDidBrush() && !game.childA.GetComponent<Child>().IsUnhappy())
            timeLeft -= Time.deltaTime;
        else
            timeLeft += Time.deltaTime * 2f;
        if (timeLeft > timeTotal)
            timeLeft = timeTotal;
        objective.SetCompletion((timeTotal - timeLeft) / timeTotal);
        if (timeLeft <= 0)
        {
            objective.Complete();
            game.childA.GetComponent<Child>().WentToSleep();
            game.ObjectiveCompleted();
        }
    }


    private bool IsInPosition()
    {
        if (game.childA.IsMoving())
            return false;
        if (!game.childA.GetGridPos().Equals(game.childA.GetComponent<Child>().bedPosition))
            return false;
        return true;
    }
}
