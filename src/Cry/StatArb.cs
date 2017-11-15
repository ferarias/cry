using System;
using System.Collections.Generic;

namespace Cry
{
    public class StatArb
    {
        public static void BackTesting(IEnumerable<Comparison> comparison)
        {
            //# Backtesting Stat Arb trading strategy for ETH/USD at Exmo and Kraken
            //# cryptocurrency exchanges

            //# initial parameters
            double investment = 10000f;  //# USD
            double account1 = investment / 2;
            double account2 = investment / 2;  //# USD
            var position = 0.5 * (investment / 2); //# USD

            var roi = new List<double>();
            var ac1 = new List<double> { account1 };
            var ac2 = new List<double> { account2 };
            var money = new List<double>();
            var exchange1ProfitAndLoss = new List<double>();
            var exchange2ProfitAndLoss = new List<double>();

            var trade = false;
            var new_trade = false;
            var tradeProfitAndLoss = new List<double>();

            Mode asset1Mode = Mode.Undefined;
            Mode asset2Mode = Mode.Undefined;

            Mode openAsset1Mode = Mode.Undefined;
            Mode openAsset2Mode = Mode.Undefined;
            double openAsset1Price = 0;
            double openAsset2Price = 0;


            bool firstTrade = true;
            foreach (var item in comparison)
            {

                if (item.Coin1 > item.Coin2)
                {
                    asset1Mode = Mode.Short;
                    asset2Mode = Mode.Long;
                    if (!trade)
                    {
                        openAsset1Price = item.Coin1;  //# open prices
                        openAsset2Price = item.Coin2;
                        openAsset1Mode = asset1Mode;
                        openAsset2Mode = asset2Mode;
                        trade = true;
                        Console.WriteLine("new traded opened");
                        new_trade = false;

                    }
                    else if (asset1Mode == openAsset1Mode)
                    {
                        new_trade = false; //  # flag
                    }
                    else if (asset1Mode == openAsset2Mode)
                    {
                        new_trade = true;  // # flag
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
                        Console.WriteLine("new traded opened");
                        new_trade = false;

                    }
                    else if (asset1Mode == openAsset1Mode)
                    {
                        new_trade = false; //  # flag
                    }
                    else if (asset1Mode == openAsset2Mode)
                    {
                        new_trade = true;  // # flag
                    }
                }

                if (firstTrade)
                {
                    Console.WriteLine($"{item.Coin1}, {item.Coin1}, {asset1Mode}, {asset2Mode}, {trade}, ----first trade info");
                }
                else
                {
                    if (new_trade)
                    {
                        // # close current position
                        if (openAsset1Mode == Mode.Short)
                        {
                            // # PnL of both trades
                            var pnl_asset1 = openAsset1Price / item.Coin1 - 1;
                            var pnl_asset2 = item.Coin2 / openAsset2Price - 1;
                            exchange1ProfitAndLoss.Add(pnl_asset1);
                            exchange2ProfitAndLoss.Add(pnl_asset2);
                            Console.WriteLine($"{openAsset1Price}, {item.Coin1}, {openAsset2Price}, {item.Coin2}, {openAsset1Mode}, {openAsset2Mode}, {pnl_asset1}, {pnl_asset2}");
                            // # update both accountsS
                            account1 = account1 + position * pnl_asset1;
                            account2 = account2 + position * pnl_asset2;
                            Console.WriteLine("accounts [USD] = {0}, {1}", account1, account2);
                            if ((account1 <= 0) || (account2 <= 0))
                            {
                                Console.WriteLine("--trading halted");
                                break;
                            }
                            // # return on investment (ROI)
                            var total = account1 + account2;
                            roi.Add(total / investment - 1);
                            ac1.Add(account1);
                            ac2.Add(account2);
                            money.Add(total);
                            Console.WriteLine("ROI = {0}", total / investment - 1);
                            Console.WriteLine("trade closed\n");
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
                            Console.WriteLine("new trade opened {0} {1} {2} {3}", asset1Mode, asset2Mode, openAsset1Price, openAsset2Price);
                        }

                        // # close current position
                        if (openAsset1Mode == Mode.Short)
                        {
                            //# PnL of both trades
                            var pnl_asset1 = item.Coin1 / openAsset1Price - 1;
                            var pnl_asset2 = openAsset2Price / item.Coin2 - 1;
                            exchange1ProfitAndLoss.Add(pnl_asset1);
                            exchange2ProfitAndLoss.Add(pnl_asset2);
                            Console.WriteLine($"{openAsset1Price}, {item.Coin1}, {openAsset2Price}, {item.Coin2}, {openAsset1Mode}, {openAsset2Mode}, {pnl_asset1}, {pnl_asset2}");
                            // # update both accounts
                            account1 = account1 + position * pnl_asset1;
                            account2 = account2 + position * pnl_asset2;
                            Console.WriteLine("accounts [USD] = {0}, {1}", account1, account2);
                            if ((account1 <= 0) || (account2 <= 0))
                            {
                                Console.WriteLine("--trading halted");
                                break;
                            }
                            // # return on investment (ROI)
                            var total = account1 + account2;
                            roi.Add(total / investment - 1);
                            ac1.Add(account1);
                            ac2.Add(account2);
                            money.Add(total);
                            tradeProfitAndLoss.Add(pnl_asset1 + pnl_asset2);
                            Console.WriteLine("ROI = {0}", total / investment - 1);
                            Console.WriteLine("trade closed\n");
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
                            new_trade = false;
                            trade = true;
                            Console.WriteLine("new trade opened: {0} {1} {2} {3}", asset1Mode, asset2Mode, openAsset1Price, openAsset2Price);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"   {item.Coin1}, {item.Coin2}, {asset1Mode}, {asset2Mode}");
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
