using DefaultNamespace.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    public class PlayerBaseControls : Singleton<PlayerBaseControls>
    {
        // All Controls of the game will be here
        PlayerControls m_Controls;
        
        
        // Movement on X axis
        Vector2 m_MovementInput;
        
        // Jump Button
        private bool m_IsJumpPressed;

        private bool m_IsEscapePressed;

        public Vector2 movementInput => m_MovementInput;
        
        // get Mouse position
        Vector3 mousePosition;
        
        // Fire Button
        private bool m_GunFireInput;
        private bool m_GunRetractInput;
        

       public Vector3 GetMousePosition() => mousePosition;
       public Vector2 GetMovementInput() => m_MovementInput;
       
       public bool GetJumpInput() => m_IsJumpPressed;
       
       public bool GetEscapeInput() => m_IsEscapePressed;



        void Awake()
        {  
            m_Controls = new PlayerControls();
            
            m_Controls.PlayerControl.Movement.started += OnMovementInput;
            m_Controls.PlayerControl.Movement.canceled += OnMovementInput;
            m_Controls.PlayerControl.Movement.performed += OnMovementInput;
            
            m_Controls.PlayerControl.Jump.started += OnJumpInput;
            m_Controls.PlayerControl.Jump.canceled += OnJumpInput;
            m_Controls.PlayerControl.Jump.performed += OnJumpInput;
            
            m_Controls.PlayerControl.Fire.started += OnFireButtonInput;
            m_Controls.PlayerControl.Fire.canceled += OnFireButtonInput;
            m_Controls.PlayerControl.Fire.performed += OnFireButtonInput;
            
            m_Controls.PlayerControl.Aim.performed += OnAimInput;
            
            m_Controls.PlayerControl.Retract.started += OnRetractInput;
            m_Controls.PlayerControl.Retract.canceled += OnRetractInput;
            m_Controls.PlayerControl.Retract.performed += OnRetractInput;

            m_Controls.MenuControl.Escape.started += OnEscapeInput;
            m_Controls.MenuControl.Escape.canceled += OnEscapeInput;
            m_Controls.MenuControl.Escape.performed += OnEscapeInput;

        }

        private void OnEscapeInput(InputAction.CallbackContext context)
        {
            m_IsEscapePressed = context.ReadValueAsButton();
            Debug.Log($"Escape pressed: {m_IsEscapePressed}");
        }
        private void OnRetractInput(InputAction.CallbackContext context)
        {
            m_GunRetractInput = context.ReadValueAsButton();
        }

        private void OnAimInput(InputAction.CallbackContext context)
        {
            mousePosition = context.ReadValue<Vector2>();
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = Camera.main.nearClipPlane;
        }

        private void OnFireButtonInput(InputAction.CallbackContext context)
        {
            m_GunFireInput  = context.ReadValueAsButton();
            Debug.Log("Firing: "+m_GunFireInput);
        }

        void OnEnable()
        {
            m_Controls.Enable();

        }

        private void OnDisable()
        {
            m_Controls.Disable();
            m_Controls.PlayerControl.Movement.started += OnMovementInput;
            m_Controls.PlayerControl.Movement.canceled += OnMovementInput;
            m_Controls.PlayerControl.Movement.performed += OnMovementInput;
            
            m_Controls.PlayerControl.Jump.started += OnJumpInput;
            m_Controls.PlayerControl.Jump.canceled += OnJumpInput;
            m_Controls.PlayerControl.Jump.performed += OnJumpInput;
            
            m_Controls.PlayerControl.Fire.started -= OnFireButtonInput;
            m_Controls.PlayerControl.Fire.canceled -= OnFireButtonInput;
            m_Controls.PlayerControl.Fire.performed -= OnFireButtonInput;
            
            m_Controls.PlayerControl.Aim.performed -= OnAimInput;
            
            m_Controls.PlayerControl.Retract.started -= OnRetractInput;
            m_Controls.PlayerControl.Retract.canceled -= OnRetractInput;
            m_Controls.PlayerControl.Retract.performed -= OnRetractInput;
        }

        private void OnMovementInput(InputAction.CallbackContext context)
        {
            m_MovementInput = context.ReadValue<Vector2>();
        }

        private void OnJumpInput(InputAction.CallbackContext context)
        {
            m_IsJumpPressed = context.ReadValueAsButton();
        }
    }
}