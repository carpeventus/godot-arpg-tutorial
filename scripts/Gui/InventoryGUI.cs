using Godot;
using Godot.Collections;

/**
 * 库存的视图
 */
public partial class InventoryGUI : Control {

    // 库存Slot的视图
    public Array<InventorySlotGui> InventorySlotGuiArray { get; private set; } = new();
    // 库存的数据
    public Inventory InventoryRes { get; private set; }
    
    public override void _Ready() {
        InventoryRes = ResourceLoader.Load<Inventory>("res://resources/inventory.tres");
        Array<Node> slotGuiArray = GetNode("NinePatchRect/GridContainer").GetChildren();
        foreach (var slotGui in slotGuiArray) {
            if (slotGui is InventorySlotGui inventorySlotGui) {
                InventorySlotGuiArray.Add(inventorySlotGui);
            }
        }

        InventoryRes.InventoryChanged += UpdateInventoryGui;
        UpdateInventoryGui();
    }
    private void UpdateInventoryGui() {
        for (var i = 0; i < InventoryRes.InventorySlots.Count; i++) {
            InventorySlotGuiArray[i].UpdateSlot(InventoryRes.InventorySlots[i]);
        }
    }

    public bool IsOpen { get; private set; }
    
    public void OpenInventory() {
        IsOpen = true;
        Visible = true;
        GetTree().Paused = true;
    }
    
    public void CloseInventory() {
        IsOpen = false;
        Visible = false;
        GetTree().Paused = false;
    }

}
