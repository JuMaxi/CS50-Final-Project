using PropagatingKindness.Domain.Services;

namespace PropagatingKindness.Test;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var service = new UserService(null);
        await service.Insert(new Domain.DTO.UserDTO() 
        {
            Password = "Batatinha"
        });
    }
}