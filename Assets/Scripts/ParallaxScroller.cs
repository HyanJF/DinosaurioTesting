using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    Material mat;
    float distance;

    [Range(0f, 2f)]
    public float speed = 0.2f;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }
    private void Update()
    {
        distance += Time.deltaTime * speed;
        mat.SetTextureOffset("_MainTex", Vector2.right * distance);
    }
}
