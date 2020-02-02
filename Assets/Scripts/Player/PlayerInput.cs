using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(MeleeAttack))]
[RequireComponent(typeof(WeaponManager))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask _enemyLayerMask = new LayerMask();

    [SerializeField] private InteractableController _intractableController = null;
    
    private Camera _camera = null;
    private PlayerController _playerController;
    private WeaponManager _weaponManager;
    private CameraInput _cameraInput;
    private MeleeAttack _meleeAttack;


    private Plane _plane;

    //EVENTS
    public event Action<Vector3, Vector3> OnHitPointSet;

    private void Awake()
    {
        _camera = CameraDatabase.MainCamera;
        _playerController = GetComponent<PlayerController>();
        _meleeAttack = GetComponent<MeleeAttack>();
        _weaponManager = GetComponent<WeaponManager>();
        _cameraInput = _camera.GetComponent<CameraInput>();
        _plane = new Plane(Vector3.up, Vector3.up);
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) // If we have UI showing, we won't continue our Input Loop
            return;

        // Movement
        if (_cameraInput.IsPanning == false)
            _playerController.Move(Movement());
        else
            _playerController.Move(Vector3.zero);

        // Rotation and Aiming [Weapon Cursor set here with Weapon Manager]
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Vector3 hitPoint = Vector3.zero;
        bool enemyRayCast = false;
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _enemyLayerMask, QueryTriggerInteraction.Collide))
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            enemyRayCast = true;
            hitPoint = hitInfo.point;
        }
        else if (_plane.Raycast(ray, out float distance))
            hitPoint = ray.GetPoint(distance);

        float lengthToPlayer = hitPoint.sqrLengthTo(transform.position);
        if (lengthToPlayer > 1)
            _playerController.LookAt(hitPoint);
        _weaponManager.SetCursorPositionAndLookAt(hitPoint, lengthToPlayer, enemyRayCast);
        OnHitPointSet?.Invoke(hitPoint, _camera.transform.forward);

        //Melee Attack
        if (Input.GetKey(KeyCode.Space))
        {
            _meleeAttack.Attack();
            return;
        }

        //Shooting - Control Weapon Update Method Here
        _weaponManager.CurrentWeaponTick();
        _intractableController.Tick();


        //Changing Weapon
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            _weaponManager.SwapWeapon(+1);
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            _weaponManager.SwapWeapon(-1);

        //Toggle Weapon Enability
        if (Input.GetButtonDown("ToggleWeaponEnable"))
            _weaponManager.ToggleWeaponEnable();
    }

    public float GetPlayerActiveRange() => _weaponManager.GetWeaponActiveRange();

    private Vector3 Movement()
    {
        /* Based On World Axis
        Vector3 movementDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        return movementDir;
        */

        /* Based on Camera Axis */
        var camForward = _camera.transform.forward.Flat().normalized;
        var camRight = _camera.transform.right.Flat().normalized;

        Vector3 movementDir = camForward * Input.GetAxisRaw("Vertical") + camRight * Input.GetAxisRaw("Horizontal");
        return movementDir.normalized;

        /* Based on Player Axis
        Vector3 movementDir = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal");
        return movementDir.normalized;
        */


    }
}
