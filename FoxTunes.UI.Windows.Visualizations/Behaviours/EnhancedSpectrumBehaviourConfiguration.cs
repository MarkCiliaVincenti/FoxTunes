﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FoxTunes
{
    public static class EnhancedSpectrumBehaviourConfiguration
    {
        public const string SECTION = VisualizationBehaviourConfiguration.SECTION;

        public const string BANDS_ELEMENT = "AABBF573-83D3-498E-BEF8-F1DB5A329B9D";

        public const string BANDS_10_OPTION = "AAAA058C-2C96-4540-9ABE-10A584A17CE4";

        public const string BANDS_14_OPTION = "BBBB2B6D-E6FE-43F1-8358-AEE0299F0F8E";

        public const string BANDS_21_OPTION = "CCCCC739-B777-474B-B4A8-F96375254FAC";

        public const string BANDS_31_OPTION = "DDDD0A9D-0512-4F9F-903D-9AC33A9C6CFD";

        public const string BANDS_40_OPTION = "EEEE9CF3-A711-45D7-BFAC-A72E769106B9";

        public const string BANDS_80_OPTION = "FFFF83BC-5871-491D-963E-3D12554FF4BE";

        public const string BANDS_160_OPTION = "GGGG5E28-CC67-43F2-8778-61570785C766";

        public const string PEAKS_ELEMENT = "BBBBDCF0-8B24-4321-B7BE-74DADE59D4FA";

        public const string HOLD_ELEMENT = "CCCCA25C-F8FA-4C37-82E8-2C5F297D2ED3";

        public const int MIN_HOLD = 500;

        public const int MAX_HOLD = 5000;

        public const int DEFAULT_HOLD = 1000;

        public const string RMS_ELEMENT = "DDDEE2B6A-188E-4FF4-A277-37D140D49C45";

        public const string CREST_ELEMENT = "DEEEFFB9-2014-4004-94F9-E566874317ED";

        public static IEnumerable<ConfigurationSection> GetConfigurationSections()
        {
            yield return new ConfigurationSection(SECTION)
                .WithElement(new SelectionConfigurationElement(BANDS_ELEMENT, Strings.EnhancedSpectrumBehaviourConfiguration_Bands, path: Strings.EnhancedSpectrumBehaviourConfiguration_Path).WithOptions(GetBandsOptions()))
                .WithElement(new BooleanConfigurationElement(PEAKS_ELEMENT, Strings.EnhancedSpectrumBehaviourConfiguration_Peaks, path: string.Format("{0}/{1}", Strings.EnhancedSpectrumBehaviourConfiguration_Path, Strings.General_Advanced)).WithValue(true))
                .WithElement(new IntegerConfigurationElement(HOLD_ELEMENT, Strings.EnhancedSpectrumBehaviourConfiguration_Hold, path: string.Format("{0}/{1}", Strings.EnhancedSpectrumBehaviourConfiguration_Path, Strings.General_Advanced)).WithValue(DEFAULT_HOLD).WithValidationRule(new IntegerValidationRule(MIN_HOLD, MAX_HOLD)).DependsOn(SECTION, PEAKS_ELEMENT))
                .WithElement(new BooleanConfigurationElement(RMS_ELEMENT, Strings.EnhancedSpectrumBehaviourConfiguration_Rms, path: string.Format("{0}/{1}", Strings.EnhancedSpectrumBehaviourConfiguration_Path, Strings.General_Advanced)).WithValue(true))
                .WithElement(new BooleanConfigurationElement(CREST_ELEMENT, Strings.EnhancedSpectrumBehaviourConfiguration_Crest, path: string.Format("{0}/{1}", Strings.EnhancedSpectrumBehaviourConfiguration_Path, Strings.General_Advanced)).WithValue(false)
            );
        }

        private static IEnumerable<SelectionConfigurationOption> GetBandsOptions()
        {
            yield return new SelectionConfigurationOption(BANDS_10_OPTION, "10");
            yield return new SelectionConfigurationOption(BANDS_14_OPTION, "14").Default();
            yield return new SelectionConfigurationOption(BANDS_21_OPTION, "21");
            yield return new SelectionConfigurationOption(BANDS_31_OPTION, "31");
            yield return new SelectionConfigurationOption(BANDS_40_OPTION, "40");
            yield return new SelectionConfigurationOption(BANDS_80_OPTION, "80");
            yield return new SelectionConfigurationOption(BANDS_160_OPTION, "160");
        }

        private static IDictionary<string, int[]> Bands = new Dictionary<string, int[]>(StringComparer.OrdinalIgnoreCase)
        {
            { BANDS_10_OPTION, new[] {
                    20,
                    50,
                    100,
                    200,
                    500,
                    1000,
                    2000,
                    5000,
                    10000,
                    20000
                }
            },
            { BANDS_14_OPTION, new[] {
                    20,
                    50,
                    100,
                    200,
                    500,
                    1000,
                    1400,
                    2000,
                    3000,
                    5000,
                    7500,
                    10000,
                    17000,
                    20000
                }
            },
            { BANDS_21_OPTION, new[] {
                    20,
                    35,
                    50,
                    70,
                    100,
                    160,
                    200,
                    360,
                    500,
                    760,
                    1000,
                    1400,
                    2000,
                    2600,
                    3000,
                    5000,
                    7500,
                    10000,
                    13000,
                    17000,
                    20000
                }
            },
            { BANDS_31_OPTION, new[] {
                    20,
                    35,
                    50,
                    70,
                    100,
                    120,
                    160,
                    200,
                    300,
                    360,
                    440,
                    500,
                    600,
                    760,
                    1000,
                    1200,
                    1400,
                    1600,
                    2000,
                    2600,
                    3000,
                    3600,
                    4000,
                    5000,
                    7500,
                    10000,
                    12000,
                    14000,
                    17000,
                    20000
                }
            },
            { BANDS_40_OPTION, new[] {
                    50,
                    59,
                    69,
                    80,
                    94,
                    110,
                    129,
                    150,
                    176,
                    206,
                    241,
                    282,
                    331,
                    387,
                    453,
                    530,
                    620,
                    726,
                    850,
                    1000,
                    1200,
                    1400,
                    1600,
                    1900,
                    2200,
                    2600,
                    3000,
                    3500,
                    4100,
                    4800,
                    5600,
                    6600,
                    7700,
                    9000,
                    11000,
                    12000,
                    14000,
                    17000,
                    20000,
                    23000
                }
            },
            { BANDS_80_OPTION,  new[] {
                    50,
                    54,
                    59,
                    63,
                    69,
                    74,
                    80,
                    87,
                    94,
                    102,
                    110,
                    119,
                    129,
                    139,
                    150,
                    163,
                    176,
                    191,
                    206,
                    223,
                    241,
                    261,
                    282,
                    306,
                    331,
                    358,
                    387,
                    419,
                    453,
                    490,
                    530,
                    574,
                    620,
                    671,
                    726,
                    786,
                    850,
                    920,
                    1000,
                    1100,
                    1200,
                    1300,
                    1400,
                    1500,
                    1600,
                    1700,
                    1900,
                    2000,
                    2200,
                    2400,
                    2600,
                    2800,
                    3000,
                    3200,
                    3500,
                    3800,
                    4100,
                    4400,
                    4800,
                    5200,
                    5600,
                    6100,
                    6600,
                    7100,
                    7700,
                    8300,
                    9000,
                    10000,
                    11000,
                    11500,
                    12000,
                    13000,
                    14000,
                    16000,
                    17000,
                    18000,
                    20000,
                    21000,
                    23000,
                    25000
                }
            },
            { BANDS_160_OPTION, new[] {
                    50,
                    52,
                    54,
                    56,
                    59,
                    61,
                    63,
                    66,
                    69,
                    71,
                    77,
                    80,
                    83,
                    87,
                    90,
                    94,
                    98,
                    102,
                    106,
                    110,
                    114,
                    119,
                    124,
                    129,
                    134,
                    139,
                    145,
                    150,
                    157,
                    163,
                    169,
                    176,
                    183,
                    191,
                    198,
                    206,
                    214,
                    223,
                    232,
                    241,
                    251,
                    261,
                    272,
                    282,
                    294,
                    306,
                    318,
                    331,
                    344,
                    358,
                    372,
                    387,
                    402,
                    419,
                    435,
                    453,
                    453,
                    471,
                    490,
                    510,
                    530,
                    551,
                    574,
                    597,
                    620,
                    645,
                    671,
                    698,
                    726,
                    755,
                    786,
                    817,
                    850,
                    884,
                    920,
                    1000,
                    1030,
                    1060,
                    1100,
                    1150,
                    1200,
                    1250,
                    1300,
                    1350,
                    1400,
                    1450,
                    1500,
                    1550,
                    1600,
                    1700,
                    1750,
                    1800,
                    1900,
                    1950,
                    2000,
                    2100,
                    2200,
                    2300,
                    2400,
                    2500,
                    2600,
                    2700,
                    2800,
                    2900,
                    3000,
                    3100,
                    3200,
                    3400,
                    3500,
                    3600,
                    3800,
                    3900,
                    4100,
                    4300,
                    4400,
                    4600,
                    4800,
                    5000,
                    5200,
                    5400,
                    5600,
                    5800,
                    6100,
                    6300,
                    6600,
                    6800,
                    7100,
                    7400,
                    7700,
                    8000,
                    8300,
                    8700,
                    9000,
                    9400,
                    10000,
                    10500,
                    11000,
                    11300,
                    11600,
                    12000,
                    12500,
                    13000,
                    13500,
                    14000,
                    14500,
                    15000,
                    16000,
                    16500,
                    17000,
                    18000,
                    18500,
                    19000,
                    20000,
                    21000,
                    21500,
                    22000,
                    23000,
                    24000,
                    25000,
                    26000
                }
            }
        };

        public static int[] GetBands(SelectionConfigurationOption option)
        {
            var bands = default(int[]);
            if (!Bands.TryGetValue(option.Id, out bands))
            {
                bands = Bands[BANDS_14_OPTION];
            }
            return bands;
        }

        public static int GetWidth(SelectionConfigurationOption option)
        {
            var bands = GetBands(option);
            return (bands.Length * 2) - 1;
        }

        public static int GetFFTSize(SelectionConfigurationOption fftSize, SelectionConfigurationOption bands)
        {
            var size = VisualizationBehaviourConfiguration.GetFFTSize(fftSize);
            var count = GetBands(bands).Length;
            //More bands requires more FFT bins, increase if required.
            switch (count)
            {
                case 40:
                case 80:
                    size = Math.Min(size, 8192);
                    break;
                case 160:
                    size = Math.Min(size, 16384);
                    break;
            }
            return size;
        }
    }
}
