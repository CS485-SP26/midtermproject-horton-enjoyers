using Farming;

/// <summary>
/// Legacy component kept so existing prefabs don't lose their reference.
/// Pickup/drop logic now lives in ToolPickup (base class).
/// toolType defaults to WaterCan — override in Inspector if needed.
/// </summary>
public class WateringCan : ToolPickup
{
    protected override void Start()
    {
        if (toolType == ToolType.None)
            toolType = ToolType.WaterCan;

        base.Start();
    }
}
