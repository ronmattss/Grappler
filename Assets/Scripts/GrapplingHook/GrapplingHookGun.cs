using System;
using System.Numerics;
using Controls;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace GrapplingHook
{
    public class GrapplingHookGun : MonoBehaviour
    {
        // Start is called before the first frame update
        PlayerControls m_Controls;

        public Camera mainCam;
        public LineRenderer lineRenderer;
        public GameObject hookPoint; // The point where the hook is attached to used for hooking on a moving platforms
        public Vector2 hookDistance;

        bool isFiring = false;
        private bool isFireButtonPressed = false;
        private bool canFire = false;
        private bool isRetractInput = false;

        private Vector3 mouseWorldPosition;

        [Header("Distance Joint")] public DistanceJoint2D distanceJoint;
        public Rigidbody2D hookRigidbody;


        //Pivots
        [Header("Pivot points")] public Transform gunPivot;
        public Transform gunFirePoint;
        public LayerMask whereToCollide;

        [Header("GunProperties")] public Rope rope;
        public float ropeLength = 20;
        public float grappleRadius = 1;
        public float distanceReductionRate = 2f;
        public float reductionVelocityX = 1.5f;
        public float reductionVelocityY = 2.5f;
        public float reductionVelocity = 5;

        [Header("Debug")] public GameObject debugPoint;


        void Awake()
        {
            mainCam = Camera.main;
            m_Controls = new PlayerControls();
            distanceJoint = GetComponent<DistanceJoint2D>();
            distanceJoint.distance = ropeLength;
            m_Controls.PlayerControl.Fire.started += OnFireButtonInput;
            m_Controls.PlayerControl.Fire.canceled += OnFireButtonInput;
            m_Controls.PlayerControl.Fire.performed += OnFireButtonInput;
            m_Controls.PlayerControl.Retract.started += OnRetractInput;
            m_Controls.PlayerControl.Retract.canceled += OnRetractInput;
            m_Controls.PlayerControl.Retract.performed += OnRetractInput;
            debugPoint.transform.localScale = Vector3.one * grappleRadius;
        }

        private void OnRetractInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                isRetractInput = true;
            }

            if (context.performed)
            {
                isRetractInput = true;
            }

            if (context.canceled)
            {
                isRetractInput = false;
            }
        }

        void Update()
        {
            mouseWorldPosition = PlayerBaseControls.Instance.GetMousePosition();
            RotateTowardsMousePosition(gunPivot);
            if (isFireButtonPressed)
            {
                
                FireGun();
                //UpdateHookPoint(); // why did I comment this?
                Retract();
            }
            else
            {
                lineRenderer.enabled = false;
            }


            debugPoint.transform.position = RayCastToMousePosition();
        }

        private void OnEnable()
        {
            m_Controls.Enable();
        }

        private void OnDisable()
        {
            m_Controls.Disable();
            m_Controls.PlayerControl.Fire.started -= OnFireButtonInput;
            m_Controls.PlayerControl.Fire.canceled -= OnFireButtonInput;
            m_Controls.PlayerControl.Fire.performed -= OnFireButtonInput;
            m_Controls.PlayerControl.Retract.started -= OnRetractInput;
            m_Controls.PlayerControl.Retract.canceled -= OnRetractInput;
            m_Controls.PlayerControl.Retract.performed -= OnRetractInput;
        }


        // Update is called once per frame

        // this should be abstracted such that it can be used for any type of Grappling hook Gun

       

        void FireGun()
        {
            if (canFire)
            {
                mouseWorldPosition = CircleCastOnMousePosition();
                // RULE ONLY GRAPPLE IF HOOKPOINT HAS A PARENT OBJECT
                if (hookPoint.transform.parent != null)
                {                distanceJoint.enabled = true;
                        
                    if (DistanceBetweenTwoPoints(gunFirePoint.position, mouseWorldPosition) > ropeLength)
                    {
                        // var maxDistance = GetDistanceBetweenTwoPoints(mouseWorldPosition, gunFirePoint.position);
                        var maxDistance =
                            SetPositionViaDistance(gunFirePoint.position, CircleCastOnMousePosition(), ropeLength);

                        distanceJoint.connectedAnchor = maxDistance;
                        distanceJoint.distance = DistanceBetweenTwoPoints(gunFirePoint.position, maxDistance);
                    }
                    else
                    {
                        if (hookRigidbody != null) // if moving object
                        {
                            var position = hookRigidbody.position;

                            distanceJoint.connectedBody = hookRigidbody;

                            distanceJoint.connectedAnchor = Vector2.zero;
                            distanceJoint.distance =
                                DistanceBetweenTwoPoints(gunFirePoint.position, position);
                        }
                        else
                        {
                            distanceJoint.connectedAnchor = CircleCastOnMousePosition();
                            distanceJoint.distance =
                                DistanceBetweenTwoPoints(gunFirePoint.position, CircleCastOnMousePosition());
                        }
                    }
                    rope.enabled = true;
                    

                    hookDistance = distanceJoint.connectedAnchor - (Vector2)gunFirePoint.position;
                }
                // IF there is no parent then don't fire

                canFire = false;
                isFiring = true;
                Debug.Log("canFire true");
            }
            else
            {
                // hookRigidbody = null;
                Debug.Log("canFire false");
            }
            UpdateHookPoint();

        }

        void UpdateHookPoint()
        {
            // Update the if statement if bugs occur
            if (isFiring && hookPoint.transform.parent != null &&
                DistanceBetweenTwoPoints(gunFirePoint.position, hookPoint.transform.position) <= ropeLength)
            {
                if (hookRigidbody != null)
                {
                //     distanceJoint.connectedAnchor = Vector2.zero;
                    lineRenderer.SetPosition(1, hookRigidbody.transform.position);
                }
                else
                {
                    distanceJoint.connectedAnchor = hookPoint.transform.position;
                    //  lineRenderer.SetPosition(1, hookPoint.transform.position);
                }
            }
        }

        // Sets the position from the maximum distance
        Vector3 SetPositionViaDistance(Vector3 point1, Vector3 point2, float distance)
        {
            return point1 + (point2 - point1).normalized * distance;
        }


        void SetLineRendererZeroPosition()
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, gunFirePoint.position);
        }


        float DistanceBetweenTwoPoints(Vector2 pointA, Vector2 pointB)
        {
            return Vector2.Distance(pointA, pointB);
        }

        void OnFireButtonInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                isFireButtonPressed = true;
            }

            if (context.performed)
            {
                isFireButtonPressed = true;
              //  lineRenderer.enabled = true;
            }

            if (context.canceled)
            {
                isFireButtonPressed = false;
                distanceJoint.enabled = false;
                //   lineRenderer.enabled = false;
                rope.enabled = false;

                canFire = true;
                isFiring = false;
                distanceJoint.connectedBody = null;
                hookRigidbody = null;
                hookPoint.transform.parent = null;
            }
        }

        void RotateTowardsMousePosition(Transform pivotPoint)
        {
            Vector3 mousePosition = mouseWorldPosition;
            Vector2 direction = new Vector2(mousePosition.x - pivotPoint.position.x,
                mousePosition.y - pivotPoint.position.y);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            pivotPoint.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        void ReduceDistance()
        {
            if (!isRetractInput) return;
            var playerBody = this.gameObject.GetComponent<Rigidbody2D>();

            //if (hookRigidbody != null)

            distanceJoint.distance -= distanceReductionRate;
            var velocity = playerBody.velocity;

            velocity = new Vector2(velocity.x + CheckIfPlayerIsLeftOrRightGameObject(reductionVelocityX),
                velocity.y + CheckIfPlayerIsAboveGameObject(reductionVelocityY));


            playerBody.velocity = velocity;
        }

        void Retract()
        {
            if (!isRetractInput) return;
            var playerBody = this.gameObject.GetComponent<Rigidbody2D>();
            distanceJoint.distance -= distanceReductionRate;
            var velocity = playerBody.velocity;
            var gunVelocity = gunPivot.right * reductionVelocity;
            velocity = new Vector2(velocity.x + gunVelocity.x, velocity.y + gunVelocity.y);
            // velocity = new Vector2(velocity.x + CheckIfPlayerIsLeftOrRightGameObject(reductionVelocityX),
            //     velocity.y + CheckIfPlayerIsAboveGameObject(reductionVelocityY));


            playerBody.velocity = velocity;
        }

        float CheckIfPlayerIsAboveGameObject(float speed)
        {
            return this.gameObject.transform.position.y > distanceJoint.connectedAnchor.y ? -speed : speed;
        }

        float CheckIfPlayerIsLeftOrRightGameObject(float speed)
        {
            return this.gameObject.transform.position.x > distanceJoint.connectedAnchor.x ? -speed : speed;
        }

        void SetVelocityBasedOnMouseDirection(Vector3 direction)
        {
            if (hookRigidbody != null)
            {
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(
                    this.gameObject.GetComponent<Rigidbody2D>().velocity.x + direction.x,
                    this.gameObject.GetComponent<Rigidbody2D>().velocity.y + direction.y);
            }
        }

        Vector3 RayCastToMousePosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(gunFirePoint.position, mouseWorldPosition - gunFirePoint.position,
                ropeLength, whereToCollide);
            if (hit.collider != null)
            {
                // try Get Component<Rigidbody2D>


                //   Debug.Log($"Hit Point: {hit.point}");

                return hit.point;
            }
            else
            {
                return mouseWorldPosition;
            }
        }

        Vector3 CircleCastOnMousePosition()
        {
            RaycastHit2D hit = Physics2D.CircleCast(RayCastToMousePosition(), grappleRadius, Vector2.up, Mathf.Infinity,
                whereToCollide);

            if (hit.collider)
            {
                hookPoint.transform.position = hit.point;
                hookPoint.transform.parent = hit.transform;
                if (hit.collider.TryGetComponent(out Rigidbody2D hookedRigidbody))
                {
                    hookRigidbody = hookedRigidbody;
                }

                return hit.point;
            }

            return Vector3.zero;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(hookPoint.transform.position, grappleRadius);
        }

        // update hookPoint position if hookPoint it is not null
        void SetHookPointPosition()
        {
            hookPoint.transform.position = hookRigidbody.transform.position;
        }
    }
}