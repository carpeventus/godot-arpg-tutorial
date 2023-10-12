using Godot;

public partial class Collectable : Area2D {
	[Export] public InventoryItem InventoryItem;
	public override void _Ready() {
		BodyEntered += OnPlayerEntered;
	}

	private void OnPlayerEntered(Node2D node2D) {
		if (node2D is Player player) {
			Collected(player);
		}
	}

	protected virtual void Collected(Player player) {
		player.Inventory.AddItem(InventoryItem);
		QueueFree();
	}
}
