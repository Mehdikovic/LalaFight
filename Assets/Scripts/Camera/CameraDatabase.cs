using UnityEngine;

public class CameraDatabase : MonoBehaviour
{
    static private Camera _mainCamera = null;

    static public Camera MainCamera {
        get {
            if (_mainCamera == null)
                _mainCamera = Camera.main;
            return _mainCamera;
        }
    }
}