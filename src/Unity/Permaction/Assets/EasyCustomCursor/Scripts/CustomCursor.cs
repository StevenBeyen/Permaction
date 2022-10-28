using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    [Header("Sprite Sizes")]
    [Tooltip("The width in pixels of the sprite that will be used as cursor.")]
    public float SpriteWidth;
    [Tooltip("The height in pixels of the sprite that will be used as cursor.")]
    public float SpriteHeight;
    
    [Header("Cursor Sprites")]
    [Tooltip("Sprite for when the cursor is in its default state.")]
    public Sprite DefaultState;
    [Tooltip("Sprite for when the left mouse button is held. If no sprite is set, the default will be used.")]
    public Sprite LeftClickHeld;
    [Tooltip("Sprite for when the left mouse button is held. If no sprite is set, the default will be used.")]
    public Sprite RightClickHeld;

    [Space(10)]
    [Tooltip("Sprite for when the scroll button is used for scrolling. If no sprite is set, the default will be used.")]
    public Sprite Scrolling;
    [Tooltip("Sprite for when the scroll button is held. If no sprite is set, the default will be used.")]
    public Sprite ScrollButtonHeld;

    [Space(10)]
    [Tooltip("If set, activates this particle system at the cursor, creating a trail.")]
    public ParticleSystem ParticleSystem;

    private Image _cursorImage;
    private CustomCursorInputs _controls;
    private bool _usingInputSystem;

    private void Awake()
    {
        _controls = new CustomCursorInputs();
        
        // Left-click
        _controls.CustomCursor.LeftClicking.performed += _ => OnLeftClick();
        _controls.CustomCursor.LeftClicking.canceled += _ => Invoke("SetDefaultState", 0.15f);
        
        // Right-click
        _controls.CustomCursor.RightClicking.performed += _ => OnRightClick();
        _controls.CustomCursor.RightClicking.canceled += _ => Invoke("SetDefaultState", 0.15f);
        
        // Scroll button clicked
        _controls.CustomCursor.ScrollClicking.performed += _ => OnScrollClick();
        _controls.CustomCursor.ScrollClicking.canceled += _ => Invoke("SetDefaultState", 0.15f);
        
        // Scrolling
        _controls.CustomCursor.Scrolling.performed += _ => OnScrolling();
        _controls.CustomCursor.Scrolling.canceled += _ => Invoke("SetDefaultState", 0.5f);

        _cursorImage = GetComponent<Image>();
        if (DefaultState == null)
        {
            throw new Exception("You need to set the default Sprite!");
        }
        
        // Set the cursor to the default state
        SetCurrSprite(DefaultState);
        
        if (ParticleSystem != null)
        {
            // Place the particle system on the cursor and start playing it
            ParticleSystem.transform.position = this.transform.position;
            ParticleSystem.transform.parent = this.transform;
            ParticleSystem.Play();
        }
    }
    
    void Update()
    {
        Cursor.visible = false;
        Vector3 CursorPos = Mouse.current.position.ReadValue();
        _cursorImage.transform.position = CursorPos;
    }

    private void SetCurrSprite(Sprite newSprite)
    {
        if (newSprite == null)
        {
            return;
        }
        _cursorImage.sprite = newSprite;
        _cursorImage.rectTransform.sizeDelta = new Vector2(SpriteWidth, SpriteHeight);
    }
    
    private void OnLeftClick()
    {
        SetCurrSprite(LeftClickHeld);
    }
    
    private void OnRightClick()
    {
       SetCurrSprite(RightClickHeld);
    }
    
    private void OnScrollClick()
    {
        SetCurrSprite(ScrollButtonHeld);
    }

    private void OnScrolling()
    {
        SetCurrSprite(Scrolling);
    }

    private void SetDefaultState()
    {
        SetCurrSprite(DefaultState);
    }

    private void OnEnable()
    {
         _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }
}
