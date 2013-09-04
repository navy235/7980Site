using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadSite.Models
{
    public class ArticleCate
    {

        public ArticleCate()
        {
            this.Article = new HashSet<Article>();

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(50)]
        [Display(Name = "分类名称")]
        public string CateName { get; set; }

        [Display(Name = "父级分类")]
        public int? PID { get; set; }

        [Display(Name = "分类代码")]
        public int Code { get; set; }

        [Display(Name = "分类级别")]
        public int Level { get; set; }

        [Display(Name = "排序代码")]
        public int OrderIndex { get; set; }

        [Display(Name = "单篇分类")]
        public bool IsSingle { get; set; }

        [ScriptIgnore]
        public virtual ArticleCate PCate { get; set; }

        [ScriptIgnore]
        public virtual ICollection<ArticleCate> ChildCates { get; set; }

        public virtual ICollection<Article> Article { get; set; }
    }
}