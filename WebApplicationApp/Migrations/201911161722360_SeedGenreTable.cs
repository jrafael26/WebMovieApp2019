namespace WebApplicationApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedGenreTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Genres (Id,Name) values (1,'Acción')");
            Sql("INSERT INTO Genres (Id,Name) values (2,'Terror')");
            Sql("INSERT INTO Genres (Id,Name) values (3,'Comedia')");
            Sql("INSERT INTO Genres (Id,Name) values (4,'Suspenso')");
            Sql("INSERT INTO Genres (Id,Name) values (5,'Drama')");
            Sql("INSERT INTO Genres (Id,Name) values (6,'Adventura')");
            Sql("INSERT INTO Genres (Id,Name) values (7,'Sci-Fi')");
        }
        
        public override void Down()
        {
        }
    }
}
