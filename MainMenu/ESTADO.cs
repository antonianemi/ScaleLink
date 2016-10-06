using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainMenu
{
    public class ESTADO
    {
        #region enums
        public enum botonesEdicionEnum
        {        
            PKGRUPOS = 0,
            PKBASCULAS,
            PKPRODUCTOS,
            PKNUTRICION,
            PKINGREDIENTE,
            PKPUBLICIDAD,
            PKOFERTA,
            PKVENDEDORES,
            PKIMPORT,
            PKEXPORT,
            PKPURGAR,
            PKCOMPACTAR,
            PKBACKUP,
            PKUSER,
            PKCONFIG,
            PKGENERAL,
            PKSTADITICO,
            PKVENTAS,
            PKIPAD,
            PKSCALE
        };

        public enum botonesVistasEnum
        {
            PKNINGUNO = 0,
            PKINICIO,
            PKCATALOGOS,
            PKCARPETAS,   //  PKPRODUCTOS,
            PKPRECIOS,    // PKVENDEDORES, // PKGRUPOS,
            PKCONFIGURACIONES,
            PKSINCRONIZACION,
            PKESTADISTICAS,
            PKHERRAMIENTAS,
            PKMANTENIMIENTO,
            PKAYUDA,
            PKSALIR      //    PKPUBLICIDAD,     //   PKOFERTA,     //  PKINGREDIENTE,     //  PKNUTRICIONAL,
        };


        public enum tipoConexionesEnum
        {
            PKWIFI = 0,
            //PKETHERNET,            
            PKUSBCOM,
        };

        public enum EstadoRegistro
        {
            PKTRATADO = 0,
            PKNOTRATADO,
            PKPARCIAL
        };

        public enum botonesEnvioDato
        {
            SDPRODUCTO = 0,
            SDPUBLICIDAD,
            SDOFERTA,
            SDVENDEDOR,
            SDINGREDIENTE,
            SDNUTRICIONAL,
            SDIMAGEN,
            SDCARPETA
        };

        public enum FileSource
        {
            fPrecios = 0,
            fProductos,
            fInfoAdicional,
            fOfertas,
            fMensajes,
            fVendedores,
            fcarpetas
        };

        #endregion
        

    }
}
