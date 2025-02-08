using System.Collections.Generic;
using UnityEngine;

namespace WFC
{
    public class GridCell
    {
        public List<RoomData> possibleRooms; // Rooms that can still be placed here
        public RoomData collapsedRoom; // The final chosen room
        public Vector2Int position; // Grid position

        public bool IsCollapsed => collapsedRoom != null;

        public GridCell(Vector2Int pos, List<RoomData> allRooms)
        {
            position = pos;
            possibleRooms = new List<RoomData>(allRooms); // Initially, all rooms are possible
        }
    }
}
