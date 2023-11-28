using Godot;
using System;

public partial class InventorySlotGui : Button {
	private Sprite2D _backgroundSprite;
	private CenterContainer _centerContainer;

	public ItemStackGui ItemStackGui { get; private set; }
	
	
	public override void _Ready() {
		_backgroundSprite = GetNode<Sprite2D>("Background");
		_centerContainer = GetNode<CenterContainer>("CenterContainer");

	}
	
	public void InsertItem(ItemStackGui itemStack) {
		ItemStackGui = itemStack;
		_backgroundSprite.Frame = 1;
		_centerContainer.AddChild(itemStack);
	}
}
