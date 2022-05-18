using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Grappler : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private DistanceJoint2D distanceJoint2D;
    [SerializeField] private float coolDownTime;
    private float timeStamp = 0;

    private void Start()
    {
        distanceJoint2D.enabled = false;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        Grapple();
    }

    private void Grapple()
    {
        if (timeStamp < Time.time)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                timeStamp = Time.time + coolDownTime;
                Vector2 mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
                lineRenderer.SetPosition(0, mousePos);
                lineRenderer.SetPosition(1, transform.position);
                distanceJoint2D.connectedAnchor = mousePos;
                distanceJoint2D.enabled = true;
                lineRenderer.enabled = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            distanceJoint2D.enabled = false;
            lineRenderer.enabled = false;
        }
        if (distanceJoint2D.enabled)
        {
            lineRenderer.SetPosition(1, transform.position);
        }
    }
}
