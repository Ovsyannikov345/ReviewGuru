using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewGuru.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ModificationDateSetterTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE OR REPLACE FUNCTION update_last_modification() RETURNS TRIGGER AS $$
            BEGIN
                NEW.""DateOfLastModification"" = NOW();
                RETURN NEW;
            END;
            $$ LANGUAGE plpgsql;
        ");

            migrationBuilder.Sql(@"
            CREATE TRIGGER update_last_modification_trigger 
            BEFORE UPDATE ON ""Reviews"" 
            FOR EACH ROW 
            EXECUTE PROCEDURE update_last_modification();
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER update_last_modification_trigger ON \"Reviews\";");
            migrationBuilder.Sql("DROP FUNCTION update_last_modification;");
        }
    }
}
