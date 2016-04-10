using UnityEngine;

namespace BriLib
{
    public class EasingTester : MonoBehaviour
    {
        public Easing.Method EasingMethod;
        public float Duration;

        private Vector3 startPoint;
        private Vector3 endPoint;
        private float startTime;
        private float endTime;
        private bool easing = false;
        
        private void Update()
        {
            if (easing)
            {
                transform.position = Easing.Ease(startPoint, endPoint, startTime, endTime, Time.time, EasingMethod);
                easing = Time.time < endTime;
            }
            else if (Input.GetMouseButton(0))
            {
                var mouse = Input.mousePosition;
                startPoint = transform.position;
                var diff = Camera.main.transform.position.z - startPoint.z;
                var position = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, Mathf.Abs(diff)));
                position.z = transform.position.z;
                endPoint = position;
                startTime = Time.time;
                endTime = startTime + Duration;
                easing = true;
            }
        }
    }
}
