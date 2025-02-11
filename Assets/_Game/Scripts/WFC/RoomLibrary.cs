using System.Collections.Generic;
using UnityEngine;

namespace WFC
{
    [CreateAssetMenu(fileName = "RoomLibrary", menuName = "WFC/RoomLibrary")]
    public class RoomLibrary : ScriptableObject
    {
        public List<RoomData> rooms;
        public RoomData emptyRoom;
    }
}
