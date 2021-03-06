﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace SalesAdminPortal.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class DateRange
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }

    public class Sale
    {
        [Required]
        public string OrderId { get; set; }

        [Required]
        public string SellingPrice { get; set; }

        [Required]
        public string AgentCode { get; set; }

    }

    public class DocumentModel
    {
        [Required]
        [Display(Name="Document Id")]
        public int DocumentId { get; set; }

        [Required]
        [Display(Name = "Document Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Document Description")]
        public string DocumentDesc { get; set; }

        [Required]
        [Display(Name = "Choose Type")]
        public string DocType { get; set; }

        [UIHint("tinymce_full"), System.Web.Mvc.AllowHtml]
        public string Content { get; set; }

        public HttpPostedFileBase File { get; set; }
    }

    public class Feed
    {
        [Required]
        [Display(Name="News Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name="News Description")]
        [UIHint("tinymce_full"), System.Web.Mvc.AllowHtml]
        public string Description { get; set; }
    }

}