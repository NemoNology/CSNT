using System.Collections.Generic;

namespace CSNT_Lab_3
{
    internal class Commutator
    {
        private List<Device> _ports = new List<Device>(8);

        private List<Device> _matrix = new List<Device>(256);


        // Restart/Reset commutator
        public void Reset()
        {
            _matrix.Clear();
        }

        public void GetRequest(byte portNum, string package)
        {

        }

    }
}
