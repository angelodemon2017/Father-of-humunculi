using UnityEngine;

public class GameplayAdapter : MonoBehaviour
{
    public static GameplayAdapter Instance;

    [SerializeField] private LayerMask _mask;
    private RaycastHit hit;
    //[SerializeField] 
    private GameProcess gameProcess;
    private float GameTime = 0f;

    public bool ENABLECAPACITYTEST;
    public int TESTENTITYCOUNT;
    public int TESTCOMPONENTS;

    private void Awake()
    {
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