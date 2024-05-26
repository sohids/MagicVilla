using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla.Api.Migrations
{
    /// <inheritdoc />
    public partial class addaminitytoseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Amenity", "CreatedDate" },
                values: new object[] { "Sample amenity", new DateTime(2024, 5, 26, 23, 14, 49, 2, DateTimeKind.Local).AddTicks(1445) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Amenity", "CreatedDate" },
                values: new object[] { "Sample amenity", new DateTime(2024, 5, 26, 23, 14, 49, 2, DateTimeKind.Local).AddTicks(1461) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Amenity", "CreatedDate" },
                values: new object[] { "Sample amenity", new DateTime(2024, 5, 26, 23, 14, 49, 2, DateTimeKind.Local).AddTicks(1463) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Amenity", "CreatedDate" },
                values: new object[] { "Sample amenity", new DateTime(2024, 5, 26, 23, 14, 49, 2, DateTimeKind.Local).AddTicks(1464) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Amenity", "CreatedDate" },
                values: new object[] { "Sample amenity", new DateTime(2024, 5, 26, 23, 14, 49, 2, DateTimeKind.Local).AddTicks(1466) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Amenity", "CreatedDate" },
                values: new object[] { "", new DateTime(2024, 5, 26, 11, 15, 52, 776, DateTimeKind.Local).AddTicks(7172) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Amenity", "CreatedDate" },
                values: new object[] { "", new DateTime(2024, 5, 26, 11, 15, 52, 776, DateTimeKind.Local).AddTicks(7185) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Amenity", "CreatedDate" },
                values: new object[] { "", new DateTime(2024, 5, 26, 11, 15, 52, 776, DateTimeKind.Local).AddTicks(7187) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Amenity", "CreatedDate" },
                values: new object[] { "", new DateTime(2024, 5, 26, 11, 15, 52, 776, DateTimeKind.Local).AddTicks(7188) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Amenity", "CreatedDate" },
                values: new object[] { "", new DateTime(2024, 5, 26, 11, 15, 52, 776, DateTimeKind.Local).AddTicks(7190) });
        }
    }
}
