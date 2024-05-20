using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] Texture2D _cursorTexture;

    // Expected to be the center of the texture
    Vector2 _cursorHotspot;
    Camera _mainCamera;

    void Start()
    {
        //_cursorHotspot = new Vector2(_cursorTexture.width / 2.0f, _cursorTexture.height / 2.0f);
        //Cursor.SetCursor(_cursorTexture, _cursorHotspot, CursorMode.Auto);
        Cursor.visible = false;
        _mainCamera = Camera.main;
    }

    void Update()
    {
        //Debug.Log("Camera.main.nearClipPlane: " + _mainCamera.nearClipPlane);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = new Vector3(mousePosition.x, mousePosition.y, _mainCamera.nearClipPlane);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, 0.0f);
        //Cursor.visible = false;
        Debug.Log("Cursor.visible: " + Cursor.visible);
    }

    // TODO: Name this class "CrosshairController"
    // public method:  UpdateCrosshairFromMouseMovement()
    // public method:  UpdateCrosshairFromControllerAim()
}
