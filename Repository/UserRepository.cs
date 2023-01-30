using AuthenticationService.UserModel;
using System.Text.RegularExpressions;

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
                Regex r = new Regex(@"^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}");
                var user = _context.UserDetails.Where(p => p.UserName == userDetail.UserName).FirstOrDefault();

                if (user != null)
                {
                    return "1";
                }
                if (userDetail != null)
                {
                    //if (r.IsMatch(userDetail.Phone.ToString()))
                    //{
                    //    _context.UserDetails.Add(new UserDetail { Phone = userDetail.Phone, Password = userDetail.Password});
                    //    _context.SaveChanges();
                    //    return "2";
                    //}
                    _context.UserDetails.Add(userDetail);
                    _context.SaveChanges();
                    return "2";
                }
                else
                {
                    return "3";
                }
            }
            catch (Exception ex)

            {
                System.Diagnostics.Debug.WriteLine(ex);
                return "4";
            }
        }

        public bool Signin(UserDetail userDetail)
        {
            throw new NotImplementedException();
        }

        public void Signup()
        {
            throw new NotImplementedException();
        }
    }
}
