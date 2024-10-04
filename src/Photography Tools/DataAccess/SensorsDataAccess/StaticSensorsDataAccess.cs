using System.Collections.Frozen;
using System.Collections.Immutable;

namespace Photography_Tools.DataAccess.SensorsDataAccess;

public class StaticSensorsDataAccess : ISensorsDataAccess
{
    private readonly static FrozenDictionary<string, Sensor> sensors;
    private readonly static ImmutableArray<string> keys;

    static StaticSensorsDataAccess()
    {
        // Source: CameraSensorSizeDatabase, https://github.com/openMVG/CameraSensorSizeDatabase, access date: 08.08.2024
        sensors = new Dictionary<string, Sensor>()
        {
            { "Full Frame 24 Mpix", new Sensor(24, 36, 24.0) },

            { "Canon EOS 1000D, 400D, 40D", new Sensor(14.8, 22.2, 10.1) },
            { "Canon EOS 1100D, 450D, Rebel T3", new Sensor(14.8, 22.2, 12.2) },
            { "Canon EOS 100D, 1200D, 550D, 60D, 60Da, 600D, 650D, 700D, 7D, M, Kiss X4, Rebel SL1, Rebel T2i, Rebel T3i, Rebel T4i, Rebel T5, Rebel T5i", new Sensor(14.9, 22.3, 18) },
            { "Canon EOS 10D, 300D, D60", new Sensor(15.1, 22.7, 6.3) },
            { "Canon EOS 20D, 20Da", new Sensor(15, 22.5, 8.2) },
            { "Canon EOS 30D", new Sensor(15, 22.5, 8.2) },
            { "Canon EOS 350D", new Sensor(14.8, 22.2, 8) },
            { "Canon EOS 500D, 50D, Rebel T1i", new Sensor(14.9, 22.3, 15.1) },
            { "Canon EOS 5D", new Sensor(23.8, 35.8, 12.7) },
            { "Canon EOS 5D Mark II", new Sensor(24, 36, 21) },
            { "Canon EOS 5D Mark III", new Sensor(24, 36, 22.3) },
            { "Canon EOS 5D Mark IV", new Sensor(24, 36, 30.1) },
            { "Canon EOS 6D", new Sensor(23.9, 35.8, 20.2) },
            { "Canon EOS 70D", new Sensor(15, 22.5, 20.2) },
            { "Canon EOS 7D Mark II", new Sensor(15, 22.4, 20.2) },
            { "Canon EOS D30", new Sensor(15.1, 22.7, 3.1) },
            { "Canon EOS-1D", new Sensor(19.1, 28.7, 4.1) },
            { "Canon EOS-1D C, X", new Sensor(24, 36, 18.1) },
            { "Canon EOS-1D Mark II, Mark II N", new Sensor(19.1, 28.7, 8.2) },
            { "Canon EOS-1D Mark III", new Sensor(18.7, 28.7, 10.1) },
            { "Canon EOS-1D Mark IV", new Sensor(18.6, 27.9, 16.1) },
            { "Canon EOS-1Ds", new Sensor(23.8, 35.8, 11) },
            { "Canon EOS-1Ds Mark II", new Sensor(24, 36, 16.7) },
            { "Canon EOS-1Ds Mark III", new Sensor(24, 36, 21.1) },

            { "Fujifilm FinePix IS Pro, S2 Pro, S5 Pro", new Sensor(15.5, 23, 6.1) },
            { "Fujifilm FinePix S1 Pro", new Sensor(15.5, 23, 3.1) },
            { "Fujifilm FinePix S3 Pro", new Sensor(15.6, 23.5, 6.1) },
            { "Fujifilm FinePix X100", new Sensor(15.8, 23.6, 12.3) },
            { "Fujifilm X-A1, X-E1, X-E2, X-M1, X-Pro1, X-T1, X100T", new Sensor(15.6, 23.6, 16.3) },
            { "Fujifilm X100S", new Sensor(15.8, 23.6, 16.3) },

            { "Leica Digilux 3", new Sensor(13, 17.3, 7.4) },
            { "Leica M Typ 240, M-P", new Sensor(24, 36, 24) },
            { "Leica M-E Typ 220, M-Monochrom, M9, M9 Titanium, M9-P", new Sensor(23.9, 35.8, 18) },
            { "Leica M8, M8.2", new Sensor(18, 27, 10.3) },
            { "Leica S (Type 007), S-E, S2", new Sensor(30, 45, 37.5) },
            { "Leica T (Typ 701)", new Sensor(15.7, 23.6, 16.3) },
            { "Leica X (Typ 113), X Vario, X-E", new Sensor(15.7, 23.6, 16.2) },
            { "Leica X1", new Sensor(15.8, 23.6, 12.2) },
            { "Leica X2", new Sensor(15.8, 23.6, 16.2) },

            { "Nikon 1 AW1, J3, S2, V2", new Sensor(8.8, 13.2, 14.2) },
            { "Nikon 1 J1, J2, S1, V1", new Sensor(8.8, 13.2, 10.1) },
            { "Nikon 1 J4, V3", new Sensor(8.8, 13.2, 18.4) },
            { "Nikon D1, D1H", new Sensor(15.5, 23.7, 2.6) },
            { "Nikon D100", new Sensor(15.5, 23.7, 6.0) },
            { "Nikon D1x", new Sensor(15.5, 23.7, 5.3) },
            { "Nikon D200", new Sensor(15.8, 23.6, 10.2) },
            { "Nikon D2H", new Sensor(15.5, 23.7, 4.0) },
            { "Nikon D2Hs", new Sensor(15.4, 23.2, 4.1) },
            { "Nikon D2x", new Sensor(15.7, 23.7, 12.2) },
            { "Nikon D2xs", new Sensor(15.7, 23.7, 12.4) },
            { "Nikon D3, D3s", new Sensor(23.9, 36, 12.1) },
            { "Nikon D300, D300s", new Sensor(15.8, 23.6, 12.3) },
            { "Nikon D3000", new Sensor(15.8, 23.6, 10.2) },
            { "Nikon D3100", new Sensor(15.4, 23.1, 14.2) },
            { "Nikon D3200", new Sensor(15.4, 23.2, 24.2) },
            { "Nikon D3300, D3400, D3500", new Sensor(15.6, 23.5, 24.2) },
            { "Nikon D3X", new Sensor(24, 35.9, 24.5) },
            { "Nikon D4, D4s, Df", new Sensor(23.9, 36, 16.2) },
            { "Nikon D40", new Sensor(15.6, 23.7, 6.1) },
            { "Nikon D40x", new Sensor(15.8, 23.6, 10.2) },
            { "Nikon D5", new Sensor(23.9, 35.8, 20.8) },
            { "Nikon D50", new Sensor(15.6, 23.7, 6.1) },
            { "Nikon D5000", new Sensor(15.8, 23.6, 12.3) },
            { "Nikon D5100", new Sensor(15.6, 23.6, 16.2) },
            { "Nikon D5200", new Sensor(15.6, 23.5, 24.1) },
            { "Nikon D5300, D5500, D5600", new Sensor(15.6, 23.5, 24.2) },
            { "Nikon D6", new Sensor(23.9, 35.9, 20.8) },
            { "Nikon D60", new Sensor(15.8, 23.6, 10.2) },
            { "Nikon D600, D610", new Sensor(24, 35.9, 24.3) },
            { "Nikon D70, D70s", new Sensor(15.6, 23.7, 6.1) },
            { "Nikon D700", new Sensor(23.9, 36, 12.1) },
            { "Nikon D7000", new Sensor(15.6, 23.6, 16.2) },
            { "Nikon D7100", new Sensor(15.6, 23.5, 24.1) },
            { "Nikon D7200", new Sensor(15.6, 23.5, 24.2) },
            { "Nikon D750", new Sensor(24, 35.9, 24.3) },
            { "Nikon D7500", new Sensor(15.7, 23.5, 20.9) },
            { "Nikon D780", new Sensor(23.9, 35.9, 24.5) },
            { "Nikon D80", new Sensor(15.8, 23.6, 10.2) },
            { "Nikon D800, D800E, D810", new Sensor(24, 35.9, 36.3) },
            { "Nikon D850", new Sensor(23.9, 35.9, 45.7) },
            { "Nikon D90", new Sensor(15.8, 23.6, 12.3) },
            { "Nikon Z30, Z50, Z fc", new Sensor(15.7, 23.5, 20.90) },
            { "Nikon Z5", new Sensor(23.9, 35.9, 24.3) },
            { "Nikon Z6, Z6 II, Z6 III, Z f", new Sensor(23.9, 35.9, 24.5) },
            { "Nikon Z7, Z7 II, Z8, Z9", new Sensor(23.9, 35.9, 45.7 ) },

            { "Olympus E-1", new Sensor(13, 17.3, 4.9) },
            { "Olympus E-3", new Sensor(13, 17.3, 10.1) },
            { "Olympus E-30, E-5, E-600, E-620, PEN E-P1, PEN E-P2, PEN E-P3, PEN E-PL1, PEN E-PL1s, PEN E-PL2, PEN E-PL3, PEN E-PM1", new Sensor(13, 17.3, 12.3) },
            { "Olympus E-300, E-500, EVOLT E-300, EVOLT E-500", new Sensor(13, 17.3, 8) },
            { "Olympus E-400, E-410, E-420, E-450, E-510, E-520, EVOLT E-410, EVOLT E-510", new Sensor(13, 17.3, 10) },
            { "Olympus OM-D E-M1", new Sensor(13, 17.3, 16.3) },
            { "Olympus OM-D E-M10, OM-D E-M5, PEN E-P5, PEN E-PL5, PEN E-PL6, PEN E-PL7, PEN E-PM2", new Sensor(13, 17.3, 16.1) },

            { "Panasonic Lumix DMC-FZ1000, DMC-ZS100z TZ100", new Sensor(8.8, 13.2, 20.1) },
            { "Panasonic Lumix DMC-G1, DMC-G10, DMC-G2, DMC-GF1, DMC-GF2, DMC-GF3, DMC-GF5, DMC-GH1", new Sensor(13, 17.3, 12.1) },
            { "Panasonic Lumix DMC-G3", new Sensor(13, 17.3, 15.8) },
            { "Panasonic Lumix DMC-G5, DMC-G6, DMC-GH2, DMC-GH3, DMC-GH4", new Sensor(13, 17.3, 16) },
            { "Panasonic Lumix DMC-G7, DMC-GF6, DMC-GM1, DMC-GM5, DMC-GX1, DMC-GX7", new Sensor(13, 17.3, 16) },
            { "Panasonic Lumix DMC-GH5", new Sensor(13, 17.3, 20.3) },
            { "Panasonic Lumix DMC-L1", new Sensor(13, 17.3, 7.4) },
            { "Panasonic Lumix DMC-L10", new Sensor(13, 17.3, 10) },
            { "Panasonic Lumix DMC-LX100", new Sensor(13, 17.3, 12.8) },

            { "Pentax *ist D, *ist DL, *ist DL2, *ist DS, *ist DS2, K100D, K100D Super", new Sensor(15.7, 23.5, 6.1) },
            { "Pentax 645D", new Sensor(33, 44, 40) },
            { "Pentax 645Z", new Sensor(33, 44, 51.4) },
            { "Pentax K-01, K-30, K-5 II, K-50, K-500", new Sensor(15.7, 23.7, 16.3) },
            { "Pentax K-3", new Sensor(15.6, 23.5, 24.4) },
            { "Pentax K-5", new Sensor(15.7, 23.6, 16.3) },
            { "Pentax K-7, K20D", new Sensor(15.6, 23.4, 14.6) },
            { "Pentax K-m, K200D", new Sensor(15.7, 23.5, 10.2) },
            { "Pentax K-r, K-x", new Sensor(15.8, 23.6, 12.4) },
            { "Pentax K-S1", new Sensor(15.6, 23.5, 20.1) },
            { "Pentax K10D", new Sensor(15.7, 23.5, 10) },
            { "Pentax K110D", new Sensor(15.5, 23.7, 6.1) },
            { "Pentax Optio LS465", new Sensor(15.7, 23.7, 16) },

            { "Sony Alpha 7", new Sensor(23.9, 35.8, 24.3) },
            { "Sony Alpha 7R", new Sensor(24, 35.9, 36.4) },
            { "Sony Alpha 7S", new Sensor(23.8, 35.6, 12.2) },
            { "Sony Alpha a3000, a5000, QX1", new Sensor(15.6, 23.5, 20.1) },
            { "Sony Alpha a5100, a6000, NEX-7, SLT-A65, SLT-A77, SLT-A77 II", new Sensor(15.6, 23.5, 24.3) },
            { "Sony Alpha DSLR-A100", new Sensor(15.8, 23.6, 10) },
            { "Sony Alpha DSLR-A200, DSLR-A300", new Sensor(15.8, 23.6, 10.2) },
            { "Sony Alpha DSLR-A230, DSLR-A330", new Sensor(15.7, 23.5, 10.2) },
            { "Sony Alpha DSLR-A290, DSLR-A390", new Sensor(15.7, 23.5, 14.2) },
            { "Sony Alpha DSLR-A350, DSLR-A380", new Sensor(15.8, 23.6, 14.2) },
            { "Sony Alpha DSLR-A450, DSLR-A550, NEX-3, NEX-5", new Sensor(15.6, 23.4, 14.2) },
            { "Sony Alpha DSLR-A500", new Sensor(15.6, 23.5, 12.3) },
            { "Sony Alpha DSLR-A560, SLT-A33", new Sensor(15.6, 23.5, 14.2) },
            { "Sony Alpha DSLR-A580, SLT-A35, SLT-A55", new Sensor(15.6, 23.5, 16.2) },
            { "Sony Alpha DSLR-A700", new Sensor(15.6, 23.5, 12.2) },
            { "Sony Alpha DSLR-A850, DSLR-A900", new Sensor(24, 35.9, 24.6) },
            { "Sony Alpha NEX-3N, NEX-6, SLT-A37, SLT-A57", new Sensor(15.6, 23.5, 16.1) },
            { "Sony Alpha NEX-5N, NEX-5R, NEX-5T, NEX-F3", new Sensor(15.6, 23.4, 16.1) },
            { "Sony Alpha NEX-C3", new Sensor(15.6, 23.4, 16.2) },
            { "Sony SLT-A58", new Sensor(15.4, 23.2, 20.1) },
            { "Sony SLT-A99", new Sensor(23.8, 35.8, 24.3) },
        }.ToFrozenDictionary();

        keys = [.. sensors.Keys.Order()];
    }

    public ImmutableArray<string> GetSensorNames() => keys;

    public ImmutableArray<Sensor> GetSensors() => sensors.Values;

    public Sensor? GetSensor(string sensorName)
    {
        if (sensorName is null)
            return null;

        return sensors.TryGetValue(sensorName, out Sensor? sensor) ? sensor : null;
    }
}
