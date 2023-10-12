using Godot;

public partial class HitBox : Area2D {
    [Signal]
    public delegate void HitEventHandler(HurtBox hurtBox);

}
