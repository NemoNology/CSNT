using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSNT_Lab_5
{
    class Wi_Fi
    {
        private static string[] TypesString = { "2,4 ГГц", "5 ГГц" };

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
                switch (type)
                {
                    case 0:

                        return 0;

                    case 1:

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
                switch (type)
                {
                    case 0:

                        return 50;

                    case 1:

                        return 5;

                    default:

                        return -1;
                }
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
