using Godot;
using System;

public partial class InventorySlotGui : Panel {
	private Sprite2D _backgroundSprite;
	private Sprite2D _itemSprite;
	private Label _label;
	
	public override void _Ready() {
		_backgroundSprite = GetNode<Sprite2D>("Background");
		_itemSprite = GetNode<Sprite2D>("CenterContainer/Panel/Item");
		_label = GetNode<Label>("CenterContainer/Panel/Label");
	}

	public void UpdateSlot(InventorySlot slot) {
		if (slot.InventoryItem is null) {
			_backgroundSprite.Frame = 0;
			_itemSprite.Visible = false;
			_label.Visible = false;
		}
		else {
			_backgroundSprite.Frame = 1;
			_itemSprite.Visible = true;
			_itemSprite.Texture = slot.InventoryItem.Texture;
			if (slot.Amount > 1) {
				_label.Visible = true;
				_label.Text = slot.Amount.ToString();
			}
		}
	}
}
