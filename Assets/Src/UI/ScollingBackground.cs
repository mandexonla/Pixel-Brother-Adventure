using UnityEngine;

public class ScollingBackground : MonoBehaviour
{
    [SerializeField] private float ScrollSpeed;
    [SerializeField] private Renderer bgRender;

    // Update is called once per frame
    void Update()
    {
        bgRender.material.mainTextureOffset += new Vector2(0f, ScrollSpeed * Time.deltaTime);
    }
}
