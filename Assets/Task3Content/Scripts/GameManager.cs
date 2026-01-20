using UnityEngine;
using UnityEngine.InputSystem;

namespace GameTask3
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        
        public PlayerInputActions Input {  get; private set; }


        void Awake()
        {
           if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            instance = this;
            
            Input = new PlayerInputActions();
        }
        void OnEnable()
        {
            Input.Gameplay.Enable();
        }

        private void OnDisable()
        {
            Input.Gameplay.Disable();
        }
    }
}
