using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    public Material material;

    private void Start()
    {
        material.SetColor("_RandomColor", new Color(Random.value, Random.value, Random.value, 0f));
    }
}
