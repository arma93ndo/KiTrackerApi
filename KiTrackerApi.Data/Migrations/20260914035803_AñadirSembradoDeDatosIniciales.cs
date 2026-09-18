using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KiTrackerApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AñadirSembradoDeDatosIniciales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Colores",
                columns: new[] { "Id", "CodigoHex", "Descripcion" },
                values: new object[,]
                {
                    { 1, "#FF0000", "Rojo Pasión" },
                    { 2, "#00FF00", "Verde Namek" },
                    { 3, "#0000FF", "Azul Ki" },
                    { 4, "#FFFF00", "Amarillo Super Saiyajin" },
                    { 5, "#800080", "Púrpura Real" },
                    { 6, "#000000", "Negro Azabache" },
                    { 7, "#FFFFFF", "Blanco Puro" },
                    { 8, "#FFA500", "Naranja Kame" },
                    { 9, "#FFD700", "Dorado Aura" },
                    { 10, "#C0C0C0", "Plateado Metálico" }
                });

            migrationBuilder.InsertData(
                table: "Especies",
                columns: new[] { "Id", "Descripcion", "Multiplicador" },
                values: new object[,]
                {
                    { 1, "Saiyajin", 1.5 },
                    { 2, "Humano", 1.0 },
                    { 3, "Namekuseijin", 1.2 },
                    { 4, "Raza de Freezer", 2.0 },
                    { 5, "Androide / Bio-Androide", 1.8 },
                    { 6, "Majin", 2.5 },
                    { 7, "Shin-jin (Dios)", 5.0 },
                    { 8, "Habitante del Más Allá", 1.1000000000000001 },
                    { 9, "Tritón Alienígena", 1.3 },
                    { 10, "Alienígena Desconocido", 1.3999999999999999 }
                });

            migrationBuilder.InsertData(
                table: "Dispositivos",
                columns: new[] { "Id", "ColorId", "FechaUltimoUso", "Fingerprint", "ModeloHardware", "Tipo" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 8, 10, 14, 30, 0, 0, DateTimeKind.Unspecified), "FP-SCOUTER-RED-01", "iPhone 15 Pro Max", "mobile" },
                    { 2, 2, new DateTime(2026, 8, 12, 9, 15, 0, 0, DateTimeKind.Unspecified), "FP-SCOUTER-GRN-02", "Samsung Galaxy S24 Ultra", "mobile" },
                    { 3, 3, new DateTime(2026, 8, 15, 18, 45, 0, 0, DateTimeKind.Unspecified), "FP-SCOUTER-BLU-03", "MacBook Pro 16 M3", "web" },
                    { 4, 4, new DateTime(2026, 8, 20, 11, 20, 0, 0, DateTimeKind.Unspecified), "FP-CAPSULE-001", "Google Pixel 8 Pro", "mobile" },
                    { 5, 9, new DateTime(2026, 8, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), "FP-CAPSULE-002", "Dell XPS 15 9530", "web" },
                    { 6, 6, new DateTime(2026, 8, 28, 20, 10, 0, 0, DateTimeKind.Unspecified), "FP-REDRIBBON-01", "Xiaomi 14 Pro", "mobile" },
                    { 7, 5, new DateTime(2026, 8, 30, 10, 5, 0, 0, DateTimeKind.Unspecified), "FP-BABIDI-MAG-01", "Lenovo ThinkPad X1 Carbon", "web" },
                    { 8, 5, new DateTime(2026, 9, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "FP-SCOUTER-PUR-04", "OnePlus 12", "mobile" },
                    { 9, 7, new DateTime(2026, 9, 5, 17, 40, 0, 0, DateTimeKind.Unspecified), "FP-CAPSULE-003", "ASUS ROG Zephyrus G16", "web" },
                    { 10, 9, new DateTime(2026, 9, 10, 8, 22, 0, 0, DateTimeKind.Unspecified), "FP-SCOUTER-GLD-05", "HP Spectre x360", "web" }
                });

            migrationBuilder.InsertData(
                table: "Luchadores",
                columns: new[] { "Id", "EspecieId", "FechaRegistro", "FotoPerfilUrl", "Nombre" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 2, 2, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/goku.jpg", "Son Goku" },
                    { 2, 1, new DateTime(2026, 3, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/vegeta.jpg", "Vegeta" },
                    { 3, 1, new DateTime(2026, 4, 4, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/gohan.jpg", "Son Gohan" },
                    { 4, 1, new DateTime(2026, 5, 5, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/trunks_futuro.jpg", "Trunks del Futuro" },
                    { 5, 3, new DateTime(2026, 6, 6, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/piccolo.jpg", "Piccolo" },
                    { 6, 2, new DateTime(2026, 7, 7, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/krillin.jpg", "Krillin" },
                    { 7, 2, new DateTime(2026, 8, 8, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/roshi.jpg", "Maestro Roshi" },
                    { 8, 2, new DateTime(2026, 9, 9, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/tenshinhan.jpg", "Ten Shin Han" },
                    { 9, 2, new DateTime(2026, 10, 10, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/yamcha.jpg", "Yamcha" },
                    { 10, 2, new DateTime(2026, 11, 11, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/chaoz.jpg", "Chaoz" },
                    { 11, 4, new DateTime(2026, 12, 12, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/freezer.jpg", "Freezer" },
                    { 12, 5, new DateTime(2026, 1, 13, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/cell.jpg", "Cell" },
                    { 13, 6, new DateTime(2026, 2, 14, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/majin_boo.jpg", "Majin Boo" },
                    { 14, 5, new DateTime(2026, 3, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/androide_17.jpg", "Androide 17" },
                    { 15, 5, new DateTime(2026, 4, 16, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/androide_18.jpg", "Androide 18" },
                    { 16, 5, new DateTime(2026, 5, 17, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/androide_16.jpg", "Androide 16" },
                    { 17, 5, new DateTime(2026, 6, 18, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/androide_19.jpg", "Androide 19" },
                    { 18, 5, new DateTime(2026, 7, 19, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/dr_gero.jpg", "Dr. Gero (Androide 20)" },
                    { 19, 1, new DateTime(2026, 8, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/raditz.jpg", "Raditz" },
                    { 20, 1, new DateTime(2026, 9, 21, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/nappa.jpg", "Nappa" },
                    { 21, 4, new DateTime(2026, 10, 22, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/dodoria.jpg", "Dodoria" },
                    { 22, 9, new DateTime(2026, 11, 23, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/zarbon.jpg", "Zarbon" },
                    { 23, 10, new DateTime(2026, 12, 24, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/capitan_ginyu.jpg", "Capitán Ginyu" },
                    { 24, 10, new DateTime(2026, 1, 25, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/recoome.jpg", "Recoome" },
                    { 25, 10, new DateTime(2026, 2, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/burter.jpg", "Burter" },
                    { 26, 10, new DateTime(2026, 3, 27, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/jeice.jpg", "Jeice" },
                    { 27, 10, new DateTime(2026, 4, 28, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/guldo.jpg", "Guldo" },
                    { 28, 3, new DateTime(2026, 5, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/dende.jpg", "Dende" },
                    { 29, 2, new DateTime(2026, 6, 2, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/mr_satan.jpg", "Mr. Satán" },
                    { 30, 2, new DateTime(2026, 7, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/videl.jpg", "Videl" },
                    { 31, 1, new DateTime(2026, 8, 4, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/goten.jpg", "Goten" },
                    { 32, 1, new DateTime(2026, 9, 5, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/trunks_nino.jpg", "Trunks (Niño)" },
                    { 33, 6, new DateTime(2026, 10, 6, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/dabura.jpg", "Dabura" },
                    { 34, 7, new DateTime(2026, 11, 7, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/kaioshin_este.jpg", "Kaioshin del Este" },
                    { 35, 7, new DateTime(2026, 12, 8, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/kibito.jpg", "Kibito" },
                    { 36, 7, new DateTime(2026, 1, 9, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/anciano_kaioshin.jpg", "Anciano Kaioshin" },
                    { 37, 7, new DateTime(2026, 2, 10, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/kaio_del_norte.jpg", "King Kai (Kaio del Norte)" },
                    { 38, 2, new DateTime(2026, 3, 11, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/yajirobe.jpg", "Yajirobe" },
                    { 39, 1, new DateTime(2026, 4, 12, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/bardock.jpg", "Bardock" },
                    { 40, 1, new DateTime(2026, 5, 13, 8, 0, 0, 0, DateTimeKind.Unspecified), "https://kitracker.blob.core.windows.net/luchadores/broly.jpg", "Broly" }
                });

            migrationBuilder.InsertData(
                table: "Lecturas",
                columns: new[] { "Id", "DispositivoId", "FechaLectura", "LuchadorId", "NivelKi", "RutaFotografia" },
                values: new object[,]
                {
                    { 1, 9, new DateTime(2026, 3, 24, 5, 57, 33, 0, DateTimeKind.Unspecified), 32, 21715456L, "https://kitracker.blob.core.windows.net/lecturas/trunks_nino_lectura_1.jpg" },
                    { 2, 3, new DateTime(2026, 8, 5, 7, 29, 43, 0, DateTimeKind.Unspecified), 8, 11776L, "https://kitracker.blob.core.windows.net/lecturas/tenshinhan_lectura_2.jpg" },
                    { 3, 10, new DateTime(2026, 3, 1, 20, 30, 19, 0, DateTimeKind.Unspecified), 21, 22000L, "https://kitracker.blob.core.windows.net/lecturas/dodoria_lectura_3.jpg" },
                    { 4, 1, new DateTime(2026, 5, 12, 3, 52, 13, 0, DateTimeKind.Unspecified), 10, 610L, "https://kitracker.blob.core.windows.net/lecturas/chaoz_lectura_4.jpg" },
                    { 5, 1, new DateTime(2026, 4, 20, 8, 44, 3, 0, DateTimeKind.Unspecified), 27, 10079L, "https://kitracker.blob.core.windows.net/lecturas/guldo_lectura_5.jpg" },
                    { 6, 8, new DateTime(2026, 4, 6, 19, 54, 12, 0, DateTimeKind.Unspecified), 16, 320000000L, "https://kitracker.blob.core.windows.net/lecturas/androide_16_lectura_6.jpg" },
                    { 7, 10, new DateTime(2026, 7, 28, 2, 0, 3, 0, DateTimeKind.Unspecified), 12, 312781498L, "https://kitracker.blob.core.windows.net/lecturas/cell_lectura_7.jpg" },
                    { 8, 5, new DateTime(2026, 2, 17, 20, 58, 36, 0, DateTimeKind.Unspecified), 3, 926066226L, "https://kitracker.blob.core.windows.net/lecturas/gohan_lectura_8.jpg" },
                    { 9, 1, new DateTime(2026, 6, 12, 16, 5, 44, 0, DateTimeKind.Unspecified), 8, 3804L, "https://kitracker.blob.core.windows.net/lecturas/tenshinhan_lectura_9.jpg" },
                    { 10, 1, new DateTime(2026, 5, 13, 9, 41, 27, 0, DateTimeKind.Unspecified), 22, 28169L, "https://kitracker.blob.core.windows.net/lecturas/zarbon_lectura_10.jpg" },
                    { 11, 2, new DateTime(2026, 2, 18, 20, 0, 54, 0, DateTimeKind.Unspecified), 36, 952329L, "https://kitracker.blob.core.windows.net/lecturas/anciano_kaioshin_lectura_11.jpg" },
                    { 12, 1, new DateTime(2026, 2, 22, 5, 39, 46, 0, DateTimeKind.Unspecified), 37, 4136L, "https://kitracker.blob.core.windows.net/lecturas/kaio_del_norte_lectura_12.jpg" },
                    { 13, 3, new DateTime(2026, 2, 16, 15, 26, 7, 0, DateTimeKind.Unspecified), 32, 43236081L, "https://kitracker.blob.core.windows.net/lecturas/trunks_nino_lectura_13.jpg" },
                    { 14, 6, new DateTime(2026, 5, 19, 6, 50, 47, 0, DateTimeKind.Unspecified), 37, 4106L, "https://kitracker.blob.core.windows.net/lecturas/kaio_del_norte_lectura_14.jpg" },
                    { 15, 9, new DateTime(2026, 4, 18, 10, 19, 42, 0, DateTimeKind.Unspecified), 37, 4825L, "https://kitracker.blob.core.windows.net/lecturas/kaio_del_norte_lectura_15.jpg" },
                    { 16, 8, new DateTime(2026, 1, 8, 7, 11, 39, 0, DateTimeKind.Unspecified), 8, 11956L, "https://kitracker.blob.core.windows.net/lecturas/tenshinhan_lectura_16.jpg" },
                    { 17, 2, new DateTime(2026, 1, 20, 21, 18, 24, 0, DateTimeKind.Unspecified), 30, 14L, "https://kitracker.blob.core.windows.net/lecturas/videl_lectura_17.jpg" },
                    { 18, 3, new DateTime(2026, 7, 26, 21, 35, 10, 0, DateTimeKind.Unspecified), 8, 10178L, "https://kitracker.blob.core.windows.net/lecturas/tenshinhan_lectura_18.jpg" },
                    { 19, 8, new DateTime(2026, 4, 3, 16, 21, 40, 0, DateTimeKind.Unspecified), 3, 81373511L, "https://kitracker.blob.core.windows.net/lecturas/gohan_lectura_19.jpg" },
                    { 20, 2, new DateTime(2026, 6, 3, 15, 2, 18, 0, DateTimeKind.Unspecified), 20, 4000L, "https://kitracker.blob.core.windows.net/lecturas/nappa_lectura_20.jpg" },
                    { 21, 8, new DateTime(2026, 7, 28, 9, 44, 3, 0, DateTimeKind.Unspecified), 12, 378809462L, "https://kitracker.blob.core.windows.net/lecturas/cell_lectura_21.jpg" },
                    { 22, 7, new DateTime(2026, 1, 20, 21, 55, 25, 0, DateTimeKind.Unspecified), 10, 610L, "https://kitracker.blob.core.windows.net/lecturas/chaoz_lectura_22.jpg" },
                    { 23, 7, new DateTime(2026, 7, 16, 21, 37, 37, 0, DateTimeKind.Unspecified), 8, 15302L, "https://kitracker.blob.core.windows.net/lecturas/tenshinhan_lectura_23.jpg" },
                    { 24, 10, new DateTime(2026, 7, 16, 12, 0, 30, 0, DateTimeKind.Unspecified), 24, 56213L, "https://kitracker.blob.core.windows.net/lecturas/recoome_lectura_24.jpg" },
                    { 25, 2, new DateTime(2026, 5, 2, 13, 30, 17, 0, DateTimeKind.Unspecified), 16, 320000000L, "https://kitracker.blob.core.windows.net/lecturas/androide_16_lectura_25.jpg" },
                    { 26, 4, new DateTime(2026, 6, 18, 5, 27, 14, 0, DateTimeKind.Unspecified), 29, 13L, "https://kitracker.blob.core.windows.net/lecturas/mr_satan_lectura_26.jpg" },
                    { 27, 1, new DateTime(2026, 2, 9, 3, 34, 52, 0, DateTimeKind.Unspecified), 6, 32742L, "https://kitracker.blob.core.windows.net/lecturas/krillin_lectura_27.jpg" },
                    { 28, 3, new DateTime(2026, 2, 27, 8, 34, 3, 0, DateTimeKind.Unspecified), 4, 175658608L, "https://kitracker.blob.core.windows.net/lecturas/trunks_futuro_lectura_28.jpg" },
                    { 29, 8, new DateTime(2026, 7, 28, 7, 15, 38, 0, DateTimeKind.Unspecified), 32, 11186762L, "https://kitracker.blob.core.windows.net/lecturas/trunks_nino_lectura_29.jpg" },
                    { 30, 8, new DateTime(2026, 1, 22, 5, 52, 16, 0, DateTimeKind.Unspecified), 27, 10398L, "https://kitracker.blob.core.windows.net/lecturas/guldo_lectura_30.jpg" },
                    { 31, 9, new DateTime(2026, 8, 28, 22, 38, 9, 0, DateTimeKind.Unspecified), 12, 794354291L, "https://kitracker.blob.core.windows.net/lecturas/cell_lectura_31.jpg" },
                    { 32, 2, new DateTime(2026, 6, 3, 7, 17, 49, 0, DateTimeKind.Unspecified), 25, 55760L, "https://kitracker.blob.core.windows.net/lecturas/burter_lectura_32.jpg" },
                    { 33, 4, new DateTime(2026, 4, 12, 16, 36, 20, 0, DateTimeKind.Unspecified), 17, 11166316L, "https://kitracker.blob.core.windows.net/lecturas/androide_19_lectura_33.jpg" },
                    { 34, 7, new DateTime(2026, 6, 25, 11, 42, 38, 0, DateTimeKind.Unspecified), 28, 29L, "https://kitracker.blob.core.windows.net/lecturas/dende_lectura_34.jpg" },
                    { 35, 6, new DateTime(2026, 8, 16, 18, 4, 47, 0, DateTimeKind.Unspecified), 24, 59005L, "https://kitracker.blob.core.windows.net/lecturas/recoome_lectura_35.jpg" },
                    { 36, 7, new DateTime(2026, 4, 18, 5, 7, 37, 0, DateTimeKind.Unspecified), 18, 17997973L, "https://kitracker.blob.core.windows.net/lecturas/dr_gero_lectura_36.jpg" },
                    { 37, 9, new DateTime(2026, 5, 2, 19, 25, 21, 0, DateTimeKind.Unspecified), 32, 100720272L, "https://kitracker.blob.core.windows.net/lecturas/trunks_nino_lectura_37.jpg" },
                    { 38, 9, new DateTime(2026, 6, 6, 0, 54, 15, 0, DateTimeKind.Unspecified), 35, 217686523L, "https://kitracker.blob.core.windows.net/lecturas/kibito_lectura_38.jpg" },
                    { 39, 7, new DateTime(2026, 6, 26, 21, 28, 13, 0, DateTimeKind.Unspecified), 32, 103986927L, "https://kitracker.blob.core.windows.net/lecturas/trunks_nino_lectura_39.jpg" },
                    { 40, 9, new DateTime(2026, 5, 18, 10, 14, 26, 0, DateTimeKind.Unspecified), 39, 10000L, "https://kitracker.blob.core.windows.net/lecturas/bardock_lectura_40.jpg" },
                    { 41, 9, new DateTime(2026, 6, 4, 12, 0, 19, 0, DateTimeKind.Unspecified), 1, 135706509L, "https://kitracker.blob.core.windows.net/lecturas/goku_lectura_41.jpg" },
                    { 42, 8, new DateTime(2026, 4, 26, 18, 43, 8, 0, DateTimeKind.Unspecified), 22, 28691L, "https://kitracker.blob.core.windows.net/lecturas/zarbon_lectura_42.jpg" },
                    { 43, 7, new DateTime(2026, 3, 22, 2, 2, 18, 0, DateTimeKind.Unspecified), 12, 269784132L, "https://kitracker.blob.core.windows.net/lecturas/cell_lectura_43.jpg" },
                    { 44, 2, new DateTime(2026, 2, 17, 3, 59, 36, 0, DateTimeKind.Unspecified), 8, 14357L, "https://kitracker.blob.core.windows.net/lecturas/tenshinhan_lectura_44.jpg" },
                    { 45, 3, new DateTime(2026, 7, 28, 9, 21, 49, 0, DateTimeKind.Unspecified), 27, 10567L, "https://kitracker.blob.core.windows.net/lecturas/guldo_lectura_45.jpg" },
                    { 46, 5, new DateTime(2026, 4, 6, 9, 47, 11, 0, DateTimeKind.Unspecified), 13, 1530960573L, "https://kitracker.blob.core.windows.net/lecturas/majin_boo_lectura_46.jpg" },
                    { 47, 8, new DateTime(2026, 3, 24, 5, 7, 37, 0, DateTimeKind.Unspecified), 2, 31057418L, "https://kitracker.blob.core.windows.net/lecturas/vegeta_lectura_47.jpg" },
                    { 48, 1, new DateTime(2026, 1, 20, 4, 9, 59, 0, DateTimeKind.Unspecified), 32, 56934520L, "https://kitracker.blob.core.windows.net/lecturas/trunks_nino_lectura_48.jpg" },
                    { 49, 10, new DateTime(2026, 1, 21, 2, 8, 42, 0, DateTimeKind.Unspecified), 7, 175L, "https://kitracker.blob.core.windows.net/lecturas/roshi_lectura_49.jpg" },
                    { 50, 10, new DateTime(2026, 1, 9, 3, 31, 7, 0, DateTimeKind.Unspecified), 40, 2816913501L, "https://kitracker.blob.core.windows.net/lecturas/broly_lectura_50.jpg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Colores",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Colores",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Especies",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Lecturas",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Dispositivos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Dispositivos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Dispositivos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Dispositivos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Dispositivos",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Dispositivos",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Dispositivos",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Dispositivos",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Dispositivos",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Dispositivos",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Luchadores",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Colores",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Colores",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Colores",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Colores",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Colores",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Colores",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Colores",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Colores",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Especies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Especies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Especies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Especies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Especies",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Especies",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Especies",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Especies",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Especies",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
