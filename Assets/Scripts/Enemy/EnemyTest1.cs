using Aoiti.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyTest1 : Entity
{
    public GameObject player;
    public float currentTimeBetweenSteps = 0f;
    [SerializeField] private SkillList skills;

    Vector3Int[] directions = new Vector3Int[8] {Vector3Int.left,Vector3Int.right,Vector3Int.up,Vector3Int.down, Vector3Int.right+ Vector3Int.up, Vector3Int.right + Vector3Int.down
    , Vector3Int.left + Vector3Int.up,Vector3Int.left + Vector3Int.down };//add diagonal move

    public Tilemap tilemap;
    public TileAndMovementCost[] tiles;
    Pathfinder<Vector3Int> pathfinder;

    [System.Serializable]
    public struct TileAndMovementCost
    {
        public Tile tile;
        public bool movable;
        public float movementCost;
    }

    public List<Vector3Int> path;
    [Range(0.001f, 1f)]
    public float stepTime;

    public float DistanceFunc(Vector3Int a, Vector3Int b)
    {
        return (a - b).sqrMagnitude;
    }

    public Dictionary<Vector3Int, float> connectionsAndCosts(Vector3Int a)
    {
        Dictionary<Vector3Int, float> result = new Dictionary<Vector3Int, float>();
        foreach (Vector3Int dir in directions)
        {
            foreach (TileAndMovementCost tmc in tiles)
            {
                if (tilemap.GetTile(a + dir) == tmc.tile)
                {
                    if (tmc.movable) result.Add(a + dir, tmc.movementCost);

                }
            }

        }
        return result;
    }

    void Start()
    {
        pathfinder = new Pathfinder<Vector3Int>(DistanceFunc, connectionsAndCosts);
    }

    void Update()
    {
        currentTimeBetweenSteps -= Time.deltaTime;
            var currentCellPos = tilemap.WorldToCell(transform.position);
            var target = tilemap.WorldToCell(player.transform.position);
            target.z = 0;
            pathfinder.GenerateAstarPath(currentCellPos, target, out path);
        if(path.Count ==1&&turn)
        {
            skills.Attack(player.GetComponent<Entity>());
            ChangeTurn();
            return;
        }
        if (currentTimeBetweenSteps <= 0)
        {
            Move2();
        }

    }

    public void Move2()
    {
        if (path.Count > 0 && turn)
        {
            transform.position = tilemap.CellToWorld(path[0]);
            path.RemoveAt(0);
            currentTimeBetweenSteps = stepTime;
            ChangeTurn();
            gameObject.transform.Translate(0.5f, 0.5f, 0);
        }
    }

}
