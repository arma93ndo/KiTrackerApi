using KiTrackerApi.Data.Context;
using KiTrackerApi.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace KiTrackerApi.Data.Seed;

public static class DbInitializer
{
    public static void SeedKiTrackerApiAsync(ModelBuilder modelBuilder)
    {
        // En este método, siembro datos de prueba para la base de datos de mi API. Todos los modelos
        // deben tener instancias.
        
        // 1. Colores.
        modelBuilder.Entity<Color>().HasData(
            new { Id = 1, Descripcion = "Rojo Pasión", CodigoHex = "#FF0000" },
            new { Id = 2, Descripcion = "Verde Namek", CodigoHex = "#00FF00" },
            new { Id = 3, Descripcion = "Azul Ki", CodigoHex = "#0000FF" },
            new { Id = 4, Descripcion = "Amarillo Super Saiyajin", CodigoHex = "#FFFF00" },
            new { Id = 5, Descripcion = "Púrpura Real", CodigoHex = "#800080" },
            new { Id = 6, Descripcion = "Negro Azabache", CodigoHex = "#000000" },
            new { Id = 7, Descripcion = "Blanco Puro", CodigoHex = "#FFFFFF" },
            new { Id = 8, Descripcion = "Naranja Kame", CodigoHex = "#FFA500" },
            new { Id = 9, Descripcion = "Dorado Aura", CodigoHex = "#FFD700" },
            new { Id = 10, Descripcion = "Plateado Metálico", CodigoHex = "#C0C0C0" }
        );

        // 2. Especies.
        modelBuilder.Entity<Especie>().HasData(
            new { Id = 1, Descripcion = "Saiyajin", Multiplicador = 1.5 },
            new { Id = 2, Descripcion = "Humano", Multiplicador = 1.0 },
            new { Id = 3, Descripcion = "Namekuseijin", Multiplicador = 1.2 },
            new { Id = 4, Descripcion = "Raza de Freezer", Multiplicador = 2.0 },
            new { Id = 5, Descripcion = "Androide / Bio-Androide", Multiplicador = 1.8 },
            new { Id = 6, Descripcion = "Majin", Multiplicador = 2.5 },
            new { Id = 7, Descripcion = "Shin-jin (Dios)", Multiplicador = 5.0 },
            new { Id = 8, Descripcion = "Habitante del Más Allá", Multiplicador = 1.1 },
            new { Id = 9, Descripcion = "Tritón Alienígena", Multiplicador = 1.3 },
            new { Id = 10, Descripcion = "Alienígena Desconocido", Multiplicador = 1.4 }
        );

        // 3. Dispositivos.
        modelBuilder.Entity<Dispositivo>().HasData(
            new { Id = 1, Fingerprint = "FP-SCOUTER-RED-01", Tipo = "mobile", ModeloHardware = "iPhone 15 Pro Max", FechaUltimoUso = DateTime.Parse("2026-08-10T14:30:00"), ColorId = 1 },
            new { Id = 2, Fingerprint = "FP-SCOUTER-GRN-02", Tipo = "mobile", ModeloHardware = "Samsung Galaxy S24 Ultra", FechaUltimoUso = DateTime.Parse("2026-08-12T09:15:00"), ColorId = 2 },
            new { Id = 3, Fingerprint = "FP-SCOUTER-BLU-03", Tipo = "web", ModeloHardware = "MacBook Pro 16 M3", FechaUltimoUso = DateTime.Parse("2026-08-15T18:45:00"), ColorId = 3 },
            new { Id = 4, Fingerprint = "FP-CAPSULE-001", Tipo = "mobile", ModeloHardware = "Google Pixel 8 Pro", FechaUltimoUso = DateTime.Parse("2026-08-20T11:20:00"), ColorId = 4 },
            new { Id = 5, Fingerprint = "FP-CAPSULE-002", Tipo = "web", ModeloHardware = "Dell XPS 15 9530", FechaUltimoUso = DateTime.Parse("2026-08-25T16:00:00"), ColorId = 9 },
            new { Id = 6, Fingerprint = "FP-REDRIBBON-01", Tipo = "mobile", ModeloHardware = "Xiaomi 14 Pro", FechaUltimoUso = DateTime.Parse("2026-08-28T20:10:00"), ColorId = 6 },
            new { Id = 7, Fingerprint = "FP-BABIDI-MAG-01", Tipo = "web", ModeloHardware = "Lenovo ThinkPad X1 Carbon", FechaUltimoUso = DateTime.Parse("2026-08-30T10:05:00"), ColorId = 5 },
            new { Id = 8, Fingerprint = "FP-SCOUTER-PUR-04", Tipo = "mobile", ModeloHardware = "OnePlus 12", FechaUltimoUso = DateTime.Parse("2026-09-01T12:00:00"), ColorId = 5 },
            new { Id = 9, Fingerprint = "FP-CAPSULE-003", Tipo = "web", ModeloHardware = "ASUS ROG Zephyrus G16", FechaUltimoUso = DateTime.Parse("2026-09-05T17:40:00"), ColorId = 7 },
            new { Id = 10, Fingerprint = "FP-SCOUTER-GLD-05", Tipo = "web", ModeloHardware = "HP Spectre x360", FechaUltimoUso = DateTime.Parse("2026-09-10T08:22:00"), ColorId = 9 }
        );

        // 4. Luchadores.
        modelBuilder.Entity<Luchador>().HasData(
            new { Id = 1, Nombre = "Son Goku", EspecieId = 1, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/goku.jpg", FechaRegistro = DateTime.Parse("2026-02-02T08:00:00") },
            new { Id = 2, Nombre = "Vegeta", EspecieId = 1, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/vegeta.jpg", FechaRegistro = DateTime.Parse("2026-03-03T08:00:00") },
            new { Id = 3, Nombre = "Son Gohan", EspecieId = 1, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/gohan.jpg", FechaRegistro = DateTime.Parse("2026-04-04T08:00:00") },
            new { Id = 4, Nombre = "Trunks del Futuro", EspecieId = 1, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/trunks_futuro.jpg", FechaRegistro = DateTime.Parse("2026-05-05T08:00:00") },
            new { Id = 5, Nombre = "Piccolo", EspecieId = 3, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/piccolo.jpg", FechaRegistro = DateTime.Parse("2026-06-06T08:00:00") },
            new { Id = 6, Nombre = "Krillin", EspecieId = 2, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/krillin.jpg", FechaRegistro = DateTime.Parse("2026-07-07T08:00:00") },
            new { Id = 7, Nombre = "Maestro Roshi", EspecieId = 2, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/roshi.jpg", FechaRegistro = DateTime.Parse("2026-08-08T08:00:00") },
            new { Id = 8, Nombre = "Ten Shin Han", EspecieId = 2, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/tenshinhan.jpg", FechaRegistro = DateTime.Parse("2026-09-09T08:00:00") },
            new { Id = 9, Nombre = "Yamcha", EspecieId = 2, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/yamcha.jpg", FechaRegistro = DateTime.Parse("2026-10-10T08:00:00") },
            new { Id = 10, Nombre = "Chaoz", EspecieId = 2, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/chaoz.jpg", FechaRegistro = DateTime.Parse("2026-11-11T08:00:00") },
            new { Id = 11, Nombre = "Freezer", EspecieId = 4, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/freezer.jpg", FechaRegistro = DateTime.Parse("2026-12-12T08:00:00") },
            new { Id = 12, Nombre = "Cell", EspecieId = 5, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/cell.jpg", FechaRegistro = DateTime.Parse("2026-01-13T08:00:00") },
            new { Id = 13, Nombre = "Majin Boo", EspecieId = 6, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/majin_boo.jpg", FechaRegistro = DateTime.Parse("2026-02-14T08:00:00") },
            new { Id = 14, Nombre = "Androide 17", EspecieId = 5, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/androide_17.jpg", FechaRegistro = DateTime.Parse("2026-03-15T08:00:00") },
            new { Id = 15, Nombre = "Androide 18", EspecieId = 5, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/androide_18.jpg", FechaRegistro = DateTime.Parse("2026-04-16T08:00:00") },
            new { Id = 16, Nombre = "Androide 16", EspecieId = 5, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/androide_16.jpg", FechaRegistro = DateTime.Parse("2026-05-17T08:00:00") },
            new { Id = 17, Nombre = "Androide 19", EspecieId = 5, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/androide_19.jpg", FechaRegistro = DateTime.Parse("2026-06-18T08:00:00") },
            new { Id = 18, Nombre = "Dr. Gero (Androide 20)", EspecieId = 5, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/dr_gero.jpg", FechaRegistro = DateTime.Parse("2026-07-19T08:00:00") },
            new { Id = 19, Nombre = "Raditz", EspecieId = 1, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/raditz.jpg", FechaRegistro = DateTime.Parse("2026-08-20T08:00:00") },
            new { Id = 20, Nombre = "Nappa", EspecieId = 1, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/nappa.jpg", FechaRegistro = DateTime.Parse("2026-09-21T08:00:00") },
            new { Id = 21, Nombre = "Dodoria", EspecieId = 4, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/dodoria.jpg", FechaRegistro = DateTime.Parse("2026-10-22T08:00:00") },
            new { Id = 22, Nombre = "Zarbon", EspecieId = 9, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/zarbon.jpg", FechaRegistro = DateTime.Parse("2026-11-23T08:00:00") },
            new { Id = 23, Nombre = "Capitán Ginyu", EspecieId = 10, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/capitan_ginyu.jpg", FechaRegistro = DateTime.Parse("2026-12-24T08:00:00") },
            new { Id = 24, Nombre = "Recoome", EspecieId = 10, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/recoome.jpg", FechaRegistro = DateTime.Parse("2026-01-25T08:00:00") },
            new { Id = 25, Nombre = "Burter", EspecieId = 10, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/burter.jpg", FechaRegistro = DateTime.Parse("2026-02-26T08:00:00") },
            new { Id = 26, Nombre = "Jeice", EspecieId = 10, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/jeice.jpg", FechaRegistro = DateTime.Parse("2026-03-27T08:00:00") },
            new { Id = 27, Nombre = "Guldo", EspecieId = 10, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/guldo.jpg", FechaRegistro = DateTime.Parse("2026-04-28T08:00:00") },
            new { Id = 28, Nombre = "Dende", EspecieId = 3, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/dende.jpg", FechaRegistro = DateTime.Parse("2026-05-01T08:00:00") },
            new { Id = 29, Nombre = "Mr. Satán", EspecieId = 2, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/mr_satan.jpg", FechaRegistro = DateTime.Parse("2026-06-02T08:00:00") },
            new { Id = 30, Nombre = "Videl", EspecieId = 2, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/videl.jpg", FechaRegistro = DateTime.Parse("2026-07-03T08:00:00") },
            new { Id = 31, Nombre = "Goten", EspecieId = 1, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/goten.jpg", FechaRegistro = DateTime.Parse("2026-08-04T08:00:00") },
            new { Id = 32, Nombre = "Trunks (Niño)", EspecieId = 1, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/trunks_nino.jpg", FechaRegistro = DateTime.Parse("2026-09-05T08:00:00") },
            new { Id = 33, Nombre = "Dabura", EspecieId = 6, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/dabura.jpg", FechaRegistro = DateTime.Parse("2026-10-06T08:00:00") },
            new { Id = 34, Nombre = "Kaioshin del Este", EspecieId = 7, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/kaioshin_este.jpg", FechaRegistro = DateTime.Parse("2026-11-07T08:00:00") },
            new { Id = 35, Nombre = "Kibito", EspecieId = 7, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/kibito.jpg", FechaRegistro = DateTime.Parse("2026-12-08T08:00:00") },
            new { Id = 36, Nombre = "Anciano Kaioshin", EspecieId = 7, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/anciano_kaioshin.jpg", FechaRegistro = DateTime.Parse("2026-01-09T08:00:00") },
            new { Id = 37, Nombre = "King Kai (Kaio del Norte)", EspecieId = 7, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/kaio_del_norte.jpg", FechaRegistro = DateTime.Parse("2026-02-10T08:00:00") },
            new { Id = 38, Nombre = "Yajirobe", EspecieId = 2, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/yajirobe.jpg", FechaRegistro = DateTime.Parse("2026-03-11T08:00:00") },
            new { Id = 39, Nombre = "Bardock", EspecieId = 1, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/bardock.jpg", FechaRegistro = DateTime.Parse("2026-04-12T08:00:00") },
            new { Id = 40, Nombre = "Broly", EspecieId = 1, FotoPerfilUrl = "https://kitracker.blob.core.windows.net/luchadores/broly.jpg", FechaRegistro = DateTime.Parse("2026-05-13T08:00:00") }
        );

        // 5. Lecturas.
        modelBuilder.Entity<Lectura>().HasData(
            new { Id = 1, LuchadorId = 32, DispositivoId = 9, NivelKi = 21715456L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/trunks_nino_lectura_1.jpg", FechaLectura = DateTime.Parse("2026-03-24T05:57:33") },
            new { Id = 2, LuchadorId = 8, DispositivoId = 3, NivelKi = 11776L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/tenshinhan_lectura_2.jpg", FechaLectura = DateTime.Parse("2026-08-05T07:29:43") },
            new { Id = 3, LuchadorId = 21, DispositivoId = 10, NivelKi = 22000L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/dodoria_lectura_3.jpg", FechaLectura = DateTime.Parse("2026-03-01T20:30:19") },
            new { Id = 4, LuchadorId = 10, DispositivoId = 1, NivelKi = 610L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/chaoz_lectura_4.jpg", FechaLectura = DateTime.Parse("2026-05-12T03:52:13") },
            new { Id = 5, LuchadorId = 27, DispositivoId = 1, NivelKi = 10079L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/guldo_lectura_5.jpg", FechaLectura = DateTime.Parse("2026-04-20T08:44:03") },
            new { Id = 6, LuchadorId = 16, DispositivoId = 8, NivelKi = 320000000L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/androide_16_lectura_6.jpg", FechaLectura = DateTime.Parse("2026-04-06T19:54:12") },
            new { Id = 7, LuchadorId = 12, DispositivoId = 10, NivelKi = 312781498L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/cell_lectura_7.jpg", FechaLectura = DateTime.Parse("2026-07-28T02:00:03") },
            new { Id = 8, LuchadorId = 3, DispositivoId = 5, NivelKi = 926066226L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/gohan_lectura_8.jpg", FechaLectura = DateTime.Parse("2026-02-17T20:58:36") },
            new { Id = 9, LuchadorId = 8, DispositivoId = 1, NivelKi = 3804L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/tenshinhan_lectura_9.jpg", FechaLectura = DateTime.Parse("2026-06-12T16:05:44") },
            new { Id = 10, LuchadorId = 22, DispositivoId = 1, NivelKi = 28169L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/zarbon_lectura_10.jpg", FechaLectura = DateTime.Parse("2026-05-13T09:41:27") },
            new { Id = 11, LuchadorId = 36, DispositivoId = 2, NivelKi = 952329L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/anciano_kaioshin_lectura_11.jpg", FechaLectura = DateTime.Parse("2026-02-18T20:00:54") },
            new { Id = 12, LuchadorId = 37, DispositivoId = 1, NivelKi = 4136L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/kaio_del_norte_lectura_12.jpg", FechaLectura = DateTime.Parse("2026-02-22T05:39:46") },
            new { Id = 13, LuchadorId = 32, DispositivoId = 3, NivelKi = 43236081L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/trunks_nino_lectura_13.jpg", FechaLectura = DateTime.Parse("2026-02-16T15:26:07") },
            new { Id = 14, LuchadorId = 37, DispositivoId = 6, NivelKi = 4106L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/kaio_del_norte_lectura_14.jpg", FechaLectura = DateTime.Parse("2026-05-19T06:50:47") },
            new { Id = 15, LuchadorId = 37, DispositivoId = 9, NivelKi = 4825L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/kaio_del_norte_lectura_15.jpg", FechaLectura = DateTime.Parse("2026-04-18T10:19:42") },
            new { Id = 16, LuchadorId = 8, DispositivoId = 8, NivelKi = 11956L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/tenshinhan_lectura_16.jpg", FechaLectura = DateTime.Parse("2026-01-08T07:11:39") },
            new { Id = 17, LuchadorId = 30, DispositivoId = 2, NivelKi = 14L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/videl_lectura_17.jpg", FechaLectura = DateTime.Parse("2026-01-20T21:18:24") },
            new { Id = 18, LuchadorId = 8, DispositivoId = 3, NivelKi = 10178L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/tenshinhan_lectura_18.jpg", FechaLectura = DateTime.Parse("2026-07-26T21:35:10") },
            new { Id = 19, LuchadorId = 3, DispositivoId = 8, NivelKi = 81373511L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/gohan_lectura_19.jpg", FechaLectura = DateTime.Parse("2026-04-03T16:21:40") },
            new { Id = 20, LuchadorId = 20, DispositivoId = 2, NivelKi = 4000L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/nappa_lectura_20.jpg", FechaLectura = DateTime.Parse("2026-06-03T15:02:18") },
            new { Id = 21, LuchadorId = 12, DispositivoId = 8, NivelKi = 378809462L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/cell_lectura_21.jpg", FechaLectura = DateTime.Parse("2026-07-28T09:44:03") },
            new { Id = 22, LuchadorId = 10, DispositivoId = 7, NivelKi = 610L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/chaoz_lectura_22.jpg", FechaLectura = DateTime.Parse("2026-01-20T21:55:25") },
            new { Id = 23, LuchadorId = 8, DispositivoId = 7, NivelKi = 15302L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/tenshinhan_lectura_23.jpg", FechaLectura = DateTime.Parse("2026-07-16T21:37:37") },
            new { Id = 24, LuchadorId = 24, DispositivoId = 10, NivelKi = 56213L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/recoome_lectura_24.jpg", FechaLectura = DateTime.Parse("2026-07-16T12:00:30") },
            new { Id = 25, LuchadorId = 16, DispositivoId = 2, NivelKi = 320000000L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/androide_16_lectura_25.jpg", FechaLectura = DateTime.Parse("2026-05-02T13:30:17") },
            new { Id = 26, LuchadorId = 29, DispositivoId = 4, NivelKi = 13L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/mr_satan_lectura_26.jpg", FechaLectura = DateTime.Parse("2026-06-18T05:27:14") },
            new { Id = 27, LuchadorId = 6, DispositivoId = 1, NivelKi = 32742L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/krillin_lectura_27.jpg", FechaLectura = DateTime.Parse("2026-02-09T03:34:52") },
            new { Id = 28, LuchadorId = 4, DispositivoId = 3, NivelKi = 175658608L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/trunks_futuro_lectura_28.jpg", FechaLectura = DateTime.Parse("2026-02-27T08:34:03") },
            new { Id = 29, LuchadorId = 32, DispositivoId = 8, NivelKi = 11186762L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/trunks_nino_lectura_29.jpg", FechaLectura = DateTime.Parse("2026-07-28T07:15:38") },
            new { Id = 30, LuchadorId = 27, DispositivoId = 8, NivelKi = 10398L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/guldo_lectura_30.jpg", FechaLectura = DateTime.Parse("2026-01-22T05:52:16") },
            new { Id = 31, LuchadorId = 12, DispositivoId = 9, NivelKi = 794354291L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/cell_lectura_31.jpg", FechaLectura = DateTime.Parse("2026-08-28T22:38:09") },
            new { Id = 32, LuchadorId = 25, DispositivoId = 2, NivelKi = 55760L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/burter_lectura_32.jpg", FechaLectura = DateTime.Parse("2026-06-03T07:17:49") },
            new { Id = 33, LuchadorId = 17, DispositivoId = 4, NivelKi = 11166316L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/androide_19_lectura_33.jpg", FechaLectura = DateTime.Parse("2026-04-12T16:36:20") },
            new { Id = 34, LuchadorId = 28, DispositivoId = 7, NivelKi = 29L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/dende_lectura_34.jpg", FechaLectura = DateTime.Parse("2026-06-25T11:42:38") },
            new { Id = 35, LuchadorId = 24, DispositivoId = 6, NivelKi = 59005L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/recoome_lectura_35.jpg", FechaLectura = DateTime.Parse("2026-08-16T18:04:47") },
            new { Id = 36, LuchadorId = 18, DispositivoId = 7, NivelKi = 17997973L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/dr_gero_lectura_36.jpg", FechaLectura = DateTime.Parse("2026-04-18T05:07:37") },
            new { Id = 37, LuchadorId = 32, DispositivoId = 9, NivelKi = 100720272L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/trunks_nino_lectura_37.jpg", FechaLectura = DateTime.Parse("2026-05-02T19:25:21") },
            new { Id = 38, LuchadorId = 35, DispositivoId = 9, NivelKi = 217686523L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/kibito_lectura_38.jpg", FechaLectura = DateTime.Parse("2026-06-06T00:54:15") },
            new { Id = 39, LuchadorId = 32, DispositivoId = 7, NivelKi = 103986927L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/trunks_nino_lectura_39.jpg", FechaLectura = DateTime.Parse("2026-06-26T21:28:13") },
            new { Id = 40, LuchadorId = 39, DispositivoId = 9, NivelKi = 10000L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/bardock_lectura_40.jpg", FechaLectura = DateTime.Parse("2026-05-18T10:14:26") },
            new { Id = 41, LuchadorId = 1, DispositivoId = 9, NivelKi = 135706509L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/goku_lectura_41.jpg", FechaLectura = DateTime.Parse("2026-06-04T12:00:19") },
            new { Id = 42, LuchadorId = 22, DispositivoId = 8, NivelKi = 28691L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/zarbon_lectura_42.jpg", FechaLectura = DateTime.Parse("2026-04-26T18:43:08") },
            new { Id = 43, LuchadorId = 12, DispositivoId = 7, NivelKi = 269784132L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/cell_lectura_43.jpg", FechaLectura = DateTime.Parse("2026-03-22T02:02:18") },
            new { Id = 44, LuchadorId = 8, DispositivoId = 2, NivelKi = 14357L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/tenshinhan_lectura_44.jpg", FechaLectura = DateTime.Parse("2026-02-17T03:59:36") },
            new { Id = 45, LuchadorId = 27, DispositivoId = 3, NivelKi = 10567L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/guldo_lectura_45.jpg", FechaLectura = DateTime.Parse("2026-07-28T09:21:49") },
            new { Id = 46, LuchadorId = 13, DispositivoId = 5, NivelKi = 1530960573L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/majin_boo_lectura_46.jpg", FechaLectura = DateTime.Parse("2026-04-06T09:47:11") },
            new { Id = 47, LuchadorId = 2, DispositivoId = 8, NivelKi = 31057418L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/vegeta_lectura_47.jpg", FechaLectura = DateTime.Parse("2026-03-24T05:07:37") },
            new { Id = 48, LuchadorId = 32, DispositivoId = 1, NivelKi = 56934520L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/trunks_nino_lectura_48.jpg", FechaLectura = DateTime.Parse("2026-01-20T04:09:59") },
            new { Id = 49, LuchadorId = 7, DispositivoId = 10, NivelKi = 175L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/roshi_lectura_49.jpg", FechaLectura = DateTime.Parse("2026-01-21T02:08:42") },
            new { Id = 50, LuchadorId = 40, DispositivoId = 10, NivelKi = 2816913501L, RutaFotografia = "https://kitracker.blob.core.windows.net/lecturas/broly_lectura_50.jpg", FechaLectura = DateTime.Parse("2026-01-09T03:31:07") }
        );

        Console.WriteLine("🌱 BBDD sembrada con datos de prueba satisfactoriamente.");
    }
}