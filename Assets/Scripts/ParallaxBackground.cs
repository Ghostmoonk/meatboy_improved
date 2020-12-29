using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] Vector2 parallaxEffectMultiplier;
    Transform cameraTransform;
    Vector3 lastCameraPos;
    float textureUnitSizeX;
    float textureUnitSizeY;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPos = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPos;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPos = cameraTransform.position;

        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            transform.position = new Vector3(cameraTransform.position.x, transform.position.y);
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
        }

        if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
        {
            transform.position = new Vector3(cameraTransform.position.x, transform.position.y);
            float offsetPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
            transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offsetPositionY);
        }
    }
}
