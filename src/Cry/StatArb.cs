using System;
using System.Collections.Generic;
using System.Linq;

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
            var pnl_exch1 = new List<double>();
            var pnl_exch2 = new List<double>();

            var trade = false;
            var new_trade = false;
            var trade_pnl = new List<double>();

            Mode asset1 = Mode.Undefined;
            Mode asset2 = Mode.Undefined;
            Mode open_asset1 = Mode.Undefined;
            Mode open_asset2 = Mode.Undefined;

            double open_p1 = double.NaN;
            double open_p2 = double.NaN;

            var n = comparison.Count(); //  # number of data points

            bool firstTrade = true;
            foreach (var item in comparison)
            {
                var p1 = item.Coin1;
                var p2 = item.Coin2;

                if (p1 > p2)
                {
                    asset1 = Mode.Short;
                    asset2 = Mode.Long;
                    if (trade == false)
                    {
                        open_p1 = p1;  //# open prices
                        open_p2 = p2;
                        open_asset1 = asset1;
                        open_asset2 = asset2;
                        trade = true;
                        Console.WriteLine("new traded opened");
                        new_trade = false;

                    }
                    else if (asset1 == open_asset1)
                    {
                        new_trade = false; //  # flag
                    }
                    else if (asset1 == open_asset2)
                    {
                        new_trade = true;  // # flag
                    }
                }
                else if (p2 > p1)
                {
                    asset1 = Mode.Long;
                    asset2 = Mode.Short;
                    if (trade == false)
                    {
                        open_p1 = p1;  //# open prices
                        open_p2 = p2;
                        open_asset1 = asset1;
                        open_asset2 = asset2;
                        trade = true;
                        Console.WriteLine("new traded opened");
                        new_trade = false;

                    }
                    else if (asset1 == open_asset1)
                    {
                        new_trade = false; //  # flag
                    }
                    else if (asset1 == open_asset2)
                    {
                        new_trade = true;  // # flag
                    }
                }

                if (firstTrade)
                {
                    Console.WriteLine($"{item.Coin1}, {item.Coin1}, {asset1}, {asset2}, {trade}, ----first trade info");
                }
                else
                {
                    if (new_trade)
                    {
                        // # close current position
                        if (open_asset1 == Mode.Short)
                        {
                            // # PnL of both trades
                            var pnl_asset1 = open_p1 / p1 - 1;
                            var pnl_asset2 = p2 / open_p2 - 1;
                            pnl_exch1.Add(pnl_asset1);
                            pnl_exch2.Add(pnl_asset2);
                            Console.WriteLine($"{open_p1}, {p1}, {open_p2}, {p2}, {open_asset1}, {open_asset2}, {pnl_asset1}, {pnl_asset2}");
                            // # update both accountsS
                            account1 = account1 + position * pnl_asset1;
                            account2 = account2 + position * pnl_asset2;
                            Console.WriteLine("accounts [USD] = ", account1, account2);
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
                            Console.WriteLine("ROI = ", roi[-1]);
                            Console.WriteLine("trade closed\n");
                            trade = false;

                            // # open a new trade
                            if (asset1 == Mode.Short)
                            {
                                open_p1 = p1;
                                open_p2 = p2;
                                open_asset1 = asset1;
                                open_asset2 = asset2;
                            }
                            else
                            {
                                open_p1 = p1;
                                open_p2 = p2;
                                open_asset1 = asset1;
                                open_asset2 = asset2;
                            }
                            trade = true;
                            Console.WriteLine("new trade opened", asset1, asset2, open_p1, open_p2);
                        }

                        // # close current position
                        if (open_asset1 == Mode.Short)
                        {
                            //# PnL of both trades
                            var pnl_asset1 = p1 / open_p1 - 1;
                            var pnl_asset2 = open_p2 / p2 - 1;
                            pnl_exch1.Add(pnl_asset1);
                            pnl_exch2.Add(pnl_asset2);
                            Console.WriteLine($"{open_p1}, {p1}, {open_p2}, {p2}, {open_asset1}, {open_asset2}, {pnl_asset1}, {pnl_asset2}");
                            // # update both accounts
                            account1 = account1 + position * pnl_asset1;
                            account2 = account2 + position * pnl_asset2;
                            Console.WriteLine("accounts [USD] = ", account1, account2);
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
                            trade_pnl.Add(pnl_asset1 + pnl_asset2);
                            Console.WriteLine("ROI = ", roi[-1]);
                            Console.WriteLine("trade closed\n");
                            trade = false;

                            // # open a new trade
                            if (open_asset1 == Mode.Short)
                            {
                                open_p1 = p1;
                                open_p2 = p2;
                                open_asset1 = asset1;
                                open_asset2 = asset2;
                            }
                            else
                            {
                                open_p1 = p1;
                                open_p2 = p2;
                                open_asset1 = asset1;
                                open_asset2 = asset2;
                            }
                            new_trade = false;
                            trade = true;
                            Console.WriteLine("new trade opened:", asset1, asset2, open_p1, open_p2);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"   {item.Coin1}, {item.Coin2}, {asset1}, {asset2}");
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
