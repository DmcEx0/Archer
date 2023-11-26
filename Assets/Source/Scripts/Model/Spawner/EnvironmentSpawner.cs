using MTAssets.EasyMeshCombiner;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    [SerializeField] private RuntimeMeshCombiner _meshCombiner;

    [Space]
    [SerializeField] private Transform _parentTree;
    [SerializeField] private Transform _parentHouse;
    [SerializeField] private Transform _parentEnvironment;

    [Space]
    [SerializeField] private List<GameObject> _listTrees;
    [SerializeField] private List<GameObject> _listHouse;
    [SerializeField] private List<GameObject> _listEnvironment;

    private Transform[] _treeSpawmPoints;
    private Transform[] _houseSpawmPoints;
    private Transform[] _environmentSpawmPoints;

    private void Start()
    {
        int bionIndex = Random.Range(0, 3);

        Spawn(ref _treeSpawmPoints, _listTrees, _parentTree);
        Spawn(ref _houseSpawmPoints, _listHouse, _parentHouse);
        Spawn(ref _environmentSpawmPoints, _listEnvironment, _parentEnvironment);

        _meshCombiner.CombineMeshes();
    }

    private void FindSpawnPoints(ref Transform[] spawnPoints, Transform parent)
    {
        spawnPoints = new Transform[parent.childCount];

        for (int i = 0; i < parent.childCount; i++)
        {
            spawnPoints[i] = parent.GetChild(i);
        }
    }

    private void Spawn(ref Transform[] spawnPoints, List<GameObject> environments, Transform parent)
    {
        FindSpawnPoints(ref spawnPoints, parent);

        foreach (Transform spawnPoint in spawnPoints)
        {
            Instantiate(environments[Random.Range(0, environments.Count)], spawnPoint.position, spawnPoint.rotation, spawnPoint);
        }
    }
}