using System;
using System.Collections.Generic;

namespace Cry
{
    public class StatArb
    {
        public static void BackTesting(Comparison comparison, double investment)
        {
            // Backtesting Stat Arb trading strategy cryptocurrency exchanges

            var account1 = investment / 2;
            var account2 = investment / 2;
            var position = 0.5 * (investment / 2);

            var roi = new List<double>();
            var ac1 = new List<double> { account1 };
            var ac2 = new List<double> { account2 };
            var money = new List<double>();
            var exchange1ProfitAndLoss = new List<double>();
            var exchange2ProfitAndLoss = new List<double>();
            var tradeProfitAndLoss = new List<double>();

            var openTradeMode = Mode.Undefined;

            double openAsset1Price = 0;
            double openAsset2Price = 0;


            var firstTrade = true;
            foreach (var row in comparison)
            {
                var tradeMode = (row.Exchange1Value >= row.Exchange2Value) ? Mode.ShortLong : Mode.LongShort;

                if (firstTrade)
                {
                    openAsset1Price = row.Exchange1Value;
                    openAsset2Price = row.Exchange2Value;
                    openTradeMode = tradeMode;
                    Console.WriteLine($"                 {comparison.Market1,-12} {comparison.Market2,-12} Mode");
                    Console.WriteLine($"New trade opened {row.Exchange1Value,-12:C} {row.Exchange2Value,-12:C} {tradeMode,-7}");
                    firstTrade = false;
                    continue;
                }

                if (tradeMode != openTradeMode)
                {
                    double profitAndLossAsset1 = 0, profitAndLossAsset2 = 0;
                    // close current position and calculate profit and Loss of both trades
                    if (openTradeMode == Mode.ShortLong)
                    {
                        profitAndLossAsset1 = openAsset1Price / row.Exchange1Value - 1;
                        profitAndLossAsset2 = row.Exchange2Value / openAsset2Price - 1;
                    }
                    if (openTradeMode == Mode.LongShort)
                    {
                        profitAndLossAsset1 = row.Exchange1Value / openAsset1Price - 1;
                        profitAndLossAsset2 = openAsset2Price / row.Exchange2Value - 1;
                    }

                    exchange1ProfitAndLoss.Add(profitAndLossAsset1);
                    exchange2ProfitAndLoss.Add(profitAndLossAsset2);
                    Console.WriteLine($"      Trade info {row.Exchange1Value,-12:C} {row.Exchange2Value,-12:C} {openTradeMode,-7} ");
                    Console.WriteLine($"Profit and Loss: {profitAndLossAsset1,-12:C} {profitAndLossAsset2,-12:C}");

                    // update both accounts
                    account1 = account1 + position * profitAndLossAsset1;
                    account2 = account2 + position * profitAndLossAsset2;
                    if ((account1 <= 0) || (account2 <= 0))
                    {
                        Console.WriteLine("XX Trading halted");
                        break;
                    }

                    // return on investment (ROI)
                    var total = account1 + account2;
                    roi.Add(total / investment - 1);
                    ac1.Add(account1);
                    ac2.Add(account2);
                    money.Add(total);
                    tradeProfitAndLoss.Add(profitAndLossAsset1 + profitAndLossAsset2);
                    Console.WriteLine($"Accounts ({comparison.FiatCurrency}) = {account1,-20} {account2,-20} ROI = {total / investment - 1}");
                    Console.WriteLine("<< Trade closed");

                    // Open a new trade
                    Console.WriteLine($"                 {comparison.Market1,-12} {comparison.Market2,-12} Mode");
                    Console.WriteLine($"New trade opened {openAsset1Price,-12:C} {openAsset2Price,-12:C} {tradeMode,-7}");
                    openAsset1Price = row.Exchange1Value;
                    openAsset2Price = row.Exchange2Value;
                    openTradeMode = tradeMode;
                }
                else
                {
                    Console.WriteLine($"                 {row.Exchange1Value,-12:C} {row.Exchange2Value,-12:C} {tradeMode,-7}");
                }

            }
        }
        public enum Mode
        {
            Undefined = -1,
            ShortLong = 0,
            LongShort = 1
        }
    }

}
