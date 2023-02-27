using System;

namespace IP_calculator__WF_
{
    class Network
    {
        #region Fields & Properties

        private bool _isIPv6 = false;
        private byte[] _ip = new byte[4];
        private byte[] _mask = new byte[4];
        public readonly int OktetSize = 8;

        /// <summary>
        /// Return true if it's IPv6, false - IPv4
        /// </summary>
        public bool VersionOfIP
        {
            get { return _isIPv6; }
            set { _isIPv6 = value; }
        }

        /// <summary>
        /// Return digits amount in IP (32 for IPv4, 48 - IPv6)
        /// </summary>
        public int DigitsAmountInIP => OktetSize * (_isIPv6 ? 6 : 4);

        /// <summary>
        /// IP Address
        /// </summary>
        public byte[] IP
        {
            get
            {
                return _ip;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(IP), $"{nameof(IP)} can't be null");
                }
                else if (value.Length != _ip.Length)
                {
                    throw new ArgumentException($"The inputed value length can't be different from the Address IP ({_ip.Length})", nameof(IP));
                }

                _ip = value;
            }
        }

        /// <summary>
        /// IP Address as string <br/>
        /// <example>  <b> Examples: </b> <br/> <i> 192.168.11.1 <br/> 0.0.12.0 </i>  </example>
        /// </summary>
        public string IPString
        {
            get
            {
                string res = string.Empty;

                foreach (var item in _ip)
                {
                    res += item.ToString() + ".";
                }

                return res.Substring(0, res.Length - 1);
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(IPString), $"{nameof(IPString)} can't be null");
                }

                var buffer = value.Split('.', ',', ' ');

                if (buffer.Length != _ip.Length)
                {
                    throw new ArgumentException($"{nameof(IPString)} has incorrect length", nameof(IPString));
                }

                byte[] res = new byte[_ip.Length];

                for (int i = 0; i < _ip.Length; i++)
                {
                    if (!Byte.TryParse(buffer[i], out res[i]))
                    {
                        throw new ArgumentException($"{nameof(IPString)} has incorrect value", nameof(IPString));
                    }
                }

                _ip = res;
            }
        }

        /// <summary>
        /// Network Mask
        /// </summary>
        public byte[] NetworkMask
        {
            get { return _mask; }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(NetworkMask), $"{nameof(NetworkMask)} can't be null");
                }
                else if (value.Length != NetworkMask.Length)
                {
                    throw new ArgumentException($"The inputed value length can't be different from the Network Mask ({_mask.Length})", nameof(value));
                }

                string buffer = string.Empty;

                foreach (var item in value)
                {
                    buffer += Convert.ToString(item, 2).PadRight(OktetSize, '0');
                }

                if (buffer.LastIndexOf("1") > buffer.IndexOf("0"))
                {
                    throw new ArgumentException($"The {nameof(NetworkMask)} was incorrect", nameof(NetworkMask));
                }

                _mask = value;
            }
        }

        /// <summary>
        /// Network Mask as string
        /// </summary>
        public string NetworkMaskString
        {
            get
            {
                string res = string.Empty;

                foreach (var item in _mask)
                {
                    res += item.ToString() + ".";
                }

                return res.Substring(0, res.Length - 1);
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(NetworkMaskString), $"{nameof(NetworkMaskString)} can't be null");
                }

                var buffer = value.Split('.', ',', ' ');

                if (buffer.Length != _mask.Length)
                {
                    throw new ArgumentException($"{nameof(NetworkMaskString)} has incorrect length", nameof(NetworkMaskString));
                }

                byte[] res = new byte[_mask.Length];

                for (int i = 0; i < _mask.Length; i++)
                {
                    if (!Byte.TryParse(buffer[i], out res[i]))
                    {
                        throw new ArgumentException($"{nameof(NetworkMaskString)} has incorrect value", nameof(NetworkMaskString));
                    }
                }

                _ip = res;
            }
        }

        /// <summary>
        /// Bit Mask - Network mask prefix
        /// </summary>
        public int BitMask
        {
            get
            {
                string buffer = string.Empty;

                foreach (var item in _mask)
                {
                    buffer += Convert.ToString(item, 2).PadLeft(OktetSize, '0');
                }

                return buffer.LastIndexOf('1') + 1;
            }

            set
            {
                if (value < 0 || value > DigitsAmountInIP)
                {
                    throw new ArgumentException($"{nameof(BitMask)} was incorrect", nameof(BitMask));
                }

                string buffer = string.Empty.PadLeft(value, '1');
                buffer = buffer.PadRight(DigitsAmountInIP, '0');

                for (int i = 0; i < buffer.Length; i += OktetSize)
                {
                    _mask[i / OktetSize] = Convert.ToByte(buffer.Substring(i, OktetSize), 2);
                }
            }
        }

        /// <summary>
        /// Return Wild Mask
        /// </summary>
        public byte[] WildMask
        {
            get
            {
                byte[] res = new byte[_mask.Length];

                string buffer;

                for (int i = 0; i < _mask.Length; i++)
                {
                    buffer = Convert.ToString(~_mask[i], 2);
                    buffer = buffer.Substring(sizeof(int) * OktetSize - OktetSize, OktetSize);

                    res[i] = Convert.ToByte(buffer, 2);
                }

                return res;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(WildMask), $"{nameof(WildMask)} can't be null");
                }
                else if (value.Length != _mask.Length)
                {
                    throw new ArgumentException($"The inputed value length can't be different from the Mask length ({_mask.Length})", nameof(WildMask));
                }

                string buffer;

                for (int i = 0; i < _mask.Length; i++)
                {
                    buffer = Convert.ToString(~value[i], 2);
                    buffer = buffer.Substring(sizeof(int) * OktetSize - OktetSize, OktetSize);

                    _mask[i] = Convert.ToByte(buffer, 2);
                }
            }
        }

        /// <summary>
        /// Return Wild Mask as string
        /// </summary>
        public string WildMaskString
        {
            get
            {
                string res = string.Empty;

                string buffer;

                foreach (var item in _mask)
                {
                    buffer = Convert.ToString(~item, 2);
                    buffer = buffer.Substring(sizeof(int) * OktetSize - OktetSize, OktetSize);

                    res += Convert.ToByte(buffer, 2).ToString() + ".";
                }

                return res.Substring(0, res.Length - 1);
            }
        }

        /// <summary>
        /// Return Network Class
        /// </summary>
        public string NetworkClass 
        { 
            get
            {
                string buffer = Convert.ToString(_ip[0], 2).PadLeft(OktetSize, '0');

                if (buffer.StartsWith("0"))
                {
                    return "A";
                }
                else if (buffer.StartsWith("10"))
                {
                    return "B";
                }
                else if (buffer.StartsWith("110"))
                {
                    return "C";
                }
                else if (buffer.StartsWith("1110"))
                {
                    return "D";
                }
                else if (buffer.StartsWith("1111"))
                {
                    return "E";
                }
                else
                {
                    return "Unknown class";
                }

            }
        }

        /// <summary>
        /// Return Broadcast Address as string
        /// </summary>
        public string Broadcast 
        { 
            get
            {
                string res = string.Empty;

                byte[] wildMask = WildMask;

                byte[] ip = _ip;

                for (int i = 0; i < ip.Length; i++)
                {
                    res += (ip[i] | wildMask[i]) + ".";
                }

                return res.Substring(0, res.Length - 1);
            }

        }

        /// <summary>
        /// Return Network Address as string
        /// </summary>
        public string Address
        {
            get
            {
                string res = string.Empty;

                for (int i = 0; i < _ip.Length; i++)
                {
                    res += (_ip[i] & _mask[i]) + ".";
                }

                return res.Substring(0, res.Length - 1);
            }
        }

        /// <summary>
        /// Return available Hosts Amount
        /// </summary>
        public uint HostsAmount => (uint)(Math.Pow(2, DigitsAmountInIP - BitMask) - 2);

        /// <summary>
        /// Return First Network Address as string
        /// </summary>
        public string FirstAddress
        {
            get
            {
                byte[] bufferIP = _ip;

                IPString = Address;

                IPInt += 1;

                string res = IPString;

                _ip = bufferIP;

                return res;
            }
        }

        /// <summary>
        /// Return Last Network Address as string
        /// </summary>
        public string LastAddress
        {
            get
            {
                
                byte[] bufferIP = _ip;

                IPString = Broadcast;

                IPInt -= 1;

                string res = IPString;

                _ip = bufferIP;

                return res;
            }
        }

        /// <summary>
        /// Return IP as int (if IP Version is 4) <br/>
        /// Otherside - 0;
        /// </summary>
        public int IPInt
        {
            get
            {
                if (_isIPv6)
                {
                    return 0;
                }

                string fullIP_d = string.Empty;

                foreach (var item in IP)
                {
                    fullIP_d += Convert.ToString(item, 2).PadLeft(OktetSize, '0');
                }

                return Convert.ToInt32(fullIP_d, 2);
            }

            set
            {
                string buffer = Convert.ToString(value, 2).PadLeft(DigitsAmountInIP, '0');

                for (int i = 0; i < buffer.Length; i += OktetSize)
                {
                    IP[i / OktetSize] = Convert.ToByte(buffer.Substring(i, OktetSize), 2);
                }
            }
        }

        /// <summary>
        /// IP Address as string with spaces
        /// </summary>
        public string IPStringWithSpaces
        {
            get
            {
                string res = string.Empty;

                foreach (var item in _ip)
                {
                    res += item.ToString().PadRight(3, ' ') + ".";
                }

                return res.Substring(0, res.Length - 1);
            }
        }

        #endregion
    }
}