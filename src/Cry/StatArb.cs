using System;
using System.Collections.Generic;

namespace Cry
{
    public class StatArb
    {
        public static void BackTesting(IEnumerable<ComparisonRow> comparison, double investment)
        {
            //# Backtesting Stat Arb trading strategy for ETH/USD at Exmo and Kraken
            //# cryptocurrency exchanges

            var account1 = investment / 2;
            var account2 = investment / 2;  //# USD
            var position = 0.5 * (investment / 2); //# USD

            var roi = new List<double>();
            var ac1 = new List<double> { account1 };
            var ac2 = new List<double> { account2 };
            var money = new List<double>();
            var exchange1ProfitAndLoss = new List<double>();
            var exchange2ProfitAndLoss = new List<double>();

            var tradeProfitAndLoss = new List<double>();

            var asset1Mode = Mode.Undefined;
            var asset2Mode = Mode.Undefined;

            var openAsset1Mode = Mode.Undefined;
            var openAsset2Mode = Mode.Undefined;
            double openAsset1Price = 0;
            double openAsset2Price = 0;


            var trade = false;
            var newTrade = false;
            var firstTrade = true;
            foreach (var item in comparison)
            {

                if (item.Coin1 > item.Coin2)
                {
                    asset1Mode = Mode.Short;
                    asset2Mode = Mode.Long;
                    if (!trade)
                    {
                        // open prices
                        openAsset1Price = item.Coin1;  
                        openAsset2Price = item.Coin2;
                        openAsset1Mode = asset1Mode;
                        openAsset2Mode = asset2Mode;
                        trade = true;
                        Console.WriteLine(">> New trade opened");
                        newTrade = false;

                    }
                    else if (asset1Mode == openAsset1Mode)
                    {
                        newTrade = false; //  # flag
                    }
                    else if (asset1Mode == openAsset2Mode)
                    {
                        newTrade = true;  // # flag
                    }
                }
                else if (item.Coin2 > item.Coin1)
                {
                    asset1Mode = Mode.Long;
                    asset2Mode = Mode.Short;
                    if (!trade)
                    {
                        openAsset1Price = item.Coin1;  //# open prices
                        openAsset2Price = item.Coin2;
                        openAsset1Mode = asset1Mode;
                        openAsset2Mode = asset2Mode;
                        trade = true;
                        Console.WriteLine(">> New trade opened");
                        newTrade = false;

                    }
                    else if (asset1Mode == openAsset1Mode)
                    {
                        newTrade = false; //  # flag
                    }
                    else if (asset1Mode == openAsset2Mode)
                    {
                        newTrade = true;  // # flag
                    }
                }

                if (firstTrade)
                {
                    Console.WriteLine("                 Value Coin 1 Value Coin 2 Asset 1 Asset 2 Trade");
                    Console.WriteLine($"First trade info {item.Coin1,-12:C} {item.Coin2,-12:C} {asset1Mode,-7} {asset2Mode,-7} {trade,-5}");
                }
                else
                {
                    if (newTrade)
                    {
                        // # close current position
                        if (openAsset1Mode == Mode.Short)
                        {
                            // # Profit and Loss of both trades
                            var pnlAsset1 = openAsset1Price / item.Coin1 - 1;
                            var pnlAsset2 = item.Coin2 / openAsset2Price - 1;
                            exchange1ProfitAndLoss.Add(pnlAsset1);
                            exchange2ProfitAndLoss.Add(pnlAsset2);
                            Console.WriteLine($"Open Asset 1: {openAsset1Price}, Coin1: {item.Coin1}, Open Asset 2:{openAsset2Price}, Coin 2:{item.Coin2}, Asset 1:{openAsset1Mode}, Asset 2:{openAsset2Mode}, PnL Asset 1:{pnlAsset1:C}, PnL Asset 2:{pnlAsset2:C}");
                            // # update both accountsS
                            account1 = account1 + position * pnlAsset1;
                            account2 = account2 + position * pnlAsset2;
                            Console.WriteLine("Accounts (USD) = {0}, {1}", account1, account2);
                            if (account1 <= 0 || account2 <= 0)
                            {
                                Console.WriteLine("XX Trading halted");
                                break;
                            }
                            // # return on investment (ROI)
                            var total = account1 + account2;
                            roi.Add(total / investment - 1);
                            ac1.Add(account1);
                            ac2.Add(account2);
                            money.Add(total);
                            Console.WriteLine("ROI = {0}", total / investment - 1);
                            Console.WriteLine("<< Trade closed");
                            trade = false;

                            // # open a new trade
                            if (asset1Mode == Mode.Short)
                            {
                                openAsset1Price = item.Coin1;
                                openAsset2Price = item.Coin2;
                                openAsset1Mode = asset1Mode;
                                openAsset2Mode = asset2Mode;
                            }
                            else
                            {
                                openAsset1Price = item.Coin1;
                                openAsset2Price = item.Coin2;
                                openAsset1Mode = asset1Mode;
                                openAsset2Mode = asset2Mode;
                            }
                            trade = true;
                            Console.WriteLine("                 Open Asset 1 Open Asset 2 Asset 1 Asset 2 Trade");
                            Console.WriteLine($"New trade opened {openAsset1Price,-12:C} {openAsset2Price,-12:C} {asset1Mode,-7} {asset2Mode,-7}");
                        }

                        // # close current position
                        if (openAsset1Mode == Mode.Short)
                        {
                            //# Profit and Loss of both trades
                            var pnlAsset1 = item.Coin1 / openAsset1Price - 1;
                            var pnlAsset2 = openAsset2Price / item.Coin2 - 1;
                            exchange1ProfitAndLoss.Add(pnlAsset1);
                            exchange2ProfitAndLoss.Add(pnlAsset2);
                            Console.WriteLine($"{openAsset1Price}, {item.Coin1}, {openAsset2Price}, {item.Coin2}, {openAsset1Mode}, {openAsset2Mode}, {pnlAsset1}, {pnlAsset2}");
                            // # update both accounts
                            account1 = account1 + position * pnlAsset1;
                            account2 = account2 + position * pnlAsset2;
                            Console.WriteLine("Accounts (USD) = {0}, {1}", account1, account2);
                            if ((account1 <= 0) || (account2 <= 0))
                            {
                                Console.WriteLine("XX Trading halted");
                                break;
                            }
                            // # return on investment (ROI)
                            var total = account1 + account2;
                            roi.Add(total / investment - 1);
                            ac1.Add(account1);
                            ac2.Add(account2);
                            money.Add(total);
                            tradeProfitAndLoss.Add(pnlAsset1 + pnlAsset2);
                            Console.WriteLine("ROI = {0}", total / investment - 1);
                            Console.WriteLine("<< Trade closed");
                            trade = false;

                            // # open a new trade
                            if (openAsset1Mode == Mode.Short)
                            {
                                openAsset1Price = item.Coin1;
                                openAsset2Price = item.Coin2;
                                openAsset1Mode = asset1Mode;
                                openAsset2Mode = asset2Mode;
                            }
                            else
                            {
                                openAsset1Price = item.Coin1;
                                openAsset2Price = item.Coin2;
                                openAsset1Mode = asset1Mode;
                                openAsset2Mode = asset2Mode;
                            }
                            newTrade = false;
                            trade = true;
                            Console.WriteLine("                 Open Asset 1 Open Asset 2 Asset 1 Asset 2 Trade");
                            Console.WriteLine($"New trade opened {openAsset1Price,-12:C} {openAsset2Price,-12:C} {asset1Mode,-7} {asset2Mode,-7}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"                 {item.Coin1,-12:C} {item.Coin2,-12:C} {asset1Mode,-7} {asset2Mode,-7}");
                    }
                }
                firstTrade = false;
            }
        }
        public enum Mode
        {
            Undefined = -1,
            Short = 0,
            Long = 1
        }
    }

}
