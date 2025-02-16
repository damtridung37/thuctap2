using System.Collections;
using System.Collections.Generic;
using D;
using UnityEngine;

namespace WFC
{
    public class WFCGrid : MonoBehaviour
    {
        public Vector2Int gridSize = new Vector2Int(3, 3);
        public RoomLibrary roomLibrary;
        public float delay = 0.1f;
        public int roomSize = 1;
        private GridCell[,] grid;
        private Queue<GridCell> expansionQueue = new Queue<GridCell>(); // Queue for BFS expansion

        private Room firstRoom;
        private Room lastRoom;
        private List<Room> rooms = new List<Room>();

        public Vector3 MapCenter => new Vector3((gridSize.x - 1) / 2f, (gridSize.y - 1) / 2f) * roomSize;
        public Vector2Int MapSize => gridSize * roomSize;

        [SerializeField] private float shopAppearChance = 0.2f;

        public IEnumerator Init(int size = 3)
        {
            ClearGrid();
            gridSize = new Vector2Int(size, size);
            yield return StartCoroutine(nameof(InitializeGrid));
            yield return StartCoroutine(nameof(RunWaveFunctionCollapse));
        }

        private void ClearGrid()
        {
            PrefabManager.Instance.ClearEnemy();
            foreach (var room in rooms)
            {
                Destroy(room.gameObject);
            }
            rooms.Clear();
        }

        IEnumerator InitializeGrid()
        {
            grid = new GridCell[gridSize.x, gridSize.y];
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    grid[x, y] = new GridCell(new Vector2Int(x, y), roomLibrary.rooms);
                    yield return null;

                    // Remove invalid rooms based on edges
                    Vector2Int pos = grid[x, y].position;
                    grid[x, y].possibleRooms.RemoveAll(r =>
                        (pos.x == 0 && r.GetEdgeType(Direction.Left) == EdgeType.Door) ||
                        (pos.y == 0 && r.GetEdgeType(Direction.Bottom) == EdgeType.Door) ||
                        (pos.x == gridSize.x - 1 && r.GetEdgeType(Direction.Right) == EdgeType.Door) ||
                        (pos.y == gridSize.y - 1 && r.GetEdgeType(Direction.Top) == EdgeType.Door)
                    );
                }
            }
        }

        IEnumerator RunWaveFunctionCollapse()
        {
            Debug.Log("Running Wave Function Collapse...");

            // 1️⃣ First time: Choose a random cell and collapse it (ensure it's not empty)
            Vector2Int startPos = new Vector2Int(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y));
            GridCell startCell = grid[startPos.x, startPos.y];
            CollapseCell(startCell);
            StoreConnectedNeighbors(startCell);

            yield return new WaitForSeconds(delay);

            // 2️⃣ Process cells from the queue
            while (expansionQueue.Count > 0)
            {
                GridCell currentCell = expansionQueue.Dequeue();
                if (!currentCell.IsCollapsed)
                {
                    CollapseCell(currentCell);
                    StoreConnectedNeighbors(currentCell);
                }
                yield return new WaitForSeconds(delay);
            }

            // 3️⃣ Collapse all remaining uncollapsed cells to empty
            // foreach (var cell in grid)
            // {
            //     if (!cell.IsCollapsed)
            //     {
            //         CollapseAsEmpty(cell);
            //     }
            // }

            yield return null;

            firstRoom = rooms[0];
            lastRoom = rooms[^1];

            firstRoom.Init(Room.RoomType.Entrance);
            lastRoom.Init(Room.RoomType.Portal);

            bool shopAppeared = (Random.value < shopAppearChance);

            for (int i = 1; i < rooms.Count - 1; i++)
            {
                Room room = rooms[i];
                if (room.doorCount < 3 && shopAppeared)
                {
                    room.Init(Room.RoomType.Shop);
                    shopAppeared = false;
                    continue;
                }
                room.Init(Room.RoomType.Arena);
            }

            D.Player.Instance.transform.position = new Vector3(firstRoom.transform.position.x, firstRoom.transform.position.y, -10);
        }

        void StoreConnectedNeighbors(GridCell cell)
        {
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            foreach (var dir in directions)
            {
                Vector2Int neighborPos = cell.position + dir;
                if (!IsValidPosition(neighborPos)) continue;

                GridCell neighbor = grid[neighborPos.x, neighborPos.y];
                if (neighbor.IsCollapsed) continue;
                // Filter neighbor's options based on this cell's room
                neighbor.possibleRooms.RemoveAll(r => !IsCompatible(cell.collapsedRoom, r, dir));

                if (AreRoomsConnectedByDoor(cell, neighbor, dir))
                {
                    expansionQueue.Enqueue(neighbor);
                }
            }
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

        bool AreRoomsConnectedByDoor(GridCell cell, GridCell neighbor, Vector2Int direction)
        {
            if (direction == Vector2Int.up)
                return cell.collapsedRoom.GetEdgeType(Direction.Top) == EdgeType.Door;
            if (direction == Vector2Int.down)
                return cell.collapsedRoom.GetEdgeType(Direction.Bottom) == EdgeType.Door;
            if (direction == Vector2Int.left)
                return cell.collapsedRoom.GetEdgeType(Direction.Left) == EdgeType.Door;
            if (direction == Vector2Int.right)
                return cell.collapsedRoom.GetEdgeType(Direction.Right) == EdgeType.Door;

            return false;
        }

        void CollapseCell(GridCell cell)
        {
            if (cell.IsCollapsed) return;

            // Choose a random room based on probability
            float totalWeight = 0;
            foreach (var room in cell.possibleRooms)
                totalWeight += room.probability;

            float randomWeight = Random.Range(0, totalWeight);
            float currentWeight = 0;
            foreach (var room in cell.possibleRooms)
            {
                currentWeight += room.probability;
                if (randomWeight <= currentWeight)
                {
                    cell.collapsedRoom = room;
                    SpawnRoom(room, cell.position);
                    cell.possibleRooms.Clear();
                    cell.possibleRooms.Add(room);
                    break;
                }
            }
        }

        void CollapseAsEmpty(GridCell cell)
        {
            cell.collapsedRoom = roomLibrary.emptyRoom;
            SpawnRoom(cell.collapsedRoom, cell.position, true);
        }

        void SpawnRoom(RoomData room, Vector2Int position, bool isEmptyRoom = false)
        {
            if (room == null || room.prefab == null)
            {
                Debug.LogError($"RoomData or prefab is NULL for cell at {position}!");
                return;
            }

            if (!isEmptyRoom)
            {
                Vector3 worldPos = new Vector3(position.x, position.y, 0) * roomSize;
                Room temp = Instantiate(room.prefab, worldPos, Quaternion.identity);
                temp.gameObject.SetActive(true);

                rooms.Add(temp);
            }
        }

        bool IsValidPosition(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < gridSize.x && pos.y >= 0 && pos.y < gridSize.y;
        }
    }
}
