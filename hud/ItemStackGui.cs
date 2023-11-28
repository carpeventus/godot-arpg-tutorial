using Godot;

public partial class ItemStackGui : Panel {
    private Sprite2D _itemSprite;
    private Label _label;
    public InventorySlot InventorySlot { set; get; }

    public override void _Ready() {
        _itemSprite = GetNode<Sprite2D>("Item");
        _label = GetNode<Label>("Label");
    }

    public void UpdateSlot() {
        if (InventorySlot?.InventoryItem is null) {
            return;
        }


        _itemSprite.Visible = true;
        _itemSprite.Texture = InventorySlot.InventoryItem.Texture;
        if (InventorySlot.Amount > 1) {
            _label.Visible = true;
            _label.Text = InventorySlot.Amount.ToString();
        }
        else {
            _label.Visible = false;
        }
    }
}