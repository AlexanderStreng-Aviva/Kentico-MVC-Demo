using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;

namespace DancingGoat.Generator
{
    public class EcommerceGenerator
    {
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly IList<CountryInfo> countries;

        private readonly string[] customerNames = new string[100]
        {
            "Deneen Fernald",
            "Antonio Buker",
            "Marlon Loos",
            "Nolan Steckler",
            "Johnetta Tall",
            "Florence Ramsdell",
            "Modesto Speaker",
            "Alissa Ferguson",
            "Calvin Hollier",
            "Diamond Paik",
            "Mardell Dohrmann",
            "Dinorah Clower",
            "Andrea Humbert",
            "Tyrell Galvan",
            "Yong Inskeep",
            "Tom Goldschmidt",
            "Kimbery Rincon",
            "Genaro Kneeland",
            "Roselyn Mulvey",
            "Nancee Jacobson",
            "Jaime Link",
            "Fonda Belnap",
            "Muoi Ishmael",
            "Pearlene Minjarez",
            "Eustolia Studstill",
            "Marilynn Manos",
            "Pamila Turnbow",
            "Lieselotte Satcher",
            "Sharron Mellon",
            "Bennett Heatherington",
            "Spring Hessel",
            "Lashay Blazier",
            "Veronika Lecuyer",
            "Mark Spitz",
            "Peggy Olson",
            "Tyron Bednarczyk",
            "Terese Betty",
            "Bibi Kling",
            "Bruno Spier",
            "Cristen Bussey",
            "Daine Pridemore",
            "Gerald Turpen",
            "Lela Briese",
            "Sharda Bonnie",
            "Omar Martin",
            "Marlyn Pettie",
            "Shiela Cleland",
            "Marica Granada",
            "Garland Reagan",
            "Mora Gillmore",
            "Mariana Rossow",
            "Betty Pollan",
            "Analisa Costilla",
            "Evelyn Mendez",
            "April Rubino",
            "Zachariah Roberson",
            "Sheilah Steinhauser",
            "Araceli Vallance",
            "Lashawna Weise",
            "Charline Durante",
            "Melania Nightingale",
            "Ema Stiltner",
            "Lynelle Threet",
            "Dorcas Cully",
            "Gregg Carranco",
            "Karla Heiner",
            "Judson Siegmund",
            "Alyson Oday",
            "Winston Laxton",
            "Jarod Turrentine",
            "Israel Shanklin",
            "Miquel Jorstad",
            "Brianne Darrow",
            "Tamara Rulison",
            "Elliot Rameriz",
            "Gearldine Nova",
            "Debi Fritts",
            "Leota Cape",
            "Tyler Saleem",
            "Starr Hyden",
            "Loreen Spigner",
            "Raisa Germain",
            "Grace Vigue",
            "Maryann Munsch",
            "Jason Chon",
            "Gisele Mcquillen",
            "Juliane Comeaux",
            "Willette Dodrill",
            "Sherril Weymouth",
            "Ashleigh Dearman",
            "Bret Bourne",
            "Brittney Cron",
            "Dustin Evans",
            "Barbie Dinwiddie",
            "Ricki Wiener",
            "Bess Pedretti",
            "Monica King",
            "Edgar Schuetz",
            "Jettie Boots",
            "Jefferson Hinkle"
        };

        private readonly Random rand;

        public EcommerceGenerator()
        {
            countries = CountryInfoProvider.GetCountries().ToList();
            rand = new Random(DateTime.Now.Millisecond);
        }

        public void Generate(int siteID)
        {
            GenerateEcommerceData(siteID);
        }

        private void GenerateEcommerceData(int siteID)
        {
            var siteName = SiteInfoProvider.GetSiteName(siteID);
            var currencyInfo = CurrencyInfoProvider.GetCurrencies(siteID)
                .Where("CurrencyIsMain", QueryOperator.Equals, 1).TopN(1).FirstOrDefault();
            var list1 = PaymentOptionInfoProvider.GetPaymentOptions(siteID).ToList();
            var list2 = ShippingOptionInfoProvider.GetShippingOptions(siteID).ToList();
            
            var orderStatusList = OrderStatusInfoProvider.GetOrderStatuses(siteID).ToDictionary(status => status.StatusName);

            var manufacturerExceptionList = new List<int>
            {
                ManufacturerInfoProvider.GetManufacturerInfo("Aerobie", siteName).ManufacturerID,
                //ManufacturerInfoProvider.GetManufacturerInfo("Chemex", siteName).ManufacturerID,
                //ManufacturerInfoProvider.GetManufacturerInfo("Espro", siteName).ManufacturerID
            };
            var list3 = SKUInfoProvider.GetSKUs(siteID).ToList().Where(sku =>
            {
                if (sku.IsProduct)
                    return !manufacturerExceptionList.Contains(sku.SKUManufacturerID);
                return false;
            }).ToList();
            int num1;
            IList<int> intList;
            
            if (CustomerInfoProvider.GetCustomers().WhereEquals("CustomerSiteID", siteID).Count < 50)
            {
                num1 = customerNames.Length;
                intList = new List<int>();
                for (var index = 0; index < num1; ++index)
                    intList.Add(GenerateCustomer(customerNames[index], siteID).CustomerID);
            }
            else
            {
                intList = DataHelper.GetIntegerValues(CustomerInfoProvider.GetCustomers().Column("CustomerID")
                        .WhereEquals("CustomerSiteID", siteID).WhereNotEquals("CustomerEmail", "alex").Tables[0],
                    "CustomerID");
                num1 = intList.Count;
            }

            var num2 = 0;
            var num3 = 0;
            for (var index1 = 0; index1 <= 30; ++index1)
            {
                ++num2;
                var num4 = 0;
                if (index1 > 5)
                    num4 = rand.Next(-1, 2);
                for (var index2 = 0; index2 < num2 / 2 + num4; ++index2)
                {
                    var orderStatusInfo = index1 >= 25
                        ? index1 >= 29 ? orderStatusList["New"] : orderStatusList["InProgress"]
                        : orderStatusList["Completed"];
                    var orderInfo = new OrderInfo
                    {
                        OrderCustomerID = intList[num3 % num1],
                        OrderCurrencyID = currencyInfo.CurrencyID,
                        OrderSiteID = siteID,
                        OrderStatusID = orderStatusInfo.StatusID,
                        OrderIsPaid = "Completed".Equals(orderStatusInfo.StatusName, StringComparison.Ordinal) ||
                                      (uint) rand.Next(0, 2) > 0U,
                        OrderShippingOptionID = list2[rand.Next(list2.Count)].ShippingOptionID,
                        OrderPaymentOptionID = list1[rand.Next(list1.Count)].PaymentOptionID,
                        OrderGrandTotal = decimal.Zero,
                        OrderGrandTotalInMainCurrency = decimal.Zero,
                        OrderTotalPrice = decimal.Zero,
                        OrderTotalPriceInMainCurrency = decimal.Zero,
                        OrderTotalShipping = new decimal(10),
                        OrderTotalTax = new decimal(10)
                    };
                    OrderInfoProvider.SetOrderInfo(orderInfo);
                    var orderItems = GenerateOrderItems(orderInfo, list3);
                    GenerateOrderAddress(orderInfo.OrderID, GetRandomCountryId(), AddressType.Billing);
                    GenerateOrderAddress(orderInfo.OrderID, GetRandomCountryId(), AddressType.Shipping);
                    orderInfo.OrderDate = DateTime.Now.AddDays(index1 - 30);
                    orderInfo.OrderTotalPrice = orderItems;
                    orderInfo.OrderTotalPriceInMainCurrency = orderItems;
                    orderInfo.OrderGrandTotal = orderItems;
                    orderInfo.OrderGrandTotalInMainCurrency = orderItems;
                    var cartInfoFromOrder = ShoppingCartInfoProvider.GetShoppingCartInfoFromOrder(orderInfo.OrderID);
                    orderInfo.OrderInvoiceNumber = OrderInfoProvider.GenerateInvoiceNumber(cartInfoFromOrder);
                    orderInfo.OrderInvoice = ShoppingCartInfoProvider.GetOrderInvoice(cartInfoFromOrder);
                    OrderInfoProvider.SetOrderInfo(orderInfo);
                    ++num3;
                }
            }

            if (UserInfoProvider.GetUserInfo("alex") != null)
                return;
            var customerInfo = new CustomerInfo
            {
                CustomerEmail = "alex@localhost.local",
                CustomerFirstName = "Alexander",
                CustomerLastName = "Adams",
                CustomerSiteID = siteID,
                CustomerCompany = "Alex & Co. Ltd",
                CustomerTaxRegistrationID = "12S379BDF798",
                CustomerOrganizationID = "WRQ7987VRG79"
            };
            CustomerInfoProvider.SetCustomerInfo(customerInfo);
            var userInfo = CustomerInfoProvider.RegisterCustomer(customerInfo, "", "alex");
            var roleInfo = RoleInfoProvider.GetRoleInfo("SilverPartner", siteID);
            if (roleInfo != null)
                UserInfoProvider.AddUserToRole(userInfo.UserID, roleInfo.RoleID);
            for (var index = 0; index < 5; ++index)
            {
                var cart = new ShoppingCartInfo();
                cart.ShoppingCartCulture = CultureHelper.GetDefaultCultureCode(siteName);
                cart.ShoppingCartCurrencyID = currencyInfo.CurrencyID;
                cart.ShoppingCartSiteID = siteID;
                cart.ShoppingCartCustomerID = customerInfo.CustomerID;
                cart.ShoppingCartBillingAddress = GenerateAddress(GetRandomCountryId(), customerInfo.CustomerID);
                cart.ShoppingCartShippingAddress = GenerateAddress(GetRandomCountryId(), customerInfo.CustomerID);
                cart.User = userInfo;
                ShoppingCartInfoProvider.SetShoppingCartInfo(cart);
                ShoppingCartInfoProvider.SetShoppingCartItem(cart,
                    new ShoppingCartItemParameters(list3.ElementAt(rand.Next(list3.Count)).SKUID, rand.Next(5)));
                cart.Evaluate();
                ShoppingCartInfoProvider.SetOrder(cart);
            }
        }

        private int GetRandomCountryId() => countries[rand.Next(countries.Count)].CountryID;

        private decimal GenerateOrderItems(OrderInfo order, IList<SKUInfo> products)
        {
            var skuInfoSet = new HashSet<SKUInfo>();
            var num1 = new decimal();
            do
            {
                var product = products[rand.Next(products.Count)];
                if (!skuInfoSet.Contains(product))
                    skuInfoSet.Add(product);
            } while (skuInfoSet.Count < rand.Next(1, 3));

            foreach (var skuInfo in skuInfoSet)
            {
                var orderItemInfo = new OrderItemInfo
                {
                    OrderItemOrderID = order.OrderID,
                    OrderItemSKUID = skuInfo.SKUID,
                    OrderItemSKUName = skuInfo.SKUName,
                    OrderItemUnitPrice = skuInfo.SKUPrice,
                    OrderItemUnitCount = rand.Next(1, 3)
                };
                var num2 = orderItemInfo.OrderItemUnitPrice * orderItemInfo.OrderItemUnitCount;
                orderItemInfo.OrderItemTotalPrice = num2;
                orderItemInfo.Insert();
                num1 += num2;
            }

            return num1;
        }

        private static CustomerInfo GenerateCustomer(string fullName, int siteID)
        {
            var strArray = fullName.Trim().Split(' ');
            var str1 = strArray[0];
            var str2 = strArray[1];
            var customerObj = new CustomerInfo();
            customerObj.CustomerEmail = str1.ToLowerInvariant() + "@" + str2.ToLowerInvariant() + ".local";
            customerObj.CustomerFirstName = str1;
            customerObj.CustomerLastName = str2;
            customerObj.CustomerSiteID = siteID;
            CustomerInfoProvider.SetCustomerInfo(customerObj);
            return customerObj;
        }

        private void GenerateOrderAddress(int orderId, int countryId, AddressType addressType)
        {
            OrderAddressInfoProvider.SetAddressInfo(new OrderAddressInfo
            {
                AddressLine1 = "Main street " + rand.Next(300),
                AddressCity = "City " + rand.Next(300),
                AddressZip = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 8)
                    .Select(s => s[rand.Next(s.Length)]).ToArray()),
                AddressCountryID = countryId,
                AddressPersonalName = "Home address",
                AddressOrderID = orderId,
                AddressType = (int) addressType
            });
        }

        private AddressInfo GenerateAddress(int countryId, int customerId)
        {
            var addressObj = new AddressInfo
            {
                AddressName = "Address " + rand.Next(300),
                AddressLine1 = "Main street " + rand.Next(300),
                AddressCity = "City " + rand.Next(300),
                AddressZip = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 8).Select(s => s[rand.Next(s.Length)]).ToArray()),
                AddressCountryID = countryId,
                AddressCustomerID = customerId,
                AddressPersonalName = "Home address"
            };

            AddressInfoProvider.SetAddressInfo(addressObj);
            return addressObj;
        }
    }
}