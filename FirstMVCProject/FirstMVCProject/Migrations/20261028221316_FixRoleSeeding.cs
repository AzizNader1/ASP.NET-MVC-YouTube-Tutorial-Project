using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FirstMVCProject.Migrations
{
    /// <inheritdoc />
    public partial class FixRoleSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityRole");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM [AspNetRoles] WHERE [Id] = '1')
                BEGIN
                    INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Description], [Name], [NormalizedName])
                    VALUES ('1', '1', 'Administrator role', 'Admin', 'ADMIN');
                END
                ELSE
                BEGIN
                    UPDATE [AspNetRoles]
                    SET [ConcurrencyStamp] = '1', [Description] = 'Administrator role', [Name] = 'Admin', [NormalizedName] = 'ADMIN'
                    WHERE [Id] = '1';
                END

                IF NOT EXISTS (SELECT 1 FROM [AspNetRoles] WHERE [Id] = '2')
                BEGIN
                    INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Description], [Name], [NormalizedName])
                    VALUES ('2', '2', 'Standard user role', 'User', 'USER');
                END
                ELSE
                BEGIN
                    UPDATE [AspNetRoles]
                    SET [ConcurrencyStamp] = '2', [Description] = 'Standard user role', [Name] = 'User', [NormalizedName] = 'USER'
                    WHERE [Id] = '2';
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentityRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "Admin", "ADMIN" },
                    { "2", null, "User", "USER" }
                });
        }
    }
}
