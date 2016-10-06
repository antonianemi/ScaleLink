using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainMenu
{
    public enum DatoCadena{
        iId,
        sCodigo,
        iNoPlu,
        sNombre,
        dPrecio,
        sImagen,
        iTipoId,
        iPrecioEditable,
        iCaducidadDias,
        dImpuesto,
        iInfoNutriId,
        iInfoAddId,
        sFechaUit,
        dTara,
        iMultiplo,
        iPublicidad1Id,
        iPublicidad2Id,
        iPublicidad3Id,
        iPublicidad4Id,
        iOferta,
        iGrupo,
        cTypeGrupo,
        ibImagen,
        iCarpetaId,
        cChecksum
    };

    public enum DatoPublicidad
    {
        iId,
        sTitulo,            
        sMensaje,
        cChecksum
    }

    public enum DatoIngredientes
    {
        iId,
        sNombre,
        sInformacion,
        cChecksum
    }

    public enum DatoPrecio
    {
        iIdSuc,
        IdProd,
        dPrecio,
        cChecksum
    }

    public enum DatoOferta
    {
        iId,
        sNombre,
        sFeIncio,
        sFeFinal,
        iTipoDesc,
        dDescuento,
        iVentas,
        cChecksum
    }

    public enum DatoLeerProducto
    {
        idproduct,
        idTipoId,
        cTipoId,
        cChecksum
    }

    public enum DatoVendedor
    {
        iId,
        sNombre,
        iVtaEnable,
        iMsjEnable,
        dMetaVenta,
        iPublicidad1Id,
        iPublicidad2Id,
        cChecksum
    }

    public enum DatoLeerCarpeta
    {
        idGrupo,
        cTipo,
        iIdCarpeta,
        cChecksum
    }

    public class ValidateData
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataToSave"></param>
        /// <param name="mystruct"></param>
        /// <returns></returns>
        public int iValidateCreateNewProductFromIpad(string[] DataToSave, ref f6Sincronizacion.DataProductFromIpad mystruct)
        {
            int iIndexData = 0;
            int iResultFunct = 0;

            if (DataToSave.Length - 1 != Enum.GetValues(typeof(DatoCadena)).Length)
            {
                Console.WriteLine("Error numero de datos diferente al esperado");
                return 1;
            }

            for (iIndexData = 0; iIndexData < DataToSave.Length - 1; iIndexData++)
            {
                switch (iIndexData)
                {
                    case (int)DatoCadena.iId:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iId = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.sCodigo:
                        iResultFunct = iValidateString(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.sCodigo = DataToSave[iIndexData];
                        }
                        break;
                    case (int)DatoCadena.iNoPlu:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iNoPlu = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.sNombre:
                        iResultFunct = iValidateString(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.sNombre = DataToSave[iIndexData];
                        }
                        break;
                    case (int)DatoCadena.dPrecio:
                        iResultFunct = iValidateDouble(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.dPrecio = Convert.ToDouble(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.sImagen:
                        iResultFunct = iValidateString(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.sImagen = DataToSave[iIndexData];
                        }
                        break;
                    case (int)DatoCadena.iTipoId:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iTipoId = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.iPrecioEditable:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iPrecioEditable = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.iCaducidadDias:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iCaducidadDias = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.dImpuesto:
                        iResultFunct = iValidateDouble(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.dImpuesto = Convert.ToDouble(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.iInfoNutriId:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iInfoNutriId = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.iInfoAddId:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.infoAddId = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.sFechaUit:
                        iResultFunct = iValidateString(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.sFechaUit = DataToSave[iIndexData];
                        }
                        break;
                    case (int)DatoCadena.dTara:
                        iResultFunct = iValidateDouble(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.dTara = Convert.ToDouble(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.iMultiplo:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iMultiplo = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.iPublicidad1Id:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iPublicidad1Id = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.iPublicidad2Id:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iPublicidad2Id = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.iPublicidad3Id:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iPublicidad3Id = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.iPublicidad4Id:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iPublicidad4Id = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.iOferta:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iOferta = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.iGrupo:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iGrupo = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.cTypeGrupo:
                        iResultFunct = iValidateString(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.cTypeGrupo = Convert.ToChar(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.ibImagen:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.ibImagen = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoCadena.iCarpetaId:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iCarpetaId = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                }

                if (iResultFunct != 0)
                {
                    Console.WriteLine("Error dato: {0}, difiere en tipo de variable esperada", Enum.GetName(typeof(DatoCadena), iIndexData));
                    return iIndexData;
                }
            }
            
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataToSave"></param>
        /// <param name="mystruct"></param>
        /// <returns></returns>
        public int iValidateGM(string[] DataToSave, ref f6Sincronizacion.DataPublicidadFromIpad mystruct)
        {
            int iIndexData = 0;
            int iResultFunct = 0;

            if (DataToSave.Length - 1 != Enum.GetValues(typeof(DatoPublicidad)).Length)
            {
                Console.WriteLine("Error numero de datos diferente al esperado");
                return 1;
            }

            for (iIndexData = 0; iIndexData < DataToSave.Length - 1; iIndexData++)
            {
                switch (iIndexData)
                {
                    case (int)DatoPublicidad.iId:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iId = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoPublicidad.sTitulo:
                        iResultFunct = iValidateString(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.sTitulo = DataToSave[iIndexData];
                        }
                        break;
                    case (int)DatoPublicidad.sMensaje:
                        iResultFunct = iValidateString(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.sMensaje = DataToSave[iIndexData];
                        }
                        break;
                }

                if (iResultFunct != 0)
                {
                    Console.WriteLine("Error dato: {0}, difiere en tipo de variable esperada", Enum.GetName(typeof(DatoPublicidad), iIndexData));
                    return iIndexData;
                }
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataToSave"></param>
        /// <param name="mystruct"></param>
        /// <returns></returns>
        public int iValidateGI(string[] DataToSave, ref f6Sincronizacion.DataIngredienteFromIpad mystruct)
        {
            int iIndexData = 0;
            int iResultFunct = 0;

            if (DataToSave.Length - 1 != Enum.GetValues(typeof(DatoIngredientes)).Length)
            {
                Console.WriteLine("Error numero de datos diferente al esperado");
                return 1;
            }

            for (iIndexData = 0; iIndexData < DataToSave.Length - 1; iIndexData++)
            {
                switch (iIndexData)
                {
                    case (int)DatoIngredientes.iId:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iId = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoIngredientes.sNombre:
                        iResultFunct = iValidateString(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.sNombre = DataToSave[iIndexData];
                        }
                        break;
                    case (int)DatoIngredientes.sInformacion:
                        iResultFunct = iValidateString(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.sInformacion = DataToSave[iIndexData];
                        }
                        break;
                }

                if (iResultFunct != 0)
                {
                    Console.WriteLine("Error dato: {0}, difiere en tipo de variable esperada", Enum.GetName(typeof(DatoIngredientes), iIndexData));
                    return iIndexData;
                }
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataToSave"></param>
        /// <param name="mystruct"></param>
        /// <returns></returns>
        public int iValidateGp(string[] DataToSave, ref f6Sincronizacion.DataPrecioFromIpad mystruct)
        {
            int iIndexData = 0;
            int iResultFunct = 0;

            if (DataToSave.Length - 1 != Enum.GetValues(typeof(DatoPrecio)).Length)
            {
                Console.WriteLine("Error numero de datos diferente al esperado");
                return 1;
            }

            for (iIndexData = 0; iIndexData < DataToSave.Length - 1; iIndexData++)
            {
                switch (iIndexData)
                {
                    case (int)DatoPrecio.iIdSuc:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iIdSuc = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoPrecio.IdProd:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iIdProd = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoPrecio.dPrecio:
                        iResultFunct = iValidateDouble(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.dPrecio = Convert.ToDouble(DataToSave[iIndexData]);
                        }
                        break;
                }

                if (iResultFunct != 0)
                {
                    Console.WriteLine("Error dato: {0}, difiere en tipo de variable esperada", Enum.GetName(typeof(DatoPrecio), iIndexData));
                    return iIndexData;
                }
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataToSave"></param>
        /// <param name="mystruct"></param>
        /// <returns></returns>
        public int iValidateGO(string[] DataToSave, ref f6Sincronizacion.DataOfertaFromIpad mystruct)
        {
            int iIndexData = 0;
            int iResultFunct = 0;

            if (DataToSave.Length - 1 != Enum.GetValues(typeof(DatoOferta)).Length)
            {
                Console.WriteLine("Error numero de datos diferente al esperado");
                return 1;
            }

            for (iIndexData = 0; iIndexData < DataToSave.Length - 1; iIndexData++)
            {
                switch (iIndexData)
                {
                    case (int)DatoOferta.iId:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iId = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoOferta.sNombre:
                        iResultFunct = iValidateString(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.sNombre = DataToSave[iIndexData];
                        }
                        break;
                    case (int)DatoOferta.sFeIncio:
                        iResultFunct = iValidateString(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.sFeIncio = DataToSave[iIndexData];
                        }
                        break;

                    case (int)DatoOferta.sFeFinal:
                        iResultFunct = iValidateString(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.sFeFinal = DataToSave[iIndexData];
                        }
                        break;
                    case (int)DatoOferta.iTipoDesc:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iTipoDesc = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoOferta.dDescuento:
                        iResultFunct = iValidateDouble(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.dDescuento = Convert.ToDouble(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoOferta.iVentas:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iVentas = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                }
                
                if (iResultFunct != 0)
                {
                    Console.WriteLine("Error dato: {0}, difiere en tipo de variable esperada", Enum.GetName(typeof(DatoOferta), iIndexData));
                    return iIndexData;
                }
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataToSave"></param>
        /// <param name="mystruct"></param>
        /// <returns></returns>
        public int iValidateLP(string[] DataToSave, ref f6Sincronizacion.DataLeerProductoFromIpad mystruct)
        {
            int iIndexData = 0;
            int iResultFunct = 0;

            if (DataToSave.Length - 1 != Enum.GetValues(typeof(DatoLeerProducto)).Length)
            {
                Console.WriteLine("Error numero de datos diferente al esperado");
                return 1;
            }

            for (iIndexData = 0; iIndexData < DataToSave.Length - 1; iIndexData++)
            {
                switch (iIndexData)
                {
                    case (int)DatoLeerProducto.idproduct:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.idproduct = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoLeerProducto.idTipoId:
                        iResultFunct = iValidateString(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.idTipoId = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoLeerProducto.cTipoId:
                        iResultFunct = iValidateChar(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.cTipoId =  Convert.ToChar(DataToSave[iIndexData]);
                        }
                        break;
                }

                if (iResultFunct != 0)
                {
                    Console.WriteLine("Error dato: {0}, difiere en tipo de variable esperada", Enum.GetName(typeof(DatoLeerProducto), iIndexData));
                    return iIndexData;
                }
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataToSave"></param>
        /// <param name="mystruct"></param>
        /// <returns></returns>
        public int iValidateGV(string[] DataToSave, ref f6Sincronizacion.DataVendedorFromIpad mystruct)
        {
            int iIndexData = 0;
            int iResultFunct = 0;

            if (DataToSave.Length - 1 != Enum.GetValues(typeof(DatoVendedor)).Length)
            {
                Console.WriteLine("Error numero de datos diferente al esperado");
                return 1;
            }

            for (iIndexData = 0; iIndexData < DataToSave.Length - 1; iIndexData++)
            {
                switch (iIndexData)
                {
                    case (int)DatoVendedor.iId:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iId = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoVendedor.sNombre:
                        iResultFunct = iValidateString(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.sNombre = DataToSave[iIndexData];
                        }
                        break;
                    case (int)DatoVendedor.iVtaEnable:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iVtaEnable = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoVendedor.iMsjEnable:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iMsjEnable = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoVendedor.dMetaVenta:
                        iResultFunct = iValidateDouble(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.dMetaVenta = Convert.ToDouble(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoVendedor.iPublicidad1Id:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iPublicidad1Id = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoVendedor.iPublicidad2Id:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iPublicidad2Id = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                }

                if (iResultFunct != 0)
                {
                    Console.WriteLine("Error dato: {0}, difiere en tipo de variable esperada", Enum.GetName(typeof(DatoVendedor), iIndexData));
                    return iIndexData;
                }
            }

            return 0;
        }

        public int iValidateLC(string[] DataToSave, ref f6Sincronizacion.DataLeerCarpetaFromIpad mystruct)
        {
            int iIndexData = 0;
            int iResultFunct = 0;

            if (DataToSave.Length - 1 != Enum.GetValues(typeof(DatoLeerCarpeta)).Length)
            {
                Console.WriteLine("Error numero de datos diferente al esperado");
                return 1;
            }

            for (iIndexData = 0; iIndexData < DataToSave.Length - 1; iIndexData++)
            {
                switch (iIndexData)
                {
                    case (int)DatoLeerCarpeta.idGrupo:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.idGrupo = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoLeerCarpeta.cTipo:
                        iResultFunct = iValidateChar(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.cTipo = Convert.ToChar(DataToSave[iIndexData]);
                        }
                        break;
                    case (int)DatoLeerCarpeta.iIdCarpeta:
                        iResultFunct = iValidateInt(DataToSave[iIndexData]);
                        if (iResultFunct == 0)
                        {
                            mystruct.iIdCarpeta = Convert.ToInt32(DataToSave[iIndexData]);
                        }
                        break;
                }

                if (iResultFunct != 0)
                {
                    Console.WriteLine("Error dato: {0}, difiere en tipo de variable esperada", Enum.GetName(typeof(DatoLeerCarpeta), iIndexData));
                    return iIndexData;
                }
            }

            return 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringInt"></param>
        /// <returns></returns>
        public int iValidateInt(string stringInt)
        {
            int i=0;

            for (i = 0; i < stringInt.Length; i++)
            {
                if (stringInt[i] < '0' || stringInt[i] > '9')
                {
                    return 1;
                }
            }

            if (i > 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringInt"></param>
        /// <returns></returns>
        public int iValidateDouble(string stringInt)
        {
            int iCountDecimal = 0;
            int i;

            for (i = 0; i < stringInt.Length; i++)
            {
                if (stringInt[i] < '0' || stringInt[i] > '9')
                {
                    if (stringInt[i] == '.' && iCountDecimal == 0)
                    {
                        iCountDecimal++;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }

            if (i > 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Texto"></param>
        /// <returns></returns>
        public int iValidateString(string Texto)
        {
            int i;

            for (i = 0; i < Texto.Length; i++)
            {
                if (Texto[i] >= '0' && Texto[i] <= '9')
                {

                }
                else if (Texto[i] >= 'A' && Texto[i] <= 'Z')
                {

                }
                else if (Texto[i] >= 'a' && Texto[i] <= 'z')
                {

                }
                else if (Texto[i] >= ' ')
                {
                
                }
                else
                {
                    return 1;
                }                
            }

            if (i > 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Texto"></param>
        /// <returns></returns>
        public int iValidateChar(string Texto)
        {
            

            if (Texto.Length == 1)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
