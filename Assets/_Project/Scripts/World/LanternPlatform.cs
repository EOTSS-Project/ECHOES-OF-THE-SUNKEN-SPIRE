using UnityEngine;

// A ledge born of light: only solid, and clearly visible, while the player's lantern is close.
public class LanternPlatform : MonoBehaviour
{
    [SerializeField] private Transform lantern;
    [SerializeField] private float lightRadius = 3.5f;
    [SerializeField] private float ghostAlpha = 0.15f;
    [SerializeField] private float fadeSpeed = 5f;

    private Collider col;
    private Renderer rend;
    private MaterialPropertyBlock block;
    private Color baseColor;
    private float alpha;

    public bool IsSolid => col.enabled;

    void Awake()
    {
        col = GetComponent<Collider>();
        rend = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();
        baseColor = rend.sharedMaterial.GetColor("_BaseColor");
        alpha = ghostAlpha;
    }

    void Update()
    {
        if (lantern == null) return;

        // Distance from the lantern to the nearest point of the ledge
        Vector3 nearest = rend.bounds.ClosestPoint(lantern.position);
        bool lit = Vector3.Distance(nearest, lantern.position) <= lightRadius;
        col.enabled = lit;

        alpha = Mathf.MoveTowards(alpha, lit ? 1f : ghostAlpha, fadeSpeed * Time.deltaTime);
        Color c = baseColor;
        c.a = alpha;
        block.SetColor("_BaseColor", c);
        rend.SetPropertyBlock(block);
    }
}
