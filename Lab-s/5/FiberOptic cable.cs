using System;

namespace CSNT_Lab_5
{
    class FiberOptic_cable
    {
        private static string[] TypesString = { "ОМ-1", "ОМ-2", "ОМ-3", "ОМ-4" };


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
                    case 0: case 1:

                        return 2;

                    case 2: case 3:

                        return 3;

                    default:

                        return -1;
                }
            }
        }

        public static int GetMaxLength(long speed)
        {
            switch (type)
            {
                case 0:

                    return 33;

                case 1:

                    return 82;

                case 2:

                    if (speed < 3)
                    {
                        return 300;
                    }
                    else
                    {
                        return 100;
                    }

                case 3:

                    if (speed < 3)
                    {
                        return 550;
                    }
                    else
                    {
                        return 150;
                    }

                default:

                    return -1;
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

            if (Speed > maxSpeed || Lenght > GetMaxLength(Speed))
            {
                return false;
            }

            return true;

        }

    }
}

