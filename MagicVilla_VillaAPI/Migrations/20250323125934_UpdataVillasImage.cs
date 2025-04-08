using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_VillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdataVillasImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 23, 0, 0, 0, 0, DateTimeKind.Local), "images/villa/villa1.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 23, 0, 0, 0, 0, DateTimeKind.Local), "images/villa/villa2.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 23, 0, 0, 0, 0, DateTimeKind.Local), "htt" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 23, 0, 0, 0, 0, DateTimeKind.Local), "images/villa/villa3.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 23, 0, 0, 0, 0, DateTimeKind.Local), "images/villa/villa4.jpg" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 14, 0, 0, 0, 0, DateTimeKind.Local), "https://dotnetmasteryimages.com/bluevillaimages/villa3.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 14, 0, 0, 0, 0, DateTimeKind.Local), "https://dotnetmasteryimages.com/bluevillaimages/villa1.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 14, 0, 0, 0, 0, DateTimeKind.Local), "https://dotnetmasteryimages.com/bluevillaimages/villa4.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 14, 0, 0, 0, 0, DateTimeKind.Local), "https://dotnetmasteryimages.com/bluevillaimages/villa5.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2025, 3, 14, 0, 0, 0, 0, DateTimeKind.Local), "https://dotnetmasteryimages.com/bluevillaimages/villa2.jpg" });
        }
    }
}
