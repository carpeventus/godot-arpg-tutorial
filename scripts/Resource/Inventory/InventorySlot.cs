using Godot;
using System;

[GlobalClass]
public partial class InventorySlot : Resource {
    [Export] public InventoryItem InventoryItem;
    public int Amount { get; set; }

    public bool isFull() {
        return Amount >= InventoryItem.MaxAmount;
    }
}
