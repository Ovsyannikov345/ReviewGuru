using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewGuru.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateDeleteTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE OR REPLACE FUNCTION hide_on_delete() RETURNS TRIGGER AS $$
            BEGIN
                NEW.""DateOfDeleting"" = NOW();
                RETURN NEW;
            END;
            $$ LANGUAGE plpgsql;
        ");

            migrationBuilder.Sql(@"
            CREATE TRIGGER hide_on_delete_trigger 
            BEFORE DELETE ON ""Reviews"" 
            FOR EACH ROW 
            EXECUTE PROCEDURE hide_on_delete();
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER hide_on_delete_trigger ON \"Reviews\";");
            migrationBuilder.Sql("DROP FUNCTION hide_on_delete;");
        }
    }
}
