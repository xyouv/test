using Microsoft.IdentityModel.Tokens;
using PhoneManagement.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PhoneManagement.Service {


    public interface IAuthenticationService {
        ResponseLoginModel Authenticator(RequestLoginModel model);
        int Register(RequestRegisterModel model);
        int VerifyEmail(RequestVerifyEmailModel model);
        int ForgetPassword(RequestForgetPasswordModel model);
        int ResetPassword(RequestResetPasswordModel model);
    }
    public class AuthenticationService : IAuthenticationService {
        private readonly string Key = "qwertyuiopasdfghjklzxcvbnmasdasdasdasdasdasdasdas";
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IEmailService _emailService;
        public AuthenticationService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IEmailService emailService) {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _emailService = emailService;
        }

        public ResponseLoginModel Authenticator(RequestLoginModel model) {
            var account = _userRepository.getUser(model.Account, model.Password);
            //if (account == null) {
            //    throw
            //}
            var token = CreateJwtToken(account);
            var refreshToken = CreateRefreshToken(account);
            var result = new ResponseLoginModel {
                FullName = account.Name,
                UserId = account.Id,
                Token = token,
                RefreshToken = refreshToken.Token
            };
            return result;
        }

        private RefreshTokens CreateRefreshToken(User? account) {
            var randomByte = new Byte[64];
            var token = Convert.ToBase64String(randomByte);
            var refreshToken = new RefreshTokens {
                UserId = account.Id,
                ExpireTime = DateTime.UtcNow.AddDays(1),
                IsActive = true,
                Token = token,
            };
            _refreshTokenRepository.addRefreshTokens(refreshToken);
            return refreshToken;
        }

        private ResponseLoginModel RefreshToken(string token) {
            var isExistToken = _refreshTokenRepository.getAllRefreshToken()
                .Where(x => x.Token == token && x.ExpireTime > DateTime.Now)
                .FirstOrDefault();
            //if (isExistToken == null) {
            //    throw
            //}
            var user = _userRepository.getUserByUserID(isExistToken.UserId);
            var newToken = CreateJwtToken(user);
            var newRefreshToken = CreateRefreshToken(user);
            var result = new ResponseLoginModel {
                FullName = user.Name,
                UserId = user.Id,
                Token = newToken,
                RefreshToken = newRefreshToken.Token
            };
            return result;
        }

        private string CreateJwtToken(User? account) {      // có 3 bước để khởi tạo 1 token
            // B2: tạo 1 token handler để xử lí token   
            var tokenHandler = new JwtSecurityTokenHandler();
            // B1: define key, xử lí key
            var key = Encoding.UTF8.GetBytes(Key);
            var securityKey = new SymmetricSecurityKey(key);
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // B3: tạo 1 description để lưu thông tin token
            var tokenDescription = new SecurityTokenDescriptor {
                Audience = "",
                Issuer = "",
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, account.Name),
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim("PhoneNumber", "1")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credential,
            };
            //sử dụng handler để biến tất cả thông tin trên thành token string
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }

        public int Register(RequestRegisterModel model) {
            var emailExist = _userRepository.getUserByAccNEmail(model.Account, model.Email);
            if (emailExist != null) {
                return 0;
            }
            var verifyToken = new Random().Next(100000, 1000000).ToString();
            var user = new User {
                Name = model.Name,
                Account = model.Account,
                Password = model.Password,
                VertificationToken = verifyToken,
                Email = model.Email,
            };
            var save = _userRepository.addUser(user);
            var mailModel = new SendMailModel {
                recieveAddress = model.Email,
                subject = "OTP Authentication",
                content = verifyToken,
            };
            _emailService.sendMail(mailModel);
            return save;
        }

        public int VerifyEmail(RequestVerifyEmailModel model) {
            var isExist = _userRepository.getAllUsers()
                .Where(x => x.Account == model.Account && x.VertificationToken == model.VertificationToken)
                .FirstOrDefault();
            if (isExist == null) {
                return 0;
            }
            isExist.VertificationToken = null;
            isExist.IsActive = true;
            return _userRepository.updateVerifyUser(isExist);
        }

        public int ForgetPassword(RequestForgetPasswordModel model) {
            var isExist = _userRepository.getUserByAccNEmail(model.Account, model.Email);
            if (isExist == null) {
                return 0;
            }
            var OTP = new Random().Next(100000, 1000000).ToString();
            isExist.IsActive = false;
            isExist.VertificationToken = OTP;
            var save = _userRepository.updateVerifyUser(isExist);
            var mailModel = new SendMailModel {
                recieveAddress = model.Email,
                content = OTP,
                subject = "Reset password OTP"
            };
            _emailService.sendMail(mailModel);
            return save;
        }

        public int ResetPassword(RequestResetPasswordModel model) {
            var isExist = _userRepository.getAllUsers()
                .Where(x => x.VertificationToken == model.VertificationToken && x.Account == model.Account).FirstOrDefault();
            if (isExist == null || model.NewPassword != model.ReNewPassword) {
                return 0;
            }
            isExist.VertificationToken = null;
            isExist.IsActive = true;
            isExist.Password = model.NewPassword;
            return _userRepository.updateVerifyUser(isExist);
        }
    }
}


