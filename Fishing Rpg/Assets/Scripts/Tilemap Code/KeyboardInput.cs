using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardInput : MonoBehaviour
{
    public Player Player;
    
    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        
        
        if (keyboard.wKey.isPressed)
        {
            Player.Move(Vector2.up);
        }
        if (keyboard.sKey.isPressed)
        {
            Player.Move(Vector2.down);
        }
        if (keyboard.aKey.isPressed)
        {
            Player.Move(Vector2.left);
        }
        if (keyboard.dKey.isPressed)
        {
            Player.Move(Vector2.right);
        }
        
        
        if (keyboard.eKey.wasPressedThisFrame)
        {
            Player.Interact();
        }
    }
}