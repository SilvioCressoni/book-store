using FluentMigrator;

namespace Users.Migrations
{
    [Migration(20191119080000)]
    public class AddAddressTable : Migration
    {
        public override void Up()
        {
            Create
                .Table("Addresses")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_Address_Id")
                .WithColumn("Line").AsString(100).NotNullable()
                .WithColumn("Number").AsInt32().NotNullable()
                .WithColumn("PostCode").AsString(10).NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Address");
        }
    }
}