using Godot;
using System;
using Godot.Collections;

public partial class HurtBox : Area2D {
    [Signal]
    public delegate void HurtEventHandler(HitBox hitBox);
    
    public Array<HitBox> HitBoxArray { get; private set; }
    public Timer HurtIntervalTimer { get; private set; }
    public override void _Ready() {
        HitBoxArray = new Array<HitBox>();
        HurtIntervalTimer = GetNode<Timer>("HurtIntervalTimer");
        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;
        HurtIntervalTimer.Timeout += CheckShouldEmitHurtSignal;
    }

    private void OnAreaEntered(Area2D area) {
        if (area is HitBox hitBox && Owner != hitBox.Owner) {
            HitBoxArray.Add(hitBox);
        }
        CheckShouldEmitHurtSignal();

    }

    private void CheckShouldEmitHurtSignal() {
        if (HitBoxArray.Count > 0 && HurtIntervalTimer.IsStopped()) {
            for (int i = 0; i < HitBoxArray.Count; i++) {
                EmitSignal(SignalName.Hurt, HitBoxArray[i]);
                // HitBoxArray[i].EmitSignal(HitBox.SignalName.Hit, this);
            }
            HurtIntervalTimer.Start();
        }
    }

    private void OnAreaExited(Area2D area) {
        var hitBox  = area as HitBox;
        HitBoxArray.Remove(hitBox);
    }
}
