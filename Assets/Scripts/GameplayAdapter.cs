using UnityEngine;

public class GameplayAdapter : MonoBehaviour
{
    private GameProcess gameProcess;

    private float GameTime = 0f;

    private void Awake()
    {
        gameProcess = GameProcess.Instance;
    }

    private void Update()
    {
        if (gameProcess != null && GameTime != 0)
        {
            gameProcess.GameTime(Time.deltaTime * GameTime);
        }
    }

    public void Newgame()
    {
        WorldData world = new WorldData();//Load from file...
        gameProcess.NewGame(world);

        GameTime = 1f;
    }

    public void ExitGame()
    {
//        gameProcess.GameWorld.Save();//save to file
        GameTime = 0f;
        gameProcess.StopGame();
    }
}