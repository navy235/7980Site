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
        public IDbSet<Favorite> Favorite { get; set; }
        public IDbSet<Message> Message { get; set; }

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


            modelBuilder.Entity<OwnerCate>()
                .HasOptional(c => c.PCate)
                .WithMany(pc => pc.ChildCates)
                .HasForeignKey(c => c.PID)
                .WillCascadeOnDelete(false);


            #endregion


            #region Member

            modelBuilder.Entity<Member>()
                .HasOptional(m => m.Member_Profile)
                .WithRequired(x => x.Member);

            modelBuilder.Entity<Member>()
                .HasOptional(m => m.Company)
                .WithRequired(c => c.Member);

            modelBuilder.Entity<Member>()
                .HasMany(x => x.CompanyCredentialsImg)
                .WithRequired(x => x.Member)
                .HasForeignKey(x => x.MemberID);

            modelBuilder.Entity<Member>()
                .HasMany(x => x.CompanyMessage)
                .WithRequired(x => x.Member)
                .HasForeignKey(x => x.MemberID);

            modelBuilder.Entity<Member>()
                .HasMany(x => x.CompanyNotice)
                .WithRequired(x => x.Member)
                .HasForeignKey(x => x.MemberID);

            modelBuilder.Entity<Member>()
                .HasMany(x => x.Scheme)
                .WithRequired(x => x.Member)
                .HasForeignKey(x => x.MemberID);

            modelBuilder.Entity<Scheme>()
                .HasMany(s => s.SchemeItem)
                .WithRequired(m => m.Scheme)
                .HasForeignKey(o => o.SchemeID)
                .WillCascadeOnDelete(true);

            #endregion


            #region permission

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

            #endregion

            modelBuilder.Entity<Article>()
                .HasRequired(m => m.ArticleCate)
                .WithMany(c => c.Article)
                .HasForeignKey(o => o.ArticleCode)
                .WillCascadeOnDelete(false);

            #region media

            modelBuilder.Entity<OutDoor>()
                .HasRequired(m => m.Member)
                .WithMany(c => c.OutDoor)
                .HasForeignKey(o => o.MemberID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OutDoor>()
               .HasRequired(m => m.CityCate)
               .WithMany(c => c.OutDoor)
               .HasForeignKey(o => o.CityCode)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<OutDoor>()
                .HasRequired(m => m.PeriodCate)
                .WithMany(c => c.OutDoor)
                .HasForeignKey(o => o.PeriodCode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OutDoor>()
                .HasRequired(m => m.FormatCate)
                .WithMany(c => c.OutDoor)
                .HasForeignKey(o => o.FormatCode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OutDoor>()
                 .HasRequired(o => o.OwnerCate)
                 .WithMany(oc => oc.OutDoor)
                 .HasForeignKey(o => o.OwnerCode)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<OutDoor>()
                .HasRequired(m => m.MediaCate)
                .WithMany(c => c.OutDoor)
                .HasForeignKey(o => o.MediaCode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OutDoor>()
              .HasMany(b => b.AreaCate)
              .WithMany(c => c.OutDoor)
              .Map
              (
                  m =>
                  {
                      m.MapLeftKey("OutDoorID");
                      m.MapRightKey("AreaCateID");
                      m.ToTable("OutDoor_AreaCate");
                  }
              );

            modelBuilder.Entity<OutDoor>()
              .HasMany(b => b.CrowdCate)
              .WithMany(c => c.OutDoor)
              .Map
              (
                  m =>
                  {
                      m.MapLeftKey("OutDoorID");
                      m.MapRightKey("CrowdCateID");
                      m.ToTable("OutDoor_CrowdCate");
                  }
              );


            modelBuilder.Entity<OutDoor>()
           .HasMany(b => b.IndustryCate)
           .WithMany(c => c.OutDoor)
           .Map
           (
               m =>
               {
                   m.MapLeftKey("OutDoorID");
                   m.MapRightKey("IndustryCateID");
                   m.ToTable("OutDoor_IndustryCate");
               }
           );

            modelBuilder.Entity<OutDoor>()
            .HasMany(b => b.PurposeCate)
            .WithMany(c => c.OutDoor)
            .Map
            (
                m =>
                {
                    m.MapLeftKey("OutDoorID");
                    m.MapRightKey("PurposeCateID");
                    m.ToTable("OutDoor_PurposeCate");
                }
            );
            #endregion


            base.OnModelCreating(modelBuilder);
        }
    }
}