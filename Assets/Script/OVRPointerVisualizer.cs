using UnityEngine;

namespace UIControllerLaserPointer
{
 public class OVRPointerVisualizer : MonoBehaviour
 {
     [Tooltip("Object which points with Z axis. E.g. CentreEyeAnchor from OVRCameraRig")]
     public Transform rayTransform;

     [Header("Visual Elements")] [Tooltip("Line Renderer used to draw selection ray.")]
     public LineRenderer linePointer = null;

     [Tooltip("Visually, how far out should the ray be drawn.")]
     public float rayDrawDistance = 2.5f;
     
     [SerializeField] private OVRGazePointer _ovrGazePointer = null;

     void LateUpdate()
     {
         linePointer.enabled = (OVRInput.GetActiveController() == OVRInput.Controller.Touch);
         Ray ray = new Ray(rayTransform.position, rayTransform.forward);
         linePointer.SetPosition(0, ray.origin);
         
         if (!_ovrGazePointer.hidden)
         {
             linePointer.SetPosition(1, _ovrGazePointer.transform.position);
             return;
         }
         linePointer.SetPosition(1, ray.origin + ray.direction * rayDrawDistance);
     }
 }
}
