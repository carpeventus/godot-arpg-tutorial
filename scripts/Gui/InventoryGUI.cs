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

    public PackedScene ItemStackGuiScene { get; private set; }
    
    public override void _Ready() {
        InventoryRes = ResourceLoader.Load<Inventory>("res://resources/inventory.tres");
        ItemStackGuiScene =  ResourceLoader.Load<PackedScene>("res://hud/item_stack_gui.tscn");
        Array<Node> slotGuiArray = GetNode("NinePatchRect/GridContainer").GetChildren();
        foreach (var slotGui in slotGuiArray) {
            if (slotGui is InventorySlotGui inventorySlotGui) {
                InventorySlotGuiArray.Add(inventorySlotGui);
            }
        }

        CollectedSlots();
        InventoryRes.InventoryChanged += UpdateInventoryGui;
        UpdateInventoryGui();
    }
    private void UpdateInventoryGui() {
        for (var i = 0; i < InventoryRes.InventorySlots.Count; i++) {
            InventorySlot slotRes = InventoryRes.InventorySlots[i];
            if (slotRes.InventoryItem is null) {
                continue;
            }
            ItemStackGui stackGui = InventorySlotGuiArray[i].ItemStackGui;
            if (stackGui is null) {
                stackGui = ItemStackGuiScene.Instantiate<ItemStackGui>();
                InventorySlotGuiArray[i].InsertItem(stackGui);
            }

            stackGui.InventorySlot = slotRes;
            stackGui.UpdateSlot();
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


    private void CollectedSlots() {
        foreach (var slot in InventorySlotGuiArray) {
            slot.Pressed += () => {OnSlotClicked(slot);};
        }
    }
    private void OnSlotClicked(InventorySlotGui slotGui) {
        
    }
}
