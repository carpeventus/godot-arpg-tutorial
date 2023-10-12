using Godot;
using System;
using Godot.Collections;

public partial class HealthBarHud : HBoxContainer {
	public const int HealthPerHeart = 2;
	
	public const int FullHeartFrame = 0;
	public const int HalfHeartFrame = 2;
	public const int EmptyHeartFrame = 4;
	private PackedScene _heartScene;
	public override void _Ready() {
		_heartScene = ResourceLoader.Load<PackedScene>("res://hud/heart.tscn");
	}

	public void SetMaxHealth(int heartCount) {
		for (int i = 0; i < heartCount; i++) {
			AddChild(_heartScene.Instantiate());
		}
	}
	
	public void UpdateHealthBar(int currentHealth) {
		Array<Node> hearts = GetChildren();
		int usedHeartCount = (currentHealth + 1) / HealthPerHeart;
		int heartLeft = currentHealth % HealthPerHeart;
		for (int i = 0; i < usedHeartCount; i++) {
			if (hearts[i] is Heart heart) {
				heart.Sprite.Frame = FullHeartFrame;
				if (i == usedHeartCount - 1) {
					heart.Sprite.Frame = heartLeft == 0 ? FullHeartFrame : HalfHeartFrame;
				}
			}
		}
		
		for (int i = usedHeartCount; i < hearts.Count; i++) {
			if (hearts[i] is Heart heart) {
				heart.Sprite.Frame = EmptyHeartFrame;
			}
		}
	}
	
}
