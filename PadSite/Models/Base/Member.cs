
namespace PadSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class Member
    {
        public Member()
        {
            this.Member_Action = new HashSet<Member_Action>();
            this.CompanyCredentialsImg = new HashSet<CompanyCredentialsImg>();
            this.CompanyMessage = new HashSet<CompanyMessage>();
            this.CompanyNotice = new HashSet<CompanyNotice>();
            this.MemberID = 100000;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "用户ID")]
        public int MemberID { get; set; }

        public int? CompanyID { get; set; }

        [Display(Name = "用户类型")]
        public int MemberType { get; set; }

        [MaxLength(50)]
        [Display(Name = "手机")]
        public string Mobile { get; set; }

        [MaxLength(50)]
        [Display(Name = "电子邮箱")]
        public string Email { get; set; }

        [MaxLength(50)]
        [Display(Name = "昵称")]
        public string NickName { get; set; }

        [MaxLength(50)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [MaxLength(500)]
        [Display(Name = "头像")]
        public string AvtarUrl { get; set; }

        public int GroupID { get; set; }

        [Display(Name = "最后登录")]
        public System.DateTime LastTime { get; set; }

        [MaxLength(50)]
        [Display(Name = "最后登录IP")]
        public string LastIP { get; set; }

        [Display(Name = "创建时间")]
        public System.DateTime AddTime { get; set; }

        public int OpenType { get; set; }

        [MaxLength(100)]
        public string OpenID { get; set; }

        [MaxLength(50)]
        [Display(Name = "创建IP")]
        public string AddIP { get; set; }

        [Display(Name = "登录次数")]
        public int LoginCount { get; set; }

        public int Status { get; set; }

        public virtual Group Group { get; set; }

        public virtual ICollection<Member_Action> Member_Action { get; set; }

        public virtual Member_Profile Member_Profile { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<CompanyCredentialsImg> CompanyCredentialsImg { get; set; }

        public virtual ICollection<CompanyMessage> CompanyMessage { get; set; }

        public virtual ICollection<CompanyNotice> CompanyNotice { get; set; }

        public virtual ICollection<OutDoor> OutDoor { get; set; }

        public virtual ICollection<Scheme> Scheme { get; set; }

    }
}
