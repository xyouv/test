namespace PhoneManagement {
    public class RequestResetPasswordModel {
        public string VertificationToken { get; set; }
        public string Account { get; set; }
        public string NewPassword { get; set; }
        public string ReNewPassword { get; set; }
    }
}
