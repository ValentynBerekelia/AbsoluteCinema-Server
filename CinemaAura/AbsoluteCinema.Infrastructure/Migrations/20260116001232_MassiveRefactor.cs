using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaAura.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MassiveRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movie_actors_Actors_actor_id",
                table: "movie_actors");

            migrationBuilder.DropForeignKey(
                name: "FK_movie_actors_Movies_movie_id",
                table: "movie_actors");

            migrationBuilder.DropForeignKey(
                name: "FK_movie_genres_Genres_genre_id",
                table: "movie_genres");

            migrationBuilder.DropForeignKey(
                name: "FK_movie_genres_Movies_movie_id",
                table: "movie_genres");

            migrationBuilder.DropForeignKey(
                name: "FK_movie_media_Medias_media_id",
                table: "movie_media");

            migrationBuilder.DropForeignKey(
                name: "FK_movie_media_Movies_movie_id",
                table: "movie_media");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permissions_Role_role_id",
                table: "role_permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Halls_hall",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Movies_movie",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Seats_seat",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Sessions_session",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_user_id",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_Role_role_id",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_Users_user_id",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sessions",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seats",
                table: "Seats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movies",
                table: "Movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Medias",
                table: "Medias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Halls",
                table: "Halls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genres",
                table: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Actors",
                table: "Actors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypePrices",
                table: "TypePrices");

            migrationBuilder.DropIndex(
                name: "IX_TypePrices_session_id_seat_type_id",
                table: "TypePrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeatTypes",
                table: "SeatTypes");

            migrationBuilder.DropIndex(
                name: "IX_SeatTypes_type_name",
                table: "SeatTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_Name",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "date",
                table: "Tickets");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Tickets",
                newName: "tickets");

            migrationBuilder.RenameTable(
                name: "Sessions",
                newName: "sessions");

            migrationBuilder.RenameTable(
                name: "Seats",
                newName: "seats");

            migrationBuilder.RenameTable(
                name: "Movies",
                newName: "movies");

            migrationBuilder.RenameTable(
                name: "Medias",
                newName: "medias");

            migrationBuilder.RenameTable(
                name: "Halls",
                newName: "halls");

            migrationBuilder.RenameTable(
                name: "Genres",
                newName: "genres");

            migrationBuilder.RenameTable(
                name: "Actors",
                newName: "actors");

            migrationBuilder.RenameTable(
                name: "TypePrices",
                newName: "type_prices");

            migrationBuilder.RenameTable(
                name: "SeatTypes",
                newName: "seat_types");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "roles");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "users",
                newName: "user_name");

            migrationBuilder.RenameColumn(
                name: "PasswordHash_Salt",
                table: "users",
                newName: "password_salt");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "users",
                newName: "ix_users_email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tickets",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "session",
                table: "tickets",
                newName: "session_id");

            migrationBuilder.RenameColumn(
                name: "seat",
                table: "tickets",
                newName: "seat_id");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_user_id",
                table: "tickets",
                newName: "ix_tickets_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_session",
                table: "tickets",
                newName: "ix_tickets_session_id");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_seat",
                table: "tickets",
                newName: "IX_tickets_seat_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "sessions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "movie",
                table: "sessions",
                newName: "movie_id");

            migrationBuilder.RenameColumn(
                name: "hall",
                table: "sessions",
                newName: "hall_id");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "sessions",
                newName: "start_time");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_movie",
                table: "sessions",
                newName: "ix_sessions_movie_id");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_hall",
                table: "sessions",
                newName: "ix_sessions_hall_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "seats",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_role_permissions_role_id_permission_code",
                table: "role_permissions",
                newName: "uq_role_permissions_role_permission");

            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "movies",
                newName: "rate");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "movies",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "movies",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "movies",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "AgeLimit",
                table: "movies",
                newName: "age_limit");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "medias",
                newName: "url");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "medias",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "medias",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "halls",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "HallName",
                table: "halls",
                newName: "hall_name");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "genres",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "genres",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "actors",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Bio",
                table: "actors",
                newName: "bio");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "actors",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "actors",
                newName: "birth_date");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "type_prices",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "seat_types",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "roles",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "roles",
                newName: "id");

            migrationBuilder.AddColumn<decimal>(
                name: "price_paid",
                table: "tickets",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "purchased_at",
                table: "tickets",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "tickets",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AddColumn<Guid>(
                name: "hall_id",
                table: "seats",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<short>(
                name: "row",
                table: "seats",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AlterColumn<decimal>(
                name: "rate",
                table: "movies",
                type: "numeric(3,2)",
                precision: 3,
                scale: 2,
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "movies",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "url",
                table: "medias",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "medias",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "hall_name",
                table: "halls",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "bio",
                table: "actors",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tickets",
                table: "tickets",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sessions",
                table: "sessions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_seats",
                table: "seats",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_movies",
                table: "movies",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_medias",
                table: "medias",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_halls",
                table: "halls",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_genres",
                table: "genres",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_actors",
                table: "actors",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_type_prices",
                table: "type_prices",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_seat_types",
                table: "seat_types",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_roles",
                table: "roles",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_tickets_purchased_at",
                table: "tickets",
                column: "purchased_at");

            migrationBuilder.CreateIndex(
                name: "ix_tickets_status",
                table: "tickets",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "uq_tickets_session_seat",
                table: "tickets",
                columns: new[] { "session_id", "seat_id" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "ck_tickets_price_positive",
                table: "tickets",
                sql: "price_paid >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_tickets_status_valid",
                table: "tickets",
                sql: "status IN ('Pending', 'Confirmed', 'Cancelled', 'Refunded')");

            migrationBuilder.CreateIndex(
                name: "ix_sessions_start_time",
                table: "sessions",
                column: "start_time");

            migrationBuilder.CreateIndex(
                name: "ix_seats_hall_id",
                table: "seats",
                column: "hall_id");

            migrationBuilder.CreateIndex(
                name: "ix_seats_seat_type_id",
                table: "seats",
                column: "seat_type_id");

            migrationBuilder.CreateIndex(
                name: "uq_seats_position",
                table: "seats",
                columns: new[] { "hall_id", "row", "number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_movies_rate",
                table: "movies",
                column: "rate");

            migrationBuilder.AddCheckConstraint(
                name: "ck_movies_age_limit_positive",
                table: "movies",
                sql: "age_limit >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_movies_rate_range",
                table: "movies",
                sql: "rate >= 0 AND rate <= 10");

            migrationBuilder.AddCheckConstraint(
                name: "ck_halls_horizontal_size_positive",
                table: "halls",
                sql: "horizontal_size > 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_halls_vertical_size_positive",
                table: "halls",
                sql: "vertical_size > 0");

            migrationBuilder.CreateIndex(
                name: "uq_genres_name",
                table: "genres",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_actors_name",
                table: "actors",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_type_prices_seat_type_id",
                table: "type_prices",
                column: "seat_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_type_prices_session_id",
                table: "type_prices",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "uq_type_prices_session_seat_type",
                table: "type_prices",
                columns: new[] { "session_id", "seat_type_id" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "ck_type_prices_price_positive",
                table: "type_prices",
                sql: "price >= 0");

            migrationBuilder.CreateIndex(
                name: "uq_seat_types_type_name",
                table: "seat_types",
                column: "type_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_roles_name",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_movie_actors_actors_actor_id",
                table: "movie_actors",
                column: "actor_id",
                principalTable: "actors",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_movie_actors_movies_movie_id",
                table: "movie_actors",
                column: "movie_id",
                principalTable: "movies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_movie_genres_genres_genre_id",
                table: "movie_genres",
                column: "genre_id",
                principalTable: "genres",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_movie_genres_movies_movie_id",
                table: "movie_genres",
                column: "movie_id",
                principalTable: "movies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_movie_media_medias_media_id",
                table: "movie_media",
                column: "media_id",
                principalTable: "medias",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_movie_media_movies_movie_id",
                table: "movie_media",
                column: "movie_id",
                principalTable: "movies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permissions_roles_role_id",
                table: "role_permissions",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_seats_halls_hall_id",
                table: "seats",
                column: "hall_id",
                principalTable: "halls",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_seats_seat_types_seat_type_id",
                table: "seats",
                column: "seat_type_id",
                principalTable: "seat_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sessions_halls_hall_id",
                table: "sessions",
                column: "hall_id",
                principalTable: "halls",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sessions_movies_movie_id",
                table: "sessions",
                column: "movie_id",
                principalTable: "movies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_tickets_seats_seat_id",
                table: "tickets",
                column: "seat_id",
                principalTable: "seats",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_tickets_sessions_session_id",
                table: "tickets",
                column: "session_id",
                principalTable: "sessions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_tickets_users_user_id",
                table: "tickets",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_type_prices_seat_types_seat_type_id",
                table: "type_prices",
                column: "seat_type_id",
                principalTable: "seat_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_type_prices_sessions_session_id",
                table: "type_prices",
                column: "session_id",
                principalTable: "sessions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_roles_role_id",
                table: "user_roles",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_users_user_id",
                table: "user_roles",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movie_actors_actors_actor_id",
                table: "movie_actors");

            migrationBuilder.DropForeignKey(
                name: "FK_movie_actors_movies_movie_id",
                table: "movie_actors");

            migrationBuilder.DropForeignKey(
                name: "FK_movie_genres_genres_genre_id",
                table: "movie_genres");

            migrationBuilder.DropForeignKey(
                name: "FK_movie_genres_movies_movie_id",
                table: "movie_genres");

            migrationBuilder.DropForeignKey(
                name: "FK_movie_media_medias_media_id",
                table: "movie_media");

            migrationBuilder.DropForeignKey(
                name: "FK_movie_media_movies_movie_id",
                table: "movie_media");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permissions_roles_role_id",
                table: "role_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_seats_halls_hall_id",
                table: "seats");

            migrationBuilder.DropForeignKey(
                name: "fk_seats_seat_types_seat_type_id",
                table: "seats");

            migrationBuilder.DropForeignKey(
                name: "fk_sessions_halls_hall_id",
                table: "sessions");

            migrationBuilder.DropForeignKey(
                name: "fk_sessions_movies_movie_id",
                table: "sessions");

            migrationBuilder.DropForeignKey(
                name: "fk_tickets_seats_seat_id",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "fk_tickets_sessions_session_id",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "fk_tickets_users_user_id",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "fk_type_prices_seat_types_seat_type_id",
                table: "type_prices");

            migrationBuilder.DropForeignKey(
                name: "fk_type_prices_sessions_session_id",
                table: "type_prices");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_roles_role_id",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_users_user_id",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tickets",
                table: "tickets");

            migrationBuilder.DropIndex(
                name: "ix_tickets_purchased_at",
                table: "tickets");

            migrationBuilder.DropIndex(
                name: "ix_tickets_status",
                table: "tickets");

            migrationBuilder.DropIndex(
                name: "uq_tickets_session_seat",
                table: "tickets");

            migrationBuilder.DropCheckConstraint(
                name: "ck_tickets_price_positive",
                table: "tickets");

            migrationBuilder.DropCheckConstraint(
                name: "ck_tickets_status_valid",
                table: "tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sessions",
                table: "sessions");

            migrationBuilder.DropIndex(
                name: "ix_sessions_start_time",
                table: "sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_seats",
                table: "seats");

            migrationBuilder.DropIndex(
                name: "ix_seats_hall_id",
                table: "seats");

            migrationBuilder.DropIndex(
                name: "ix_seats_seat_type_id",
                table: "seats");

            migrationBuilder.DropIndex(
                name: "uq_seats_position",
                table: "seats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_movies",
                table: "movies");

            migrationBuilder.DropIndex(
                name: "ix_movies_rate",
                table: "movies");

            migrationBuilder.DropCheckConstraint(
                name: "ck_movies_age_limit_positive",
                table: "movies");

            migrationBuilder.DropCheckConstraint(
                name: "ck_movies_rate_range",
                table: "movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_medias",
                table: "medias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_halls",
                table: "halls");

            migrationBuilder.DropCheckConstraint(
                name: "ck_halls_horizontal_size_positive",
                table: "halls");

            migrationBuilder.DropCheckConstraint(
                name: "ck_halls_vertical_size_positive",
                table: "halls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_genres",
                table: "genres");

            migrationBuilder.DropIndex(
                name: "uq_genres_name",
                table: "genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_actors",
                table: "actors");

            migrationBuilder.DropIndex(
                name: "ix_actors_name",
                table: "actors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_type_prices",
                table: "type_prices");

            migrationBuilder.DropIndex(
                name: "ix_type_prices_seat_type_id",
                table: "type_prices");

            migrationBuilder.DropIndex(
                name: "ix_type_prices_session_id",
                table: "type_prices");

            migrationBuilder.DropIndex(
                name: "uq_type_prices_session_seat_type",
                table: "type_prices");

            migrationBuilder.DropCheckConstraint(
                name: "ck_type_prices_price_positive",
                table: "type_prices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_seat_types",
                table: "seat_types");

            migrationBuilder.DropIndex(
                name: "uq_seat_types_type_name",
                table: "seat_types");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roles",
                table: "roles");

            migrationBuilder.DropIndex(
                name: "uq_roles_name",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "price_paid",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "purchased_at",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "hall_id",
                table: "seats");

            migrationBuilder.DropColumn(
                name: "row",
                table: "seats");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "tickets",
                newName: "Tickets");

            migrationBuilder.RenameTable(
                name: "sessions",
                newName: "Sessions");

            migrationBuilder.RenameTable(
                name: "seats",
                newName: "Seats");

            migrationBuilder.RenameTable(
                name: "movies",
                newName: "Movies");

            migrationBuilder.RenameTable(
                name: "medias",
                newName: "Medias");

            migrationBuilder.RenameTable(
                name: "halls",
                newName: "Halls");

            migrationBuilder.RenameTable(
                name: "genres",
                newName: "Genres");

            migrationBuilder.RenameTable(
                name: "actors",
                newName: "Actors");

            migrationBuilder.RenameTable(
                name: "type_prices",
                newName: "TypePrices");

            migrationBuilder.RenameTable(
                name: "seat_types",
                newName: "SeatTypes");

            migrationBuilder.RenameTable(
                name: "roles",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "password_salt",
                table: "Users",
                newName: "PasswordHash_Salt");

            migrationBuilder.RenameIndex(
                name: "ix_users_email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Tickets",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "session_id",
                table: "Tickets",
                newName: "session");

            migrationBuilder.RenameColumn(
                name: "seat_id",
                table: "Tickets",
                newName: "seat");

            migrationBuilder.RenameIndex(
                name: "ix_tickets_user_id",
                table: "Tickets",
                newName: "IX_Tickets_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_tickets_session_id",
                table: "Tickets",
                newName: "IX_Tickets_session");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_seat_id",
                table: "Tickets",
                newName: "IX_Tickets_seat");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Sessions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "movie_id",
                table: "Sessions",
                newName: "movie");

            migrationBuilder.RenameColumn(
                name: "hall_id",
                table: "Sessions",
                newName: "hall");

            migrationBuilder.RenameColumn(
                name: "start_time",
                table: "Sessions",
                newName: "date");

            migrationBuilder.RenameIndex(
                name: "ix_sessions_movie_id",
                table: "Sessions",
                newName: "IX_Sessions_movie");

            migrationBuilder.RenameIndex(
                name: "ix_sessions_hall_id",
                table: "Sessions",
                newName: "IX_Sessions_hall");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Seats",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "uq_role_permissions_role_permission",
                table: "role_permissions",
                newName: "IX_role_permissions_role_id_permission_code");

            migrationBuilder.RenameColumn(
                name: "rate",
                table: "Movies",
                newName: "Rate");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Movies",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Movies",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Movies",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "age_limit",
                table: "Movies",
                newName: "AgeLimit");

            migrationBuilder.RenameColumn(
                name: "url",
                table: "Medias",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "Medias",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Medias",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Halls",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "hall_name",
                table: "Halls",
                newName: "HallName");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Genres",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Genres",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Actors",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "bio",
                table: "Actors",
                newName: "Bio");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Actors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "birth_date",
                table: "Actors",
                newName: "BirthDate");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "TypePrices",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "SeatTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Role",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Role",
                newName: "Id");

            migrationBuilder.AddColumn<DateTime>(
                name: "date",
                table: "Tickets",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<float>(
                name: "Rate",
                table: "Movies",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(3,2)",
                oldPrecision: 3,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Movies",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Medias",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Medias",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "HallName",
                table: "Halls",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "Actors",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sessions",
                table: "Sessions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seats",
                table: "Seats",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movies",
                table: "Movies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medias",
                table: "Medias",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Halls",
                table: "Halls",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genres",
                table: "Genres",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Actors",
                table: "Actors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypePrices",
                table: "TypePrices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeatTypes",
                table: "SeatTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TypePrices_session_id_seat_type_id",
                table: "TypePrices",
                columns: new[] { "session_id", "seat_type_id" });

            migrationBuilder.CreateIndex(
                name: "IX_SeatTypes_type_name",
                table: "SeatTypes",
                column: "type_name");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_movie_actors_Actors_actor_id",
                table: "movie_actors",
                column: "actor_id",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_movie_actors_Movies_movie_id",
                table: "movie_actors",
                column: "movie_id",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_movie_genres_Genres_genre_id",
                table: "movie_genres",
                column: "genre_id",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_movie_genres_Movies_movie_id",
                table: "movie_genres",
                column: "movie_id",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_movie_media_Medias_media_id",
                table: "movie_media",
                column: "media_id",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_movie_media_Movies_movie_id",
                table: "movie_media",
                column: "movie_id",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permissions_Role_role_id",
                table: "role_permissions",
                column: "role_id",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Halls_hall",
                table: "Sessions",
                column: "hall",
                principalTable: "Halls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Movies_movie",
                table: "Sessions",
                column: "movie",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Seats_seat",
                table: "Tickets",
                column: "seat",
                principalTable: "Seats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Sessions_session",
                table: "Tickets",
                column: "session",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_user_id",
                table: "Tickets",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_Role_role_id",
                table: "user_roles",
                column: "role_id",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_Users_user_id",
                table: "user_roles",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
