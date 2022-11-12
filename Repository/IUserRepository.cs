using AuthenticationService.UserModel;

namespace AuthenticationService.Repository
{
    public interface IUserRepository
    {
        string Signup(UserDetail userDetail);
        bool Signin(UserDetail userDetail);

    }
}
