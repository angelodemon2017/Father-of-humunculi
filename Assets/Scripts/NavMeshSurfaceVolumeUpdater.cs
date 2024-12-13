using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using static OptimazeExtensions;

/// <summary>
/// Update a NavMeshSurface with Volume object collection
/// </summary>
[DefaultExecutionOrder(-102)]
public class NavMeshSurfaceVolumeUpdater : MonoBehaviour
{
    protected int Uid;
    private NavMeshAgent trackedAgent = new();

    private NavMeshSurface m_Surface;
    private Vector3 m_VolumeSize;

    void Awake()
    {
        m_Surface = GetComponent<NavMeshSurface>();
        Uid = PoolCounter<NavMeshSurfaceVolumeUpdater>.NextUid();
    }

    public void Init(NavMeshAgent agent)
    {
        trackedAgent = agent;
        m_VolumeSize = Vector3.one * Config.ChunkSize;
        m_Surface.agentTypeID = agent.agentTypeID;
        m_Surface.center = QuantizedCenter();
        m_Surface.BuildNavMesh();
    }

    void Update()
    {
        UpdateNavMeshOnCenterOrSizeChange();
    }

    private void UpdateNavMeshOnCenterOrSizeChange()
    {
        var updatedCenter = QuantizedCenter();
        var updateNavMesh = false;

        if (m_Surface.center != updatedCenter)
        {
            m_Surface.center = updatedCenter;
            updateNavMesh = true;
        }

        if (m_Surface.size != m_VolumeSize)
        {
            m_VolumeSize = m_Surface.size;
            updateNavMesh = true;
        }

        if (updateNavMesh)
            m_Surface.UpdateNavMesh(m_Surface.navMeshData);
    }

    static Vector3 Quantize(Vector3 v, Vector3 quant)
    {
        float x = quant.x * Mathf.Floor(v.x / quant.x);
        float y = quant.y * Mathf.Floor(v.y / quant.y);
        float z = quant.z * Mathf.Floor(v.z / quant.z);
        return new Vector3(x, y, z);
    }

    private Vector3 QuantizedCenter()
    {
        if (trackedAgent == null)
        {
            return Vector3.zero;
        }
        // Quantize the center position to update only when there's a 10% change in position relative to size
        var center = trackedAgent.transform.position;
        return Quantize(center, 0.1f * m_Surface.size);
    }

    internal void VirtualCreate()
    {
        gameObject.SetActive(true);
    }

    internal void VirtualDestroy()
    {
        gameObject.SetActive(false);
        trackedAgent = null;
    }

    public override int GetHashCode()
    {
        return Uid;
    }

    public override bool Equals(object obj)
    {
        if (obj is NavMeshSurfaceVolumeUpdater other)
        {
            return Uid == other.Uid;
        }
        return false;
    }
}