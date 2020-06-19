using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FlyawayComments.Data.Models
{
    public partial class lawasitecore91prodexternaldbContext : DbContext
    {
        public lawasitecore91prodexternaldbContext()
        {
        }

        public lawasitecore91prodexternaldbContext(DbContextOptions<lawasitecore91prodexternaldbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LaxgroundTransportation> LaxgroundTransportation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LaxgroundTransportation>(entity =>
            {
                entity.HasKey(e => e.TransportId);

                entity.ToTable("LAXGroundTransportation");

                entity.Property(e => e.TransportId).HasColumnName("TransportID");

                entity.Property(e => e.AddedDateTime).HasColumnType("datetime");

                entity.Property(e => e.BoardWhere).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.CommentOtherText).HasMaxLength(100);

                entity.Property(e => e.Comments).HasMaxLength(3000);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.HowLong).HasMaxLength(100);

                entity.Property(e => e.License).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(12);

                entity.Property(e => e.ServiceSubType).HasMaxLength(100);

                entity.Property(e => e.ServiceType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ServiceTypeOther).HasMaxLength(100);

                entity.Property(e => e.State).HasMaxLength(100);

                entity.Property(e => e.StreetAddress).HasMaxLength(200);

                entity.Property(e => e.WhatDate).HasColumnType("datetime");

                entity.Property(e => e.WhatTime).HasMaxLength(50);

                entity.Property(e => e.Zip).HasMaxLength(12);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
