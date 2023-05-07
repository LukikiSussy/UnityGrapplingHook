using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    [Header("Grappling Gun Script Ref:")]
    public GrapplingGun grapplingGunSc;

    [Header("General/Physics Refernces:")]
    public LineRenderer lineRenderer;
    public SpringJoint2D springJoint;

    [Header("Animation Settings:")]
    public AnimationCurve ropeAnimationCurve;
    public AnimationCurve animationArc;
    public bool animate;
    public float animationSpeed;  
    public int percision = 40;
    public float StartwaveSize = 15;
    public int straightenLineSpeed = 5;

    float waveSize;
    float elapsed;
    float moveTime;
    Vector3 currentPos;

    void Start()
    {
        lineRenderer.enabled = false;
    }

    void Update()
    {
        moveTime += Time.deltaTime;
        DrawRope();
    }

    private void OnEnable()
    {
        moveTime = 0;
        lineRenderer.positionCount = percision;
        waveSize = StartwaveSize;

        lineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;
    }

    public void DrawRope() {
        if (animate) {

            if (waveSize > 0) {
                waveSize -= Time.deltaTime * straightenLineSpeed;
                DrawWaves();
            }
            else {
                waveSize = 0;

                DrawNoWaves();
            }

        }
        else {
            
            DrawNoWaves();
        }

        void DrawWaves() {

            for (int i = 0; i < percision; i++) {

                float delta = i / (percision - 1f);
                Vector3 offset = Vector2.Perpendicular(grapplingGunSc.distance).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
                Vector3 targetPos = Vector3.Lerp(grapplingGunSc.firePoint.transform.position, grapplingGunSc.grapplePoint, delta) + offset;
                Vector3 currentPos = Vector3.Lerp(grapplingGunSc.firePoint.transform.position, targetPos, animationArc.Evaluate(moveTime) * animationSpeed);
                lineRenderer.SetPosition(i, currentPos);

            }

            if (animationArc.Evaluate(moveTime) * animationSpeed >= 1) {
                springJoint.enabled = true;
            }
        }

        void DrawNoWaves () {

            if (lineRenderer.positionCount != 2) {
                lineRenderer.positionCount = 2;
            }

            springJoint.enabled = true;

            lineRenderer.SetPosition(0, grapplingGunSc.grapplePoint);
            lineRenderer.SetPosition(1, grapplingGunSc.firePoint.transform.position);

        }
    }
}
