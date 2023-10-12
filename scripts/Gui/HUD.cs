using Godot;
using System;

public partial class HUD : CanvasLayer {
    private InventoryGUI _inventoryGui;
    public override void _Ready() {
        _inventoryGui = GetNode<InventoryGUI>("InventoryGUI");
        _inventoryGui.CloseInventory();
    }

    public override void _Input(InputEvent @event) {
        if (@event.IsActionPressed("inventory")) {
            if (_inventoryGui.IsOpen) {
                _inventoryGui.CloseInventory();
            }
            else {
                _inventoryGui.OpenInventory();
            }
        }
    }
}
