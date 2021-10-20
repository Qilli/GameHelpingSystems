using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.UI
{
    [ExecuteInEditMode]
    public class UISaveAreaHandler : MonoBehaviour
    {
        private RectTransform rect;
        private Vector2 screen;
        private Rect safeRect;
        // Start is called before the first frame update
        void Start()
        {
            rect = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            screen.x = Screen.width;
            screen.y = Screen.height;
            safeRect = Screen.safeArea;
            rect.anchorMin = (safeRect.position) / screen ;
            rect.anchorMax = (safeRect.position + safeRect.size)/screen;
        }
    }
}
