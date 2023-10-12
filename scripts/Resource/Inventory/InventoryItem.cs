using Godot;

[GlobalClass]
// 没有在这个类中新增amount字段，是因为Resource只会被加载一次，后续加载都会拿到第一次加载的结果。
// 如果有其他Player或NPC拾取了库存物品，并对amount+1，那么所有人的背包中此物品都会+1
public partial class InventoryItem : Resource {
    [Export] public string Name;
    [Export] public Texture2D Texture;
    [Export] public int MaxAmount;
}
