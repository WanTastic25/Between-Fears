using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CameraController : MonoBehaviour
{
    //public PlayerController playerController;

    public Transform targetTransform;
    public Transform cameraPivot;
    public Transform cameraTransform;
    public LayerMask collisionLayers;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPos;

    public float targetPosition;
    public float cameraFollowSpeed = 0.2f;
    public float lookSpeed = 1f;
    private float lookAngle;
    private float pivotAngle;
    public float defaultPos;

    private void Awake()
    {
        defaultPos = cameraTransform.localPosition.z;
    }

    public void FollowTarget()
    {
        Vector3 target = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);

        transform.position = target;
    }

    public void OrbitingCamera(float cameraInputX, float cameraInputY)
    {
        Vector3 rotation;

        lookAngle = lookAngle + (cameraInputX * lookSpeed);
        pivotAngle = pivotAngle - (cameraInputY * lookSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, -35, 35);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    public void CameraCollisions()
    {
        targetPosition = defaultPos;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast
             (cameraPivot.transform.position, 0.2f , direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            //Debug.Log("Pivot Pos " + cameraPivot.position);
            //Debug.Log("Hit Pos " + hit.point);
            //Debug.Log(distance);
            targetPosition = -distance + 0.2f;
        }

        cameraVectorPos.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);

        cameraTransform.localPosition = cameraVectorPos;
    }
}

