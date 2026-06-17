namespace FourLines.Application.Services;

public class TestService(IStandardRepository<Role> rolesRepository)
{
    private readonly IStandardRepository<Role> rolesRepository = rolesRepository;

    public async Task<Role> GetSomething()
    {
        var allRoles = await rolesRepository.GetAllAsync();
        return allRoles.First();
    }
}
