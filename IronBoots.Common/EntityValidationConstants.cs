namespace IronBoots.Common
{
    public static class EntityValidationConstants
    {
        public static class DateTimeValidation
        {
            public const string RequiredDateFormat = "dd-MM-YYYY";
            public const string TimeFormat = "HH:mm:ss";
        }
        public static class AddressValidation
        {
            public const int AddressMin = 3;
            public const int AddressMax = 80;
        }

        public static class ApplicationUserValidation
        {
            public const int NameMin = 1;
            public const int NameMax = 50;
        }

        public static class ClientValidation
        {
            public const int NameMin = 1;
            public const int NameMax = 50;
        }

        public static class MaterialValidation
        {
            public const int NameMin = 1;
            public const int NameMax = 20;

            public const decimal PriceMin = 0.01m;
            public const decimal PriceMax = decimal.MaxValue;
        }

        public static class OrderValidation
        {
            public const decimal PriceMin = 0.01m;
            public const decimal PriceMax = decimal.MaxValue;
        }

        public static class OrderProductValidation
        {
            public const int QuantityMin = 1;
            public const int QuantityMax = 200;
        }

        public static class ProductValidation
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

        public static class ProductMaterialValidation
        {
            public const int QuantityMin = 1;
            public const int QuantityMax = 200;
        }

        public static class TownValidation
        {
            public const int NameMin = 3;
            public const int NameMax = 60;
        }

        public static class VehicleValidation
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
