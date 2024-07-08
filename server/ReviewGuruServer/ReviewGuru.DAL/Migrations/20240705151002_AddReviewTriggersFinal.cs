using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewGuru.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewTriggersFinal : Migration
    {
        
            protected override void Up(MigrationBuilder migrationBuilder)
            {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS update_last_modification_trigger ON \"Reviews\";");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS update_last_modification;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS hide_on_delete_trigger ON \"Reviews\";");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS hide_on_delete;");
            // Trigger to update DateOfLastModification
            migrationBuilder.Sql(@"
                    CREATE OR REPLACE FUNCTION update_last_modification() RETURNS TRIGGER AS $$
                    BEGIN
                        IF OLD.""DateOfDeleting"" IS NOT DISTINCT FROM NEW.""DateOfDeleting"" THEN
                            NEW.""DateOfLastModification"" = NOW();
                        END IF;
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

                        // Trigger to "soft delete" and update DateOfDeleting
                        migrationBuilder.Sql(@"
                    CREATE OR REPLACE FUNCTION hide_on_delete() RETURNS TRIGGER AS $$
                    BEGIN
                        NEW.""DateOfDeleting"" = NOW();
                        UPDATE ""Reviews"" SET ""DateOfDeleting"" = NEW.""DateOfDeleting"" WHERE ""ReviewId"" = OLD.""ReviewId"";
                        RETURN NULL;
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
                migrationBuilder.Sql("DROP TRIGGER IF EXISTS update_last_modification_trigger ON \"Reviews\";");
                migrationBuilder.Sql("DROP FUNCTION IF EXISTS update_last_modification;");
                migrationBuilder.Sql("DROP TRIGGER IF EXISTS hide_on_delete_trigger ON \"Reviews\";");
                migrationBuilder.Sql("DROP FUNCTION IF EXISTS hide_on_delete;");
            }
        }

    
}
