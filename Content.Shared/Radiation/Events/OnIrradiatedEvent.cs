namespace Content.Shared.Radiation.Events;

/// <summary>
///     Raised on entity when it was irradiated
///     by some radiation source.
/// </summary>
<<<<<<< HEAD
public readonly record struct OnIrradiatedEvent(float FrameTime, float RadsPerSecond, EntityUid? Origin)
=======
public readonly record struct OnIrradiatedEvent(float FrameTime, float RadsPerSecond)
>>>>>>> parent of c43f3d500d (3mo xeno archeology (first phase) (#33370))
{
    public readonly float FrameTime = FrameTime;

    public readonly float RadsPerSecond = RadsPerSecond;

<<<<<<< HEAD
    public readonly EntityUid? Origin = Origin;

=======
>>>>>>> parent of c43f3d500d (3mo xeno archeology (first phase) (#33370))
    public float TotalRads => RadsPerSecond * FrameTime;
}
