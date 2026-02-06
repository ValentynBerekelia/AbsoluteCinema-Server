using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Domain.Specifications;

public class UserByEmailSpec : Specification<User>
{
    public UserByEmailSpec(string email)
    {
        Criteria = (u => u.Email == email);
        AsNoTracking = false;
    }
}