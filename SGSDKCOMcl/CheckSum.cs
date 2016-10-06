using System;
using System.Text;

namespace Utileria
{
    /// <summary>
    /// Descripción breve de CheckSum.
    /// </summary>
    public class CheckSum
    {
        public char ChkSum(string MyStringIn, string MyStringOut)
        {
            long s, r;
            int c, n;
            char chk;
            byte[] ss;

            ASCIIEncoding AE = new ASCIIEncoding();
            s = 0; r = 0; n = 0; c = 2;                 //inicializa variables

            if (MyStringIn != null)
            {
                ss = AE.GetBytes(MyStringIn);
                n = ss.Length - 1;
            }
            else
            {
                ss = AE.GetBytes(MyStringOut);
                n = ss.Length;
            }

            for (int k = 0; k < n; k++)                 //Solo la cadena de caracteres valida
            {
                chk = (char)ss[k];
                r = ss[k] * c;					        //el valor de la posición por el contador de posición
                s = s + r;                              //se suma los valores
                c++;                                    //se incrementa la posición
                if (c == 9) c = 2;                      //9 es el máximo contador de posición
            }
            r = (s * 10) % 11;                          // mod 11  residuo			
            r += 48;                                    //solo caracteres visibles
            chk = (char)r;                              //checksum como unsigned char
            return (chk);
        }
    }

    public class Crc16 {
        const ushort polynomial = 0xA001;
        ushort[] table = new ushort[256];

        public ushort ComputeChecksum(byte[] bytes) {
            ushort crc = 0;
            for(int i = 0; i < bytes.Length; ++i) {
                byte index = (byte)(crc ^ bytes[i]);
                crc = (ushort)((crc >> 8) ^ table[index]);
                //Console.WriteLine("({0}) ({1}), ({2})", bytes[i].ToString(),index.ToString(),crc.ToString());
            }
            return crc;
        }

        public byte[] ComputeChecksumBytes(byte[] bytes) {
            ushort crc = ComputeChecksum(bytes);
            return BitConverter.GetBytes(crc);
        }

        public Crc16() {
            ushort value;
            ushort temp;
            for(ushort i = 0; i < table.Length; ++i) {
                value = 0;
                temp = i;
                for(byte j = 0; j < 8; ++j) {
                    if(((value ^ temp) & 0x0001) != 0) {
                        value = (ushort)((value >> 1) ^ polynomial);
                    }else {
                        value >>= 1;
                    }
                    temp >>= 1;
                }
                table[i] = value;
            }
        }
    }
}