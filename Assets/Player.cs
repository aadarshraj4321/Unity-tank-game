using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    private Rigidbody rb;


    [Header("Movement Data")]
    [SerializeField] private float tankMoveSpeed;
    [SerializeField] private float tankRotationSpeed;
    [SerializeField] private float verticalInput;
    [SerializeField] private float horizontalInput;



    [Header("Aim Data")]
    [SerializeField] private Transform aimPosition;
    [SerializeField] private LayerMask whatIsAimMask;


    [Header("Tank Tower Transform Data")]
    [SerializeField] private Transform tankTowerTransform;
    [SerializeField] private float tankTowerRotationSpeed;


    [Header("Gun Data")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulledSpeed;
    [SerializeField] private Transform gunPoint;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        inputData();
        applyAim();
    }



    private void FixedUpdate()
    {
        applyMovement();
        applyRotation();
        applyTankTowerRotation();
    }


    private void applyMovement()
    {
        Vector3 movement = transform.forward * tankMoveSpeed * verticalInput;
        rb.velocity = movement;
    }

    private void applyRotation()
    {
        transform.Rotate(0, tankRotationSpeed * horizontalInput, 0);

    }


    private void inputData()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            shootBullet();
        }


        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        if (verticalInput < 0)
        {
            horizontalInput = -Input.GetAxis("Horizontal");
        }
    }


    private void applyTankTowerRotation()
    {
        Vector3 direction = aimPosition.position - tankTowerTransform.position;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        tankTowerTransform.rotation = Quaternion.RotateTowards(tankTowerTransform.rotation, targetRotation, tankTowerRotationSpeed);
    }


    private void applyAim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsAimMask))
        {
            // aimPosition.position = hit.point;
            // Debug.Log(hit.point);
            float fixedY = aimPosition.position.y;
            aimPosition.position = new Vector3(hit.point.x, fixedY, hit.point.z);
        }
    }


    private void shootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = gunPoint.forward * bulledSpeed;

        Destroy(bullet, 4);
    }
}
