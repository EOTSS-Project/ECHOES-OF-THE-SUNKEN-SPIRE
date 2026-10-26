using UnityEngine;

// Shows a line of story the first time the player walks into a new section.
[RequireComponent(typeof(BoxCollider))]
public class SectionHint : MonoBehaviour
{
    [SerializeField, TextArea] private string text;

    private bool shown;

    void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (shown || other.GetComponentInParent<PlayerRespawn>() == null) return;
        shown = true;
        GameEvents.RaiseHint(text);
    }
}
