// using Application.Repositories;
// using Data.Entities.Generated;
// using Domain.Shared.DTO;


// namespace Application.Services;

// public interface IObservationService
// {
//     Task<Observation> CreateAsync(
//         CreateObservationRequest request, CancellationToken ct = default);
 
//     System.Threading.Tasks.Task RecomputeAstroDataAsync(
//         int observationId, CancellationToken ct = default);
// }

// public sealed class ObservationService(
//     IBaseRepository<Observation>       obsRepo,
//     IBaseRepository<ObservationSession> sessionRepo,
//     IBaseRepository<Target>            targetRepo,
//     IBaseRepository<ObservationSite>   siteRepo,
//     IAstronomyComputeService           astro,
//     ICurrentUserService                currentUser
// ) : IObservationService
// {
    
// }
