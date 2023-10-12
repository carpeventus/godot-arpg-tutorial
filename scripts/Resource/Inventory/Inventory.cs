using Godot;
using System;
using System.Linq;
using Godot.Collections;
using Array = Godot.Collections.Array;

[GlobalClass]
// 背包的数据存储层
public partial class Inventory : Resource {
    [Export] public Array<InventorySlot> InventorySlots;

    [Signal] public delegate void InventoryChangedEventHandler();
    public void AddItem(InventoryItem item) {
        var itemSlot = InventorySlots.Where(s => s.InventoryItem == item).ToList();
        foreach (var slot in itemSlot) {
            if (!slot.isFull()) {
                slot.Amount += 1;
                if (slot.isFull()) {
                    slot.Amount = item.MaxAmount;
                }
                EmitSignal(SignalName.InventoryChanged);
                return;
            }
        }
        // first time
        for (int i = 0; i < InventorySlots.Count; i++) {
            if (InventorySlots[i].InventoryItem is null) {
                InventorySlots[i].InventoryItem = item;
                InventorySlots[i].Amount = 1;
                EmitSignal(SignalName.InventoryChanged);
                return;
            }
        }
        
        
        
    }
}
