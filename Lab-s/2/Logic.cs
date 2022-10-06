using System;
using vector;



namespace LogicSpace
{

    class LogicSpace
    {

        public coding(string BC)
        {

            vector<string> partsOfBC;
            vector<float> Y; 
            int i = 0;

            while (i < BC.Length)
            {
                partsOfBC[i].append((string)BC[i] + (string)BC[i + 1]);
                i += 2;
            }

            for (i = 0; i < partsOfBC.Length; i++)
            {

                 switch (partsOfBC[i])
                 {

                    case "00": Y[i] = -3;//-2.5;
                                break;
                    case "01": Y[i] = -1; //-0.833;
                                break;
                    case "10": Y[i] = +3;//2.5;
                                break;
                    case "11": Y[i] = +1;//0.833;
                                break;
                    case default: break;

                 }

            }

            foreach (string pair in partsOfBC)
            {

                Console.Write(pair + "\t");

            }

            Console.Write("\n");

            foreach (float y in Y)
            {

                Console.Write((string)y + "\t");

            }
            Console.Write("\n");


        }

    }

}