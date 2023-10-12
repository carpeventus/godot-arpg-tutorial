using Godot;
using System;

public partial class CollectableSword : Collectable {
	private AnimationPlayer _animationPlayer;
	public override void _Ready() {
		base._Ready();
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	protected override void Collected(Player player) {
		WaitForAnimationFinished(player);
	}
	
	private async void WaitForAnimationFinished(Player player) {
		_animationPlayer.Play("sword_spin");
		await ToSignal(_animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
		base.Collected(player);
	}
}
