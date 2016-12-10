using UnityEngine;
using Zenject;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
public class CameraFollow : Entity {
    [Inject]
    public IManager Manager { get; set; }

    public GameObject Target;

    public float CameraSpeed;

    public float Height
    {
        get { return 2.0f * _camera.orthographicSize; }
    }

    public float Width
    {
        get { return Height * _camera.aspect; }
    }

    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();

        if (Height > Manager.MapHeight) {
          Debug.LogError("The camera is taller than a map segment.");
        }

        if (Width > Manager.MapWidth) {
          Debug.LogError("The camera is wider than a map segment.");
        }
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
        var minX = Mathf.Floor(Target.transform.position.x / Manager.MapWidth) * Manager.MapWidth;
        var minY = Mathf.Floor(Target.transform.position.y / Manager.MapHeight) * Manager.MapHeight;

        var maxX = minX + Manager.MapWidth;
        var maxY = minY + Manager.MapHeight;

        minX += Width / 2;
        maxX -= Width / 2;

        minY += Height / 2;
        maxY -= Height / 2;

        var camX = Mathf.Clamp(Target.transform.position.x, minX, maxX);
        var camY = Mathf.Clamp(Target.transform.position.y, minY, maxY);

        var desiredPosition = new Vector3(camX, camY, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * CameraSpeed);
    }
}