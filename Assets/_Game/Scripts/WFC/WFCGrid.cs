using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections;

namespace WFC
{
    public class WFCGrid : MonoBehaviour
    {
        public Vector2Int gridSize = new Vector2Int(3, 3);
        public RoomLibrary roomLibrary; // Reference to all rooms
        private GridCell[,] grid;

        IEnumerator Start()
        {
            yield return StartCoroutine(nameof(InitializeGrid));
            yield return StartCoroutine(nameof(RunWaveFunctionCollapse));
        }

        IEnumerator InitializeGrid()
        {
            grid = new GridCell[gridSize.x, gridSize.y];

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    grid[x, y] = new GridCell(new Vector2Int(x, y), roomLibrary.rooms);
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        
        bool IsOnBorder(Vector2Int pos)
        {
            return pos.x == 0 || pos.x == grid.GetLength(0) - 1 ||
                   pos.y == 0 || pos.y == grid.GetLength(1) - 1;
        }

        
        void CollapseCell(GridCell cell)
        {
            if (cell.IsCollapsed) return;
            if (cell.possibleRooms.Count == 0)
            {
                Debug.LogError($"No possible rooms for cell at {cell.position}!");
                return;
            }
            
            Vector2Int pos = cell.position;
    
            // Remove rooms with doors on edges if on border
            cell.possibleRooms.RemoveAll(r =>
                    (pos.x == 0 && r.GetEdgeType(Direction.Left) == EdgeType.Door) ||  // Left border
                    (pos.y == 0 && r.GetEdgeType(Direction.Bottom) == EdgeType.Door) || // Bottom border
                    (pos.x == gridSize.x - 1 && r.GetEdgeType(Direction.Right) == EdgeType.Door) || // Right border
                    (pos.y == gridSize.y - 1 && r.GetEdgeType(Direction.Top) == EdgeType.Door)  // Top border
            );
            
            // Choose a room randomly based on probability
            float totalWeight = 0;
            foreach (var room in cell.possibleRooms)
            {
                totalWeight += room.probability;
            }

            float randomWeight = Random.Range(0, totalWeight);
            float currentWeight = 0;
            foreach (var room in cell.possibleRooms)
            {
                currentWeight += room.probability;
                if (randomWeight <= currentWeight)
                {
                    cell.collapsedRoom = room;
                    // 🛠 Immediately spawn the room prefab
                    SpawnRoom(room, cell.position);
                    Debug.Log($"Collapsing cell at {cell.position} with room {cell.collapsedRoom.roomName}");

                    cell.possibleRooms.Clear();
                    cell.possibleRooms.Add(room);
                    break;
                }
            }
        }

        void PropagateWave(GridCell cell)
        {
            Debug.Log($"Propagating wave from cell at {cell.position}");
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            foreach (var dir in directions)
            {
                Vector2Int neighborPos = cell.position + dir;
                if (!IsValidPosition(neighborPos)) continue;

                GridCell neighbor = grid[neighborPos.x, neighborPos.y];
                if (neighbor.IsCollapsed) continue;

                // Filter neighbor's options based on this cell's room
                neighbor.possibleRooms.RemoveAll(r => !IsCompatible(cell.collapsedRoom, r, dir));
            }
        }

        IEnumerator RunWaveFunctionCollapse()
        {
            Debug.Log("Running Wave Function Collapse...");
    
            while (HasUncollapsedCells())
            {
                GridCell cell = GetLowestEntropyCell();
                CollapseCell(cell);
                PropagateWave(cell);
                yield return new WaitForEndOfFrame();
            }

            // Validate connectivity and fix if needed
            //EnsureConnectivity();

            // yield return new WaitForSeconds(2f);
            // SpawnRooms();
        }
        
        // void SpawnRooms()
        // {
        //     Debug.Log("Spawning rooms...");
        //     foreach (var cell in grid)
        //     {
        //         if (cell.collapsedRoom != null)
        //         {
        //             Debug.Log($"Spawning room {cell.collapsedRoom.roomName} at {cell.position}");
        //             Instantiate(cell.collapsedRoom.prefab, new Vector3(cell.position.x, cell.position.y, 0f), Quaternion.identity,transform);
        //         }
        //     }
        // }
        
        void SpawnRoom(RoomData room, Vector2Int position)
        {
            if (room == null || room.prefab == null)
            {
                Debug.LogError($"RoomData or prefab is NULL for cell at {position}!");
                return;
            }

            Vector3 worldPos = new Vector3(position.x * 1, position.y * 1, 0);
            Instantiate(room.prefab, worldPos, Quaternion.identity);
        }
        
        bool IsValidPosition(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < gridSize.x && pos.y >= 0 && pos.y < gridSize.y;
        }
        
        bool IsCompatible(RoomData roomA, RoomData roomB, Vector2Int direction)
        {
            if (direction == Vector2Int.up && roomA.GetEdgeType(Direction.Top) == EdgeType.Door && roomB == null) return false;
            if (direction == Vector2Int.down && roomA.GetEdgeType(Direction.Bottom) == EdgeType.Door && roomB == null) return false;
            if (direction == Vector2Int.left && roomA.GetEdgeType(Direction.Left) == EdgeType.Door && roomB == null) return false;
            if (direction == Vector2Int.right && roomA.GetEdgeType(Direction.Right) == EdgeType.Door && roomB == null) return false;

            if (direction == Vector2Int.up) return AreEdgesCompatible(roomA.GetEdgeType(Direction.Top), roomB.GetEdgeType(Direction.Bottom));
            if (direction == Vector2Int.down) return AreEdgesCompatible(roomA.GetEdgeType(Direction.Bottom), roomB.GetEdgeType(Direction.Top));
            if (direction == Vector2Int.left) return AreEdgesCompatible(roomA.GetEdgeType(Direction.Left), roomB.GetEdgeType(Direction.Right));
            if (direction == Vector2Int.right) return AreEdgesCompatible(roomA.GetEdgeType(Direction.Right), roomB.GetEdgeType(Direction.Left));

            return false;
        }


        bool AreEdgesCompatible(EdgeType edgeA, EdgeType edgeB)
        {
            if (edgeA == EdgeType.Door && edgeB == EdgeType.Door) return true;
            if (edgeA == EdgeType.Wall && edgeB == EdgeType.Wall) return true;
            if (edgeA == EdgeType.Empty && edgeB == EdgeType.Wall) return true;
            if (edgeA == EdgeType.Wall && edgeB == EdgeType.Empty) return true;
            // if (edgeA == EdgeType.Door && edgeB == EdgeType.Door) return true;
            // if (edgeA == EdgeType.Wall && edgeB == EdgeType.Wall) return true;
            // if (edgeA == EdgeType.Empty || edgeB == EdgeType.Empty) return false; // 🚨 Prevents doors from connecting to void
    
            return false;
        }
        
        bool HasUncollapsedCells()
        {
            foreach (var cell in grid)
            {
                if (!cell.IsCollapsed)
                    return true;
            }
            return false;
        }
        
        GridCell GetLowestEntropyCell()
        {
            Debug.Log("Getting lowest entropy cell...");
            GridCell bestCell = null;
            int minOptions = int.MaxValue;

            foreach (var cell in grid)
            {
                if (!cell.IsCollapsed && cell.possibleRooms.Count < minOptions)
                {
                    minOptions = cell.possibleRooms.Count;
                    bestCell = cell;
                }
            }

            return bestCell;
        }
    }
}