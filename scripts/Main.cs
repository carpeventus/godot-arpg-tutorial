using Godot;
using System;

public partial class Main : Node2D {
	private Player _player;
	private HealthBarHud _healthBarHud;
	public override void _Ready() {
		_healthBarHud = GetNode<HealthBarHud>("HUD/HealthBarHUD");
		_player = GetNode<Player>("Player");
		_player.HealthChanged += OnHealthChanged;
		_healthBarHud.SetMaxHealth(_player.MaxHealth / HealthBarHud.HealthPerHeart);
		_healthBarHud.UpdateHealthBar(_player.CurrentHealth);
	}

	private void OnHealthChanged(int currentHealth, int maxHealth) {
		_healthBarHud.UpdateHealthBar(currentHealth);
	}
}
