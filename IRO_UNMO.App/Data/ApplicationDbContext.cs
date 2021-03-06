﻿using System;
using System.Collections.Generic;
using System.Text;
using IRO_UNMO.App.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IRO_UNMO.App.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Country> Country { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Administrator> Administrator { get; set; }
        public DbSet<Applicant> Applicant { get; set; }
        public DbSet<Application> Application { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Info> Info { get; set; }
        public DbSet<HomeInstitution> HomeInstitution { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<University> University { get; set; }
        public DbSet<Other> Other { get; set; }
        public DbSet<Documents> Documents { get; set; }
        public DbSet<Nomination> Nomination { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Timing> Timing { get; set; }
        public DbSet<Offer> Offer { get; set; }
        public DbSet<Token> Token { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
