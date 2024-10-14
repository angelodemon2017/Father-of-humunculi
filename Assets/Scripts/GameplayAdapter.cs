using System.Collections.Generic;
using UnityEngine;

public class GameplayAdapter : MonoBehaviour
{
    public static GameplayAdapter Instance;

    [SerializeField] private LayerMask _mask;
    private RaycastHit hit;
    private GameProcess gameProcess;
    private float GameTime = 0f;

    private List<Vector3> ents = new();

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

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, _mask))
            {
                Debug.Log($"Click by{hit.transform.name}");
                if (hit.transform.TryGetComponent(out BasePlaneWorld comp))
                {
                    comp.ChangeTextureRandom();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log($"Check click by {hit.transform.name}");
            }
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