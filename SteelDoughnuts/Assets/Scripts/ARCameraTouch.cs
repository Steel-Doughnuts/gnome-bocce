using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ARCameraTouch : MonoBehaviour
{
    private Transform pickedObject = null;
    private Vector3 lastPlanePoint;

    // Update is called once per frame
    void Update()
    {
        Plane targetPlane = new Plane(transform.up, transform.position);
        foreach (Touch touch in Input.touches) {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            float dist = 0.0f;
			targetPlane.Raycast(ray, out dist);            
            Vector3 planePoint = ray.GetPoint(dist);
            if (touch.phase == TouchPhase.Began) {
				RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit, 2)) {
					pickedObject = hit.transform;
                    lastPlanePoint = planePoint;
					EventTrigger.Entry trigger = pickedObject.gameObject.GetComponent<EventTrigger>().triggers [0];
					trigger.callback.Invoke(new BaseEventData(EventSystem.current));
				} else {
                    pickedObject = null;
                }
            } else if (touch.phase == TouchPhase.Moved) {
				if (pickedObject != null) {
					pickedObject.position += planePoint - lastPlanePoint;
					lastPlanePoint = planePoint;
					EventTrigger.Entry trigger = pickedObject.gameObject.GetComponent<EventTrigger> ().triggers [1];
					trigger.callback.Invoke (new BaseEventData (EventSystem.current));
				}              
            } else if (touch.phase == TouchPhase.Ended) {
				if (pickedObject != null) {
					EventTrigger.Entry trigger = pickedObject.gameObject.GetComponent<EventTrigger> ().triggers [2];
					trigger.callback.Invoke (new BaseEventData (EventSystem.current));
					pickedObject = null;
				}
            }
        }
    }
}
