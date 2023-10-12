using Godot;
using System;

public partial class Heart : Panel {
	public Sprite2D Sprite { get; private set; }
	public override void _Ready() {
		Sprite = GetNode<Sprite2D>("Sprite2D");
	}

	
}
