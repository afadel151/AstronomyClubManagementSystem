using Application.Repositories;
using Data.Entities.Generated;


namespace Application.Services;

public interface ITargetService
{

}

public sealed class TargetService(
    IBaseRepository<Target> targetRepository) : ITargetService
{
    
}
