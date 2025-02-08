using UnityEngine;

namespace WFC
{
    public class WFCGrid : MonoBehaviour
    {
        public Vector2Int gridSize = new Vector2Int(3, 3);
        public RoomLibrary roomLibrary; // Reference to all rooms
        private GridCell[,] grid;

        void Start()
        {
            InitializeGrid();
            RunWaveFunctionCollapse();
        }

        void InitializeGrid()
        {
            grid = new GridCell[gridSize.x, gridSize.y];
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    grid[x, y] = new GridCell(new Vector2Int(x, y), roomLibrary.rooms);
                }
            }
        }
        
        void CollapseCell(GridCell cell)
        {
            if (cell.IsCollapsed) return;

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
                    cell.possibleRooms.Clear();
                    cell.possibleRooms.Add(room);
                    break;
                }
            }
        }

        void PropagateWave(GridCell cell)
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
            }
        }

        void RunWaveFunctionCollapse()
        {
            while (HasUncollapsedCells())
            {
                GridCell cell = GetLowestEntropyCell();
                CollapseCell(cell);
                PropagateWave(cell);
            }

            SpawnRooms();
        }
        
        void SpawnRooms()
        {
            foreach (var cell in grid)
            {
                if (cell.collapsedRoom != null)
                {
                    Instantiate(cell.collapsedRoom.prefab, new Vector3(cell.position.x, 0, cell.position.y), Quaternion.identity);
                }
            }
        }
        
        bool IsValidPosition(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < gridSize.x && pos.y >= 0 && pos.y < gridSize.y;
        }
        
        bool IsCompatible(RoomData roomA, RoomData roomB, Vector2Int direction)
        {
            if (direction == Vector2Int.up)
                return AreEdgesCompatible(roomA.topEdge, roomB.bottomEdge);
            if (direction == Vector2Int.down)
                return AreEdgesCompatible(roomA.bottomEdge, roomB.topEdge);
            if (direction == Vector2Int.left)
                return AreEdgesCompatible(roomA.leftEdge, roomB.rightEdge);
            if (direction == Vector2Int.right)
                return AreEdgesCompatible(roomA.rightEdge, roomB.leftEdge);

            return false;
        }

        bool AreEdgesCompatible(EdgeType edgeA, EdgeType edgeB)
        {
            if (edgeA == EdgeType.Door && edgeB == EdgeType.Door) return true;
            if (edgeA == EdgeType.Wall && edgeB == EdgeType.Wall) return true;
            if (edgeA == EdgeType.Open || edgeB == EdgeType.Open) return true;
    
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