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
                new ComparisonRow {Date = DateTimeOffset.Parse("5/21/2017 12:00:00 AM +02:00", ci), Exchange1Value = 146.48, Exchange2Value = 145.25},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/22/2017 12:00:00 AM +02:00", ci), Exchange1Value = 159.14, Exchange2Value = 155.6},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/23/2017 12:00:00 AM +02:00", ci), Exchange1Value = 169, Exchange2Value = 166.95},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/24/2017 12:00:00 AM +02:00", ci), Exchange1Value = 188.29, Exchange2Value = 185.24},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/25/2017 12:00:00 AM +02:00", ci), Exchange1Value = 172, Exchange2Value = 168},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/26/2017 12:00:00 AM +02:00", ci), Exchange1Value = 161.52, Exchange2Value = 158.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/27/2017 12:00:00 AM +02:00", ci), Exchange1Value = 149.55, Exchange2Value = 151.61},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/28/2017 12:00:00 AM +02:00", ci), Exchange1Value = 167.07, Exchange2Value = 168.29},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/29/2017 12:00:00 AM +02:00", ci), Exchange1Value = 183.68, Exchange2Value = 193.3},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/30/2017 12:00:00 AM +02:00", ci), Exchange1Value = 222.5, Exchange2Value = 226.11},
                new ComparisonRow {Date = DateTimeOffset.Parse("5/31/2017 12:00:00 AM +02:00", ci), Exchange1Value = 217.22, Exchange2Value = 228.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/1/2017 12:00:00 AM +02:00", ci), Exchange1Value = 213, Exchange2Value = 219},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/2/2017 12:00:00 AM +02:00", ci), Exchange1Value = 214.49, Exchange2Value = 221.54},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/3/2017 12:00:00 AM +02:00", ci), Exchange1Value = 216.75, Exchange2Value = 224.23},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/4/2017 12:00:00 AM +02:00", ci), Exchange1Value = 236.54, Exchange2Value = 245.35},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/5/2017 12:00:00 AM +02:00", ci), Exchange1Value = 239, Exchange2Value = 247.6},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/6/2017 12:00:00 AM +02:00", ci), Exchange1Value = 250.36, Exchange2Value = 263.6},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/7/2017 12:00:00 AM +02:00", ci), Exchange1Value = 253.74, Exchange2Value = 256.19},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/8/2017 12:00:00 AM +02:00", ci), Exchange1Value = 249.41, Exchange2Value = 260.24},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/9/2017 12:00:00 AM +02:00", ci), Exchange1Value = 269, Exchange2Value = 279.85},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/10/2017 12:00:00 AM +02:00", ci), Exchange1Value = 326.42, Exchange2Value = 326},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/11/2017 12:00:00 AM +02:00", ci), Exchange1Value = 332, Exchange2Value = 333.1},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/12/2017 12:00:00 AM +02:00", ci), Exchange1Value = 390, Exchange2Value = 385},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/13/2017 12:00:00 AM +02:00", ci), Exchange1Value = 393.2, Exchange2Value = 386.13},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/14/2017 12:00:00 AM +02:00", ci), Exchange1Value = 369.41, Exchange2Value = 347.24},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/15/2017 12:00:00 AM +02:00", ci), Exchange1Value = 357.3, Exchange2Value = 345.99},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/16/2017 12:00:00 AM +02:00", ci), Exchange1Value = 365.81, Exchange2Value = 354.17},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/17/2017 12:00:00 AM +02:00", ci), Exchange1Value = 370.61, Exchange2Value = 368.95},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/18/2017 12:00:00 AM +02:00", ci), Exchange1Value = 357, Exchange2Value = 349.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/19/2017 12:00:00 AM +02:00", ci), Exchange1Value = 359.82, Exchange2Value = 356.48},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/20/2017 12:00:00 AM +02:00", ci), Exchange1Value = 358.03, Exchange2Value = 348.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/21/2017 12:00:00 AM +02:00", ci), Exchange1Value = 334, Exchange2Value = 324.48},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/22/2017 12:00:00 AM +02:00", ci), Exchange1Value = 325.95, Exchange2Value = 321.6},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/23/2017 12:00:00 AM +02:00", ci), Exchange1Value = 334, Exchange2Value = 325.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/24/2017 12:00:00 AM +02:00", ci), Exchange1Value = 317.47, Exchange2Value = 302.15},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/25/2017 12:00:00 AM +02:00", ci), Exchange1Value = 295, Exchange2Value = 277},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/26/2017 12:00:00 AM +02:00", ci), Exchange1Value = 264, Exchange2Value = 254.49},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/27/2017 12:00:00 AM +02:00", ci), Exchange1Value = 280.93, Exchange2Value = 283.66},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/28/2017 12:00:00 AM +02:00", ci), Exchange1Value = 323, Exchange2Value = 316.2},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/29/2017 12:00:00 AM +02:00", ci), Exchange1Value = 300.02, Exchange2Value = 293},
                new ComparisonRow {Date = DateTimeOffset.Parse("6/30/2017 12:00:00 AM +02:00", ci), Exchange1Value = 290, Exchange2Value = 280},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/1/2017 12:00:00 AM +02:00", ci), Exchange1Value = 273.1, Exchange2Value = 256},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/2/2017 12:00:00 AM +02:00", ci), Exchange1Value = 285.88, Exchange2Value = 284},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/3/2017 12:00:00 AM +02:00", ci), Exchange1Value = 281, Exchange2Value = 276.1},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/4/2017 12:00:00 AM +02:00", ci), Exchange1Value = 272.01, Exchange2Value = 268.4},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/5/2017 12:00:00 AM +02:00", ci), Exchange1Value = 266.61, Exchange2Value = 266.3},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/6/2017 12:00:00 AM +02:00", ci), Exchange1Value = 267.1, Exchange2Value = 266.49},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/7/2017 12:00:00 AM +02:00", ci), Exchange1Value = 250.74, Exchange2Value = 240.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/8/2017 12:00:00 AM +02:00", ci), Exchange1Value = 248.2, Exchange2Value = 245},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/9/2017 12:00:00 AM +02:00", ci), Exchange1Value = 246.61, Exchange2Value = 236.4},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/10/2017 12:00:00 AM +02:00", ci), Exchange1Value = 219.98, Exchange2Value = 210},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/11/2017 12:00:00 AM +02:00", ci), Exchange1Value = 198.09, Exchange2Value = 190.99},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/12/2017 12:00:00 AM +02:00", ci), Exchange1Value = 226.64, Exchange2Value = 223},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/13/2017 12:00:00 AM +02:00", ci), Exchange1Value = 208, Exchange2Value = 204.8},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/14/2017 12:00:00 AM +02:00", ci), Exchange1Value = 201, Exchange2Value = 197},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/15/2017 12:00:00 AM +02:00", ci), Exchange1Value = 179.79, Exchange2Value = 168.6},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/16/2017 12:00:00 AM +02:00", ci), Exchange1Value = 158.43, Exchange2Value = 155.2},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/17/2017 12:00:00 AM +02:00", ci), Exchange1Value = 185, Exchange2Value = 188.72},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/18/2017 12:00:00 AM +02:00", ci), Exchange1Value = 222.7, Exchange2Value = 228},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/19/2017 12:00:00 AM +02:00", ci), Exchange1Value = 198.45, Exchange2Value = 193.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/20/2017 12:00:00 AM +02:00", ci), Exchange1Value = 220, Exchange2Value = 227.49},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/21/2017 12:00:00 AM +02:00", ci), Exchange1Value = 215.46, Exchange2Value = 216},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/22/2017 12:00:00 AM +02:00", ci), Exchange1Value = 225, Exchange2Value = 230.13},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/23/2017 12:00:00 AM +02:00", ci), Exchange1Value = 225.59, Exchange2Value = 229.85},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/24/2017 12:00:00 AM +02:00", ci), Exchange1Value = 223.4, Exchange2Value = 225.8},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/25/2017 12:00:00 AM +02:00", ci), Exchange1Value = 206.94, Exchange2Value = 204},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/26/2017 12:00:00 AM +02:00", ci), Exchange1Value = 200.54, Exchange2Value = 202.21},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/27/2017 12:00:00 AM +02:00", ci), Exchange1Value = 200.61, Exchange2Value = 203.1},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/28/2017 12:00:00 AM +02:00", ci), Exchange1Value = 187.44, Exchange2Value = 191.55},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/29/2017 12:00:00 AM +02:00", ci), Exchange1Value = 192.08, Exchange2Value = 207.14},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/30/2017 12:00:00 AM +02:00", ci), Exchange1Value = 192.68, Exchange2Value = 198.45},
                new ComparisonRow {Date = DateTimeOffset.Parse("7/31/2017 12:00:00 AM +02:00", ci), Exchange1Value = 193.5, Exchange2Value = 201.78},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/1/2017 12:00:00 AM +02:00", ci), Exchange1Value = 216.2, Exchange2Value = 226.25},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/2/2017 12:00:00 AM +02:00", ci), Exchange1Value = 215, Exchange2Value = 218.49},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/3/2017 12:00:00 AM +02:00", ci), Exchange1Value = 220.08, Exchange2Value = 225.4},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/4/2017 12:00:00 AM +02:00", ci), Exchange1Value = 217.71, Exchange2Value = 222.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/5/2017 12:00:00 AM +02:00", ci), Exchange1Value = 239.57, Exchange2Value = 253.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/6/2017 12:00:00 AM +02:00", ci), Exchange1Value = 259.23, Exchange2Value = 265.67},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/7/2017 12:00:00 AM +02:00", ci), Exchange1Value = 260.77, Exchange2Value = 269.8},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/8/2017 12:00:00 AM +02:00", ci), Exchange1Value = 289.56, Exchange2Value = 296.98},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/9/2017 12:00:00 AM +02:00", ci), Exchange1Value = 292.91, Exchange2Value = 297.78},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/10/2017 12:00:00 AM +02:00", ci), Exchange1Value = 296, Exchange2Value = 300.2},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/11/2017 12:00:00 AM +02:00", ci), Exchange1Value = 298.2, Exchange2Value = 310},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/12/2017 12:00:00 AM +02:00", ci), Exchange1Value = 303.5, Exchange2Value = 309.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/13/2017 12:00:00 AM +02:00", ci), Exchange1Value = 292.9, Exchange2Value = 298},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/14/2017 12:00:00 AM +02:00", ci), Exchange1Value = 295.7, Exchange2Value = 302.7},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/15/2017 12:00:00 AM +02:00", ci), Exchange1Value = 278, Exchange2Value = 286.65},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/16/2017 12:00:00 AM +02:00", ci), Exchange1Value = 291, Exchange2Value = 302.26},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/17/2017 12:00:00 AM +02:00", ci), Exchange1Value = 297.5, Exchange2Value = 300.79},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/18/2017 12:00:00 AM +02:00", ci), Exchange1Value = 294.55, Exchange2Value = 293},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/19/2017 12:00:00 AM +02:00", ci), Exchange1Value = 289, Exchange2Value = 294.63},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/20/2017 12:00:00 AM +02:00", ci), Exchange1Value = 295.91, Exchange2Value = 299.45},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/21/2017 12:00:00 AM +02:00", ci), Exchange1Value = 323.2, Exchange2Value = 322},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/22/2017 12:00:00 AM +02:00", ci), Exchange1Value = 315, Exchange2Value = 314.52},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/23/2017 12:00:00 AM +02:00", ci), Exchange1Value = 313.72, Exchange2Value = 317.31},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/24/2017 12:00:00 AM +02:00", ci), Exchange1Value = 318.24, Exchange2Value = 326.65},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/25/2017 12:00:00 AM +02:00", ci), Exchange1Value = 323, Exchange2Value = 331.21},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/26/2017 12:00:00 AM +02:00", ci), Exchange1Value = 328.9, Exchange2Value = 332.65},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/27/2017 12:00:00 AM +02:00", ci), Exchange1Value = 340, Exchange2Value = 347.6},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/28/2017 12:00:00 AM +02:00", ci), Exchange1Value = 341.73, Exchange2Value = 347.54},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/29/2017 12:00:00 AM +02:00", ci), Exchange1Value = 367, Exchange2Value = 371.99},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/30/2017 12:00:00 AM +02:00", ci), Exchange1Value = 378, Exchange2Value = 383.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("8/31/2017 12:00:00 AM +02:00", ci), Exchange1Value = 384.55, Exchange2Value = 388},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/1/2017 12:00:00 AM +02:00", ci), Exchange1Value = 394.96, Exchange2Value = 390.01},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/2/2017 12:00:00 AM +02:00", ci), Exchange1Value = 351, Exchange2Value = 354.89},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/3/2017 12:00:00 AM +02:00", ci), Exchange1Value = 354.39, Exchange2Value = 355},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/4/2017 12:00:00 AM +02:00", ci), Exchange1Value = 305, Exchange2Value = 307.96},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/5/2017 12:00:00 AM +02:00", ci), Exchange1Value = 324.1, Exchange2Value = 322.98},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/6/2017 12:00:00 AM +02:00", ci), Exchange1Value = 337.4, Exchange2Value = 341.84},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/7/2017 12:00:00 AM +02:00", ci), Exchange1Value = 334.71, Exchange2Value = 337.96},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/8/2017 12:00:00 AM +02:00", ci), Exchange1Value = 308.74, Exchange2Value = 310.27},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/9/2017 12:00:00 AM +02:00", ci), Exchange1Value = 310, Exchange2Value = 304.74},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/10/2017 12:00:00 AM +02:00", ci), Exchange1Value = 305.7, Exchange2Value = 301.54},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/11/2017 12:00:00 AM +02:00", ci), Exchange1Value = 304.2, Exchange2Value = 297.64},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/12/2017 12:00:00 AM +02:00", ci), Exchange1Value = 301.12, Exchange2Value = 294},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/13/2017 12:00:00 AM +02:00", ci), Exchange1Value = 279, Exchange2Value = 276.21},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/14/2017 12:00:00 AM +02:00", ci), Exchange1Value = 230, Exchange2Value = 223.85},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/15/2017 12:00:00 AM +02:00", ci), Exchange1Value = 267.21, Exchange2Value = 259.42},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/16/2017 12:00:00 AM +02:00", ci), Exchange1Value = 261.79, Exchange2Value = 254.81},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/17/2017 12:00:00 AM +02:00", ci), Exchange1Value = 263.56, Exchange2Value = 259.1},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/18/2017 12:00:00 AM +02:00", ci), Exchange1Value = 298.57, Exchange2Value = 297.98},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/19/2017 12:00:00 AM +02:00", ci), Exchange1Value = 287.27, Exchange2Value = 282.61},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/20/2017 12:00:00 AM +02:00", ci), Exchange1Value = 290, Exchange2Value = 282.36},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/21/2017 12:00:00 AM +02:00", ci), Exchange1Value = 263.62, Exchange2Value = 258.8},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/22/2017 12:00:00 AM +02:00", ci), Exchange1Value = 264.63, Exchange2Value = 262.92},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/23/2017 12:00:00 AM +02:00", ci), Exchange1Value = 279.71, Exchange2Value = 285.99},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/24/2017 12:00:00 AM +02:00", ci), Exchange1Value = 283.06, Exchange2Value = 282.95},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/25/2017 12:00:00 AM +02:00", ci), Exchange1Value = 291.89, Exchange2Value = 295},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/26/2017 12:00:00 AM +02:00", ci), Exchange1Value = 291.51, Exchange2Value = 287.9},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/27/2017 12:00:00 AM +02:00", ci), Exchange1Value = 305.58, Exchange2Value = 310.33},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/28/2017 12:00:00 AM +02:00", ci), Exchange1Value = 299.57, Exchange2Value = 302.7},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/29/2017 12:00:00 AM +02:00", ci), Exchange1Value = 288.31, Exchange2Value = 292.4},
                new ComparisonRow {Date = DateTimeOffset.Parse("9/30/2017 12:00:00 AM +02:00", ci), Exchange1Value = 296.39, Exchange2Value = 302.2},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/1/2017 12:00:00 AM +02:00", ci), Exchange1Value = 298.86, Exchange2Value = 303.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/2/2017 12:00:00 AM +02:00", ci), Exchange1Value = 294.69, Exchange2Value = 296.66},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/3/2017 12:00:00 AM +02:00", ci), Exchange1Value = 289.15, Exchange2Value = 291.17},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/4/2017 12:00:00 AM +02:00", ci), Exchange1Value = 290.92, Exchange2Value = 292},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/5/2017 12:00:00 AM +02:00", ci), Exchange1Value = 293.65, Exchange2Value = 294.75},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/6/2017 12:00:00 AM +02:00", ci), Exchange1Value = 303.34, Exchange2Value = 308.7},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/7/2017 12:00:00 AM +02:00", ci), Exchange1Value = 307, Exchange2Value = 310.57},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/8/2017 12:00:00 AM +02:00", ci), Exchange1Value = 305.34, Exchange2Value = 308.59},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/9/2017 12:00:00 AM +02:00", ci), Exchange1Value = 294, Exchange2Value = 296.9},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/10/2017 12:00:00 AM +02:00", ci), Exchange1Value = 297.07, Exchange2Value = 298.02},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/11/2017 12:00:00 AM +02:00", ci), Exchange1Value = 296.82, Exchange2Value = 303},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/12/2017 12:00:00 AM +02:00", ci), Exchange1Value = 294.47, Exchange2Value = 302.12},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/13/2017 12:00:00 AM +02:00", ci), Exchange1Value = 323.03, Exchange2Value = 336.98},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/14/2017 12:00:00 AM +02:00", ci), Exchange1Value = 321.02, Exchange2Value = 337.99},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/15/2017 12:00:00 AM +02:00", ci), Exchange1Value = 321.81, Exchange2Value = 335.71},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/16/2017 12:00:00 AM +02:00", ci), Exchange1Value = 320.51, Exchange2Value = 333.42},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/17/2017 12:00:00 AM +02:00", ci), Exchange1Value = 308.51, Exchange2Value = 316.83},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/18/2017 12:00:00 AM +02:00", ci), Exchange1Value = 308.2, Exchange2Value = 314.36},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/19/2017 12:00:00 AM +02:00", ci), Exchange1Value = 304.78, Exchange2Value = 307.87},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/20/2017 12:00:00 AM +02:00", ci), Exchange1Value = 294.59, Exchange2Value = 311.52},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/21/2017 12:00:00 AM +02:00", ci), Exchange1Value = 293.44, Exchange2Value = 311.52},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/22/2017 12:00:00 AM +02:00", ci), Exchange1Value = 291.09, Exchange2Value = 311.52},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/23/2017 12:00:00 AM +02:00", ci), Exchange1Value = 282.07, Exchange2Value = 285.17},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/24/2017 12:00:00 AM +02:00", ci), Exchange1Value = 297.58, Exchange2Value = 298.4},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/25/2017 12:00:00 AM +02:00", ci), Exchange1Value = 295.69, Exchange2Value = 296.87},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/26/2017 12:00:00 AM +02:00", ci), Exchange1Value = 291.21, Exchange2Value = 295.52},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/27/2017 12:00:00 AM +02:00", ci), Exchange1Value = 295, Exchange2Value = 297.06},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/28/2017 12:00:00 AM +02:00", ci), Exchange1Value = 292.5, Exchange2Value = 295.14},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/29/2017 12:00:00 AM +02:00", ci), Exchange1Value = 295, Exchange2Value = 303.95},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/30/2017 12:00:00 AM +01:00", ci), Exchange1Value = 299.9, Exchange2Value = 306.2},
                new ComparisonRow {Date = DateTimeOffset.Parse("10/31/2017 12:00:00 AM +01:00", ci), Exchange1Value = 299.11, Exchange2Value = 304.12},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/1/2017 12:00:00 AM +01:00", ci), Exchange1Value = 289.35, Exchange2Value = 290.1},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/2/2017 12:00:00 AM +01:00", ci), Exchange1Value = 285.09, Exchange2Value = 283.75},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/3/2017 12:00:00 AM +01:00", ci), Exchange1Value = 298.8, Exchange2Value = 304.17},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/4/2017 12:00:00 AM +01:00", ci), Exchange1Value = 293.82, Exchange2Value = 299.41},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/5/2017 12:00:00 AM +01:00", ci), Exchange1Value = 293.87, Exchange2Value = 294.62},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/6/2017 12:00:00 AM +01:00", ci), Exchange1Value = 296.97, Exchange2Value = 298.5},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/7/2017 12:00:00 AM +01:00", ci), Exchange1Value = 291.64, Exchange2Value = 292.07},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/8/2017 12:00:00 AM +01:00", ci), Exchange1Value = 301.94, Exchange2Value = 308.38},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/9/2017 12:00:00 AM +01:00", ci), Exchange1Value = 315.5, Exchange2Value = 319.76},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/10/2017 12:00:00 AM +01:00", ci), Exchange1Value = 297.45, Exchange2Value = 298},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/11/2017 12:00:00 AM +01:00", ci), Exchange1Value = 313.85, Exchange2Value = 313.65},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/12/2017 12:00:00 AM +01:00", ci), Exchange1Value = 306.29, Exchange2Value = 305.13},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/13/2017 12:00:00 AM +01:00", ci), Exchange1Value = 315.19, Exchange2Value = 314.83},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/14/2017 12:00:00 AM +01:00", ci), Exchange1Value = 336.8, Exchange2Value = 335},
                new ComparisonRow {Date = DateTimeOffset.Parse("11/15/2017 12:00:00 AM +01:00", ci), Exchange1Value = 329.31, Exchange2Value = 329.31}
            };

            var comparison = new Comparison(testData)
            {
                FiatCurrency = "USD",
                CryptoCurrency = "ETH",
                Market1 = "Exmo",
                Market2 = "Kraken"
            };

            var originalConsoleOut = Console.Out;
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);
                const double investment = 10000;
                StatArb.BackTesting(comparison, investment);

                writer.Flush();
                var log = writer.GetStringBuilder().ToString();
                output.WriteLine(log);
            }
            Console.SetOut(originalConsoleOut);


        }
    }
}
