using UnityEngine;

public class CemetaryStage : IState
{
    private GameManager _gameManager;

    [SerializeField] private float stage1DurationInMinutes = 15f;

    public CemetaryStage(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
  
    public void OnEnter()
    {
        //run enter code

        //Set start time
        _gameManager.StageTimer = stage1DurationInMinutes * 60f;
        _gameManager.MasterTimer = 0f;

        // Notify UI
        _gameManager.InvokeNewStage(1);

        // setting map scenery to stage 1
        _gameManager.SetLiveStage(1);
    }

    public void OnExit()
    {
        //close down spawner


        //set cemetery static set to corrupt state.
        _gameManager.SetPostStage(1);
    }

    public void Tick()
    {
        _gameManager.StageTimer -= Time.deltaTime;
        _gameManager.MasterTimer += Time.deltaTime;
        _gameManager.UpdateUITimers(1);
    }
}
