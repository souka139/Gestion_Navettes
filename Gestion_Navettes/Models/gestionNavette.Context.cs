﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gestion_Navettes.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class gestionNavetteEntities1 : DbContext
    {
        public gestionNavetteEntities1()
            : base("name=gestionNavetteEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<abonne> abonne { get; set; }
        public virtual DbSet<abonne_abonnement> abonne_abonnement { get; set; }
        public virtual DbSet<abonnement> abonnement { get; set; }
        public virtual DbSet<administrateur> administrateur { get; set; }
        public virtual DbSet<autocar> autocar { get; set; }
        public virtual DbSet<societe> societe { get; set; }
        public virtual DbSet<ville> ville { get; set; }
        public virtual DbSet<demande> demande { get; set; }
    }
}
