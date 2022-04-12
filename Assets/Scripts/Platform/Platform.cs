using System;
using UnityEngine;

namespace Platform
{
    public class Platform : MonoBehaviour
    {

        public Vector3 targetPosition;
        public Vector3 originalPosition;
        public float speed = 1;
        public float delay = 1;
        public float rotateSpeed = 1;

        public bool canMove = false;
        public bool canRotate = false;
        public LeanTweenType easeTypeFirstMovement;
        public LeanTweenType easeTypeSecondMovement;
    
        // move from Original Point to another point using Lean Tween


        public void OnEnable()
        {
            originalPosition = transform.localPosition;
            if (canMove)
            {
                MoveTo();
            }

            if (canRotate)
            {
                RotateAtZAxis();
            }
            
        }
        
        public void MoveTo()
        {
            LeanTween.moveLocal(gameObject, targetPosition, speed).setEase(easeTypeFirstMovement).setDelay(delay).setOnComplete(MoveBack);
        }
        
        // move back to the original point
        public void MoveBack()
        {
            LeanTween.moveLocal(gameObject, originalPosition, speed).setEase(easeTypeSecondMovement).setDelay(delay).setOnComplete(MoveTo);
        }

        public void RotateAtZAxis()
        {
            LeanTween.rotateAround(gameObject, Vector3.forward, 360, rotateSpeed).setRepeat(-1);


        }
    }
}