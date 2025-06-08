using UnityEngine;
using System.Collections.Generic;

public class ChunkManager : MonoBehaviour
{
    public GameObject chunkPrefab;
    public float chunkSize = 10f;
    public float animationSpeed = 2f;
    public LayerMask groundLayer;

    private Vector2Int currentCenter;
    private Dictionary<Vector2Int, Chunk> activeChunks = new Dictionary<Vector2Int, Chunk>();

    void Update()
    {
        Vector2Int mouseChunk = GetMouseChunkCoordinate();
        if (mouseChunk != currentCenter)
        {
            UpdateActiveChunks(mouseChunk);
            currentCenter = mouseChunk;
        }
    }

    Vector2Int GetMouseChunkCoordinate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            return new Vector2Int(
                Mathf.FloorToInt(hit.point.x / chunkSize),
                Mathf.FloorToInt(hit.point.z / chunkSize)
            );
        }
        return currentCenter;
    }

    void UpdateActiveChunks(Vector2Int center)
    {
        HashSet<Vector2Int> required = new();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                required.Add(center + new Vector2Int(x, y));
            }
        }

        List<Vector2Int> toRemove = new();
        foreach (var coord in activeChunks.Keys)
        {
            if (!required.Contains(coord))
            {
                activeChunks[coord].Release();
                toRemove.Add(coord);
            }
        }

        foreach (var coord in toRemove)
        {
            activeChunks.Remove(coord);
        }

        foreach (var coord in required)
        {
            if (!activeChunks.ContainsKey(coord))
            {
                GameObject obj = Instantiate(chunkPrefab, new Vector3(
                    coord.x * chunkSize + chunkSize / 2f,
                    0f,
                    coord.y * chunkSize + chunkSize / 2f
                ), Quaternion.identity);
                Chunk chunk = obj.AddComponent<Chunk>();
                chunk.Initialize(coord, animationSpeed);
                activeChunks.Add(coord, chunk);
            }
        }
    }
}
