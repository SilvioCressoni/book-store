using FluentMigrator;

namespace Users.Migrations
{
    [Migration(20191119090000)]
    public class AddUserTable : Migration
    {
        public override void Up()
        {
            Create
            .Table("Users")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_User_Id")
                .WithColumn("FirstName").AsString(20).NotNullable()
                .WithColumn("LastNames").AsString(100).NotNullable()
                .WithColumn("Email").AsString(100).NotNullable().Unique("IX_User_Email")
                .WithColumn("BirthDay").AsDateTime2().NotNullable();

            Alter.Table("Phones")
                .AlterColumn("UserId").AsGuid().ForeignKey("FK_Users_Phones", "Users", "Id");

            Alter.Table("Addresses")
                .AlterColumn("UserId").AsGuid().ForeignKey("FK_Users_Addresses", "Users", "Id");
        }

        public override void Down()
        {
            Alter.Table("Phones")
                .AlterColumn("UserId").AsGuid();

            Alter.Table("Addresses")
                .AlterColumn("UserId").AsGuid();
            
            Delete.Table("Users");
        }
    }
}