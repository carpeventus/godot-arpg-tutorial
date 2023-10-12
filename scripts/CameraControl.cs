using Godot;
using System;

public partial class CameraControl : Camera2D {
	[Export] public TileMap TileMap;
	public override void _Ready() {
		var usedRect = TileMap.GetUsedRect();
		Vector2I tileSize = TileMap.TileSet.TileSize;
		LimitTop = usedRect.Position.Y * tileSize.Y;
		LimitRight = usedRect.End.X * tileSize.X;
		LimitBottom = usedRect.End.Y * tileSize.Y;
		LimitLeft = usedRect.Position.X * tileSize.X;
		ResetSmoothing();
	}
}
