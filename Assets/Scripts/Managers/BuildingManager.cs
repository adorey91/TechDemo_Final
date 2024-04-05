using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject MazeRangePrefab;
    public GameObject MazeRangeInstance;
    Vector3 MazeRangePosition = new(29, 6, -35);


    internal List<Rigidbody> MazeRangeList = new();

    void Start()
    {
        BuildBuildings();
    }


    /// <summary>
    /// Creates a new instance of the maze/range and then adds the rigidbodies to a list
    /// </summary>
    public void BuildBuildings()
    {
        MazeRangeInstance = Instantiate(MazeRangePrefab, MazeRangePosition, transform.rotation);
        AddRigidbodiesFromChildren(MazeRangeInstance, MazeRangeList);
    }

    internal void AddRigidbodiesFromChildren(GameObject obj, List<Rigidbody> rigidbodyList)
    {
        Rigidbody[] rigidbodies = obj.GetComponentsInChildren<Rigidbody>();
        rigidbodyList.AddRange(rigidbodies);
    }

    internal void ClearLists()
    {
        MazeRangeList.Clear();
    }

    public IEnumerator DestroyBuildings()
    {
        ClearLists();
        Destroy(MazeRangeInstance);
        yield return new WaitForSeconds(0.5f);

        BuildBuildings();
    }
}
