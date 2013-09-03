//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace PadSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class Permissions
    {
        public Permissions()
        {
            this.Roles = new HashSet<Roles>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(50)]
        [Display(Name = "权限名称")]
        public string Name { get; set; }

        [MaxLength(150)]
        [Display(Name = "权限描述")]
        public string Description { get; set; }

        [MaxLength(50)]
        [Display(Name = "权限控制器")]
        public string Controller { get; set; }

        [MaxLength(50)]
        [Display(Name = "权限操作")]
        public string Action { get; set; }

        [MaxLength(50)]
        [Display(Name = "命名空间")]
        public string Namespace { get; set; }

        [Display(Name = "所属部门")]
        public int DepartmentID { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Roles> Roles { get; set; }
    }
}
