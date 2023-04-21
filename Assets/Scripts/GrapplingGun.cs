using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public GrapplingRope grapplingRopeSc;

    public Transform grapplingGunPivot;
    public Camera mainCamera;
    public SpringJoint2D springJoint;
    public Transform firePoint;
    public Rigidbody2D playerRb;


    Vector3 mousePos;
    Vector3 currentPos;
    public Vector3 distanceNorm;
    public Vector3 distance;
    public Vector3 grapplePoint;
    Vector3 distanceToGrapplePoint;

    public float maxDistance;
    public float rotationSpeed;
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

            grapplingRopeSc.lineRenderer.enabled = false;
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
            grapplingGunPivot.transform.rotation = Quaternion.Lerp(grapplingGunPivot.rotation, Quaternion.AngleAxis(-angle + 90, Vector3.forward), Time.deltaTime * rotationSpeed);

        }
    }

    void SetGrapplePoint() {  
        CalculateDistance();

        RaycastHit2D hit = Physics2D.Raycast(firePoint.transform.position, distanceNorm);
        if (Physics2D.Raycast(firePoint.transform.position, distanceNorm)) {

            if (hit.transform.gameObject.layer == 3) {

                if (Vector2.Distance(hit.point, firePoint.position) <= maxDistance || maxDistance == 0) {
                    grapplingRopeSc.enabled = true;
                    grapplingRopeSc.DrawRope();

                    grapplingRopeSc.lineRenderer.enabled = true;
                    grapplePoint = hit.point;
                    
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