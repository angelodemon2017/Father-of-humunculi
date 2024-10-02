using UnityEngine;

public class GameplayAdapter : MonoBehaviour
{
    private GameProcess gameProcess;

    private float GameTime = 0f;

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

        gameProcess = new GameProcess(world);
        GameTime = 1f;
    }
}