using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGK_POS
{
    class Cart
    {
        public static String pName;
        public static String pCode;
        public static Decimal pSellingPrice;
        public static int availQty;
        public static Decimal costPrice;
        public static Decimal profit;
        public static int qty;
        public static Decimal totalAmount;
        public static Decimal discountedPrice;
       // private static int qty { get; set; }
       // private static Decimal totalAmount { get; set; }
       // private static Decimal discountedPrice { get; set; }
       // private static Decimal profit { get; set; }       
        
        public Cart(String n,String c, Decimal sp, int availqty,Decimal pro,Decimal tCst,int itemqty,Decimal toAmt) {
            pName = n;
            pCode = c;
            pSellingPrice = sp;
            availQty = availqty;
            profit = pro;
            totalAmount = toAmt;
            costPrice = tCst;
            qty = itemqty;
        }
        public Cart(String n, String c, Decimal sp, int qty, Decimal pro, Decimal dis, Decimal tCst, int itemqty, Decimal toAmt)
        {
            pName = n;
            pCode = c;
            pSellingPrice = sp;
            availQty = qty;
            profit = pro;
            discountedPrice = dis;
            totalAmount = toAmt;
            costPrice = tCst;
            qty = itemqty;
        }
    }
}
