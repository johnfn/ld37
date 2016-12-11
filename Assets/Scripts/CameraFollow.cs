using UnityEngine;
using Zenject;

namespace johnfn {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public class CameraFollow : Entity {
        [Inject]
        public IUtil Util { get; set; }

        public GameObject Target;

        public float CameraSpeed;

        public float Height {
            get { return 2.0f * _camera.orthographicSize; }
        }

        public float Width {
            get { return Height * _camera.aspect; }
        }

        private Camera _camera;

        void Start()
        {
            _camera = GetComponent<Camera>();
        }

        public Vector3 ClampWithinCamera(Vector3 point)
        {
            var resultX = Mathf.Clamp(point.x, transform.position.x - Width, transform.position.x + Width);
            var resultY = Mathf.Clamp(point.y, transform.position.y - Height, transform.position.y + Height);

            return new Vector3(resultX, resultY, point.z);
        }

        public bool IsWithinCamera(Vector3 point)
        {
            return point == ClampWithinCamera(point);
        }

        void LateUpdate()
        {
            /*
            var minX = Mathf.Floor(Target.transform.position.x / Util.MapWidth) * Util.MapWidth;
            var minY = Mathf.Floor(Target.transform.position.y / Util.MapHeight) * Util.MapHeight;

            var maxX = minX + Util.MapWidth;
            var maxY = minY + Util.MapHeight;

            minX += Width / 2;
            maxX -= Width / 2;

            minY += Height / 2;
            maxY -= Height / 2;

            var camX = Mathf.Clamp(Target.transform.position.x, minX, maxX);
            var camY = Mathf.Clamp(Target.transform.position.y, minY, maxY);

            var desiredPosition = new Vector3(camX, camY, transform.position.z);
            */

            var oldZ = transform.position.z;
            var desiredPosition = Target.transform.position;

            desiredPosition.z = oldZ;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * CameraSpeed);
        }
    }
}