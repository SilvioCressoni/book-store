using FluentMigrator;

namespace Users.Migrations.Migrations
{
    [Migration(20191119090000)]
    public class AddUserTable : Migration
    {
        public override void Up()
        {
            Create
                .Table("User")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_User_Id")
                .WithColumn("FirstName").AsString(20).NotNullable()
                .WithColumn("LastNames").AsString(100).NotNullable()
                .WithColumn("Email").AsString(100).NotNullable().Unique("IX_User_Email")
                .WithColumn("BirthDay").AsDateTime2().NotNullable();

            Alter.Table("Phone")
                .AlterColumn("UserId").AsGuid().ForeignKey("FK_User_Phone", "User", "Id");

            Alter.Table("Address")
                .AlterColumn("UserId").AsGuid().ForeignKey("FK_User_Address", "User", "Id");
        }

        public override void Down()
        {
            Delete.Table("Phone");
        }
    }
}