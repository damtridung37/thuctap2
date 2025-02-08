using UnityEngine;

namespace WFC
{
    [CreateAssetMenu(fileName = "RoomData", menuName = "WFC/Room")]
    public class RoomData : ScriptableObject
    {
        [Header("Room Info")]
        public string roomName;
        public GameObject prefab;
        
        /// <summary>
        /// Example of a 2x2 room with L shape layout:
        /// [1] [0]
        /// [1] [1]
        /// </summary>
        [Header("Room Layout")]
        public Vector2Int size = Vector2Int.one;
        public bool[] layout;
        
        public EdgeType topEdge;
        public EdgeType bottomEdge;
        public EdgeType leftEdge;
        public EdgeType rightEdge;
        
        [Header("Room Probability")]
        [Tooltip("Probability of this room being selected")]
        public float probability = 1.0f;
    }
}
