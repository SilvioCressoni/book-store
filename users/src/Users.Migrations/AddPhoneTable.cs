using FluentMigrator;

namespace Users.Migrations
{
    [Migration(20191119075000)]
    public class AddPhoneTable : Migration
    {
        public override void Up()
        {
            Create
                .Table("Phones")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_Phone_Id")
                .WithColumn("Number").AsString(15).NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Phone");
        }
    }
}