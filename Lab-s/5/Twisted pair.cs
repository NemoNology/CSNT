using System;

namespace CSNT_Lab_5
{
    class Twisted_pair
    {
        private static string[] TypesString = { "V4 3E", "V4 5E", "V8 3E", "V8 5E" };

        private static int _type;
        public static int type
        {
            get
            {
                return _type;
            }

            set
            {
                if (value < 0 || value >= TypesString.Length)
                {
                    throw new Exception("Invalid argument: wrong inputed type!");
                }

                _type = value;
            }
        }
        
        public static int maxSpeed
        {
            get
            {
                switch(type)
                {
                    case 0: case 1: case 2:

                        return 0;

                    case 3:

                        return 1;

                    default:

                        return -1;
                }
            }
        }

        public static int maxLength
        {
            get
            {
                return 100;
            }
        }


        public static string GetStringType()
        {
            return TypesString[type];
        }

        public static string[] GetStringTypes()
        {
            return TypesString;
        }

        public static bool IsThereConnection(int Type, int Speed, int Lenght)
        {
            type = Type;

            if (Speed > maxSpeed || Lenght > maxLength)
            {
                return false;
            }

            return true;

        }

    }
}
