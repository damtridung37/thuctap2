using System.Collections.Generic;
using UnityEngine;

namespace WFC
{
    [CreateAssetMenu(fileName = "RoomLibrary", menuName = "WFC/RoomLibrary")]
    public class RoomLibrary : ScriptableObject
    {
        public List<RoomData> rooms;
        public RoomData emptyRoom;

        public List<RoomData> bossRooms; //(floor / 10) - 1 = boss room

        public RoomData GetBossRoom(int floor)
        {
            if (floor % 10 != 0)
            {
                Debug.LogError("Floor is not a boss floor");
                return null;
            }
            return bossRooms[(floor / 10) - 1];
        }
    }
}
