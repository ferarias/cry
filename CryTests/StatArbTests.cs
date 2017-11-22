using System;
using System.Globalization;
using System.IO;
using Cry;
using Xunit;
using Xunit.Abstractions;

namespace CryTests
{
    public class StatArbTests
    {

        private readonly ITestOutputHelper output;

        public StatArbTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void BacklogTest()
        {
            var ci = new CultureInfo("en-US");
            var testData = new[]
            {
                new ComparisonRow {Date = DateTimeOffset.Parse("5/21/2017 12:00:00 AM +02:00", ci), Coin1 = 146.48, Coin2 = 145.25},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/22/2017 12:00:00 AM +02:00", ci), Coin1 = 159.14, Coin2 = 155.6},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/23/2017 12:00:00 AM +02:00", ci), Coin1 = 169, Coin2 = 166.95},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/24/2017 12:00:00 AM +02:00", ci), Coin1 = 188.29, Coin2 = 185.24},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/25/2017 12:00:00 AM +02:00", ci), Coin1 = 172, Coin2 = 168},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/26/2017 12:00:00 AM +02:00", ci), Coin1 = 161.52, Coin2 = 158.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/27/2017 12:00:00 AM +02:00", ci), Coin1 = 149.55, Coin2 = 151.61}, // *
                new ComparisonRow {Date = DateTimeOffset.Parse("5/28/2017 12:00:00 AM +02:00", ci), Coin1 = 167.07, Coin2 = 168.29},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/29/2017 12:00:00 AM +02:00", ci), Coin1 = 183.68, Coin2 = 193.3},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/30/2017 12:00:00 AM +02:00", ci), Coin1 = 222.5, Coin2 = 226.11},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/31/2017 12:00:00 AM +02:00", ci), Coin1 = 217.22, Coin2 = 228.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/1/2017 12:00:00 AM +02:00", ci), Coin1 = 213, Coin2 = 219},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/2/2017 12:00:00 AM +02:00", ci), Coin1 = 214.49, Coin2 = 221.54},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/3/2017 12:00:00 AM +02:00", ci), Coin1 = 216.75, Coin2 = 224.23},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/4/2017 12:00:00 AM +02:00", ci), Coin1 = 236.54, Coin2 = 245.35},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/5/2017 12:00:00 AM +02:00", ci), Coin1 = 239, Coin2 = 247.6},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/6/2017 12:00:00 AM +02:00", ci), Coin1 = 250.36, Coin2 = 263.6},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/7/2017 12:00:00 AM +02:00", ci), Coin1 = 253.74, Coin2 = 256.19},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/8/2017 12:00:00 AM +02:00", ci), Coin1 = 249.41, Coin2 = 260.24},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/9/2017 12:00:00 AM +02:00", ci), Coin1 = 269, Coin2 = 279.85},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/10/2017 12:00:00 AM +02:00", ci), Coin1 = 326.42, Coin2 = 326}, // *
                new ComparisonRow {Date = DateTimeOffset.Parse("6/11/2017 12:00:00 AM +02:00", ci), Coin1 = 332, Coin2 = 333.1}, // *
                new ComparisonRow {Date = DateTimeOffset.Parse("6/12/2017 12:00:00 AM +02:00", ci), Coin1 = 390, Coin2 = 385}, // *
                new ComparisonRow {Date = DateTimeOffset.Parse("6/13/2017 12:00:00 AM +02:00", ci), Coin1 = 393.2, Coin2 = 386.13},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/14/2017 12:00:00 AM +02:00", ci), Coin1 = 369.41, Coin2 = 347.24},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/15/2017 12:00:00 AM +02:00", ci), Coin1 = 357.3, Coin2 = 345.99},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/16/2017 12:00:00 AM +02:00", ci), Coin1 = 365.81, Coin2 = 354.17},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/17/2017 12:00:00 AM +02:00", ci), Coin1 = 370.61, Coin2 = 368.95},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/18/2017 12:00:00 AM +02:00", ci), Coin1 = 357, Coin2 = 349.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/19/2017 12:00:00 AM +02:00", ci), Coin1 = 359.82, Coin2 = 356.48},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/20/2017 12:00:00 AM +02:00", ci), Coin1 = 358.03, Coin2 = 348.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/21/2017 12:00:00 AM +02:00", ci), Coin1 = 334, Coin2 = 324.48},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/22/2017 12:00:00 AM +02:00", ci), Coin1 = 325.95, Coin2 = 321.6},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/23/2017 12:00:00 AM +02:00", ci), Coin1 = 334, Coin2 = 325.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/24/2017 12:00:00 AM +02:00", ci), Coin1 = 317.47, Coin2 = 302.15},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/25/2017 12:00:00 AM +02:00", ci), Coin1 = 295, Coin2 = 277},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/26/2017 12:00:00 AM +02:00", ci), Coin1 = 264, Coin2 = 254.49},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/27/2017 12:00:00 AM +02:00", ci), Coin1 = 280.93, Coin2 = 283.66}, // *
                new ComparisonRow {Date = DateTimeOffset.Parse("6/28/2017 12:00:00 AM +02:00", ci), Coin1 = 323, Coin2 = 316.2}, // *
                new ComparisonRow {Date = DateTimeOffset.Parse("6/29/2017 12:00:00 AM +02:00", ci), Coin1 = 300.02, Coin2 = 293},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/30/2017 12:00:00 AM +02:00", ci), Coin1 = 290, Coin2 = 280},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/1/2017 12:00:00 AM +02:00", ci), Coin1 = 273.1, Coin2 = 256},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/2/2017 12:00:00 AM +02:00", ci), Coin1 = 285.88, Coin2 = 284},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/3/2017 12:00:00 AM +02:00", ci), Coin1 = 281, Coin2 = 276.1},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/4/2017 12:00:00 AM +02:00", ci), Coin1 = 272.01, Coin2 = 268.4},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/5/2017 12:00:00 AM +02:00", ci), Coin1 = 266.61, Coin2 = 266.3},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/6/2017 12:00:00 AM +02:00", ci), Coin1 = 267.1, Coin2 = 266.49},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/7/2017 12:00:00 AM +02:00", ci), Coin1 = 250.74, Coin2 = 240.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/8/2017 12:00:00 AM +02:00", ci), Coin1 = 248.2, Coin2 = 245},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/9/2017 12:00:00 AM +02:00", ci), Coin1 = 246.61, Coin2 = 236.4},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/10/2017 12:00:00 AM +02:00", ci), Coin1 = 219.98, Coin2 = 210},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/11/2017 12:00:00 AM +02:00", ci), Coin1 = 198.09, Coin2 = 190.99},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/12/2017 12:00:00 AM +02:00", ci), Coin1 = 226.64, Coin2 = 223},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/13/2017 12:00:00 AM +02:00", ci), Coin1 = 208, Coin2 = 204.8},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/14/2017 12:00:00 AM +02:00", ci), Coin1 = 201, Coin2 = 197},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/15/2017 12:00:00 AM +02:00", ci), Coin1 = 179.79, Coin2 = 168.6},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/16/2017 12:00:00 AM +02:00", ci), Coin1 = 158.43, Coin2 = 155.2},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/17/2017 12:00:00 AM +02:00", ci), Coin1 = 185, Coin2 = 188.72},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/18/2017 12:00:00 AM +02:00", ci), Coin1 = 222.7, Coin2 = 228},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/19/2017 12:00:00 AM +02:00", ci), Coin1 = 198.45, Coin2 = 193.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/20/2017 12:00:00 AM +02:00", ci), Coin1 = 220, Coin2 = 227.49},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/21/2017 12:00:00 AM +02:00", ci), Coin1 = 215.46, Coin2 = 216},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/22/2017 12:00:00 AM +02:00", ci), Coin1 = 225, Coin2 = 230.13},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/23/2017 12:00:00 AM +02:00", ci), Coin1 = 225.59, Coin2 = 229.85},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/24/2017 12:00:00 AM +02:00", ci), Coin1 = 223.4, Coin2 = 225.8},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/25/2017 12:00:00 AM +02:00", ci), Coin1 = 206.94, Coin2 = 204},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/26/2017 12:00:00 AM +02:00", ci), Coin1 = 200.54, Coin2 = 202.21},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/27/2017 12:00:00 AM +02:00", ci), Coin1 = 200.61, Coin2 = 203.1},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/28/2017 12:00:00 AM +02:00", ci), Coin1 = 187.44, Coin2 = 191.55},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/29/2017 12:00:00 AM +02:00", ci), Coin1 = 192.08, Coin2 = 207.14},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/30/2017 12:00:00 AM +02:00", ci), Coin1 = 192.68, Coin2 = 198.45},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/31/2017 12:00:00 AM +02:00", ci), Coin1 = 193.5, Coin2 = 201.78},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/1/2017 12:00:00 AM +02:00", ci), Coin1 = 216.2, Coin2 = 226.25},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/2/2017 12:00:00 AM +02:00", ci), Coin1 = 215, Coin2 = 218.49},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/3/2017 12:00:00 AM +02:00", ci), Coin1 = 220.08, Coin2 = 225.4},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/4/2017 12:00:00 AM +02:00", ci), Coin1 = 217.71, Coin2 = 222.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/5/2017 12:00:00 AM +02:00", ci), Coin1 = 239.57, Coin2 = 253.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/6/2017 12:00:00 AM +02:00", ci), Coin1 = 259.23, Coin2 = 265.67},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/7/2017 12:00:00 AM +02:00", ci), Coin1 = 260.77, Coin2 = 269.8},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/8/2017 12:00:00 AM +02:00", ci), Coin1 = 289.56, Coin2 = 296.98},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/9/2017 12:00:00 AM +02:00", ci), Coin1 = 292.91, Coin2 = 297.78},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/10/2017 12:00:00 AM +02:00", ci), Coin1 = 296, Coin2 = 300.2},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/11/2017 12:00:00 AM +02:00", ci), Coin1 = 298.2, Coin2 = 310},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/12/2017 12:00:00 AM +02:00", ci), Coin1 = 303.5, Coin2 = 309.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/13/2017 12:00:00 AM +02:00", ci), Coin1 = 292.9, Coin2 = 298},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/14/2017 12:00:00 AM +02:00", ci), Coin1 = 295.7, Coin2 = 302.7},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/15/2017 12:00:00 AM +02:00", ci), Coin1 = 278, Coin2 = 286.65},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/16/2017 12:00:00 AM +02:00", ci), Coin1 = 291, Coin2 = 302.26},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/17/2017 12:00:00 AM +02:00", ci), Coin1 = 297.5, Coin2 = 300.79},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/18/2017 12:00:00 AM +02:00", ci), Coin1 = 294.55, Coin2 = 293},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/19/2017 12:00:00 AM +02:00", ci), Coin1 = 289, Coin2 = 294.63},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/20/2017 12:00:00 AM +02:00", ci), Coin1 = 295.91, Coin2 = 299.45},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/21/2017 12:00:00 AM +02:00", ci), Coin1 = 323.2, Coin2 = 322},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/22/2017 12:00:00 AM +02:00", ci), Coin1 = 315, Coin2 = 314.52},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/23/2017 12:00:00 AM +02:00", ci), Coin1 = 313.72, Coin2 = 317.31},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/24/2017 12:00:00 AM +02:00", ci), Coin1 = 318.24, Coin2 = 326.65},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/25/2017 12:00:00 AM +02:00", ci), Coin1 = 323, Coin2 = 331.21},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/26/2017 12:00:00 AM +02:00", ci), Coin1 = 328.9, Coin2 = 332.65},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/27/2017 12:00:00 AM +02:00", ci), Coin1 = 340, Coin2 = 347.6},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/28/2017 12:00:00 AM +02:00", ci), Coin1 = 341.73, Coin2 = 347.54},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/29/2017 12:00:00 AM +02:00", ci), Coin1 = 367, Coin2 = 371.99},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/30/2017 12:00:00 AM +02:00", ci), Coin1 = 378, Coin2 = 383.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/31/2017 12:00:00 AM +02:00", ci), Coin1 = 384.55, Coin2 = 388},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/1/2017 12:00:00 AM +02:00", ci), Coin1 = 394.96, Coin2 = 390.01},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/2/2017 12:00:00 AM +02:00", ci), Coin1 = 351, Coin2 = 354.89},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/3/2017 12:00:00 AM +02:00", ci), Coin1 = 354.39, Coin2 = 355},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/4/2017 12:00:00 AM +02:00", ci), Coin1 = 305, Coin2 = 307.96},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/5/2017 12:00:00 AM +02:00", ci), Coin1 = 324.1, Coin2 = 322.98},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/6/2017 12:00:00 AM +02:00", ci), Coin1 = 337.4, Coin2 = 341.84},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/7/2017 12:00:00 AM +02:00", ci), Coin1 = 334.71, Coin2 = 337.96},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/8/2017 12:00:00 AM +02:00", ci), Coin1 = 308.74, Coin2 = 310.27},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/9/2017 12:00:00 AM +02:00", ci), Coin1 = 310, Coin2 = 304.74},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/10/2017 12:00:00 AM +02:00", ci), Coin1 = 305.7, Coin2 = 301.54},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/11/2017 12:00:00 AM +02:00", ci), Coin1 = 304.2, Coin2 = 297.64},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/12/2017 12:00:00 AM +02:00", ci), Coin1 = 301.12, Coin2 = 294},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/13/2017 12:00:00 AM +02:00", ci), Coin1 = 279, Coin2 = 276.21},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/14/2017 12:00:00 AM +02:00", ci), Coin1 = 230, Coin2 = 223.85},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/15/2017 12:00:00 AM +02:00", ci), Coin1 = 267.21, Coin2 = 259.42},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/16/2017 12:00:00 AM +02:00", ci), Coin1 = 261.79, Coin2 = 254.81},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/17/2017 12:00:00 AM +02:00", ci), Coin1 = 263.56, Coin2 = 259.1},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/18/2017 12:00:00 AM +02:00", ci), Coin1 = 298.57, Coin2 = 297.98},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/19/2017 12:00:00 AM +02:00", ci), Coin1 = 287.27, Coin2 = 282.61},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/20/2017 12:00:00 AM +02:00", ci), Coin1 = 290, Coin2 = 282.36},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/21/2017 12:00:00 AM +02:00", ci), Coin1 = 263.62, Coin2 = 258.8},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/22/2017 12:00:00 AM +02:00", ci), Coin1 = 264.63, Coin2 = 262.92},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/23/2017 12:00:00 AM +02:00", ci), Coin1 = 279.71, Coin2 = 285.99},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/24/2017 12:00:00 AM +02:00", ci), Coin1 = 283.06, Coin2 = 282.95},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/25/2017 12:00:00 AM +02:00", ci), Coin1 = 291.89, Coin2 = 295},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/26/2017 12:00:00 AM +02:00", ci), Coin1 = 291.51, Coin2 = 287.9},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/27/2017 12:00:00 AM +02:00", ci), Coin1 = 305.58, Coin2 = 310.33},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/28/2017 12:00:00 AM +02:00", ci), Coin1 = 299.57, Coin2 = 302.7},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/29/2017 12:00:00 AM +02:00", ci), Coin1 = 288.31, Coin2 = 292.4},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/30/2017 12:00:00 AM +02:00", ci), Coin1 = 296.39, Coin2 = 302.2},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/1/2017 12:00:00 AM +02:00", ci), Coin1 = 298.86, Coin2 = 303.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/2/2017 12:00:00 AM +02:00", ci), Coin1 = 294.69, Coin2 = 296.66},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/3/2017 12:00:00 AM +02:00", ci), Coin1 = 289.15, Coin2 = 291.17},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/4/2017 12:00:00 AM +02:00", ci), Coin1 = 290.92, Coin2 = 292},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/5/2017 12:00:00 AM +02:00", ci), Coin1 = 293.65, Coin2 = 294.75},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/6/2017 12:00:00 AM +02:00", ci), Coin1 = 303.34, Coin2 = 308.7},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/7/2017 12:00:00 AM +02:00", ci), Coin1 = 307, Coin2 = 310.57},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/8/2017 12:00:00 AM +02:00", ci), Coin1 = 305.34, Coin2 = 308.59},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/9/2017 12:00:00 AM +02:00", ci), Coin1 = 294, Coin2 = 296.9},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/10/2017 12:00:00 AM +02:00", ci), Coin1 = 297.07, Coin2 = 298.02},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/11/2017 12:00:00 AM +02:00", ci), Coin1 = 296.82, Coin2 = 303},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/12/2017 12:00:00 AM +02:00", ci), Coin1 = 294.47, Coin2 = 302.12},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/13/2017 12:00:00 AM +02:00", ci), Coin1 = 323.03, Coin2 = 336.98},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/14/2017 12:00:00 AM +02:00", ci), Coin1 = 321.02, Coin2 = 337.99},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/15/2017 12:00:00 AM +02:00", ci), Coin1 = 321.81, Coin2 = 335.71},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/16/2017 12:00:00 AM +02:00", ci), Coin1 = 320.51, Coin2 = 333.42},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/17/2017 12:00:00 AM +02:00", ci), Coin1 = 308.51, Coin2 = 316.83},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/18/2017 12:00:00 AM +02:00", ci), Coin1 = 308.2, Coin2 = 314.36},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/19/2017 12:00:00 AM +02:00", ci), Coin1 = 304.78, Coin2 = 307.87},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/20/2017 12:00:00 AM +02:00", ci), Coin1 = 294.59, Coin2 = 311.52},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/21/2017 12:00:00 AM +02:00", ci), Coin1 = 293.44, Coin2 = 311.52},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/22/2017 12:00:00 AM +02:00", ci), Coin1 = 291.09, Coin2 = 311.52},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/23/2017 12:00:00 AM +02:00", ci), Coin1 = 282.07, Coin2 = 285.17},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/24/2017 12:00:00 AM +02:00", ci), Coin1 = 297.58, Coin2 = 298.4},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/25/2017 12:00:00 AM +02:00", ci), Coin1 = 295.69, Coin2 = 296.87},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/26/2017 12:00:00 AM +02:00", ci), Coin1 = 291.21, Coin2 = 295.52},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/27/2017 12:00:00 AM +02:00", ci), Coin1 = 295, Coin2 = 297.06},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/28/2017 12:00:00 AM +02:00", ci), Coin1 = 292.5, Coin2 = 295.14},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/29/2017 12:00:00 AM +02:00", ci), Coin1 = 295, Coin2 = 303.95},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/30/2017 12:00:00 AM +01:00", ci), Coin1 = 299.9, Coin2 = 306.2},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/31/2017 12:00:00 AM +01:00", ci), Coin1 = 299.11, Coin2 = 304.12},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/1/2017 12:00:00 AM +01:00", ci), Coin1 = 289.35, Coin2 = 290.1},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/2/2017 12:00:00 AM +01:00", ci), Coin1 = 285.09, Coin2 = 283.75},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/3/2017 12:00:00 AM +01:00", ci), Coin1 = 298.8, Coin2 = 304.17},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/4/2017 12:00:00 AM +01:00", ci), Coin1 = 293.82, Coin2 = 299.41},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/5/2017 12:00:00 AM +01:00", ci), Coin1 = 293.87, Coin2 = 294.62},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/6/2017 12:00:00 AM +01:00", ci), Coin1 = 296.97, Coin2 = 298.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/7/2017 12:00:00 AM +01:00", ci), Coin1 = 291.64, Coin2 = 292.07},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/8/2017 12:00:00 AM +01:00", ci), Coin1 = 301.94, Coin2 = 308.38},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/9/2017 12:00:00 AM +01:00", ci), Coin1 = 315.5, Coin2 = 319.76},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/10/2017 12:00:00 AM +01:00", ci), Coin1 = 297.45, Coin2 = 298},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/11/2017 12:00:00 AM +01:00", ci), Coin1 = 313.85, Coin2 = 313.65},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/12/2017 12:00:00 AM +01:00", ci), Coin1 = 306.29, Coin2 = 305.13},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/13/2017 12:00:00 AM +01:00", ci), Coin1 = 315.19, Coin2 = 314.83},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/14/2017 12:00:00 AM +01:00", ci), Coin1 = 336.8, Coin2 = 335},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/15/2017 12:00:00 AM +01:00", ci), Coin1 = 329.31, Coin2 = 329.31}
            };


            var originalConsoleOut = Console.Out;
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);
                const double investment = 10000;
                StatArb.BackTesting(testData, investment);

                writer.Flush();
                var log = writer.GetStringBuilder().ToString();
                output.WriteLine(log);
            }
            Console.SetOut(originalConsoleOut);


        }
    }
}
