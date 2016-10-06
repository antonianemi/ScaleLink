using System;
using System.Text;

namespace Validaciones
{
    /// <summary>
    /// Descripción breve de CheckSum.
    /// </summary>
    public class CheckSumCommand
    {
        public char ChkSum(string MyStringIn, string MyStringOut)
        {
            long s, r;
            int c, n;
            char chk;
            byte[] ss;

            s = 0; r = 0; n = 0; c = 2;                 //inicializa variables

            if (MyStringIn != null)
            {
                ss = Encoding.GetEncoding(437).GetBytes(MyStringIn);
                n = ss.Length - 1;
            }
            else
            {
                ss = Encoding.GetEncoding(437).GetBytes(MyStringOut);
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

    public class CheckSum
    {
        public char ChkSum(string MyString)
        {
            long s, r;
            int c, n;
            char chk;

            byte[] ss = Encoding.GetEncoding(437).GetBytes(MyString);
            s = 0; r = 0; n = 0; c = 2;                 //inicializa variables
            n = MyString.LastIndexOf((char)10) + 1; 

            for (int k = 0; k < n; k++)                 //Solo la cadena de caracteres valida
            {
                chk = (char)ss[k];
                r = chk * c;					        //el valor de la posición por el contador de posición
                s = s + r;                              //se suma los valores
                c++;                                    //se incrementa la posición
                if (c == 9) c = 2;                      //9 es el máximo contador de posición
                //Console.WriteLine("k = {0}, r = {1}, c = {2}, chk = {3}", k, r, c, (int)chk);
            }
            r = (s * 10) % 11;                          // mod 11  residuo			
            r += 48;                                    //solo caracteres visibles
            
            chk = (char)r;                              //checksum como unsigned char
            return (chk);
        }

        public char ChkSumBytes(byte[] ss)
        {
            long s, r;
            int c, n;
            char chk;

            s = 0; r = 0; n = 0; c = 2;                 //inicializa variables
            n = ss.Length;

            for (int k = 0; k < n; k++)                 //Solo la cadena de caracteres valida
            {
                chk = (char)ss[k];
                r = chk * c;					        //el valor de la posición por el contador de posición
                s = s + r;                              //se suma los valores
                c++;                                    //se incrementa la posición
                if (c == 9) c = 2;                      //9 es el máximo contador de posición
                //Console.WriteLine("k = {0}, r = {1}, c = {2}, chk = {3}", k, r, c, (byte)chk);
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

        public ushort iComputeCrc16(byte[] bytes, int iLenght)
        {
            ushort crc = 0;
            for (int i = 0; i < (iLenght); ++i)
            {
                byte index = (byte)(crc ^ bytes[i]);
                crc = (ushort)((crc >> 8) ^ table[index]);
            }
            return crc;
        }

        public int iCompareCrc16(byte[] image)
        {
            ushort iCrcInImage;
            byte[] crc16in = new byte[4];
            int fileSize = image.Length;

            crc16in[0] = image[fileSize - 4];
            crc16in[1] = image[fileSize - 3];
            crc16in[2] = image[fileSize - 2];
            crc16in[3] = image[fileSize - 1];

            iCrcInImage = iComputeCrc16(image, image.Length-4);
            Console.WriteLine("Crc16 enviado en imagen: {0}", Encoding.Default.GetString(crc16in));

            string sCrc16 = iCrcInImage.ToString("X4");

            if (string.Compare(Encoding.Default.GetString(crc16in), sCrc16) != 0)
            {
                Console.WriteLine("Error en CRC calculado: {0}", sCrc16);
                return 1;
            }

            return 0;
        }
    }
}