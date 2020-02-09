using UnityEngine;


namespace LalaFight
{
    [RequireComponent(typeof(CameraController))]
    public class CameraInput : MonoBehaviour
    {
        private CameraController _cameraController;
        private PlayerInput _target;
        bool _isPanning = false;
        bool _isPanningPrev = false;

        public bool IsPanning => _isPanning;

        private void Awake()
        {
            _cameraController = GetComponent<CameraController>();
            _target = FindObjectOfType<PlayerInput>();
            _cameraController.SetTarget(_target);
        }

        private void LateUpdate()
        {
            if (_target == null)
                return;

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _isPanningPrev = _isPanning;
                _isPanning = !_isPanning;
            }

            if (_isPanningPrev == true && _isPanning == false)
            {
                if (_cameraController.SetCameraOriginalPosition())
                {
                    _isPanningPrev = false;
                    _isPanning = false;
                }
            }

            if (_isPanning == true)
            {
                //TODO: choose between one of them with PlayerPrefs
                //Vector3 pan = new Vector3(Input.GetAxisRaw("Mouse X"), 0f, Input.GetAxisRaw("Mouse Y")).normalized * Time.deltaTime;
                Vector3 pan = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized * Time.deltaTime;
                _cameraController.PanCamera(pan);
            }

            if (_isPanningPrev == false && _isPanning == false)
            {
                float rotation = Input.GetAxisRaw("Mouse X") * Time.deltaTime;
                _cameraController.PositionCamera(Input.GetMouseButton(2) ? rotation : 0);
            }
        }
    }
}