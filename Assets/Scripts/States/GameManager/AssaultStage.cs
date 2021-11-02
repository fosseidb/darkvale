using UnityEngine;

public class AssaultStage : IState
{
    private GameManager _gameManager;

    [SerializeField] private float stage4DurationInMinutes = 15f;

    public AssaultStage(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void OnEnter()
    {
        //run enter code

        //Set start time
        _gameManager.StageTimer = stage4DurationInMinutes * 60f;

        // Notify UI
        _gameManager.InvokeNewStage(4);

        // setting map scenery to stage 4
        _gameManager.SetLiveStage(4);
    }

    public void OnExit()
    {
        //Game Over!!

        _gameManager.SetPostStage(4);
    }

    public void Tick()
    {
        _gameManager.StageTimer -= Time.deltaTime;
        _gameManager.MasterTimer += Time.deltaTime;
        _gameManager.UpdateUITimers(4);
    }
}
