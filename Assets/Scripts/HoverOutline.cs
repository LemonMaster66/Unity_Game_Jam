using UnityEngine;

public class HoverOutline : Interactable
{
    public Outline outline;
    public float   TargetOutline = 0;
    public float   BlendOutline;


    void Awake()
    {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineWidth = 0;
    }

    void Update()
    {
        outline.OutlineWidth = Mathf.SmoothDamp(outline.OutlineWidth, TargetOutline, ref BlendOutline, 0.075f);
    }

    public override void MouseOver()
    {
        TargetOutline = 20f;
    }

    public override void MouseExit()
    {
        TargetOutline = 0f;
    }
}
