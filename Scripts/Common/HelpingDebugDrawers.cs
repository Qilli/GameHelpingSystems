//K.Homa 02.03.2021
using UnityEngine;
namespace Base.CommonCode
{
    public class HelpingDebugDrawers
    {
        public static void drawRectangle(Vector2 pos, int size, Color color)
        {
            Color last = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawLine(new Vector3(pos.x, pos.y, 0), new Vector3(pos.x, pos.y + size, 0));
            Gizmos.DrawLine(new Vector3(pos.x, pos.y + size, 0), new Vector3(pos.x + size, pos.y + size, 0));
            Gizmos.DrawLine(new Vector3(pos.x + size, pos.y + size, 0), new Vector3(pos.x + size, pos.y, 0));
            Gizmos.DrawLine(new Vector3(pos.x + size, pos.y, 0), new Vector3(pos.x, pos.y, 0));

            Gizmos.color = last;
        }
    }
}
