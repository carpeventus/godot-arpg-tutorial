using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public float MoveSpeed = 60f;
	[Export] public int MaxHealth = 20;
	[Export] public float KnockedBackForce = 50f;
	[Export] public float CanNotMoveTimeWhenHurt = 0.3f;
	[Export] public float ImmuneTime = 1f;
	[Export] public Inventory Inventory { get; private set; }

	
	[Signal] public delegate void HealthChangedEventHandler(int currentHealth, int maxHealth);
	
	public Vector2 InputDirection { get; private set; }
	public int CurrentHealth { get; private set; }
	public HurtBox HurtBox { get; private set; }
	public AnimationPlayer AnimationPlayer { get; private set; }
	public AnimationPlayer EffectAnimationPlayer { get; private set; }
	public HitBox PlayerSword { get; private set; }
	public FaceDirection CurrentFaceDirection { get; private set; }
	private bool _canMove = true;
	private bool _isAttacking = false;

	public override void _Ready() {
		GetNodes();
		BindSignal();
		CurrentHealth = MaxHealth;
		CurrentFaceDirection = FaceDirection.SOUTH;
		EffectAnimationPlayer.Play("RESET");
		PlayerSword.Visible = false;
	}

	public override void _Process(double delta) {
		HandleInput();
		UpdateAnimations();
	}

	private void HandleInput() {
		InputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		UpdateFaceDirection();
		if (Input.IsActionJustPressed("attack")) {
			Attack();
		}
	}

	private async void Attack() {
		_isAttacking = true;
		if (CurrentFaceDirection == FaceDirection.SOUTH) {
			AnimationPlayer.Play("attack_down");
		} else if (CurrentFaceDirection == FaceDirection.NORTH) {
			AnimationPlayer.Play("attack_up");
		} else if (CurrentFaceDirection == FaceDirection.EAST) {
			AnimationPlayer.Play("attack_right");
		} else if (CurrentFaceDirection == FaceDirection.WEST){
			AnimationPlayer.Play("attack_left");
		}
		await ToSignal(AnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
		_isAttacking = false;
	} 
	

	private void BindSignal() {
		HurtBox.Hurt += OnHurt;
	}
	
	private void OnHurt(HitBox hitBox) {
		CurrentHealth -= 1;
		if (CurrentHealth < 0) {
			CurrentHealth = 0;
		}
		EmitSignal(SignalName.HealthChanged, CurrentHealth, MaxHealth);
		BeKnockedBack(hitBox.GlobalPosition);
		CanNotMoveWhenHurt();
		HurtBlink();
	}

	public void Heal(int increaseHp) {
		CurrentHealth += increaseHp;
		if (CurrentHealth > MaxHealth) {
			CurrentHealth = MaxHealth;
		}
		EmitSignal(SignalName.HealthChanged, CurrentHealth, MaxHealth);
	}


	private async void CanNotMoveWhenHurt() {
		_canMove = false;
		await ToSignal(GetTree().CreateTimer(CanNotMoveTimeWhenHurt), SceneTreeTimer.SignalName.Timeout);
		_canMove = true;
	}

	private async void HurtBlink() {
		EffectAnimationPlayer.Play("hurt");
		await ToSignal(GetTree().CreateTimer(ImmuneTime), SceneTreeTimer.SignalName.Timeout);
		EffectAnimationPlayer.Play("RESET");
	}
	

	private void BeKnockedBack(Vector2 enemyPosition) {
		// Y轴朝下的，因此使用Player的位置减去敌人的
		var knockedBack = (Position - enemyPosition).Normalized() * KnockedBackForce;
		Velocity = knockedBack;
		MoveAndSlide();
	}

	private void UpdateFaceDirection() {
		FaceDirection current = CurrentFaceDirection;
		if (InputDirection.Y > 0) {
			current = FaceDirection.SOUTH;
		}
		else if (InputDirection.Y < 0) {
			current = FaceDirection.NORTH;
		}
		else if (InputDirection.X > 0) {
			current = FaceDirection.EAST;
		}
		else if (InputDirection.X < 0){
			current = FaceDirection.WEST;
		}
		
		CurrentFaceDirection = current;
	}

	private void GetNodes() {
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		EffectAnimationPlayer = GetNode<AnimationPlayer>("EffectsAnimationPlayer");
		HurtBox = GetNode<HurtBox>("HurtBox");
		PlayerSword = GetNode<HitBox>("PlayerSword");
	}

	private void UpdateAnimations() {
		// attacking
		// 攻击动画是一次性的，不在这里持续播放
		if (_isAttacking) {
			return;
		}
		// idle
		if (Mathf.IsZeroApprox(InputDirection.Length())) {
			if (CurrentFaceDirection == FaceDirection.SOUTH) {
				AnimationPlayer.Play("idle_down");
			} else if (CurrentFaceDirection == FaceDirection.NORTH) {
				AnimationPlayer.Play("idle_up");
			} else if (CurrentFaceDirection == FaceDirection.EAST) {
				AnimationPlayer.Play("idle_right");
			}else if (CurrentFaceDirection == FaceDirection.WEST){
				AnimationPlayer.Play("idle_left");
			}
		}
		// move
		else {
			if (CurrentFaceDirection == FaceDirection.SOUTH) {
				AnimationPlayer.Play("walk_down");
			} else if (CurrentFaceDirection == FaceDirection.NORTH) {
				AnimationPlayer.Play("walk_up");
			} else if (CurrentFaceDirection == FaceDirection.EAST) {
				AnimationPlayer.Play("walk_right");
			}else if (CurrentFaceDirection == FaceDirection.WEST){
				AnimationPlayer.Play("walk_left");
			}
		}

	}

	public override void _PhysicsProcess(double delta) {
		if (!_canMove) {
			return;
		}
		Velocity = InputDirection.Normalized() * MoveSpeed;
		MoveAndSlide();
	}
}
