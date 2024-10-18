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

    public bool ENABLECAPACITYTEST;
    public int TESTENTITYCOUNT;
    public int TESTCOMPONENTS;

    private void Awake()
    {
        PrefabsByComponent.isInit = false;
        Instance = this;
        gameProcess = GameProcess.Instance;
    }

    public void TestCapacityENTS()
    {
        if (!ENABLECAPACITYTEST)
            return;
        for (int a = 0; a < TESTENTITYCOUNT; a++)
        {
            EntityData ed = new EntityCapacity(TESTCOMPONENTS);
            gameProcess.Entities.Add(new EntityInProcess(ed));
        }
    }

    private void Update()
    {
        if (gameProcess != null && GameTime != 0)
        {
            gameProcess.GameTime(Time.deltaTime * GameTime);
        }

        MouseWatcher();

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

    private void MouseWatcher()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, _mask))
        {//TODO maybe create component for only typs...
            if (hit.transform.TryGetComponent(out MouseInterfaceInteraction mii))
            {
                mii.ShowTip();
            }
        }
    }

    public void Newgame()
    {
        WorldData world = new WorldData();//Load from file...
        gameProcess.NewGame(world);

        GameTime = 1f;
        TestCapacityENTS();
    }

    public void ExitGame()
    {
//        gameProcess.GameWorld.Save();//save to file
        GameTime = 0f;
        gameProcess.StopGame();
    }
}