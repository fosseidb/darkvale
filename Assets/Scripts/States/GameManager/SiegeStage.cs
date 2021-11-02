using UnityEngine;

public class SiegeStage : IState
{
    private GameManager _gameManager;

    public float stage3DurationInMinutes = 15f;

    public SiegeStage(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void OnEnter()
    {
        //run enter code

        //Set start time
        _gameManager.StageTimer = stage3DurationInMinutes * 60f;

        // Notify UI
        _gameManager.InvokeNewStage(3);

        // setting map scenery to stage 1
        _gameManager.SetLiveStage(3);
    }

    public void OnExit()
    {
        _gameManager.SetPostStage(3);
    }

    public void Tick()
    {
        _gameManager.StageTimer -= Time.deltaTime;
        _gameManager.MasterTimer += Time.deltaTime;
        _gameManager.UpdateUITimers(3);
    }
}
