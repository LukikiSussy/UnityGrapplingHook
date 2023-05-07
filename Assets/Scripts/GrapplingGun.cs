using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    [Header("Grappling Rope Script Ref:")]
    public GrapplingRope grapplingRopeSc;

    [Header("Transform Ref:")]
    public Transform firePoint;
    public Transform grapplingGunPivot;

    [Header("Main Camera:")]
    public Camera mainCamera;

    [Header("Physics Ref:")]
    public SpringJoint2D springJoint;
    public Rigidbody2D playerRb;

    [Header("Settings (set maxDistance to 0 for unlimited distance):")]
    public float maxDistance;
    public float gunRotationSpeed;

    [HideInInspector] public Vector3 distance;
    [HideInInspector] public Vector3 grapplePoint;

    Vector3 distanceToGrapplePoint;
    Vector3 mousePos;
    Vector3 currentPos;
    Vector3 distanceNorm;

    bool shiftHeldDown = false;

    void Start()
    {
        springJoint.enabled = false;
        grapplingRopeSc.enabled = false;
    }

    void Update()
    {

        RotateGun();

        if (Input.GetKey(KeyCode.Mouse0)) {

            if (!shiftHeldDown) {

                SetGrapplePoint();
                shiftHeldDown = true;

            }
        }
        else {

            springJoint.enabled = false;

            shiftHeldDown = false;
            grapplingRopeSc.enabled = false;

        }
    }

    void RotateGun() {
        CalculateDistance();

        if (grapplingRopeSc.enabled) {

            float angle = Mathf.Atan2(distanceToGrapplePoint.x, distanceToGrapplePoint.y) * Mathf.Rad2Deg;
            grapplingGunPivot.transform.rotation = Quaternion.AngleAxis(-angle + 90, Vector3.forward);

        } 
        else {

            float angle = Mathf.Atan2(distanceNorm.x, distanceNorm.y) * Mathf.Rad2Deg;
            grapplingGunPivot.transform.rotation = Quaternion.Lerp(grapplingGunPivot.rotation, Quaternion.AngleAxis(-angle + 90, Vector3.forward), Time.deltaTime * gunRotationSpeed);

        }
    }

    void SetGrapplePoint() {  
        CalculateDistance();

        RaycastHit2D hit = Physics2D.Raycast(firePoint.transform.position, distanceNorm);
        if (Physics2D.Raycast(firePoint.transform.position, distanceNorm)) {

            if (hit.transform.gameObject.layer == 3) {

                if (Vector2.Distance(hit.point, firePoint.position) <= maxDistance &&  Vector2.Distance(hit.point, firePoint.position) >= 1 || maxDistance == 0) {
                    grapplingRopeSc.enabled = true;

                    grapplePoint = hit.point;
                    grapplePoint.z = transform.position.z;
                    
                    springJoint.connectedAnchor = grapplePoint;

                }
            }
        }
        else {
            springJoint.enabled = false;
        }
    }   

    void CalculateDistance() {

        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        currentPos = grapplingGunPivot.transform.position;
        distance = mousePos - currentPos;
        distanceNorm = (mousePos - currentPos).normalized;

        if (springJoint != null) {

            distanceToGrapplePoint = (grapplePoint - currentPos).normalized;

        }
    } 
}
