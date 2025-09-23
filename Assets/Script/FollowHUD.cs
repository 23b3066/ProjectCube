using UnityEngine;

namespace raspberly.ovr
{
    public class FollowHUD : MonoBehaviour
    {
        [SerializeField] private Transform rightHandAnchor;
        [SerializeField] private float followSpeed = 0.1f;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private bool isImmediate = false;

        private void LateUpdate()
        {
            if (!rightHandAnchor) return;

            if (isImmediate)
            {
                transform.position = rightHandAnchor.position;
                transform.rotation = rightHandAnchor.rotation;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, rightHandAnchor.position, followSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, rightHandAnchor.rotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
}
