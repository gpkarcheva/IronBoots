using System.Diagnostics.Contracts;

namespace IronBoots.Common
{
    public static class EntityValidationConstants
    {
        public static class Address
        {
            public const int AddressMin = 3;
            public const int AddressMax = 80;
        }

        public static class ApplicationUser
        {
            public const int NameMin = 1;
            public const int NameMax = 50;
        }

        public static class Client
        {
            public const int NameMin = 1;
            public const int NameMax = 50;
        }

        public static class Material
        {
            public const int NameMin = 1;
            public const int NameMax = 20;

            public const decimal PriceMin = 0.01m;
            public const decimal PriceMax = decimal.MaxValue;
        }

        public static class Order
        {
            public const decimal PriceMin = 0.01m;
            public const decimal PriceMax = decimal.MaxValue;
        }

        public static class OrderProduct
        {
            public const int QuantityMin = 1;
            public const int QuantityMax = 200;
        }

        public static class Product
        {
            public const int NameMin = 1;
            public const int NameMax = 50;

            public const double WeightMin = 0.01;
            public const double WeightMax = 2000;

            public const double SizeMin = 0.01;
            public const double SizeMax = 500;

            public const decimal CostMin = 0.01m;
            public const decimal CostMax = decimal.MaxValue;
        }
        
        public static class ProductMaterial
        {
            public const int QuantityMin = 1;
            public const int QuantityMax = 200;
        }

        public static class Town
        {
            public const int NameMin = 3;
            public const int NameMax = 60;
        }

        public static class Vehicle
        {
            public const int NameMin = 3;
            public const int NameMax = 30;

            public const double WeightMin = 0.01;
            public const double WeightMax = 30000;

            public const double SizeMin = 100;
            public const double SizeMax = 10000;
        }
    }
}
