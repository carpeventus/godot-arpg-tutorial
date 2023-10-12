using Godot;
using System;

public partial class Slime : CharacterBody2D {
	private AnimationPlayer _animationPlayer;
	public Vector2 StartPosition { get; private set; }
	public Vector2 EndPosition { get; private set; }
	public HurtBox HurtBox { get; private set; }
	public HitBox HitBox { get; private set; }
	public int CurrentHealth { get; private set; }
	public bool IsDead { get; private set; }
	
	[Export] public Marker2D EndPoint;
	[Export] public float MoveSpeed;
	[Export] public float DistanceThreshold;
	[Export] public int MaxHealth = 3;
	public override void _Ready() {
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		HurtBox = GetNode<HurtBox>("HurtBox");
		HitBox = GetNode<HitBox>("HitBox");
		HurtBox.Hurt += OnHurt;
		StartPosition = Position;
		EndPosition = EndPoint.GlobalPosition;
		CurrentHealth = MaxHealth;
	}

	private void OnHurt(HitBox hitBox) {
		CurrentHealth -= 1;
		if (CurrentHealth < 0) {
			CurrentHealth = 0;
			Death();
		}
	}

	private async void Death() {
		IsDead = true;
		// 本帧结尾调用设置字段方法，这里要用小写的...
		HitBox.SetDeferred("monitorable", false);
		_animationPlayer.Play("death");
		await ToSignal(_animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
		QueueFree();
	}
	private void UpdateAnimations() {
		if (IsDead) {
			return;
		}
		// 移动
		if (Velocity.Y > 0) {
			_animationPlayer.Play("walk_down");
		} else if (Velocity.Y < 0) {
			_animationPlayer.Play("walk_up");
		} else if (Velocity.X > 0) {
			_animationPlayer.Play("walk_right");
		}else if (Velocity.X < 0){
			_animationPlayer.Play("walk_left");
		}

	}
	public override void _Process(double delta) {
		UpdateAnimations();
	}

	private void ChangeDirection() {
		(StartPosition, EndPosition) = (EndPosition, StartPosition);
	}
	public override void _PhysicsProcess(double delta) {
		var direction = EndPosition - Position;
		// 接近终点时，转向
		if (direction.Length() < DistanceThreshold) {
			ChangeDirection();
		}
		Velocity = direction.Normalized() * MoveSpeed;
		MoveAndSlide();
	}
}
