namespace Domain.Shared.Schemas;

/// <summary>
/// Computed values written to Observation.JdMid, BjdTdb, AltDeg, AzDeg, Airmass.
/// Created by IAstronomyComputeService; mapped into the entity before save.
/// </summary>
public sealed record ObservationAstroData(
    decimal  JdMid,
    decimal? BjdTdb,
    decimal? AltDeg,
    decimal? AzDeg,
    decimal? Airmass);

/// <summary>
/// Computed values written to ObservationSession.JulianDateStart/End,
/// MoonPhasePct, MoonAltDeg, MoonsetUtc.
/// </summary>
public sealed record SessionAstroData(
    decimal          JulianDateStart,
    decimal?         JulianDateEnd,
    decimal          MoonPhasePct,
    decimal          MoonAltDeg,
    DateTimeOffset?  MoonsetUtc);

/// <summary>
/// Computed values written to a single EventVisibility row (one site × one event).
/// </summary>
public sealed record EventVisibilityData(
    bool             IsVisible,
    DateTimeOffset?  RiseTimeUtc,
    DateTimeOffset?  SetTimeUtc,
    DateTimeOffset?  BestViewingUtc,
    decimal?         MaxAltDeg,
    decimal?         AzimuthAtPeakDeg,
    decimal?         DurationMinutes);

/// <summary>
/// Astronomical dark window for a given night and site.
/// DuskEnd   = moment sun drops below -18° (evening).
/// DawnStart = moment sun rises back through -18° (morning).
/// Both are null in polar summer; only DawnStart is null in continuous polar night.
/// Used for session planning — not persisted directly, but feeds UI and planning logic.
/// </summary>
public sealed record AstroDarkWindow(
    DateTimeOffset? DuskEnd,
    DateTimeOffset? DawnStart)
{
    /// <summary>True when the site has at least some astronomical darkness this night.</summary>
    public bool HasDarkness => DuskEnd.HasValue && DawnStart.HasValue;

    /// <summary>Total dark minutes available, or null when HasDarkness is false.</summary>
    public double? DarkMinutes =>
        HasDarkness
            ? (DawnStart!.Value - DuskEnd!.Value).TotalMinutes
            : null;
}