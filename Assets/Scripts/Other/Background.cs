using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    //Parallax Background

    public float parallaxEffect = .8f;
    public float OffsetPosX;

    private Transform cameraTransform;
    private Vector3 cameraLastPos;
    private float textureUnitSizeX;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        cameraLastPos = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;        //Calculate the size of texture per unit (width)
    }

    void LateUpdate()
    {
        //Background following camera
        Vector3 cameraMoveDistance = cameraTransform.position - cameraLastPos;
        transform.position += cameraMoveDistance * parallaxEffect;
        cameraLastPos = cameraTransform.position;

        if(Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            OffsetPosX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + OffsetPosX, transform.position.y);
        }
    }
}
