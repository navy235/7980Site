using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Maitonn.Core;
namespace PadSite.Models
{
    public class EntitiesContext : UnitOfWork
    {
        public EntitiesContext()
            : base("pad_db")
        {

        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            #region Category

            modelBuilder.Entity<CityCate>()
                .HasOptional(c => c.PCate)
                .WithMany(pc => pc.ChildCates)
                .HasForeignKey(c => c.PID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MediaCate>()
                .HasOptional(c => c.PCate)
                .WithMany(pc => pc.ChildCates)
                .HasForeignKey(c => c.PID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ArticleCate>()
                .HasOptional(c => c.PCate)
                .WithMany(pc => pc.ChildCates)
                .HasForeignKey(c => c.PID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FormatCate>()
                .HasOptional(c => c.PCate)
                .WithMany(pc => pc.ChildCates)
                .HasForeignKey(c => c.PID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PeriodCate>()
                .HasOptional(c => c.PCate)
                .WithMany(pc => pc.ChildCates)
                .HasForeignKey(c => c.PID)
                .WillCascadeOnDelete(false);

            #endregion


            #region Member


            modelBuilder.Entity<Member>()
                .HasOptional(m => m.Member_Profile)
                .WithRequired(x => x.Member);



            modelBuilder.Entity<Member_Profile>()
                 .HasRequired(m => m.CityCate)
                 .WithMany(a => a.Member_Profile)
                 .HasForeignKey(m => m.CityCode);

            #endregion

            modelBuilder.Entity<Permissions>()
                .HasRequired(p => p.Department)
                .WithMany(d => d.Permissions)
                .HasForeignKey(p => p.DepartmentID);

            modelBuilder.Entity<Roles>()
                .HasMany(b => b.Permissions)
                .WithMany(c => c.Roles)
                .Map
                (
                    m =>
                    {
                        m.MapLeftKey("RoleID");
                        m.MapRightKey("PermissionID");
                        m.ToTable("Role_Permissions");
                    }
                );
            modelBuilder.Entity<Group>()
               .HasMany(g => g.Roles)
               .WithMany(r => r.Group)
               .Map
               (
                   m =>
                   {
                       m.MapLeftKey("GroupID");
                       m.MapRightKey("RoleID");
                       m.ToTable("Group_Roles");
                   }
               );

            modelBuilder.Entity<Article>()
                .HasRequired(m => m.ArticleCate)
                .WithMany(c => c.Article)
                .HasForeignKey(o => o.ArticleCode)
                .WillCascadeOnDelete(false);


            base.OnModelCreating(modelBuilder);
        }
    }
}