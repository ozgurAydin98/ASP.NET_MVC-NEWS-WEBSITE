namespace darkZencefil.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class veriTabani : DbContext
    {
        public veriTabani()
            : base("name=veriTabani")
        {
        }

        public virtual DbSet<Kategori> Kategori { get; set; }
        public virtual DbSet<Puan> Puan { get; set; }
        public virtual DbSet<Uye> Uye { get; set; }
        public virtual DbSet<Yazi> Yazi { get; set; }
        public virtual DbSet<Yorum> Yorum { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Uye>()
                .HasMany(e => e.Yorums)
                .WithOptional(e => e.Uye)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Yazi>()
                .HasMany(e => e.Yorums)
                .WithOptional(e => e.Yazi)
                .WillCascadeOnDelete();
        }
    }
}
