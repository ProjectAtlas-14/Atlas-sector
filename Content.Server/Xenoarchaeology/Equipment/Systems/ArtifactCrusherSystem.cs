using Content.Server.Body.Systems;
<<<<<<< HEAD
using Content.Server.Stack;
=======
using Content.Server.Popups;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Server.Stack;
using Content.Server.Storage.Components;
using Content.Server.Xenoarchaeology.XenoArtifacts;
>>>>>>> parent of c43f3d500d (3mo xeno archeology (first phase) (#33370))
using Content.Shared.Body.Components;
using Content.Shared.Storage.Components;
using Content.Shared.Whitelist;
using Content.Shared.Xenoarchaeology.Equipment;
using Robust.Shared.Collections;
using Robust.Shared.Random;

namespace Content.Server.Xenoarchaeology.Equipment.Systems;

/// <inheritdoc/>
public sealed class ArtifactCrusherSystem : SharedArtifactCrusherSystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly ArtifactSystem _artifact = default!;
    [Dependency] private readonly BodySystem _body = default!;
    [Dependency] private readonly StackSystem _stack = default!;
    [Dependency] private readonly EntityWhitelistSystem _whitelistSystem = default!;

    // TODO: Move to shared once StackSystem spawning is in Shared and we have RandomPredicted
    public override void FinishCrushing(Entity<ArtifactCrusherComponent, EntityStorageComponent> ent)
    {
        var (_, crusher, storage) = ent;
        StopCrushing((ent, ent.Comp1), false);
        AudioSystem.PlayPvs(crusher.CrushingCompleteSound, ent);
        crusher.CrushingSoundEntity = null;
        Dirty(ent, ent.Comp1);

        var contents = new ValueList<EntityUid>(storage.Contents.ContainedEntities);
        var coords = Transform(ent).Coordinates;
        foreach (var contained in contents)
        {
            if (_whitelistSystem.IsWhitelistPass(crusher.CrushingWhitelist, contained))
            {
                var amount = _random.Next(crusher.MinFragments, crusher.MaxFragments);
                var stacks = _stack.SpawnMultipleAtPosition(crusher.FragmentStackProtoId, amount, coords);
                foreach (var stack in stacks)
                {
                    ContainerSystem.Insert((stack, null, null, null), crusher.OutputContainer);
                }
                _artifact.ForceActivateArtifact(contained);
            }

            if (!TryComp<BodyComponent>(contained, out var body))
                Del(contained);

            // DeltaV acidify: false preserves inventory items, which get added to the output container
            var gibs = _body.GibBody(contained, body: body, acidify: false);
            foreach (var gib in gibs)
            {
                ContainerSystem.Insert((gib, null, null, null), crusher.OutputContainer);
            }
        }
    }
}
