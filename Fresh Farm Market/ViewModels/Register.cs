using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Fresh_Farm_Market.ViewModels
{
    public class Register
    {
        [Required]
        [DataType(DataType.Text)]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.CreditCard)]
        public string CreditCard { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string DeliveryAddress { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string AboutMe { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "Password must include lower-case, upper-case, numeric, and special characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public IFormFile Photo { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string Gender { get; set; }

	}
}
