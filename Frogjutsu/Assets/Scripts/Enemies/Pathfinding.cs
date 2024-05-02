using UnityEngine;
using System.Collections;

namespace Pathfinding
{
    [UniqueComponent(tag = "ai.destination")]
    public class AIDestinationSetter : VersionedMonoBehaviour
    {
        IAstarAI ai;
        Camera cam;

        public void Start()
        {
            //Cache the Main Camera
            cam = Camera.main;
            useGUILayout = false;
        }

        void OnEnable()
        {
            ai = GetComponent<IAstarAI>();
        }

        // Handles onClick
        public void OnGUI()
        {
            if (cam != null && Event.current.type == EventType.MouseDown && Event.current.clickCount == 1)
            {
                UpdateTargetPosition();
            }
        }

        public void UpdateTargetPosition()
        {
            Vector3 newPosition = Vector3.zero;
            bool positionFound = false;

            newPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 0;
            positionFound = true;

            if (positionFound && newPosition != ai.destination)
            {
                if (ai != null) ai.destination = newPosition;
            }
        }
    }
}