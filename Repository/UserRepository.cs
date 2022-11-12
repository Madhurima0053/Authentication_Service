using AuthenticationService.UserModel;

namespace AuthenticationService.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDetailContext _context;   
        public UserRepository(UserDetailContext context)
        {
            _context = context;
        }
        public string Signup(UserDetail userDetail)
        {

            try
            {
                var user = _context.UserDetails.Where(p => p.UserName == userDetail.UserName).FirstOrDefault();

                if (user != null)
                {
                    return "1";
                }
                if (userDetail != null)
                {
                    _context.UserDetails.Add(userDetail);
                    _context.SaveChanges();
                    return "2";
                }
                else
                {
                    return "3";
                }
            }
            catch (Exception)
            {
                return "4";
            }
        }

        public bool Signin(UserDetail userDetail)
        {
            throw new NotImplementedException();
        }
    }
}
