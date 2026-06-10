using Application.Repositories;
using Data.Entities.Generated;


namespace Application.Services;

public interface IProjectService
{

}

public sealed class ProjectService(
    IBaseRepository<Project> projectRepository) : IProjectService
{
    
}
