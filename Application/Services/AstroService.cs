// using CosineKitty;
// using Domain.Shared.Schemas;

// namespace Application.Services;

// public interface IAstronomyComputeService
// {
//     /// <summary>
//     /// Computes all derived fields for an Observation row.
//     /// Midpoint is resolved as: (start+end)/2, or start+(exposure/2), or start.
//     ///
//     /// Outputs mapped directly to entity:
//     ///   JdMid   ← JD of midpoint in UT (decimal 15,6)
//     ///   BjdTdb  ← Barycentric JD in TDB via Rømer correction (decimal 15,7)
//     ///   AltDeg  ← Target altitude at midpoint (decimal 8,4)
//     ///   AzDeg   ← Target azimuth at midpoint, N=0 (decimal 8,4)
//     ///   Airmass ← Pickering formula, clamped to 5.0, null below 5° (decimal 6,4)
//     /// </summary>
//     ObservationAstroData ComputeObservationData(
//         DateTimeOffset startUtc,
//         DateTimeOffset? endUtc,
//         decimal? exposureTimeS,
//         double raDeg,
//         double decDeg,
//         double siteLat,
//         double siteLon,
//         double siteAltM);
 
//     /// <summary>
//     /// Computes all derived fields for an ObservationSession row.
//     /// All values are computed at session start time.
//     ///
//     /// Outputs mapped to entity:
//     ///   JulianDateStart ← JD of StartTimeUtc
//     ///   JulianDateEnd   ← JD of EndTimeUtc (null when session still open)
//     ///   MoonPhasePct    ← 0–100, illuminated fraction
//     ///   MoonAltDeg      ← Moon altitude above horizon
//     ///   MoonsetUtc      ← next moonset within 24 h; null if circumpolar
//     /// </summary>
//     SessionAstroData ComputeSessionData(
//         DateTimeOffset startUtc,
//         DateTimeOffset? endUtc,
//         double siteLat,
//         double siteLon,
//         double siteAltM);
 
//     /// <summary>
//     /// Computes EventVisibility fields for one site.
//     /// Returns IsVisible=false (all nulls) when raDeg/decDeg are null —
//     /// handle solar-system events (Event.MpcDesignation set) separately.
//     ///
//     /// Rise/set/transit are approximated at ~10-min resolution via altitude
//     /// sampling over 24 h; sufficient for club event display, not for
//     /// precise contact timing (use Horizons for that).
//     /// </summary>
//     EventVisibilityData ComputeEventVisibility(
//         DateTimeOffset peakUtc,
//         double? raDeg,
//         double? decDeg,
//         double siteLat,
//         double siteLon,
//         double siteAltM);
 
//     /// <summary>
//     /// Returns the astronomical dark window (-18° twilight boundaries) for a
//     /// given date and site. Use this to populate the session planning UI and
//     /// to gate "can we observe tonight?" checks.
//     ///
//     /// Returns nulls in the record for polar summer/winter edge cases.
//     /// </summary>
//     AstroDarkWindow GetDarkWindow(
//         DateOnly date,
//         double siteLat,
//         double siteLon,
//         double siteAltM);
// }

// /// <summary>
// /// Implements IAstronomyComputeService using CosineKitty.AstronomyEngine 2.1.x.
// /// Singleton: stateless, thread-safe, no DI dependencies.
// /// </summary>
// /// 
// /// 
// public sealed class AstronomyComputeService : IAstronomyComputeService
// {
//     // J2000.0 = JD 2451545.0 — the zero epoch of AstroTime.ut and AstroTime.tt
//     private const double J2000Jd = 2451545.0;

//     // Speed of light in AU/day — used for the Rømer delay in BJD_TDB
//     private const double SpeedOfLightAuPerDay = 173.144_632_6;

//     // Needed to convert AstroTime.ut (days since J2000.0) back to a DateTime
//     private static readonly DateTime J2000Epoch =
//         new(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc);

//     // ──────────────────────────────────────────────────────────────────────────
//     // IAstronomyComputeService
//     // ──────────────────────────────────────────────────────────────────────────

//     public ObservationAstroData ComputeObservationData(
//         DateTimeOffset startUtc,
//         DateTimeOffset? endUtc,
//         decimal? exposureTimeS,
//         double raDeg,
//         double decDeg,
//         double siteLat,
//         double siteLon,
//         double siteAltM)
//     {
//         var midUtc = ResolveMidpoint(startUtc, endUtc, exposureTimeS);
//         var t      = new AstroTime(midUtc.UtcDateTime);

//         var jdMid  = ToJd(t);
//         var bjdTdb = ComputeBjdTdb(t, raDeg, decDeg);
//         var (alt, az, air) = ComputeAltAzAirmass(t, raDeg, decDeg, siteLat, siteLon, siteAltM);

//         return new ObservationAstroData(jdMid, bjdTdb, alt, az, air);
//     }

//     public SessionAstroData ComputeSessionData(
//         DateTimeOffset startUtc,
//         DateTimeOffset? endUtc,
//         double siteLat,
//         double siteLon,
//         double siteAltM)
//     {
//         var tStart   = new AstroTime(startUtc.UtcDateTime);
//         var observer = new Observer(siteLat, siteLon, siteAltM);

//         var jdStart = ToJd(tStart);
//         var jdEnd   = endUtc.HasValue
//             ? ToJd(new AstroTime(endUtc.Value.UtcDateTime))
//             : (decimal?)null;

//         // Moon illumination — phase_fraction ∈ [0, 1]
//         var illum        = Astronomy.Illumination(Body.Moon, tStart);
//         var moonPhasePct = (decimal)Math.Round(illum.phase_fraction * 100.0, 2);

//         // Moon altitude at session start
//         var moonEq = Astronomy.Equator(
//             Body.Moon, tStart, observer, EquatorEpoch.OfDate, Aberration.Corrected);
//         var moonHz = Astronomy.Horizon(
//             tStart, observer, moonEq.ra, moonEq.dec, Refraction.Normal);
//         var moonAltDeg = (decimal)Math.Round(moonHz.altitude, 2);

//         // Next moonset within 24 h; null if the moon doesn't set (circumpolar near poles)
//         DateTimeOffset? moonsetUtc = null;
//         try
//         {
//             var moonSet = Astronomy.SearchRiseSet(
//                 Body.Moon, observer, Direction.Set, tStart, 1.0);
//             if (moonSet is not null)
//                 moonsetUtc = ToDateTimeOffset(moonSet);
//         }
//         catch (AstronomyException)
//         {
//             // Circumpolar moon or no set in window — leave null
//         }

//         return new SessionAstroData(jdStart, jdEnd, moonPhasePct, moonAltDeg, moonsetUtc);
//     }

//     public EventVisibilityData ComputeEventVisibility(
//         DateTimeOffset peakUtc,
//         double? raDeg,
//         double? decDeg,
//         double siteLat,
//         double siteLon,
//         double siteAltM)
//     {
//         // Solar-system events without fixed coords — caller must handle separately
//         if (raDeg is null || decDeg is null)
//             return new EventVisibilityData(false, null, null, null, null, null, null);

//         var tPeak = new AstroTime(peakUtc.UtcDateTime);
//         var (altAtPeak, azAtPeak, _) = ComputeAltAzAirmass(
//             tPeak, raDeg.Value, decDeg.Value, siteLat, siteLon, siteAltM);

//         bool    isVisible    = altAtPeak.HasValue && altAtPeak.Value > 5m;
//         decimal? azimuthPeak = azAtPeak.HasValue
//             ? (decimal)Math.Round((double)azAtPeak.Value, 3)
//             : null;

//         var (riseUtc, setUtc, bestViewingUtc, maxAlt) = SampleRiseTransitSet(
//             peakUtc, raDeg.Value, decDeg.Value, siteLat, siteLon, siteAltM);

//         decimal? durationMin = null;
//         if (riseUtc.HasValue && setUtc.HasValue)
//             durationMin = (decimal)Math.Round(
//                 (setUtc.Value - riseUtc.Value).TotalMinutes, 2);

//         return new EventVisibilityData(
//             isVisible, riseUtc, setUtc, bestViewingUtc, maxAlt, azimuthPeak, durationMin);
//     }

//     public AstroDarkWindow GetDarkWindow(
//         DateOnly date,
//         double siteLat,
//         double siteLon,
//         double siteAltM)
//     {
//         var observer = new Observer(siteLat, siteLon, siteAltM);

//         // Start search at local noon UTC of the given date
//         var noon = new AstroTime(
//             date.ToDateTime(new TimeOnly(12, 0, 0), DateTimeKind.Utc));

//         DateTimeOffset? duskEnd    = null;
//         DateTimeOffset? dawnStart  = null;

//         // Evening: sun descends through -18° (astronomical dusk)
//         try
//         {
//             var dusk = Astronomy.SearchAltitude(
//                 Body.Sun, observer, Direction.Set, noon, 1.0, -18.0);
//             if (dusk is not null)
//                 duskEnd = ToDateTimeOffset(dusk);
//         }
//         catch (AstronomyException) { /* polar summer: no astronomical darkness */ }

//         // Morning: sun ascends back through -18° (astronomical dawn)
//         // Search from 18:00 UTC (past midnight in most longitudes)
//         try
//         {
//             var midnight = new AstroTime(
//                 date.ToDateTime(new TimeOnly(18, 0, 0), DateTimeKind.Utc));
//             var dawn = Astronomy.SearchAltitude(
//                 Body.Sun, observer, Direction.Rise, midnight, 1.0, -18.0);
//             if (dawn is not null)
//                 dawnStart = ToDateTimeOffset(dawn);
//         }
//         catch (AstronomyException) { /* polar winter: continuous darkness — dawnStart stays null */ }

//         return new AstroDarkWindow(duskEnd, dawnStart);
//     }

//     // ──────────────────────────────────────────────────────────────────────────
//     // Core private helpers
//     // ──────────────────────────────────────────────────────────────────────────

//     /// <summary>
//     /// Converts J2000 RA/Dec to topocentric horizontal coordinates.
//     /// AstronomyEngine's Horizon() accepts J2000 RA/Dec directly and applies
//     /// diurnal aberration and atmospheric refraction internally.
//     ///
//     /// raDeg is converted to hours before passing (library expects hours).
//     /// Airmass uses the Pickering (2002) formula — more stable than sec(z) near horizon.
//     /// Returns null airmass when target is below 5°.
//     /// </summary>
//     private static (decimal? alt, decimal? az, decimal? airmass) ComputeAltAzAirmass(
//         AstroTime t,
//         double raDeg,
//         double decDeg,
//         double siteLat,
//         double siteLon,
//         double siteAltM)
//     {
//         var observer = new Observer(siteLat, siteLon, siteAltM);
//         double raHours = raDeg / 15.0;

//         var hz = Astronomy.Horizon(t, observer, raHours, decDeg, Refraction.Normal);

//         var alt = (decimal)Math.Round(hz.altitude, 4);
//         var az  = (decimal)Math.Round(hz.azimuth,  4);

//         decimal? airmass = null;
//         if (hz.altitude >= 5.0)
//         {
//             // Pickering (2002): X = 1 / (sin(h) + 0.50572 * (h + 6.07995)^(-1.6364))
//             double h   = hz.altitude;
//             double raw = 1.0 / (Math.Sin(h * Math.PI / 180.0)
//                               + 0.50572 * Math.Pow(h + 6.07995, -1.6364));
//             airmass = (decimal)Math.Round(Math.Min(raw, 5.0), 4);
//         }

//         return (alt, az, airmass);
//     }

//     /// <summary>
//     /// Barycentric Julian Date in Terrestrial Dynamical Time (BJD_TDB) using
//     /// the Rømer light-travel-time correction:
//     ///   BJD_TDB = JD_TT  +  (r_⊕ · û_target) / c
//     ///
//     /// The periodic TDB-TT term (amplitude ~1.7 ms, period ~1 year) is omitted.
//     /// This is acceptable for variable-star photometry submitted to AAVSO (which
//     /// requests BJD_TDB to 0.0001 d precision). Do NOT use for pulsar timing.
//     ///
//     /// r_⊕ comes from Astronomy.BaryState(Body.Earth) — the solar-system
//     /// barycentric position of Earth in AU (ICRS).
//     /// û_target is the unit vector toward the target in J2000 ICRS.
//     /// </summary>
//     private static decimal? ComputeBjdTdb(AstroTime t, double raDeg, double decDeg)
//     {
//         try
//         {
//             double jdTt = t.tt + J2000Jd;  // JD in Terrestrial Time ≈ TDB to 2 ms

//             var earthBary = Astronomy.BaryState(Body.Earth, t);

//             double raRad  = raDeg  * Math.PI / 180.0;
//             double decRad = decDeg * Math.PI / 180.0;
//             double cosDec = Math.Cos(decRad);
//             double ux = cosDec * Math.Cos(raRad);
//             double uy = cosDec * Math.Sin(raRad);
//             double uz = Math.Sin(decRad);

//             double roemerDays = (earthBary.x * ux +
//                                  earthBary.y * uy +
//                                  earthBary.z * uz) / SpeedOfLightAuPerDay;

//             return (decimal)Math.Round(jdTt + roemerDays, 7);
//         }
//         catch
//         {
//             // BaryState can throw if body not supported; return null rather than crashing
//             return null;
//         }
//     }

//     /// <summary>
//     /// Approximates rise/transit/set for a fixed deep-sky target by sampling
//     /// altitude every 10 minutes over 24 h centred on centreUtc.
//     ///
//     /// WHY NOT SearchRiseSet:
//     ///   Astronomy.SearchRiseSet() only accepts solar-system Body values.
//     ///   There is no library API for arbitrary RA/Dec rise-set calculation,
//     ///   so we approximate by finding altitude sign changes.
//     ///
//     /// Accuracy: ~10 min. Adequate for EventVisibility.RiseTimeUtc/SetTimeUtc.
//     /// For high-precision contact timings (eclipse ingress/egress), use
//     /// the JPL Horizons API instead.
//     /// </summary>
//     private static (DateTimeOffset? rise, DateTimeOffset? set,
//                     DateTimeOffset? transit, decimal? maxAlt)
//         SampleRiseTransitSet(
//             DateTimeOffset centreUtc,
//             double raDeg, double decDeg,
//             double siteLat, double siteLon, double siteAltM)
//     {
//         const int stepMinutes = 10;
//         const int totalSteps  = 144;  // 144 × 10 min = 24 h

//         var observer  = new Observer(siteLat, siteLon, siteAltM);
//         double raHours = raDeg / 15.0;
//         var origin    = centreUtc.AddHours(-12);

//         DateTimeOffset? rise    = null;
//         DateTimeOffset? set     = null;
//         DateTimeOffset? transit = null;
//         double maxAltD  = double.MinValue;
//         double prevAlt  = double.NaN;

//         for (int i = 0; i <= totalSteps; i++)
//         {
//             var sampleDto = origin.AddMinutes(i * stepMinutes);
//             var t  = new AstroTime(sampleDto.UtcDateTime);
//             var hz = Astronomy.Horizon(t, observer, raHours, decDeg, Refraction.Normal);
//             double alt = hz.altitude;

//             // Transit — record highest altitude sample
//             if (alt > maxAltD)
//             {
//                 maxAltD  = alt;
//                 transit  = sampleDto;
//             }

//             if (!double.IsNaN(prevAlt))
//             {
//                 // Rising through 0° (first crossing only)
//                 if (prevAlt < 0 && alt >= 0 && rise is null)
//                     rise = sampleDto;

//                 // Setting through 0° (first crossing after a rise)
//                 if (prevAlt >= 0 && alt < 0 && rise is not null && set is null)
//                     set = sampleDto;
//             }

//             prevAlt = alt;
//         }

//         decimal? maxAlt = maxAltD > double.MinValue
//             ? (decimal)Math.Round(maxAltD, 3)
//             : null;

//         return (rise, set, transit, maxAlt);
//     }

//     // ──────────────────────────────────────────────────────────────────────────
//     // Utility
//     // ──────────────────────────────────────────────────────────────────────────

//     private static DateTimeOffset ResolveMidpoint(
//         DateTimeOffset start, DateTimeOffset? end, decimal? exposureS)
//     {
//         if (end.HasValue)
//             return start + (end.Value - start) / 2;

//         if (exposureS.HasValue && exposureS.Value > 0)
//             return start.AddSeconds((double)exposureS.Value / 2.0);

//         return start;
//     }

//     // JD in Universal Time (matching the OBSERVATIONS.JdMid column semantics)
//     private static decimal ToJd(AstroTime t) =>
//         (decimal)Math.Round(t.ut + J2000Jd, 6);

//     // AstroTime → DateTimeOffset (UTC) via J2000 epoch offset
//     private static DateTimeOffset ToDateTimeOffset(AstroTime t) =>
//         new(J2000Epoch.AddDays(t.ut), TimeSpan.Zero);
// }