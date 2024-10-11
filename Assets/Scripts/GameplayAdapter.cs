using System.Collections.Generic;
using UnityEngine;

public class GameplayAdapter : MonoBehaviour
{
    public static GameplayAdapter Instance;
    private GameProcess gameProcess;

    private float GameTime = 0f;

    public List<Vector3> ents = new();

    private void Awake()
    {
        Instance = this;
        gameProcess = GameProcess.Instance;
//        Newgame();
    }

    private void Update()
    {
        if (gameProcess != null && GameTime != 0)
        {
            gameProcess.GameTime(Time.deltaTime * GameTime);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            ents.Clear();
            gameProcess.Entities.ForEach(e => ents.Add(e.Position));
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