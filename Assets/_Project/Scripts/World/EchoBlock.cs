using UnityEngine;

// Echo blocks: two sets take turns being solid, like the Spire breathing.
// Set A uses phase 0, set B phase 0.5; solidFraction > 0.5 leaves a short overlap at each switch to land in.
// A block blinks for warnTime before it fades away.
public class EchoBlock : MonoBehaviour
{
    [SerializeField] private float period = 5f;            // one full breath, both sets
    [SerializeField, Range(0f, 1f)] private float phase = 0f;
    [SerializeField, Range(0.5f, 1f)] private float solidFraction = 0.56f;
    [SerializeField] private float warnTime = 0.7f;
    [SerializeField] private float ghostAlpha = 0.18f;

    private Collider col;
    private Renderer rend;
    private MaterialPropertyBlock block;
    private Color baseColor;

    public bool IsSolid { get; private set; }

    void Awake()
    {
        col = GetComponent<Collider>();
        rend = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();
        baseColor = rend.sharedMaterial.GetColor("_BaseColor");
    }

    void Update()
    {
        // Seconds since this block last became solid
        float t = Mathf.Repeat(Time.timeSinceLevelLoad / period - phase, 1f) * period;
        float solidFor = period * solidFraction;

        IsSolid = t < solidFor;
        col.enabled = IsSolid;

        float alpha = IsSolid ? 1f : ghostAlpha;
        if (IsSolid && solidFor - t < warnTime) alpha = Mathf.Repeat(t * 6f, 1f) < 0.5f ? 1f : 0.4f;

        Color c = baseColor;
        c.a = alpha;
        block.SetColor("_BaseColor", c);
        rend.SetPropertyBlock(block);
    }
}
