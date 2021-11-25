using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
    // Assign delegate {} to events to initialise them with an empty delegate
	// so we can skip the null check when we use them

    // Gameplay
    public event UnityAction<float> moveEvent = delegate { };
    public event UnityAction jumpEvent = delegate { };
    public event UnityAction jumpCanceledEvent = delegate { };
    public event UnityAction spaceEvent = delegate { };
    public event UnityAction crouchEvent = delegate { };
    public event UnityAction crouchCanceledEvent = delegate { };
    public event UnityAction zoomEvent = delegate { };

    private GameInput gameInput;

    private void OnEnable()
    {
        // Debug.Log("OnEnable Called, InputReader.cs");

        if (gameInput == null)
        {
            gameInput = new GameInput();
            gameInput.Gameplay.SetCallbacks(this);
        }

        EnableGameplayInput();
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveEvent.Invoke(context.ReadValue<float>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
		{
			case InputActionPhase.Performed:
				jumpEvent.Invoke();
				break;
			case InputActionPhase.Canceled:
				jumpCanceledEvent.Invoke();
				break;
		}
    }

    public void OnSpace(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            spaceEvent.Invoke();
    }
    
    public void OnCrouch(InputAction.CallbackContext context)
    {
        switch (context.phase)
		{
			case InputActionPhase.Performed:
				crouchEvent.Invoke();
				break;
			case InputActionPhase.Canceled:
				crouchCanceledEvent.Invoke();
				break;
		}
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            zoomEvent.Invoke();
    }

    private void EnableGameplayInput()
    {
        // Debug.Log("EnableGameplayInput Called, InputReader.cs");
        gameInput.Enable();
        // gameInput.Gameplay.Enable(); 
    }

    private void DisableAllInput()
    {
        gameInput.Disable();
    }

    public void EnablePlayerInput()
    {
        EnableGameplayInput();
    }

    public void DisablePlayerInput()
    {
        DisableAllInput();
    }

}

