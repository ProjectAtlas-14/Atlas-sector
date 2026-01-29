<<<<<<<< HEAD:Content.Server/Xenoarchaeology/XenoArtifacts/Effects/XAEPortalSystem.cs
using Content.Server.Xenoarchaeology.Artifact.XAE.Components;
using Content.Shared.Mind.Components;
using Content.Shared.Mobs.Components;
using Content.Shared.Teleportation.Systems;
using Content.Shared.Xenoarchaeology.Artifact;
using Content.Shared.Xenoarchaeology.Artifact.XAE;
========
using Content.Server.Xenoarchaeology.XenoArtifacts.Effects.Components;
using Content.Server.Xenoarchaeology.XenoArtifacts.Events;
using Content.Shared.Mind.Components;
using Content.Shared.Mobs.Components;
using Content.Shared.Teleportation.Systems;
>>>>>>>> parent of c43f3d500d (3mo xeno archeology (first phase) (#33370)):Content.Server/Xenoarchaeology/XenoArtifacts/Effects/Systems/PortalArtifactSystem.cs
using Robust.Shared.Collections;
using Robust.Shared.Containers;
using Robust.Shared.Random;

<<<<<<<< HEAD:Content.Server/Xenoarchaeology/XenoArtifacts/Effects/XAEPortalSystem.cs
namespace Content.Server.Xenoarchaeology.Artifact.XAE;
========
namespace Content.Server.Xenoarchaeology.XenoArtifacts.Effects.Systems;
>>>>>>>> parent of c43f3d500d (3mo xeno archeology (first phase) (#33370)):Content.Server/Xenoarchaeology/XenoArtifacts/Effects/Systems/PortalArtifactSystem.cs

public sealed class PortalArtifactSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly LinkedEntitySystem _link = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;

    public override void Initialize()
    {
<<<<<<<< HEAD:Content.Server/Xenoarchaeology/XenoArtifacts/Effects/XAEPortalSystem.cs
        var map = Transform(ent).MapID;
========
        base.Initialize();
        SubscribeLocalEvent<PortalArtifactComponent, ArtifactActivatedEvent>(OnActivate);
    }

    private void OnActivate(Entity<PortalArtifactComponent> artifact, ref ArtifactActivatedEvent args)
    {
        var map = Transform(artifact).MapID;
>>>>>>>> parent of c43f3d500d (3mo xeno archeology (first phase) (#33370)):Content.Server/Xenoarchaeology/XenoArtifacts/Effects/Systems/PortalArtifactSystem.cs
        var validMinds = new ValueList<EntityUid>();
        var mindQuery = EntityQueryEnumerator<MindContainerComponent, MobStateComponent, TransformComponent, MetaDataComponent>();
        while (mindQuery.MoveNext(out var uid, out var mc, out _, out var xform, out var meta))
        {
            // check if the MindContainer has a Mind and if the entity is not in a container (this also auto excludes AI) and if they are on the same map
            if (mc.HasMind && !_container.IsEntityOrParentInContainer(uid, meta: meta, xform: xform) && xform.MapID == map)
            {
                validMinds.Add(uid);
            }
        }
        //this would only be 0 if there were a station full of AIs and no one else, in that case just stop this function
        if (validMinds.Count == 0)
            return;

<<<<<<<< HEAD:Content.Server/Xenoarchaeology/XenoArtifacts/Effects/XAEPortalSystem.cs
        if(!TrySpawnNextTo(ent.Comp.PortalProto, args.Artifact, out var firstPortal))
            return;

        var target = _random.Pick(validMinds);
        if(!TrySpawnNextTo(ent.Comp.PortalProto, target, out var secondPortal))
            return;

        // Manual position swapping, because the portal that opens doesn't trigger a collision, and doesn't teleport targets the first time.
        _transform.SwapPositions(target, args.Artifact.Owner);
========
        var firstPortal = Spawn(artifact.Comp.PortalProto, _transform.GetMapCoordinates(artifact));

        var target = _random.Pick(validMinds);

        var secondPortal = Spawn(artifact.Comp.PortalProto, _transform.GetMapCoordinates(target));

        //Manual position swapping, because the portal that opens doesn't trigger a collision, and doesn't teleport targets the first time.
        _transform.SwapPositions(target, artifact.Owner);
>>>>>>>> parent of c43f3d500d (3mo xeno archeology (first phase) (#33370)):Content.Server/Xenoarchaeology/XenoArtifacts/Effects/Systems/PortalArtifactSystem.cs

        _link.TryLink(firstPortal.Value, secondPortal.Value, true);
    }
}
