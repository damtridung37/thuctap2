using D;
using UnityEngine;

namespace WFC
{
    [CreateAssetMenu(fileName = "RoomData", menuName = "WFC/Room")]
    public class RoomData : ScriptableObject
    {
        [Header("Room Info")]
        public string roomName;
        public Room prefab;
        
        /// <summary>
        /// Example of a 2x2 room with L shape layout:
        /// [1] [0]
        /// [1] [1]
        /// </summary>
        [Header("Room Layout")]
        public Vector2Int size = Vector2Int.one;
        public bool[] layout;
        
        [SerializeField] private EdgeType topEdge;
        [SerializeField] private EdgeType bottomEdge;
        [SerializeField] private EdgeType leftEdge;
        [SerializeField] private EdgeType rightEdge;
        
        public EdgeType GetEdgeType(Direction direction)
        {
            switch (direction)
            {
                case Direction.Top:
                    return topEdge;
                case Direction.Bottom:
                    return bottomEdge;
                case Direction.Left:
                    return leftEdge;
                case Direction.Right:
                    return rightEdge;
                default:
                    return EdgeType.Empty;
            }
        }
        
        [Header("Room Probability")]
        [Tooltip("Probability of this room being selected")]
        public float probability = 1.0f;
    }
    public enum Direction
    {
        Top,
        Bottom,
        Left,
        Right
    }
}

