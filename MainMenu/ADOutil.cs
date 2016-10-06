using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.ComponentModel;
using System.IO;
using System.Globalization;
using System.Diagnostics;
using System.Windows.Forms;
using System.Deployment;
using MainMenu.Properties;
using System.Threading;
using MainMenu.BaseDeDatosDataSetTableAdapters;

namespace MainMenu
{
	/// <summary>
	/// Descripción breve de ADOutil.
	/// </summary>
	///  
    public class ADOutil
	{        
        Settings str_conect = new Settings();
		public OleDbConnection dbConnection;
		public DataTable dbDataTable;
		public DataSet dbDataSet;
		public OleDbDataAdapter dbDataAdapter;
        public BaseDeDatosDataSet DeDatosDataSet;
		public string ArchivoDatos;
		public string CadenaConexion;
		public string CadenaSelect;
        public string CadenaSelect2;
		public string NombreTabla;
		public string Condicion;
        		
		public ADOutil()
		{
            CadenaConexion = str_conect.bdConexionString;        
        }

		public void ConectarDB(string nombreBaseDatos, string commandString, string nom_tabla,string conect)
		{
			try
			{
                if (CadenaSelect == "") { CadenaSelect = "SELECT * FROM " + nom_tabla; }

				if (validar_database(ref nombreBaseDatos, ref commandString, ref nom_tabla,ref conect))
				{
					dbConnection.Open();
					dbDataSet = new DataSet();
					dbDataAdapter = new OleDbDataAdapter(CadenaSelect, dbConnection);
                	OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(dbDataAdapter);
		        
					dbDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
					dbDataAdapter.Fill(dbDataSet, nom_tabla);
				}
				return;
			}
			catch (System.Data.OleDb.OleDbException ex)
			{
				ShowErrors(ex);
				return;
			}
		}

        public void ConectarDB(string commandString, string nom_tabla, string conect)
        {
            try
            {
                if (commandString == "") { CadenaSelect = "SELECT * FROM " + nom_tabla; }

                if (conect == "") { conect = CadenaConexion; }

                CadenaConexion = conect;
                dbConnection = new OleDbConnection(CadenaConexion);
                dbConnection.Open();
            }
            catch (OleDbException ex)
            {
                ShowErrors(ex);
                return;
            }
            catch (System.PlatformNotSupportedException explat)
            {
                MessageBox.Show(explat.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error:" + e.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

		public void Desconectar()
		{
			dbDataAdapter.Dispose();
			dbDataSet.Clear();
			dbDataSet.Dispose();
			dbConnection.Close();
			dbConnection.Dispose();
        }

		public void Actualizar(DataSet dbDataSet, string nombreBaseDatos, string commandString, string nom_tabla,string conect)
		{
			if (validar_database(ref nombreBaseDatos, ref commandString, ref nom_tabla,ref conect))
			{
				dbDataAdapter = new OleDbDataAdapter(CadenaSelect, dbConnection);
				dbDataAdapter.UpdateCommand = new OleDbCommand(CadenaSelect,dbConnection);
				OleDbCommandBuilder commanBuilder = new OleDbCommandBuilder(dbDataAdapter);
				commanBuilder.RefreshSchema();
				dbDataAdapter.Update(dbDataSet,NombreTabla);
			}
			return;
		}

		public void Borrar(DataSet dbDataSet, string nombreBaseDatos, string commandString, string nom_tabla,string conect)
		{
            if (validar_database(ref nombreBaseDatos, ref commandString, ref nom_tabla, ref conect))
			{
				dbDataAdapter = new OleDbDataAdapter(CadenaSelect, dbConnection);
				dbDataAdapter.DeleteCommand = new OleDbCommand(CadenaSelect,dbConnection);
				OleDbCommandBuilder commanBuilder = new OleDbCommandBuilder(dbDataAdapter);
				commanBuilder.RefreshSchema();

				dbDataAdapter.Update(dbDataSet,NombreTabla);				
			}
			return;
		}

		public void Nuevo(DataSet dbDataSet, string nombreBaseDatos, string commandString, string nom_tabla,string conect)
		{
			if (validar_database(ref nombreBaseDatos, ref commandString, ref nom_tabla,ref conect))
			{
				dbDataAdapter = new OleDbDataAdapter(CadenaSelect, dbConnection);
				dbDataAdapter.InsertCommand = new OleDbCommand(CadenaSelect,dbConnection);
				OleDbCommandBuilder commanBuilder = new OleDbCommandBuilder(dbDataAdapter);
				commanBuilder.RefreshSchema();
				dbDataAdapter.Update(dbDataSet,NombreTabla);
			}
			return;
		}
        		
		public bool validar_database(ref string nombreBaseDatos,ref string commandString, ref string nom_tabla,ref string conect)
		{
			if (nombreBaseDatos == "")
			{
				nombreBaseDatos = ArchivoDatos;
			}
			ArchivoDatos = nombreBaseDatos;

			if (ArchivoDatos == "")
			{
				return false;
			}
			if (commandString == "")
			{
				commandString = CadenaSelect;
			}
			CadenaSelect = commandString;

			if (CadenaSelect == "")
			{
				CadenaSelect = "SELECT * FROM " + nom_tabla;
			}
            if (conect == "")
            {
                conect = CadenaConexion;
            }
            CadenaConexion = conect;

			try
			{
				dbConnection = new OleDbConnection(CadenaConexion);
			}
			catch(System.PlatformNotSupportedException explat)
			{
                MessageBox.Show(explat.Message, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception e)
			{
                MessageBox.Show("Error:" + e.Message, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			return true;
		}

		public void ShowErrors(System.Data.OleDb.OleDbException e)
		{
		
			System.Data.OleDb.OleDbErrorCollection errorCollection = e.Errors;
			
			System.Text.StringBuilder bld = new System.Text.StringBuilder();
			Exception inner = e.InnerException;

			if (null != inner)
			{
				MessageBox.Show("Inner Exception: " + inner.ToString(),"",MessageBoxButtons.OK,MessageBoxIcon.Error);
               
			}
			// Enumerate the errors to a message box.
			foreach (System.Data.OleDb.OleDbError err in errorCollection) 
			{				
				if (err.SQLState == "3022") bld.Append("Ya existe el registro..");
				else bld.Append(err.Message);
                MessageBox.Show(bld.ToString(), Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);				
				bld.Remove(0, bld.Length);
			}
		}
        			
		public string[] NombresTablas()
		{
			string[] nomTablas = new String[0];
			DataTable dataTable;
			System.DBNull dbNull=null;

			//' En el ejemplo usado anteriomente indicaba "TABLES" en lugar de "TABLE"

			object[] restrictions = new object[]{dbNull, dbNull, dbNull, "TABLE"};
			int i;
			
			if (dbConnection.State != ConnectionState.Connecting)
			{
				dbConnection = new OleDbConnection(CadenaConexion);
			}

			if (dbConnection.State != ConnectionState.Open)
			{
				dbConnection.Open();
			}
			
			dataTable = dbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions);
			i = dataTable.Rows.Count - 1;

			if (i > -1)
			{
				nomTablas = new string[i];
				for (i = 0; i <= (dataTable.Rows.Count - 1); i++)
				{
					nomTablas[i] = dataTable.Rows[i].ItemArray.ToString();
				}
			}
			
			return nomTablas;
		}
		
		public string[] NombresColumnas()
		{
			DataColumn columna;
			int i, j;
			string[] nomCol=new string[0];
			
			j = dbDataSet.Tables[NombreTabla].Columns.Count - 1;
			
			nomCol= new string[j];
			for( i = 0; i< j; i++)
			{
				columna = dbDataSet.Tables[NombreTabla].Columns[i];
				nomCol[i] = columna.ColumnName;
			}
			return(nomCol);
		}

        #region Proceso de ABC y busqueda en la DB por READER
        public void ActualizaReader(string CadenaConexion,string CadenaSelect,string Nombretabla)
		{
			OleDbConnection objconexion;
			OleDbCommand Actualizacomando;
            
			try
			{
				objconexion = new OleDbConnection();
                objconexion.ConnectionString = str_conect.bdConexionString;
				objconexion.Open();
                
				Actualizacomando = new OleDbCommand();
				Actualizacomando.Connection = objconexion;
				Actualizacomando.CommandText = CadenaSelect;
				
				Actualizacomando.ExecuteNonQuery();
				objconexion.Close();
			}
			catch (OleDbException ex)
			{
				ShowErrors(ex);
			}
			catch(System.PlatformNotSupportedException explat)
			{
                MessageBox.Show(explat.Message, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception objException)
			{                
				Console.WriteLine("Actualizar:", objException.Message);
				throw objException;
			}
		}
		
		public void EliminarReader(string CadenaConexion,string CadenaSelect,string Nombretabla)
		{
			OleDbConnection objconexion;
			OleDbCommand eliminacomando;
            
			try
			{
				objconexion = new OleDbConnection();
                objconexion.ConnectionString = str_conect.bdConexionString; 
                
				objconexion.Open();                
				eliminacomando = new OleDbCommand();
				eliminacomando.Connection = objconexion;
				eliminacomando.CommandText = CadenaSelect;
               	eliminacomando.ExecuteNonQuery();
				objconexion.Close();
                
			}			
			catch (OleDbException ex)
			{
				ShowErrors(ex);
			}
			catch(System.PlatformNotSupportedException explat)
			{
                MessageBox.Show(explat.Message, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception objException)
			{
                Console.WriteLine("Eliminar:", objException.Message);
				//throw objException;
			}
		}
		
		public void InsertarReader(string CadeConexion,string CadeSelect,string Nomtabla)
		{                        
			OleDbConnection objconexion=null;
		    OleDbCommand Insertacomando;

			try
			{
				objconexion = new OleDbConnection();
                objconexion.ConnectionString = str_conect.bdConexionString; 
                if (objconexion.State == System.Data.ConnectionState.Open) { objconexion.Close(); }
				objconexion.Open();
				Insertacomando = new OleDbCommand();
				Insertacomando.Connection =objconexion; 
				Insertacomando.CommandText = CadeSelect;
				Insertacomando.ExecuteNonQuery();
				objconexion.Close();
				objconexion.Dispose();
			}   
			catch (OleDbException ex)
			{
				ShowErrors(ex);
			}
			catch(System.PlatformNotSupportedException explat)
			{
                MessageBox.Show(explat.Message, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception objException)
			{
                Console.WriteLine("Insertar:", objException.Message);
				throw objException;
			}
		}

        public OleDbDataReader Obtiene_Dato(string Select, string Conexion)
        {
            OleDbConnection objconexion;
            OleDbCommand objcomando;
            OleDbDataReader objdatareader;

            try
            {
                objconexion = new OleDbConnection();
                objconexion.ConnectionString = str_conect.bdConexionString;

                if (objconexion.State == System.Data.ConnectionState.Open) { objconexion.Close(); }
                objconexion.Open();
                objcomando = new OleDbCommand();
                objcomando.Connection = objconexion;
                objcomando.CommandText = Select;
                objdatareader = objcomando.ExecuteReader();
                return objdatareader;
            }
            catch (System.Data.OleDb.OleDbException myException)
            {
                ShowErrors(myException);

               /* for (int i = 0; i < myException.Errors.Count; i++)
                {
                    MessageBox.Show("Index #" + i + "\n" +
                        "Message: " + myException.Errors[i].Message + "\n" +
                        "Native: " + myException.Errors[i].NativeError.ToString() + "\n" +
                        "Source: " + myException.Errors[i].Source + "\n" +
                        "SQL: " + myException.Errors[i].SQLState + "\n", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }*/
                return null;
            }
            catch (System.PlatformNotSupportedException explat)
            {
                MessageBox.Show(explat.Message, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception objExeption)
            {
                Console.WriteLine("ClaseGeneral", objExeption.Message);
                throw objExeption;
            }
        }
        #endregion

        #region Proceso de ABC y busqueda por DATASET en la DB
        public int New(ref CurrencyManager cmRegister)
		{  	
			DataSet DSChanges = dbDataSet.GetChanges(DataRowState.Added);
		
			if (DSChanges != null)
			{
                try
                { InsertarReader(CadenaConexion, CadenaSelect, NombreTabla); }
                catch (System.PlatformNotSupportedException explat)
                { MessageBox.Show(explat.Message, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error); }
                catch (Exception ex)
                { MessageBox.Show("Error:" + ex.Message, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error); }
				return(cmRegister.Count + 1);					
			}
			return(cmRegister.Position);
		}

		public int Previous(ref CurrencyManager cmRegister)
		{
            if (cmRegister.Position > 0) { return (cmRegister.Position -= 1); }
            else { return 0; }
		}

		public int Next(ref CurrencyManager cmRegister)
		{
            if (cmRegister.Position != cmRegister.Count - 1) { return (cmRegister.Position += 1); }
            else { return (cmRegister.Count - 1); }
		}

		public bool Del(ref CurrencyManager cmRegister,string filtro,string conect)
		{
            if (dbDataSet.Tables[NombreTabla].Rows.Count > 0)
            {
                DataRow[] dr = dbDataSet.Tables[NombreTabla].Select(filtro);
                if (dr.Length > 0)
                {
                    dr[0].Delete();

                    DataSet DSChanges = dbDataSet.GetChanges(DataRowState.Deleted);

                    if (DSChanges != null || dr[0].RowState == DataRowState.Detached)
                    {
                        try
                        {
                            Borrar(dbDataSet, ArchivoDatos, CadenaSelect, NombreTabla,conect);
                            return true;
                        }
                        catch (DBConcurrencyException ex)
                        {
                            string customErrorMessage;

                            customErrorMessage = ex.Message.ToString();
                            MessageBox.Show(customErrorMessage.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        catch (System.PlatformNotSupportedException explat)
                        {
                            MessageBox.Show(explat.Message, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show("Error:" + ex.Message, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    return false;
                }
                return false;
            }
            else { return false; }
		}

		public bool Save(ref CurrencyManager cmRegister, ref DataRow dr)
		{	
						
			DataSet DSChanges = dbDataSet.GetChanges(DataRowState.Modified);
					
			if (DSChanges != null || dr.RowState == DataRowState.Added)
			{
				try
				{   					
					ActualizaReader(CadenaConexion,CadenaSelect,NombreTabla);
					return true;
				}
				catch(System.PlatformNotSupportedException explat)
				{
                    MessageBox.Show(explat.Message, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				catch (Exception ex)
				{
                    MessageBox.Show("Error: " + ex.Message, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
			}		
			return false;
		}

		public bool Find(ref CurrencyManager cmRegister, string ID)
		{	int i;
			bool exit = false;
		
			for (i = 0; i <= dbDataSet.Tables[NombreTabla].Rows.Count;i++)
			{
                if (dbDataSet.Tables[NombreTabla].PrimaryKey.ToString() != ID) { cmRegister.Position += 1; }
                else
                {
                    exit = true;
                    break;
                }
			}
			return exit;
		}
        #endregion
        public void Cancel()
		{			
			MessageBox.Show(Variable.SYS_MSJ[14, Variable.idioma]); //"Cancelado");
		}		

		public int buscar_posicion(int elemento,string clave,ref DataSet dbDataSet)
		{
			int desde,hasta,medio,posicion; // desde y hasta indican los límites del array que se está mirando.
			posicion = 0;
			
			for(desde=0,hasta=dbDataSet.Tables[NombreTabla].Rows.Count-1;desde<=hasta;)
			{					
				if(desde==hasta) // si el array sólo tiene un elemento:
				{
					if(Convert.ToInt32(dbDataSet.Tables[NombreTabla].Rows[desde][clave]) == elemento) // si es la solución:
						posicion=desde; // darle el valor.
					else // si no es el valor:
						posicion=-1; // no está en el array.
					break; // Salir del bucle.
				}
				medio=(desde+hasta)/2; // Divide el array en dos.
				if(Convert.ToInt32(dbDataSet.Tables[NombreTabla].Rows[medio][clave])==elemento) // Si coincide con el central:
				{
					posicion=medio; // ese es la solución
					break; // y sale del bucle.
				}
				else if(Convert.ToInt32(dbDataSet.Tables[NombreTabla].Rows[medio][clave])>elemento) // si es menor:
					hasta=medio-1; // elige el array izquierda.
				else // y si es mayor:
					desde=medio+1; // elige el array de la derecha.
			}
			return posicion;
		}
	}

    public class ImpExp
    {
        Settings str_conect = new Settings();
        private string renglon;
        //private int n = 0;
        string[] Lista_Datos;
        private char cc = (char)9;

        private delegate void ProgressDelegate(int Number, string Message);        

        BaseDeDatosDataSet baseDeDatosDataSet = new BaseDeDatosDataSet();
        BaseDeDatosDataSetTableAdapters.ProductosTableAdapter productosTableAdapter = new BaseDeDatosDataSetTableAdapters.ProductosTableAdapter();
        BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter prod_detalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.PLU_detalleTableAdapter plu_detalleTableAdapter = new BaseDeDatosDataSetTableAdapters.PLU_detalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.IngredientesTableAdapter infoadicionalTableAdapter = new BaseDeDatosDataSetTableAdapters.IngredientesTableAdapter();
        BaseDeDatosDataSetTableAdapters.Ingre_detalleTableAdapter infoadicional_detalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Ingre_detalleTableAdapter();     
        BaseDeDatosDataSetTableAdapters.OfertaTableAdapter ofertasTableAdapter = new BaseDeDatosDataSetTableAdapters.OfertaTableAdapter();
        BaseDeDatosDataSetTableAdapters.Oferta_DetalleTableAdapter ofertas_detalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Oferta_DetalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.PublicidadTableAdapter publicidadTableAdapter = new BaseDeDatosDataSetTableAdapters.PublicidadTableAdapter();
        BaseDeDatosDataSetTableAdapters.Public_DetalleTableAdapter public_detalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Public_DetalleTableAdapter();       
        BaseDeDatosDataSetTableAdapters.VendedorTableAdapter vendedorTableAdapter = new BaseDeDatosDataSetTableAdapters.VendedorTableAdapter();
        BaseDeDatosDataSetTableAdapters.Vendedor_detalleTableAdapter vendedor_detalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Vendedor_detalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.carpeta_detalleTableAdapter Carpeta_detalleTableAdapter = new BaseDeDatosDataSetTableAdapters.carpeta_detalleTableAdapter();

        ADOutil Conec = new ADOutil();

        public ImpExp()
        {
            Conec.CadenaConexion = str_conect.bdConexionString;
        }

        #region procesos de importacion
        public void importar(int op, ref FileStream MyFile, string path)
        {
            int iNumberLines = 1;

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                StreamReader sr = new StreamReader(MyFile, System.Text.UnicodeEncoding.Unicode);
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                renglon = sr.ReadLine();

                if (renglon == null)
                {
                    MessageBox.Show(Variable.SYS_MSJ[329, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    sr.Close();
                    MyFile.Close();
                    return;
                }

                int n_tab = 0;
                for (int i = 0; i < renglon.Length; i++)
                {
                    if (renglon.IndexOf(cc, i, 1) >= 0) n_tab++;
                }

                while ((renglon = sr.ReadLine()) != null)
                {
                    iNumberLines++;
                }

                sr = new StreamReader(MyFile, System.Text.UnicodeEncoding.Unicode);
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                switch (op)
                {
                    case (int)ESTADO.FileSource.fPrecios: //importar precio
                        {
                            if (n_tab == 1)
                            {
                                importprecio(ref sr, iNumberLines);
                            }
                            else MessageBox.Show(Variable.SYS_MSJ[3, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        } break;
                    case (int)ESTADO.FileSource.fProductos: //importar Producto
                        {
                            if (n_tab == 14)
                            {
                                importprod(ref sr, iNumberLines);
                            }
                            else MessageBox.Show(Variable.SYS_MSJ[3, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        } break;
                    case (int)ESTADO.FileSource.fInfoAdicional:  //importar Inforrmacion adicional
                        {
                            if (n_tab == 2)
                            {
                                importinfoadd(ref sr, iNumberLines);
                            }
                            else MessageBox.Show(Variable.SYS_MSJ[3, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        } break;
                    case (int)ESTADO.FileSource.fOfertas: //importar Oferta
                        {
                            if (n_tab == 4)
                            {
                                importofer(ref sr, iNumberLines);
                            }
                            else MessageBox.Show(Variable.SYS_MSJ[3, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        } break;
                    case (int)ESTADO.FileSource.fMensajes:  //importar Publicidad
                        {
                            if (n_tab == 2)
                            {
                                importMsj(ref sr, iNumberLines);
                            }
                            else MessageBox.Show(Variable.SYS_MSJ[3, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        } break;
                    case (int)ESTADO.FileSource.fVendedores:  //importar Vendedores
                        {
                            if (n_tab == 1)
                            {
                                importvend(ref sr, iNumberLines);
                            }
                            else MessageBox.Show(Variable.SYS_MSJ[3, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        } break;                    
                }
                sr.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
              //  MessageBox.Show(Variable.SYS_MSJ[4, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
                string d = ex.Message;
                MessageBox.Show(ex.Message, Variable.SYS_MSJ[381, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }
        private void importprod(ref StreamReader sr, int iNumberLines)
        {
            int nIndice = 0;
            int nindice2 = 0;
            bool remplazar = false;
            bool depurar = false;
            double precio_impuesto = 0; 

            Conec.CadenaSelect = "SELECT * FROM Productos ORDER BY id_producto";
            productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            productosTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            baseDeDatosDataSet.EnforceConstraints = false;
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
            
            DataRow[] DRA = baseDeDatosDataSet.Productos.Select("borrado = " + false);

            if (DRA.Length > 0)
            {
                MessageBox.Show(Variable.SYS_MSJ[5, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information); //"Hay productos en la base de datos");
                if (MessageBox.Show(Variable.SYS_MSJ[6, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    depuraplu("", "");
                    depurar = true;
                }
                else
                {
                    if (MessageBox.Show(Variable.SYS_MSJ[8, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)  //"La informacion seran remplazados");
                    {
                        remplazar = true;
                    }
                }
            }
            else
            {
               remplazar = true;
               depurar = true;
            }

            nIndice = 0;
            nindice2 = 0;
            int cod = 0;
            int iNumberNoProcesado = 0;

            ProgressContinue pro = new ProgressContinue();
            pro.IniciaProcess(iNumberLines, Variable.SYS_MSJ[326, Variable.idioma]);                       

            if (depurar || remplazar)
            {
                baseDeDatosDataSet.Productos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Productos.CodigoColumn };
                int iNumberLine = 0;
                while ((renglon = sr.ReadLine()) != null)
                {
                    if (renglon.Length > 0)
                    {
                        try
                        {
                            //if (del != null) del.Invoke(1, Variable.SYS_MSJ[326, Variable.idioma]);
                            pro.UpdateProcess(1, Variable.SYS_MSJ[326, Variable.idioma]);

                            iNumberLine++;
                            Lista_Datos = renglon.Split(cc);
                            if (Lista_Datos[0].Length > 6) {
                                if (MessageBox.Show(Variable.SYS_MSJ[434, Variable.idioma] + "\n" + Variable.SYS_MSJ[436, Variable.idioma] + " " + Lista_Datos[0], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                                    break;
                                else
                                    continue;
                            }
                            if (Lista_Datos[1].Length > 50) Lista_Datos[1] = Lista_Datos[1].Substring(0, 50);
                            if (Convert.ToInt16(Lista_Datos[3]) > 1) Lista_Datos[3] = "0";
                            if (Lista_Datos[4].Length > 5){
                                if (MessageBox.Show(Variable.SYS_MSJ[435, Variable.idioma] + "\n" + Variable.SYS_MSJ[437, Variable.idioma] + " " + Lista_Datos[4], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                                    break;
                                else
                                    continue;
                            }
                            if (Convert.ToInt16(Lista_Datos[5]) > 1) Lista_Datos[5] = "0";
                            precio_impuesto = Convert.ToDouble(Lista_Datos[2]) + Convert.ToDouble(Lista_Datos[7]);

                            string fecha = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);

                           // OleDbDataReader LP = Conec.Obtiene_Dato("SELECT id_producto FROM PRODUCTOS ORDER BY id_producto desc", Conec.CadenaConexion);
                           // if (LP.Read()) cod = Convert.ToInt32(LP.GetValue(0));
                           // LP.Close();                            
                            //productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

                            DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(Convert.ToInt32(Lista_Datos[0]));
                            if (precio_impuesto < 1000)
                            {
                                if (dr != null)
                                {
                                    nindice2++;

                                    dr.BeginEdit();
                                    dr["Nombre"] = Lista_Datos[1];
                                    dr["Precio"] = Convert.ToDecimal(Lista_Datos[2]);
                                    dr["TipoID"] = Convert.ToInt16(Lista_Datos[3]);
                                    dr["NoPlu"] = Convert.ToInt32(Lista_Datos[4]);
                                    dr["PrecioEditable"] = Convert.ToInt16(Lista_Datos[5]);
                                    dr["CaducidadDias"] = Convert.ToInt16(Lista_Datos[6]);
                                    dr["Impuesto"] = Convert.ToDecimal(Lista_Datos[7]);
                                    dr["Mutiplo"] = Convert.ToInt32(Lista_Datos[8]);
                                    dr["actualizado"] = fecha;
                                    dr["id_ingrediente"] = Convert.ToInt32(Lista_Datos[9]);
                                    dr["publicidad1"] = Convert.ToInt32(Lista_Datos[10]);
                                    dr["publicidad2"] = Convert.ToInt32(Lista_Datos[11]);
                                    dr["publicidad3"] = Convert.ToInt32(Lista_Datos[12]);
                                    dr["publicidad4"] = Convert.ToInt32(Lista_Datos[13]);
                                    dr["imagen1"] = Lista_Datos[14];
                                    dr["pendiente"] = true;
                                    dr.EndEdit();

                                    productosTableAdapter.Update(dr);
                                    baseDeDatosDataSet.Productos.AcceptChanges();

                                    string sele = "UPDATE Productos SET " +
                                        "Nombre = '" + Lista_Datos[1] +
                                        "',Precio = " + Convert.ToDecimal(Lista_Datos[2]) +
                                        ",TipoID = " + Convert.ToInt16(Lista_Datos[3]) +
                                        ",NoPlu = " + Convert.ToInt32(Lista_Datos[4]) +
                                        ",PrecioEditable = " + Convert.ToInt16(Lista_Datos[5]) +
                                        ",CaducidadDias = " + Convert.ToInt16(Lista_Datos[6]) +
                                        ",Impuesto = " + Convert.ToDecimal(Lista_Datos[7]) +
                                        ",Mutiplo = " + Convert.ToInt32(Lista_Datos[8]) +
                                        ",actualizado = '" + fecha +
                                        "',id_ingrediente = " + Convert.ToInt32(Lista_Datos[9]) +
                                        ",publicidad1 = " + Convert.ToInt32(Lista_Datos[10]) +
                                        ",publicidad2 = " + Convert.ToInt32(Lista_Datos[11]) +
                                        ",publicidad3 = " + Convert.ToInt32(Lista_Datos[12]) +
                                        ",publicidad4 = " + Convert.ToInt32(Lista_Datos[13]) +
                                        ",imagen1 = '" + Lista_Datos[14] +
                                        "',pendiente = " + true +
                                        " WHERE Codigo = " + Convert.ToInt32(Lista_Datos[0]);

                                    Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");
                                }
                                else
                                {
                                    nIndice++;
                                    cod++;
                                    DataRow drNew = baseDeDatosDataSet.Productos.NewRow();

                                    drNew.BeginEdit();
                                    drNew["id_producto"] = cod;
                                    drNew["Codigo"] = Convert.ToInt32(Lista_Datos[0]);
                                    drNew["Nombre"] = Lista_Datos[1];
                                    drNew["Precio"] = Convert.ToDecimal(Lista_Datos[2]);
                                    drNew["TipoID"] = Convert.ToInt16(Lista_Datos[3]);
                                    drNew["NoPlu"] = Convert.ToInt32(Lista_Datos[4]);
                                    drNew["PrecioEditable"] = Convert.ToInt16(Lista_Datos[5]);
                                    drNew["CaducidadDias"] = Convert.ToInt16(Lista_Datos[6]);
                                    drNew["Impuesto"] = Convert.ToDecimal(Lista_Datos[7]);
                                    drNew["Mutiplo"] = Convert.ToInt32(Lista_Datos[8]);
                                    drNew["actualizado"] = fecha;
                                    drNew["id_ingrediente"] = Convert.ToInt32(Lista_Datos[9]);
                                    drNew["publicidad1"] = Convert.ToInt32(Lista_Datos[10]);
                                    drNew["publicidad2"] = Convert.ToInt32(Lista_Datos[11]);
                                    drNew["publicidad3"] = Convert.ToInt32(Lista_Datos[12]);
                                    drNew["publicidad4"] = Convert.ToInt32(Lista_Datos[13]);
                                    drNew["imagen1"] = Lista_Datos[14];
                                    drNew["pendiente"] = true;
                                    drNew.EndEdit();

                                    baseDeDatosDataSet.Productos.Rows.Add(drNew);

                                    productosTableAdapter.Update(dr);
                                    baseDeDatosDataSet.Productos.AcceptChanges();

                                    string sele = "INSERT INTO Productos (id_producto,Codigo,Nombre,Precio,TipoID,NoPlu,PrecioEditable,CaducidadDias,Impuesto,Mutiplo,actualizado,id_ingrediente,publicidad1,publicidad2,publicidad3,publicidad4,imagen1,pendiente,id_info_nutri,tara)" +
                                        " VALUES ( " + cod + "," + Convert.ToInt32(Lista_Datos[0]) + ",'" + Lista_Datos[1] + "'," + Convert.ToDecimal(Lista_Datos[2]) + ", " +
                                        Convert.ToInt16(Lista_Datos[3]) + "," + Convert.ToInt32(Lista_Datos[4]) + "," + Convert.ToInt16(Lista_Datos[5]) + "," +
                                        Convert.ToInt16(Lista_Datos[6]) + "," + Convert.ToDecimal(Lista_Datos[7]) + "," + Convert.ToInt32(Lista_Datos[8]) + ",'" + fecha + "'," +
                                        Convert.ToInt32(Lista_Datos[9]) + "," + Convert.ToInt32(Lista_Datos[10]) + "," + Convert.ToInt32(Lista_Datos[11]) + "," +
                                        Convert.ToInt32(Lista_Datos[12]) + "," + Convert.ToInt32(Lista_Datos[13]) + ",'" + Lista_Datos[14] + "'," + true + "," + 0 + "," + "0" + ")";
                                    Conec.InsertarReader(Conec.CadenaConexion, sele, "Productos");
                                }
                            }
                            else 
                            {
                                if (MessageBox.Show(Variable.SYS_MSJ[226, Variable.idioma] + " " + Lista_Datos[0] + " " + Lista_Datos[1] +" $" + precio_impuesto.ToString() + ".\n" + Variable.SYS_MSJ[378, Variable.idioma] + " " +Variable.SYS_MSJ[190, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                                {
                                    break;
                                }
                            }
                            //productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("{0}", ex);
                            if (MessageBox.Show("Error al procesar la línea: " + iNumberLine + "\n ¿Desea continuar?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                            {
                                //newWindowThread.Abort();  
                                pro.TerminaProcess();
                                Thread.Sleep(500);
                                return;                                
                            }else{
                                iNumberNoProcesado++;
                            }
                        }
                    }
                } 
                MessageBox.Show(nIndice.ToString() + " " + Variable.SYS_MSJ[10, Variable.idioma] + "\n" + nindice2 + " " + Variable.SYS_MSJ[11, Variable.idioma] + "\n" + iNumberNoProcesado + " No procesados ", Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //newWindowThread.Abort();
            pro.TerminaProcess();
            Thread.Sleep(500);
        }
        private void importvend(ref StreamReader sr, int iNumberLines)
        {
            int nIndice = 0;
            
            Conec.CadenaSelect = "SELECT * FROM Vendedor ORDER BY id_vendedor";
            vendedorTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            vendedorTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            baseDeDatosDataSet.EnforceConstraints = false;
            vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);
            baseDeDatosDataSet.Vendedor.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Vendedor.id_vendedorColumn };

            DataRow[] DRA = baseDeDatosDataSet.Vendedor.Select("borrado = " + false);

            if (DRA.Length > 0)
            {
                MessageBox.Show(Variable.SYS_MSJ[205, Variable.idioma]); //Hay vendedores en la base de datos
                if (MessageBox.Show(Variable.SYS_MSJ[6, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                   depuravend("","");
                }
                else
                {
                    MessageBox.Show(Variable.SYS_MSJ[8, Variable.idioma]); //"La informacion seran remplazados");
                }
            }

            nIndice = 0;
            int iNumberNoProcesado = 0;
   
            ProgressContinue pro = new ProgressContinue();
            pro.IniciaProcess(iNumberLines, Variable.SYS_MSJ[330, Variable.idioma]);

            while ((renglon = sr.ReadLine()) != null)
            {
                try
                {
                    if (renglon.Length > 0)
                    {
                        iNumberLines++;
                        Lista_Datos = renglon.Split(cc);

                        if (Lista_Datos[1].Length > 50) Lista_Datos[1] = Lista_Datos[1].Substring(0, 50);

                        string fecha = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
                        int cod = 0;

                        pro.UpdateProcess(1, Variable.SYS_MSJ[330, Variable.idioma]);

                        OleDbDataReader LP = Conec.Obtiene_Dato("SELECT id_vendedor FROM VENDEDOR ORDER BY id_vendedor desc", Conec.CadenaConexion);
                        if (LP.Read()) cod = Convert.ToInt32(LP.GetValue(0));
                        LP.Close();
                        cod++;
                        
                        //vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);

                        DataRow dr = baseDeDatosDataSet.Vendedor.Rows.Find(Convert.ToInt32(Lista_Datos[0]));

                        nIndice++;

                        if (dr == null)//if (!existe)
                        {
                            DataRow drNew = baseDeDatosDataSet.Vendedor.NewRow();

                            drNew.BeginEdit();
                            drNew["Nombre"] = Lista_Datos[1];
                            drNew["actualizado"] = fecha;
                            drNew["pendiente"] = true;
                            drNew["borrado"] = false;
                            drNew.EndEdit();

                            baseDeDatosDataSet.Vendedor.Rows.Add(drNew);
                            vendedorTableAdapter.Update(dr);
                            baseDeDatosDataSet.Vendedor.AcceptChanges();

                            string sele = "INSERT INTO Vendedor (id_vendedor,Nombre,actualizado,pendiente)" +
                                 " VALUES ( " + Lista_Datos[0] + ",'" + Lista_Datos[1] + "','" + fecha + "'," + true + ")";
                            Conec.InsertarReader(Conec.CadenaConexion, sele, "Vendedor");
                        }
                        else
                        {
                            dr.BeginEdit();
                            dr["Nombre"] = Lista_Datos[1];
                            dr["actualizado"] = fecha;
                            dr["pendiente"] = true;
                            dr["borrado"] = false;
                            dr.EndEdit();

                            vendedorTableAdapter.Update(dr);
                            baseDeDatosDataSet.Vendedor.AcceptChanges();

                            string sele = "UPDATE Vendedor SET " +
                                "Nombre = '" + Lista_Datos[1] +
                                "',actualizado = '" + fecha +
                                "',pendiente = " + true +
                                ",borrado = " + false +
                                " WHERE id_vendedor = " + Convert.ToInt32(Lista_Datos[0]);

                            Conec.ActualizaReader(Conec.CadenaConexion, sele, "Vendedor");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", ex);
                    if (MessageBox.Show(Variable.SYS_MSJ[380, Variable.idioma] + " : " + iNumberLines + (char)13 + Variable.SYS_MSJ[190, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        //newWindowThread.Abort();  
                        pro.TerminaProcess();
                        Thread.Sleep(500);
                        return;
                    }
                    else
                    {
                        iNumberNoProcesado++;
                    }
                }
            }

            MessageBox.Show(nIndice + " " + Variable.SYS_MSJ[10, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            pro.TerminaProcess();
            Thread.Sleep(500);
        }
        private void importMsj(ref StreamReader sr, int iNumberLines)
        {
            int nIndice = 0;

            Conec.CadenaSelect = "SELECT * FROM Publicidad ORDER BY id_publicidad";
            publicidadTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            publicidadTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            baseDeDatosDataSet.EnforceConstraints = false;
            publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);
            baseDeDatosDataSet.Publicidad.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Publicidad.id_publicidadColumn };

            DataRow[] DRA = baseDeDatosDataSet.Publicidad.Select("borrado = " + false);

            if (DRA.Length > 0)
            {
                MessageBox.Show(Variable.SYS_MSJ[206, Variable.idioma]); //"Hay publicidad en la base de datos");
                if (MessageBox.Show(Variable.SYS_MSJ[6, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    depurapub("","");
                }
                else
                {
                    MessageBox.Show(Variable.SYS_MSJ[8, Variable.idioma]);  //"La informacion seran remplazados");
                }
            }

            nIndice = 0;
            int iNumberNoProcesado = 0;
            ProgressContinue pro = new ProgressContinue();
            pro.IniciaProcess(iNumberLines, Variable.SYS_MSJ[331, Variable.idioma]);

            while ((renglon = sr.ReadLine()) != null)
            {
                try
                {
                    if (renglon.Length > 0)
                    {
                        Lista_Datos = renglon.Split(cc);

                        if (Lista_Datos[2].Length > 249) Lista_Datos[2] = Lista_Datos[2].Substring(0, 249);

                        string fecha = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
                      // int cod = 0;

                        //OleDbDataReader LP = Conec.Obtiene_Dato("SELECT id_publicidad FROM Publicidad ORDER BY id_publicidad desc", Conec.CadenaConexion);
                        //if (LP.Read()) cod = Convert.ToInt32(LP.GetValue(0));
                        //LP.Close();
                        //cod++;
                        
                        publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);

                        DataRow dr = baseDeDatosDataSet.Publicidad.Rows.Find(Convert.ToInt32(Lista_Datos[0]));

                        nIndice++;

                        pro.UpdateProcess(1, Variable.SYS_MSJ[331, Variable.idioma]);

                        if (dr == null)
                        {
                            DataRow drNew = baseDeDatosDataSet.Publicidad.NewRow();

                            drNew.BeginEdit();
                            drNew["id_publicidad"] = Convert.ToInt32(Lista_Datos[0]);
                            drNew["Titulo"] = Lista_Datos[1];
                            drNew["Mensaje"] = Lista_Datos[2];
                            drNew["actualizado"] = fecha;
                            drNew["pendiente"] = true;
                            drNew.EndEdit();

                            baseDeDatosDataSet.Publicidad.Rows.Add(drNew);
                            publicidadTableAdapter.Update(dr);
                            baseDeDatosDataSet.Publicidad.AcceptChanges();

                            string sele = "INSERT INTO Publicidad (id_publicidad,Titulo,Mensaje,actualizado,pendiente)" +
                                " VALUES ( " + Lista_Datos[0] + ",'" + Lista_Datos[1] + "','" + Lista_Datos[2] + "','" + fecha + "'," + true + ")";

                            Conec.InsertarReader(Conec.CadenaConexion, sele, "Publicidad");
                        }
                        else
                        {
                            dr.BeginEdit();
                            dr["Titulo"] = Lista_Datos[1];
                            dr["Mensaje"] = Lista_Datos[2];
                            dr["actualizado"] = fecha;
                            dr["pendiente"] = true;
                            dr["borrado"] = false;
                            dr.EndEdit();

                            publicidadTableAdapter.Update(dr);
                            baseDeDatosDataSet.Publicidad.AcceptChanges();

                            string sele = "UPDATE Publicidad SET " +
                                "Titulo = '" + Lista_Datos[1] +
                                "',Mensaje = '" + Lista_Datos[2] +
                                "',actualizado = '" + fecha +
                                "',pendiente = " + true +
                                " ,borrado = " + false +
                                " WHERE id_publicidad = " + Convert.ToInt32(Lista_Datos[0]);

                            Conec.ActualizaReader(Conec.CadenaConexion, sele, "Publicidad");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", ex);
                    if (MessageBox.Show(Variable.SYS_MSJ[380, Variable.idioma] + ": " + iNumberLines + Variable.SYS_MSJ[190, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        //newWindowThread.Abort();  
                        pro.TerminaProcess();
                        Thread.Sleep(500);
                        return;
                    }
                    else
                    {
                        iNumberNoProcesado++;
                    }
                }
            }
            MessageBox.Show(nIndice + " " + Variable.SYS_MSJ[10, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            pro.TerminaProcess();
            Thread.Sleep(500);
        }
        private void importofer(ref StreamReader sr, int iNumberLines)
        {
            int nIndice = 0;

            Conec.CadenaSelect = "SELECT * FROM Oferta ORDER BY id_oferta";
            ofertasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            ofertasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            baseDeDatosDataSet.EnforceConstraints = false;
            ofertasTableAdapter.Fill(baseDeDatosDataSet.Oferta);
            baseDeDatosDataSet.Oferta.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Oferta.id_ofertaColumn };

            DataRow[] DRA = baseDeDatosDataSet.Oferta.Select("borrado = " + false);

            if (DRA.Length > 0)
            {
                MessageBox.Show(Variable.SYS_MSJ[10, Variable.idioma]); //"Hay oferta en la base de datos");
                if (MessageBox.Show(Variable.SYS_MSJ[6, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    depuraofer("", "");
                }
                else
                {
                    MessageBox.Show(Variable.SYS_MSJ[8, Variable.idioma]); //"La informacion seran remplazados");
                }
            }

            nIndice = 0;
            int iNumberNoProcesado = 0;
            ProgressContinue pro = new ProgressContinue();
            pro.IniciaProcess(iNumberLines, Variable.SYS_MSJ[332, Variable.idioma]);

            while ((renglon = sr.ReadLine()) != null)
            {
                try
                {
                    if (renglon.Length > 0)
                    {
                        Lista_Datos = renglon.Split(cc);

                        if (Lista_Datos[1].Length > 50) Lista_Datos[1] = Lista_Datos[1].Substring(0, 50);

                        string fecha = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
                        
                        //int cod = 0;
                        //OleDbDataReader LP = Conec.Obtiene_Dato("SELECT id_oferta FROM Oferta ORDER BY id_oferta desc", Conec.CadenaConexion);
                        //if (LP.Read()) cod = Convert.ToInt32(LP.GetValue(0));
                        //LP.Close();
                        //cod++;

                        // LP = Conec.Obtiene_Dato("SELECT id_oferta FROM Oferta WHERE id_oferta = " + Convert.ToInt32(Lista_Datos[0]) + " ORDER BY id_oferta", Conec.CadenaConexion);
                        //if (LP.Read()) existe = true;
                        //else existe = false;
                        //LP.Close();
                        //ofertasTableAdapter.Fill(baseDeDatosDataSet.Oferta);

                        DataRow dr = baseDeDatosDataSet.Oferta.Rows.Find(Convert.ToInt32(Lista_Datos[0]));

                        nIndice++;
                        pro.UpdateProcess(1, Variable.SYS_MSJ[332, Variable.idioma]);

                        if (dr == null)
                        {
                            DataRow drNew = baseDeDatosDataSet.Oferta.NewRow();

                            drNew.BeginEdit();
                            drNew["id_oferta"] = Convert.ToInt32(Lista_Datos[0]);
                            drNew["nombre"] = Lista_Datos[1];
                            drNew["fecha_inicio"] = Lista_Datos[2];
                            drNew["fecha_fin"] = Lista_Datos[3];
                            drNew["Descuento"] = Convert.ToDouble(Lista_Datos[4]);
                            drNew["actualizado"] = fecha;
                            drNew["pendiente"] = true;
                            drNew.EndEdit();

                            baseDeDatosDataSet.Oferta.Rows.Add(drNew);
                            ofertasTableAdapter.Update(dr);
                            baseDeDatosDataSet.Oferta.AcceptChanges();

                            string sele = "INSERT INTO Oferta (id_oferta,nombre,fecha_inicio,fecha_fin,Descuento,actualizado,pendiente)" +
                                " VALUES ( " + Convert.ToInt32(Lista_Datos[0]) + ",'" + Lista_Datos[1] + "','" + Lista_Datos[2] + "','" + Lista_Datos[3] + "'," + Convert.ToDouble(Lista_Datos[4]) + ",'" + fecha + "'," + true + ")";

                            Conec.InsertarReader(Conec.CadenaConexion, sele, "Oferta");
                        }
                        else
                        {
                            dr.BeginEdit();
                            dr["nombre"] = Lista_Datos[1];
                            dr["fecha_inicio"] = Lista_Datos[2];
                            dr["fecha_fin"] = Lista_Datos[3];
                            dr["Descuento"] = Convert.ToDouble(Lista_Datos[4]);
                            dr["actualizado"] = fecha;
                            dr["pendiente"] = true;
                            dr["borrardo"] = false;
                            dr.EndEdit();

                            ofertasTableAdapter.Update(dr);
                            baseDeDatosDataSet.Oferta.AcceptChanges();

                            string sele = "UPDATE Oferta SET " +
                                "Nombre = '" + Lista_Datos[1] +
                                "',fecha_inicio = '" + Lista_Datos[2] +
                                "',fecha_fin = '" + Lista_Datos[3] +
                                "',Descuento = " + Convert.ToDouble(Lista_Datos[4]) +
                                ",actualizado = '" + fecha +
                                "',pendiente = " + true +
                                " ,borrado = " + false +
                                " WHERE id_oferta = " + Convert.ToInt32(Lista_Datos[0]);

                            Conec.ActualizaReader(Conec.CadenaConexion, sele, "Oferta");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", ex);
                    if (MessageBox.Show(Variable.SYS_MSJ[380, Variable.idioma] + " : " + iNumberLines + Variable.SYS_MSJ[190, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        //newWindowThread.Abort();  
                        pro.TerminaProcess();
                        Thread.Sleep(500);
                        return;
                    }
                    else
                    {
                        iNumberNoProcesado++;
                    }
                }
            }
            MessageBox.Show(nIndice + " " + Variable.SYS_MSJ[10, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            pro.TerminaProcess();
            Thread.Sleep(500);
        }
        private void importinfoadd(ref StreamReader sr, int iNumberLines)
        {
            int nIndice = 0;
            
          //  bool existe = false;

            Conec.CadenaSelect = "SELECT * FROM Ingredientes ORDER BY id_ingrediente";
            infoadicionalTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            infoadicionalTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            baseDeDatosDataSet.EnforceConstraints = false;
            infoadicionalTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);
            baseDeDatosDataSet.Ingredientes.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Ingredientes.id_ingredienteColumn };

            DataRow[] DRA = baseDeDatosDataSet.Ingredientes.Select("borrado = " + false);

            if (DRA.Length > 0)
            {
                MessageBox.Show(Variable.SYS_MSJ[9, Variable.idioma]); //"Hay informacion adicional en la base de datos");
                if (MessageBox.Show(Variable.SYS_MSJ[6, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                   depuraInfoAdd("","");
                }
                else
                {
                    MessageBox.Show(Variable.SYS_MSJ[8, Variable.idioma]); //"La informacion seran remplazados");
                }
            }
            
            nIndice = 0;
            int iNumberNoProcesado=0;
            ProgressContinue pro = new ProgressContinue();
            pro.IniciaProcess(iNumberLines, Variable.SYS_MSJ[333, Variable.idioma]);

            while ((renglon = sr.ReadLine()) != null)
            {
                try
                {
                    if (renglon.Length > 0)
                    {
                        Lista_Datos = renglon.Split(cc);

                        if (Lista_Datos[2].Length > 249) Lista_Datos[2] = Lista_Datos[2].Substring(0, 249);

                        string fecha = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
                        //int cod = 0;

                       // OleDbDataReader LP = Conec.Obtiene_Dato("SELECT id_ingrediente FROM INGREDIENTES ORDER BY id_ingrediente desc", Conec.CadenaConexion);
                       // if (LP.Read()) cod = Convert.ToInt32(LP.GetValue(0));
                       // LP.Close();
                       // cod++;

                        // LP = Conec.Obtiene_Dato("SELECT id_ingrediente FROM INGREDIENTES WHERE id_ingrediente = " + Convert.ToInt32(Lista_Datos[0]) + " ORDER BY id_ingrediente", Conec.CadenaConexion);
                        //if (LP.Read()) existe = true;
                        //else existe = false;
                        //LP.Close();

                      //  infoadicionalTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);

                        DataRow dr = baseDeDatosDataSet.Ingredientes.Rows.Find(Convert.ToInt32(Lista_Datos[0]));

                        nIndice++;
                        pro.UpdateProcess(1, Variable.SYS_MSJ[333, Variable.idioma]);

                        if (dr == null)
                        {
                            DataRow drNew = baseDeDatosDataSet.Ingredientes.NewRow();

                            drNew.BeginEdit();
                            drNew["id_ingrediente"] = Convert.ToInt32(Lista_Datos[0]);
                            drNew["Nombre"] = Lista_Datos[1];
                            drNew["Informacion"] = Lista_Datos[2];
                            drNew["actualizado"] = fecha;
                            drNew["pendiente"] = true;
                            drNew.EndEdit();

                            baseDeDatosDataSet.Ingredientes.Rows.Add(drNew);
                            infoadicionalTableAdapter.Update(dr);
                            baseDeDatosDataSet.Ingredientes.AcceptChanges();

                            string sele = "INSERT INTO Ingredientes (id_ingrediente,Nombre,Informacion,actualizado,pendiente)" +
                                " VALUES ( " + Convert.ToInt32(Lista_Datos[0]) + ",'" + Lista_Datos[1] + "','" + Lista_Datos[2] + "','" + fecha + "'," + true + ")";

                            Conec.InsertarReader(Conec.CadenaConexion, sele, "Ingredientes");
                        }
                        else
                        {
                            dr.BeginEdit();
                            dr["id_ingrediente"] = Convert.ToInt32(Lista_Datos[0]);
                            dr["Nombre"] = Lista_Datos[1];
                            dr["Informacion"] = Lista_Datos[2];
                            dr["actualizado"] = fecha;
                            dr["pendiente"] = true;
                            dr["borrado"] = false;
                            dr.EndEdit();

                            infoadicionalTableAdapter.Update(dr);
                            baseDeDatosDataSet.Ingredientes.AcceptChanges();

                            string sele = "UPDATE Ingredientes SET " +
                                "Nombre = '" + Lista_Datos[1] +
                                "',Informacion = '" + Lista_Datos[2] +
                                "',actualizado = '" + fecha +
                                "',pendiente = " + true +
                                ",borrado = " + false +
                                " WHERE id_ingrediente = " + Convert.ToInt32(Lista_Datos[0]);

                            Conec.ActualizaReader(Conec.CadenaConexion, sele, "Ingredientes");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", ex);
                    if (MessageBox.Show(Variable.SYS_MSJ[380, Variable.idioma] + " : " + iNumberLines + Variable.SYS_MSJ[190, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        //newWindowThread.Abort();  
                        pro.TerminaProcess();
                        Thread.Sleep(500);
                        return;
                    }
                    else
                    {
                        iNumberNoProcesado++;
                    }
                }
            }
            MessageBox.Show(nIndice + " " + Variable.SYS_MSJ[10, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            pro.TerminaProcess();
            Thread.Sleep(500);
        }
        private void importprecio(ref StreamReader sr, int iNumberLines)
        {
            int nIndice = 0;
            bool existe = false;

            Conec.CadenaSelect = "SELECT * FROM Productos ORDER BY id_producto";
            productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            productosTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            baseDeDatosDataSet.EnforceConstraints = false;
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

            if (baseDeDatosDataSet.Productos.Rows.Count > 0)
            {
                nIndice = 0;
                ProgressContinue pro = new ProgressContinue();
                pro.IniciaProcess(iNumberLines, Variable.SYS_MSJ[334, Variable.idioma]);

                while ((renglon = sr.ReadLine()) != null)
                {
                    if (renglon.Length > 0)
                    {
                        Lista_Datos = renglon.Split(cc);
                        string fecha = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);

                        pro.UpdateProcess(1, Variable.SYS_MSJ[334, Variable.idioma]);

                        OleDbDataReader LP = Conec.Obtiene_Dato("SELECT codigo FROM PRODUCTOS WHERE codigo = " + Convert.ToInt32(Lista_Datos[0]) + " ORDER BY codigo", Conec.CadenaConexion);
                        if (LP.Read()) existe = true;
                        else existe = false;
                        LP.Close();

                        if (existe)
                        {
                            nIndice++;
                            string sele = "UPDATE Productos " +
                            "SET precio = " + Convert.ToDecimal(Lista_Datos[1]) +
                            ",pendiente = " + true +
                            ", actualizado = '" + fecha +
                            "' WHERE ( codigo = " + Convert.ToInt32(Lista_Datos[0]) + ")";
                            Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");
                        }
                    }
                }
                MessageBox.Show(nIndice.ToString() + " " + Variable.SYS_MSJ[167, Variable.idioma] + " " + Variable.SYS_MSJ[12, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information); //" Actualizados");
                pro.TerminaProcess();
                Thread.Sleep(500);
            }
        }
        /*private void importfolder(ref StreamReader sr, int iNumberLines, int ngrupo, int nbascula)
        {
            int nIndice = 0;

            //  bool existe = false;

            Conec.CadenaSelect = "SELECT * FROM carpeta_detalle ORDER BY ID";
            Carpeta_detalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            Carpeta_detalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            baseDeDatosDataSet.EnforceConstraints = false;
            Carpeta_detalleTableAdapter.Fill(baseDeDatosDataSet.carpeta_detalle);
            baseDeDatosDataSet.Ingredientes.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.carpeta_detalle.id_basculaColumn, baseDeDatosDataSet.carpeta_detalle.id_grupoColumn, baseDeDatosDataSet.carpeta_detalle.IDColumn };

            DataRow[] DRA = baseDeDatosDataSet.carpeta_detalle.Select("id_bascula = " + nbascula + " and id_grupo = " + ngrupo);

            if (DRA.Length > 0)
            {
                MessageBox.Show(Variable.SYS_MSJ[9, Variable.idioma]); //"Hay informacion adicional en la base de datos");              
            }
            else
            {
                nIndice = 0;
                int iNumberNoProcesado = 0;
                ProgressContinue pro = new ProgressContinue();
                pro.IniciaProcess(iNumberLines, Variable.SYS_MSJ[333, Variable.idioma]);

                while ((renglon = sr.ReadLine()) != null)
                {
                    try
                    {
                        if (renglon.Length > 0)
                        {
                            Lista_Datos = renglon.Split(cc);

                            if (Lista_Datos[2].Length > 50) Lista_Datos[2] = Lista_Datos[2].Substring(0, 50);

                            string fecha = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);

                            DataRow dr = baseDeDatosDataSet.carpeta_detalle.Rows.Find(Convert.ToInt32(Lista_Datos[0]));

                            nIndice++;
                            pro.UpdateProcess(1, Variable.SYS_MSJ[333, Variable.idioma]);

                            if (dr == null)
                            {
                                DataRow drNew = baseDeDatosDataSet.carpeta_detalle.NewRow();

                                drNew.BeginEdit();
                                drNew["id_bascula"] = nbascula;
                                drNew["id_grupo"] = ngrupo;
                                drNew["ID"] = Convert.ToInt32(Lista_Datos[0]);
                                drNew["nombre"] = Lista_Datos[1];
                                drNew["imagen"] = Lista_Datos[2];
                                drNew.EndEdit();

                                baseDeDatosDataSet.carpeta_detalle.Rows.Add(drNew);
                                Carpeta_detalleTableAdapter.Update(dr);
                                baseDeDatosDataSet.carpeta_detalle.AcceptChanges();

                                string sele = "INSERT INTO carpeta_detalle (id_grupo,id_bascula,ID,Nombre,imagen)" +
                                    " VALUES ( " + ngrupo + "," + nbascula + "," + Convert.ToInt32(Lista_Datos[0]) + ",'" + Lista_Datos[1] + "','" + Lista_Datos[2] + ")";

                                Conec.InsertarReader(Conec.CadenaConexion, sele, "carpeta_detalle");
                            }
                            else
                            {
                                dr.BeginEdit();
                                dr["nombre"] = Lista_Datos[1];
                                dr["imagen"] = Lista_Datos[2];
                                dr.EndEdit();

                                Carpeta_detalleTableAdapter.Update(dr);
                                baseDeDatosDataSet.carpeta_detalle.AcceptChanges();

                                string sele = "UPDATE carpeta_detalle SET " +
                                    " nombre = '" + Lista_Datos[1] +
                                    "',imagen = '" + Lista_Datos[2] +
                                    "' WHERE ID = " + Convert.ToInt32(Lista_Datos[0]) + " and id_bascula = " + nbascula + " and id_grupo = " + ngrupo;

                                Conec.ActualizaReader(Conec.CadenaConexion, sele, "carpeta_detalle");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("{0}", ex);
                        if (MessageBox.Show(Variable.SYS_MSJ[380, Variable.idioma] + " : " + iNumberLines + Variable.SYS_MSJ[190, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        {
                            //newWindowThread.Abort();  
                            pro.TerminaProcess();
                            return;
                        }
                        else
                        {
                            iNumberNoProcesado++;
                        }
                    }
                }

                MessageBox.Show(nIndice + " " + Variable.SYS_MSJ[10, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                pro.TerminaProcess();
            }
            
        }*/
        #endregion

        #region proceso de exportacion
        public void exportar(int op, ref FileStream MyFile)
        {
            int nIndice = 0;
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                StreamWriter sr = new StreamWriter(MyFile, System.Text.UnicodeEncoding.Unicode);

                switch (op)
                {
                    case (int)ESTADO.FileSource.fProductos: // Exportar productos
                        {
                            Conec.CadenaSelect = "SELECT Codigo,NoPlu,Nombre,Precio,TipoId,PrecioEditable,CaducidadDias,Impuesto,Mutiplo,id_ingrediente FROM Productos ORDER BY codigo";
                            productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                            productosTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

                            nIndice = 0;
                            Lista_Datos = new string[15];

                            foreach (DataRow Registro in baseDeDatosDataSet.Productos.Rows)
                            {
                                Lista_Datos[0] = Registro["Codigo"].ToString() + cc;
                                Lista_Datos[1] = Registro["Nombre"].ToString() + cc;
                                Lista_Datos[2] = Registro["Precio"].ToString() + cc;
                                Lista_Datos[3] = Registro["TipoId"].ToString() + cc;
                                Lista_Datos[4] = Registro["NoPlu"].ToString() + cc;
                                Lista_Datos[5] = Registro["PrecioEditable"].ToString() + cc;
                                Lista_Datos[6] = Registro["CaducidadDias"].ToString() + cc;
                                Lista_Datos[7] = Registro["Impuesto"].ToString() + cc;                                
                                Lista_Datos[8] = Registro["Mutiplo"].ToString() + cc;
                                Lista_Datos[9] = Registro["id_ingrediente"].ToString() + cc;
                                Lista_Datos[10] = Registro["publicidad1"].ToString() + cc;
                                Lista_Datos[11] = Registro["publicidad2"].ToString() + cc;
                                Lista_Datos[12] = Registro["publicidad3"].ToString() + cc;
                                Lista_Datos[13] = Registro["publicidad4"].ToString() + cc;
                                Lista_Datos[14] = Registro["imagen1"].ToString();
                                sr.WriteLine(Lista_Datos[0] + Lista_Datos[1] + Lista_Datos[2] + Lista_Datos[3] + Lista_Datos[4] + 
                                             Lista_Datos[5] + Lista_Datos[6] + Lista_Datos[7] + Lista_Datos[8] + Lista_Datos[9] +
                                             Lista_Datos[10] + Lista_Datos[11] + Lista_Datos[12] + Lista_Datos[13] + Lista_Datos[14]);
                                nIndice++;
                            }
                            sr.Close();
                        } break;
                    case (int)ESTADO.FileSource.fInfoAdicional:  // Exportar Info Adicional
                        {
                            Conec.CadenaSelect = "SELECT id_ingrediente,Nombre,Informacion FROM Ingredientes ORDER BY id_ingrediente";
                            infoadicionalTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                            infoadicionalTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                            infoadicionalTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);

                            nIndice = 0;
                            Lista_Datos = new string[3];

                            foreach (DataRow Registro in baseDeDatosDataSet.Ingredientes.Rows)
                            {
                                Lista_Datos[0] = Registro["id_ingrediente"].ToString() + cc;
                                Lista_Datos[1] = Registro["Nombre"].ToString() + cc;
                                Lista_Datos[2] = Registro["Informacion"].ToString();
                                sr.WriteLine(Lista_Datos[0] + Lista_Datos[1] + Lista_Datos[2]);
                                nIndice++;
                            }
                            sr.Close();
                        } break;
                    case (int)ESTADO.FileSource.fOfertas:  // Export Oferta
                        {
                            Conec.CadenaSelect = "SELECT id_oferta,nombre,fecha_inicio,fecha_fin,Descuento FROM Oferta ORDER BY id_oferta";
                            ofertasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                            ofertasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                            ofertasTableAdapter.Fill(baseDeDatosDataSet.Oferta);

                            nIndice = 0;
                            Lista_Datos = new string[5];

                            foreach (DataRow Registro in baseDeDatosDataSet.Oferta.Rows)
                            {
                                Lista_Datos[0] = Registro["id_oferta"].ToString() + cc;
                                Lista_Datos[1] = Registro["nombre"].ToString() + cc;
                                Lista_Datos[2] = Registro["fecha_inicio"].ToString() + cc;
                                Lista_Datos[3] = Registro["fecha_fin"].ToString() + cc;
                                Lista_Datos[4] = Registro["Descuento"].ToString();
                                sr.WriteLine(Lista_Datos[0] + Lista_Datos[1] + Lista_Datos[2] + Lista_Datos[3] + Lista_Datos[4]); // + Lista_Datos[5] + Lista_Datos[6]);
                                nIndice++;
                            }
                            sr.Close();
                        } break;
                    case (int)ESTADO.FileSource.fMensajes:  // Exportar Publicidad
                        {
                            Conec.CadenaSelect = "SELECT id_publicidad,Titulo,Mensaje FROM Publicidad ORDER BY id_publicidad";
                            publicidadTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                            publicidadTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                            publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);

                            nIndice = 0;
                            Lista_Datos = new string[3];

                            foreach (DataRow Registro in baseDeDatosDataSet.Publicidad.Rows)
                            {
                                Lista_Datos[0] = Registro["id_publicidad"].ToString() + cc;
                                Lista_Datos[1] = Registro["Titulo"].ToString() + cc;
                                Lista_Datos[2] = Registro["Mensaje"].ToString();
                                sr.WriteLine(Lista_Datos[0] + Lista_Datos[1] + Lista_Datos[2]);
                                nIndice++;
                            }
                            sr.Close();
                        } break;
                    case (int)ESTADO.FileSource.fVendedores: // Exportar Vendedores
                        {
                            Conec.CadenaSelect = "SELECT id_vendedores,Nombre,Msj_Enable,Meta_Enable,Meta_Ventas,publicidad1,publicidad2 FROM Vendedor ORDER BY id_vendedor";
                            vendedorTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                            vendedorTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                            vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);

                            nIndice = 0;
                            Lista_Datos = new string[2];

                            foreach (DataRow Registro in baseDeDatosDataSet.Vendedor.Rows)
                            {
                                Lista_Datos[0] = Registro["id_vendedor"].ToString() + cc;
                                Lista_Datos[1] = Registro["Nombre"].ToString();
                                sr.WriteLine(Lista_Datos[0] + Lista_Datos[1]);
                                nIndice++;
                            }
                            sr.Close();
                        } break;
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                MessageBox.Show(nIndice + " " + Variable.SYS_MSJ[13, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);  // " Exportados");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region proceso de depuracion
        public void depurar(int op, string dato1, string dato2)
        {
            bool brtaFunct = false;

            try
            {
                DialogResult depura_op = MessageBox.Show(Variable.SYS_MSJ[207, Variable.idioma], Variable.SYS_MSJ[208, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (depura_op == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    switch (op)
                    {
                        case (int)ESTADO.FileSource.fProductos:
                            {//productos
                                brtaFunct = depuraplu(dato1, dato2);
                            } break;
                        case (int)ESTADO.FileSource.fInfoAdicional:
                            {//Informacion adicional
                                brtaFunct = depuraInfoAdd(dato1, dato2);
                            } break;
                        case (int)ESTADO.FileSource.fOfertas:
                            {//Ofertas 
                                brtaFunct = depuraofer(dato1, dato2);
                            } break;
                        case (int)ESTADO.FileSource.fMensajes:
                            {//publicidad
                                brtaFunct = depurapub(dato1, dato2);
                            } break;
                        case (int)ESTADO.FileSource.fVendedores:
                            {//Vendedores
                                brtaFunct = depuravend(dato1, dato2);
                            } break;                    
                    }

                    if (brtaFunct == true)
                    {
                        MessageBox.Show(Variable.SYS_MSJ[2, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);  //"Los datos fueron depurados");
                    }

                    Cursor.Current = Cursors.Default;
                }
            }
            catch (System.Data.OleDb.OleDbException myException)
            {
                for (int i = 0; i < myException.Errors.Count; i++)
                {
                    MessageBox.Show("Index #" + i + "\n" +
                        "Message: " + myException.Errors[i].Message + "\n" +
                        "Native: " + myException.Errors[i].NativeError.ToString() + "\n" +
                        "Source: " + myException.Errors[i].Source + "\n" +
                        "SQL: " + myException.Errors[i].SQLState + "\n", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
         private bool depuraplu(string ran1, string ran2)
        {
            bool borrar = true;
            bool asignado = false;
            string sele = "";
            int iActionBorrar = 0;

            Conec.CadenaSelect = "SELECT * FROM Productos ORDER BY id_producto";
            productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            productosTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

            Conec.CadenaSelect = "SELECT * FROM Prod_detalle ORDER BY id_producto";
            prod_detalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            prod_detalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);

            Conec.CadenaSelect = "SELECT * FROM Productos ORDER BY id_producto";
            plu_detalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            plu_detalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            plu_detalleTableAdapter.Fill(baseDeDatosDataSet.PLU_detalle);


            baseDeDatosDataSet.Productos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Productos.CodigoColumn };
            baseDeDatosDataSet.Prod_detalle.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Prod_detalle.codigoColumn };
            baseDeDatosDataSet.PLU_detalle.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.PLU_detalle.id_productoColumn };

             List<Int32> listId_prod = new List<Int32>();

            if (ran1 != "" && ran2 != "")
            {
                iActionBorrar = 0;

                DataRow[] DRP = baseDeDatosDataSet.Productos.Select("codigo >= " + Convert.ToInt32(ran1) + " AND codigo <= " + Convert.ToInt32(ran2));

                if (DRP.Length > 0)
                {
                    foreach (DataRow DA in DRP)
                    {
                        listId_prod.Add(Convert.ToInt32(DA["id_producto"].ToString()));
                    }

                    DataRow[] DRPD = baseDeDatosDataSet.Prod_detalle.Select("codigo >= " + Convert.ToInt32(ran1) + " AND codigo <= " + Convert.ToInt32(ran2));

                    if (DRPD.Length > 0)
                    {
                        asignado = true;
                    }


                    for (int i = 0; i < listId_prod.Count && asignado == false; i++)
                    {
                        DataRow[] DRPLUD = baseDeDatosDataSet.PLU_detalle.Select("id_producto = " + listId_prod[i]);

                        if (DRPLUD.Length > 0)
                        {
                            asignado = true;
                        }
                    }

                    if (asignado == true)
                    {

                        if (MessageBox.Show(Variable.SYS_MSJ[209, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            borrar = true;
                        }
                        else borrar = false;
                    }
                }
                else
                {
                    MessageBox.Show(Variable.SYS_MSJ[337, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    borrar = false;
                }
            }
            else if (ran1 != "" && ran2 == "")
            {
                iActionBorrar = 1;

                DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(Convert.ToInt32(ran1));

                if (dr != null)
                {
                    listId_prod.Add(Convert.ToInt32(dr["id_producto"].ToString()));

                    DataRow drpd = baseDeDatosDataSet.Prod_detalle.Rows.Find(Convert.ToInt32(ran1));

                    if (drpd != null)
                    {
                        asignado = true;
                    }

                    DataRow drplud = baseDeDatosDataSet.PLU_detalle.Rows.Find(Convert.ToInt32(dr["id_producto"].ToString()));

                    if(drplud != null){
                        asignado = true;
                    }

                    if(asignado == true){                        

                        if (MessageBox.Show(Variable.SYS_MSJ[209, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            borrar = true;
                        }
                        else borrar = false;
                    }   
                }
                else
                {
                    MessageBox.Show(Variable.SYS_MSJ[338, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    borrar = false;
                }
            }
            else if (ran1 == "" && ran2 == "")
            {
                iActionBorrar = 2;

                DataRow[] DRPD = baseDeDatosDataSet.Prod_detalle.Select("codigo > 0");

                if (DRPD.Length > 0)
                {
                    asignado = true;
                }

                DataRow[] DRPLUD = baseDeDatosDataSet.PLU_detalle.Select("id_producto > 0");

                if (DRPLUD.Length > 0)
                {
                    asignado = true;
                }

                if (asignado == true)
                {
                    
                    if (MessageBox.Show(Variable.SYS_MSJ[209, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        borrar = true;
                    }
                    else borrar = false;
                }
            }
           
            if (borrar == true)
            {
                switch (iActionBorrar)
                {
                    case 0:             //Borrado por rango de codigo

                        if (asignado == true)
                        {

                            sele = "DELETE * FROM Prod_detalle WHERE ( codigo >= " + Convert.ToInt32(ran1) + " AND codigo <= " + Convert.ToInt32(ran2) + ")";
                            Conec.EliminarReader(Conec.CadenaConexion, sele, "Prod_detalle");

                            for (int i = 0; i < listId_prod.Count; i++)
                            {
                                sele = "DELETE * FROM PLU_detalle WHERE ( id_producto = " + listId_prod[i] + ")";
                                Conec.EliminarReader(Conec.CadenaConexion, sele, "PLU_detalle");
                            }
                        }

                        sele = "DELETE * FROM Productos WHERE ( codigo >= " + Convert.ToInt32(ran1) + " AND codigo <= " + Convert.ToInt32(ran2) + ")";
                        Conec.EliminarReader(Conec.CadenaConexion, sele, "Productos");
                        break;

                    case 1:                 //Borrado por codigo

                        if(asignado == true){

                            sele = "DELETE * FROM Prod_detalle WHERE ( codigo = " + Convert.ToInt32(ran1) + ")";
                            Conec.EliminarReader(Conec.CadenaConexion, sele, "Prod_detalle");

                            sele = "DELETE * FROM PLU_detalle WHERE ( id_producto = " + listId_prod[0] + ")";
                            Conec.EliminarReader(Conec.CadenaConexion, sele, "PLU_detalle");
                        }

                        sele = "DELETE * FROM Productos WHERE ( codigo = " + Convert.ToInt32(ran1) + ")";
                        Conec.EliminarReader(Conec.CadenaConexion, sele, "Productos");

                        break;

                    case 2:                 //Borrado completo

                        sele = "DELETE * FROM PLU_detalle";
                        Conec.EliminarReader(Conec.CadenaConexion, sele, "PLU_detalle");

                        sele = "DELETE * FROM Prod_detalle";
                        Conec.EliminarReader(Conec.CadenaConexion, sele, "Prod_detalle");

                        sele = "DELETE * FROM Productos";
                        Conec.EliminarReader(Conec.CadenaConexion, sele, "Productos");

                        break;
                }
            }

            return borrar;
        }

        private bool depuraInfoAdd(string ran1, string ran2)
         {
             bool borrar = true;
             bool asignado = false;
             string sele = "";
             int iActionBorrar = 0;

             Conec.CadenaSelect = "SELECT * FROM Productos ORDER BY id_producto";
             productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
             productosTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
             productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

             Conec.CadenaSelect = "SELECT * FROM Ingredientes ORDER BY id_ingrediente";
             infoadicionalTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
             infoadicionalTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
             infoadicionalTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);

             infoadicional_detalleTableAdapter.Fill(baseDeDatosDataSet.Ingre_detalle);

             baseDeDatosDataSet.Ingredientes.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Ingredientes.id_ingredienteColumn };

             if (ran1 != "" && ran2 != "")
             {
                 iActionBorrar = 0;

                 DataRow[] DRP = baseDeDatosDataSet.Ingredientes.Select("id_ingrediente >= " + Convert.ToInt32(ran1) + " AND id_ingrediente <= " + Convert.ToInt32(ran2));

                 if (DRP.Length > 0)
                 {
                     DataRow[] DRPD = baseDeDatosDataSet.Ingre_detalle.Select("id_ingrediente >= " + Convert.ToInt32(ran1) + " AND id_ingrediente <= " + Convert.ToInt32(ran2));

                     if (DRPD.Length > 0)
                     {
                         asignado = true;
                     }

                     DataRow[] DRPLU = baseDeDatosDataSet.Productos.Select("id_ingrediente >= " + Convert.ToInt32(ran1) + " AND id_ingrediente <= " + Convert.ToInt32(ran2));

                     if (DRPLU.Length > 0)
                     {
                         asignado = true;
                     }

                     if (asignado == true)
                     {
                         if (MessageBox.Show(Variable.SYS_MSJ[212, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                         {
                             borrar = true;
                         }
                         else borrar = false;
                     }
                 }
                 else
                 {
                     MessageBox.Show(Variable.SYS_MSJ[337, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                     borrar = false;
                 }
             }
             else if (ran1 != "" && ran2 == "")
             {
                 iActionBorrar = 1;

                 DataRow dr = baseDeDatosDataSet.Ingredientes.Rows.Find(Convert.ToInt32(ran1));

                 if (dr != null)
                 {

                     DataRow[] DRPLU = baseDeDatosDataSet.Productos.Select("id_ingrediente = " + Convert.ToInt32(ran1));

                     if (DRPLU.Length > 0)
                     {
                         asignado = true;
                     }

                     DataRow[] DRIAD = baseDeDatosDataSet.Ingre_detalle.Select("id_ingrediente = " + Convert.ToInt32(ran1));

                     if (DRIAD.Length > 0)
                     {
                         asignado = true;
                     }


                     if (asignado == true)
                     {
                         if (MessageBox.Show(Variable.SYS_MSJ[212, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                         {
                             borrar = true;
                         }
                         else borrar = false;
                     }
                 }
                 else
                 {
                     MessageBox.Show(Variable.SYS_MSJ[337, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                     borrar = false;
                 }
             }
             else if (ran1 == "" && ran2 == "")
             {
                 iActionBorrar = 2;

                 DataRow[] DRPLU = baseDeDatosDataSet.Productos.Select("id_ingrediente > 0");

                 if (DRPLU.Length > 0)
                 {
                     asignado = true;
                 }

                 DataRow[] DRIAD = baseDeDatosDataSet.Ingre_detalle.Select("id_ingrediente > 0");

                 if (DRIAD.Length > 0)
                 {
                     asignado = true;
                 }


                 if (asignado == true)
                 {
                     if (MessageBox.Show(Variable.SYS_MSJ[212, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                     {
                         borrar = true;
                     }
                     else borrar = false;
                 }
             }

             if (borrar)
             {
                 switch (iActionBorrar)
                 {

                     case 0:

                         sele = "UPDATE Productos SET id_ingrediente = 0 WHERE (id_ingrediente >= " + Convert.ToInt32(ran1) + "AND id_ingrediente <= " + Convert.ToInt32(ran2) + ")";
                         Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                         if (asignado == true)
                         {
                             sele = "DELETE * FROM Ingre_detalle WHERE ( id_ingrediente >= " + Convert.ToInt32(ran1) + " AND id_ingrediente <= " + Convert.ToInt32(ran2) + ")";
                             Conec.EliminarReader(Conec.CadenaConexion, sele, "Ingre_detalle");
                         }

                         sele = "DELETE * FROM Ingredientes WHERE ( id_ingrediente >= " + Convert.ToInt32(ran1) + " AND id_ingrediente <= " + Convert.ToInt32(ran2) + ")";
                         Conec.EliminarReader(Conec.CadenaConexion, sele, "Ingredientes");
                         break;
                         
                     case 1:
                         sele = "UPDATE Productos SET id_ingrediente = 0 WHERE (id_ingrediente = " + Convert.ToInt32(ran1) + ")";
                         Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                         if (asignado == true)
                         {
                             sele = "DELETE * FROM Ingre_detalle WHERE ( id_ingrediente = " + Convert.ToInt32(ran1) + ")";
                             Conec.EliminarReader(Conec.CadenaConexion, sele, "Ingre_detalle");
                         }

                         sele = "DELETE * FROM Ingredientes WHERE ( id_ingrediente = " + Convert.ToInt32(ran1) + ")";
                         Conec.EliminarReader(Conec.CadenaConexion, sele, "Ingredientes");

                         break;

                     case 2:

                         sele = "UPDATE Productos SET id_ingrediente = 0";
                         Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                         sele = "DELETE * FROM Ingre_detalle";
                         Conec.EliminarReader(Conec.CadenaConexion, sele, "Ingre_detalle");

                         sele = "DELETE * FROM Ingredientes";
                         Conec.EliminarReader(Conec.CadenaConexion, sele, "Ingredientes");

                         break;
                 }
             }

             return borrar;
         }

        private bool depuraofer(string ran1, string ran2)
         {
             bool borrar = true;
             bool asignado = false;
             string sele = "";
             int iActionBorrar = 0;

             Conec.CadenaSelect = "SELECT * FROM Productos ORDER BY id_producto";
             productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
             productosTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
             productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

             Conec.CadenaSelect = "SELECT * FROM Oferta ORDER BY id_oferta";
             ofertasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
             ofertasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
             ofertasTableAdapter.Fill(baseDeDatosDataSet.Oferta);

             ofertas_detalleTableAdapter.Fill(baseDeDatosDataSet.Oferta_Detalle);

             baseDeDatosDataSet.Oferta.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Oferta.id_ofertaColumn };

             if (ran1 != "" && ran2 != "")
             {
                 iActionBorrar = 0;

                 DataRow[] DRP = baseDeDatosDataSet.Oferta.Select("id_oferta >= " + Convert.ToInt32(ran1) + " AND id_oferta <= " + Convert.ToInt32(ran2));

                 if (DRP.Length > 0)
                 {
                     DataRow[] DRPD = baseDeDatosDataSet.Oferta_Detalle.Select("id_oferta >= " + Convert.ToInt32(ran1) + " AND id_oferta <= " + Convert.ToInt32(ran2));

                     if (DRPD.Length > 0)
                     {
                         asignado = true;
                     }

                     DataRow[] DRPLU = baseDeDatosDataSet.Productos.Select("oferta >= " + Convert.ToInt32(ran1) + " AND oferta <= " + Convert.ToInt32(ran2));

                     if (DRPLU.Length > 0)
                     {
                         asignado = true;
                     }

                     if (asignado == true)
                     {
                         if (MessageBox.Show(Variable.SYS_MSJ[15, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                         {
                             borrar = true;
                         }
                         else borrar = false;
                     }
                 }
                 else
                 {
                     MessageBox.Show(Variable.SYS_MSJ[337, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                     borrar = false;
                 }
             }
             else if (ran1 != "" && ran2 == "")
             {
                 iActionBorrar = 1;

                 DataRow dr = baseDeDatosDataSet.Oferta.Rows.Find(Convert.ToInt32(ran1));

                 if (dr != null)
                 {

                     DataRow[] DRPLU = baseDeDatosDataSet.Productos.Select("oferta = " + Convert.ToInt32(ran1));

                     if (DRPLU.Length > 0)
                     {
                         asignado = true;
                     }

                     DataRow[] DRIAD = baseDeDatosDataSet.Oferta_Detalle.Select("id_oferta = " + Convert.ToInt32(ran1));

                     if (DRIAD.Length > 0)
                     {
                         asignado = true;
                     }


                     if (asignado == true)
                     {
                         if (MessageBox.Show(Variable.SYS_MSJ[15, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                         {
                             borrar = true;
                         }
                         else borrar = false;
                     }
                 }
                 else
                 {
                     MessageBox.Show(Variable.SYS_MSJ[337, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                     borrar = false;
                 }
             }
             else if (ran1 == "" && ran2 == "")
             {
                 iActionBorrar = 2;

                 DataRow[] DRPLU = baseDeDatosDataSet.Productos.Select("oferta > 0");

                 if (DRPLU.Length > 0)
                 {
                     asignado = true;
                 }

                 DataRow[] DRIAD = baseDeDatosDataSet.Oferta_Detalle.Select("id_oferta > 0");

                 if (DRIAD.Length > 0)
                 {
                     asignado = true;
                 }


                 if (asignado == true)
                 {
                     if (MessageBox.Show(Variable.SYS_MSJ[15, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                     {
                         borrar = true;
                     }
                     else borrar = false;
                 }
             }

             if (borrar)
             {
                 switch (iActionBorrar)
                 {

                     case 0:

                         sele = "UPDATE Productos SET oferta = 0 WHERE (oferta >= " + Convert.ToInt32(ran1) + "AND oferta <= " + Convert.ToInt32(ran2) + ")";
                         Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                         if (asignado == true)
                         {
                             sele = "DELETE * FROM Oferta_Detalle WHERE ( id_oferta >= " + Convert.ToInt32(ran1) + " AND id_oferta <= " + Convert.ToInt32(ran2) + ")";
                             Conec.EliminarReader(Conec.CadenaConexion, sele, "Oferta_Detalle");
                         }

                         sele = "DELETE * FROM Oferta WHERE ( id_oferta >= " + Convert.ToInt32(ran1) + " AND id_oferta <= " + Convert.ToInt32(ran2) + ")";
                         Conec.EliminarReader(Conec.CadenaConexion, sele, "Oferta");
                         break;

                     case 1:
                         sele = "UPDATE Productos SET oferta = 0 WHERE (oferta = " + Convert.ToInt32(ran1) + ")";
                         Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                         if (asignado == true)
                         {
                             sele = "DELETE * FROM Oferta_Detalle WHERE ( id_oferta = " + Convert.ToInt32(ran1) + ")";
                             Conec.EliminarReader(Conec.CadenaConexion, sele, "Oferta_Detalle");
                         }

                         sele = "DELETE * FROM Oferta WHERE ( id_oferta = " + Convert.ToInt32(ran1) + ")";
                         Conec.EliminarReader(Conec.CadenaConexion, sele, "Oferta");

                         break;

                     case 2:

                         sele = "UPDATE Productos SET oferta = 0";
                         Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                         sele = "DELETE * FROM Oferta_Detalle";
                         Conec.EliminarReader(Conec.CadenaConexion, sele, "Oferta_Detalle");

                         sele = "DELETE * FROM Oferta";
                         Conec.EliminarReader(Conec.CadenaConexion, sele, "Oferta");

                         break;
                 }
             }

             return borrar;
         }

        private bool depurapub(string ran1, string ran2)
        {
            bool borrar = true;
            bool asignado = false;
            string sele = "";
            int iActionBorrar = 0;

            Conec.CadenaSelect = "SELECT * FROM Productos ORDER BY id_producto";
            productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            productosTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

            Conec.CadenaSelect = "SELECT * FROM Publicidad ORDER BY id_publicidad";
            publicidadTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            publicidadTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);

            public_detalleTableAdapter.Fill(baseDeDatosDataSet.Public_Detalle);

            baseDeDatosDataSet.Publicidad.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Publicidad.id_publicidadColumn };

            if (ran1 != "" && ran2 != "")
            {
                iActionBorrar = 0;

                DataRow[] DRP = baseDeDatosDataSet.Publicidad.Select("id_publicidad >= " + Convert.ToInt32(ran1) + " AND id_publicidad <= " + Convert.ToInt32(ran2));

                if (DRP.Length > 0)
                {
                    DataRow[] DRPD = baseDeDatosDataSet.Public_Detalle.Select("id_publicidad >= " + Convert.ToInt32(ran1) + " AND id_publicidad <= " + Convert.ToInt32(ran2));

                    if (DRPD.Length > 0)
                    {
                        asignado = true;
                    }

                    DataRow[] DRPLU = baseDeDatosDataSet.Productos.Select("publicidad1 >= " + Convert.ToInt32(ran1) + " AND publicidad1 <= " + Convert.ToInt32(ran2));

                    if (DRPLU.Length > 0)
                    {
                        asignado = true;
                    }

                    DRPLU = baseDeDatosDataSet.Productos.Select("publicidad2 >= " + Convert.ToInt32(ran1) + " AND publicidad2 <= " + Convert.ToInt32(ran2));

                    if (DRPLU.Length > 0)
                    {
                        asignado = true;
                    }


                    DRPLU = baseDeDatosDataSet.Productos.Select("publicidad3 >= " + Convert.ToInt32(ran1) + " AND publicidad3 <= " + Convert.ToInt32(ran2));

                    if (DRPLU.Length > 0)
                    {
                        asignado = true;
                    }


                    DRPLU = baseDeDatosDataSet.Productos.Select("publicidad4 >= " + Convert.ToInt32(ran1) + " AND publicidad4 <= " + Convert.ToInt32(ran2));

                    if (DRPLU.Length > 0)
                    {
                        asignado = true;
                    }


                    if (asignado == true)
                    {
                        if (MessageBox.Show(Variable.SYS_MSJ[210, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            borrar = true;
                        }
                        else borrar = false;
                    }
                }
                else
                {
                    MessageBox.Show(Variable.SYS_MSJ[337, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    borrar = false;
                }
            }
            else if (ran1 != "" && ran2 == "")
            {
                iActionBorrar = 1;

                DataRow dr = baseDeDatosDataSet.Publicidad.Rows.Find(Convert.ToInt32(ran1));

                if (dr != null)
                {

                    DataRow[] DRPLU = baseDeDatosDataSet.Productos.Select("publicidad1 = " + Convert.ToInt32(ran1));

                    if (DRPLU.Length > 0)
                    {
                        asignado = true;
                    }

                    DRPLU = baseDeDatosDataSet.Productos.Select("publicidad2 = " + Convert.ToInt32(ran1));

                    if (DRPLU.Length > 0)
                    {
                        asignado = true;
                    }

                    DRPLU = baseDeDatosDataSet.Productos.Select("publicidad3 = " + Convert.ToInt32(ran1));

                    if (DRPLU.Length > 0)
                    {
                        asignado = true;
                    }

                    DRPLU = baseDeDatosDataSet.Productos.Select("publicidad4 = " + Convert.ToInt32(ran1));

                    if (DRPLU.Length > 0)
                    {
                        asignado = true;
                    }

                    DataRow[] DRIAD = baseDeDatosDataSet.Public_Detalle.Select("id_publicidad = " + Convert.ToInt32(ran1));

                    if (DRIAD.Length > 0)
                    {
                        asignado = true;
                    }


                    if (asignado == true)
                    {
                        if (MessageBox.Show(Variable.SYS_MSJ[210, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            borrar = true;
                        }
                        else borrar = false;
                    }
                }
                else
                {
                    MessageBox.Show(Variable.SYS_MSJ[337, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    borrar = false;
                }
            }
            else if (ran1 == "" && ran2 == "")
            {
                iActionBorrar = 2;

                DataRow[] DRPLU = baseDeDatosDataSet.Productos.Select("publicidad1 > 0 ");

                if (DRPLU.Length > 0)
                {
                    asignado = true;
                }

                DRPLU = baseDeDatosDataSet.Productos.Select("publicidad2 > 0 ");

                if (DRPLU.Length > 0)
                {
                    asignado = true;
                }

                DRPLU = baseDeDatosDataSet.Productos.Select("publicidad3 > 0 ");

                if (DRPLU.Length > 0)
                {
                    asignado = true;
                }

                DRPLU = baseDeDatosDataSet.Productos.Select("publicidad4 > 0 ");

                if (DRPLU.Length > 0)
                {
                    asignado = true;
                }


                DataRow[] DRIAD = baseDeDatosDataSet.Public_Detalle.Select("id_publicidad > 0");

                if (DRIAD.Length > 0)
                {
                    asignado = true;
                }


                if (asignado == true)
                {
                    if (MessageBox.Show(Variable.SYS_MSJ[210, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        borrar = true;
                    }
                    else borrar = false;
                }
            }

            if (borrar)
            {
                switch (iActionBorrar)
                {

                    case 0:

                        sele = "UPDATE Productos SET publicidad1 = 0 WHERE (publicidad1 >= " + Convert.ToInt32(ran1) + "AND publicidad1 <= " + Convert.ToInt32(ran2) + ")";
                        Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                        sele = "UPDATE Productos SET publicidad2 = 0 WHERE (publicidad2 >= " + Convert.ToInt32(ran1) + "AND publicidad2 <= " + Convert.ToInt32(ran2) + ")";
                        Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                        sele = "UPDATE Productos SET publicidad3 = 0 WHERE (publicidad3 >= " + Convert.ToInt32(ran1) + "AND publicidad3 <= " + Convert.ToInt32(ran2) + ")";
                        Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                        sele = "UPDATE Productos SET publicidad4 = 0 WHERE (publicidad4 >= " + Convert.ToInt32(ran1) + "AND publicidad4 <= " + Convert.ToInt32(ran2) + ")";
                        Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                        if (asignado == true)
                        {
                            sele = "DELETE * FROM Public_Detalle WHERE ( id_publicidad >= " + Convert.ToInt32(ran1) + " AND id_publicidad <= " + Convert.ToInt32(ran2) + ")";
                            Conec.EliminarReader(Conec.CadenaConexion, sele, "Public_Detalle");
                        }

                        sele = "DELETE * FROM Publicidad WHERE ( id_publicidad >= " + Convert.ToInt32(ran1) + " AND id_publicidad <= " + Convert.ToInt32(ran2) + ")";
                        Conec.EliminarReader(Conec.CadenaConexion, sele, "Publicidad");
                        break;

                    case 1:
                        sele = "UPDATE Productos SET publicidad1 = 0 WHERE (publicidad1 = " + Convert.ToInt32(ran1) + ")";
                        Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                        sele = "UPDATE Productos SET publicidad2 = 0 WHERE (publicidad2 = " + Convert.ToInt32(ran1) + ")";
                        Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                        sele = "UPDATE Productos SET publicidad3 = 0 WHERE (publicidad3 = " + Convert.ToInt32(ran1) + ")";
                        Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                        sele = "UPDATE Productos SET publicidad4 = 0 WHERE (publicidad4 = " + Convert.ToInt32(ran1) + ")";
                        Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                        if (asignado == true)
                        {
                            sele = "DELETE * FROM Public_Detalle WHERE ( id_publicidad = " + Convert.ToInt32(ran1) + ")";
                            Conec.EliminarReader(Conec.CadenaConexion, sele, "Public_Detalle");
                        }

                        sele = "DELETE * FROM Publicidad WHERE ( id_publicidad = " + Convert.ToInt32(ran1) + ")";
                        Conec.EliminarReader(Conec.CadenaConexion, sele, "Publicidad");

                        break;

                    case 2:

                        sele = "UPDATE Productos SET publicidad1 = 0";
                        Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                        sele = "UPDATE Productos SET publicidad2 = 0";
                        Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                        sele = "UPDATE Productos SET publicidad3 = 0";
                        Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                        sele = "UPDATE Productos SET publicidad4 = 0";
                        Conec.ActualizaReader(Conec.CadenaConexion, sele, "Productos");

                        sele = "DELETE * FROM Public_Detalle";
                        Conec.EliminarReader(Conec.CadenaConexion, sele, "Public_Detalle");

                        sele = "DELETE * FROM Publicidad";
                        Conec.EliminarReader(Conec.CadenaConexion, sele, "Publicidad");

                        break;
                }
            }

            return borrar;
        }
        
        private bool depuravend(string ran1, string ran2)
        {
            bool borrar = true;
            bool asignado = false;
            string sele = "";
            int iActionBorrar = 0;

            Conec.CadenaSelect = "SELECT * FROM Productos ORDER BY id_producto";
            productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            productosTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

            Conec.CadenaSelect = "SELECT * FROM Vendedor ORDER BY id_vendedor";
            vendedorTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            vendedorTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);

            vendedor_detalleTableAdapter.Fill(baseDeDatosDataSet.Vendedor_detalle);

            baseDeDatosDataSet.Vendedor.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Vendedor.id_vendedorColumn };

            if (ran1 != "" && ran2 != "")
            {
                iActionBorrar = 0;

                DataRow[] DRP = baseDeDatosDataSet.Vendedor.Select("id_vendedor >= " + Convert.ToInt32(ran1) + " AND id_vendedor <= " + Convert.ToInt32(ran2));

                if (DRP.Length > 0)
                {
                    DataRow[] DRPD = baseDeDatosDataSet.Vendedor_detalle.Select("id_vendedor >= " + Convert.ToInt32(ran1) + " AND id_vendedor <= " + Convert.ToInt32(ran2));

                    if (DRPD.Length > 0)
                    {
                        asignado = true;
                    }

                    if (asignado == true)
                    {
                        if (MessageBox.Show(Variable.SYS_MSJ[339, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            borrar = true;
                        }
                        else borrar = false;
                    }
                }
                else
                {
                    MessageBox.Show(Variable.SYS_MSJ[337, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    borrar = false;
                }
            }
            else if (ran1 != "" && ran2 == "")
            {
                iActionBorrar = 1;

                DataRow dr = baseDeDatosDataSet.Vendedor.Rows.Find(Convert.ToInt32(ran1));

                if (dr != null)
                {

                    DataRow[] DRIAD = baseDeDatosDataSet.Vendedor_detalle.Select("id_vendedor = " + Convert.ToInt32(ran1));

                    if (DRIAD.Length > 0)
                    {
                        asignado = true;
                    }

                    if (asignado == true)
                    {
                        if (MessageBox.Show(Variable.SYS_MSJ[339, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            borrar = true;
                        }
                        else borrar = false;
                    }
                }
                else
                {
                    MessageBox.Show(Variable.SYS_MSJ[337, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    borrar = false;
                }
            }
            else if (ran1 == "" && ran2 == "")
            {
                iActionBorrar = 2;

                DataRow[] DRIAD = baseDeDatosDataSet.Vendedor_detalle.Select("id_vendedor > 0");

                if (DRIAD.Length > 0)
                {
                    asignado = true;
                }

                if (asignado == true)
                {
                    if (MessageBox.Show(Variable.SYS_MSJ[339, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        borrar = true;
                    }
                    else borrar = false;
                }
            }

            if (borrar)
            {
                switch (iActionBorrar)
                {

                    case 0:

                        if (asignado == true)
                        {
                            sele = "DELETE * FROM Vendedor_detalle WHERE ( id_vendedor >= " + Convert.ToInt32(ran1) + " AND id_vendedor <= " + Convert.ToInt32(ran2) + ")";
                            Conec.EliminarReader(Conec.CadenaConexion, sele, "Vendedor_detalle");
                        }

                        sele = "DELETE * FROM Vendedor WHERE ( id_vendedor >= " + Convert.ToInt32(ran1) + " AND id_vendedor <= " + Convert.ToInt32(ran2) + ")";
                        Conec.EliminarReader(Conec.CadenaConexion, sele, "Vendedor");
                        break;

                    case 1:

                        if (asignado == true)
                        {
                            sele = "DELETE * FROM Vendedor_detalle WHERE ( id_vendedor = " + Convert.ToInt32(ran1) + ")";
                            Conec.EliminarReader(Conec.CadenaConexion, sele, "Vendedor_detalle");
                        }

                        sele = "DELETE * FROM Vendedor WHERE ( id_vendedor = " + Convert.ToInt32(ran1) + ")";
                        Conec.EliminarReader(Conec.CadenaConexion, sele, "Vendedor");

                        break;

                    case 2:

                        sele = "DELETE * FROM Vendedor_detalle";
                        Conec.EliminarReader(Conec.CadenaConexion, sele, "Vendedor_detalle");

                        sele = "DELETE * FROM Vendedor";
                        Conec.EliminarReader(Conec.CadenaConexion, sele, "Vendedor");

                        break;
                }
            }

            return borrar;
        }

        #endregion

    /*    private int Load_file(string path)
        {
            OleDbConnection oConn = new OleDbConnection();
            OleDbCommand oCmd = new OleDbCommand();
            OleDbDataAdapter oDa = new OleDbDataAdapter();
            DataSet oDs = new DataSet();

            oConn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=" +
                path + ";Extended Properties=Excel 8.0;";

            oConn.Open();
            oCmd.CommandText = "SELECT * FROM [Hoja1$]";
            oCmd.Connection = oConn;
            oDa.SelectCommand = oCmd;
            oDa.Fill(oDs);
            oConn.Close();

            path = Variable.appPath;
            oDs.WriteXmlSchema(path + "\\MiTabla.xsd");
            System.IO.StreamWriter xmlSW = new System.IO.StreamWriter(path + "\\MisDatos.xml", false);
            oDs.WriteXml(xmlSW, XmlWriteMode.IgnoreSchema);
            xmlSW.Close();

            DataSet myDS = new DataSet("NewDataSet");
            myDS.ReadXmlSchema(path + "\\MiTabla.xsd");
            myDS.ReadXml(path + "\\MisDatos.xml", XmlReadMode.IgnoreSchema);
            string padre;
            string tipo;
            string icod;
            string iddes;
            string ipre;
            string iplu;
            string iiva;
            string imul;
            string iedi;
            string idca;
            string itip;
            string ingre;
            string ifec;
            string ID;
            string num_id = "";
            int nreg = 0;
            bool existe = false;
            bool existe2 = false;
            bool borrado = true;

            System.Data.OleDb.OleDbDataReader AR = Conec.Obtiene_Dato("SELECT * FROM Productos", Conec.CadenaConexion);
            if (AR.Read()) existe = true;
            AR.Close();
            System.Data.OleDb.OleDbDataReader ARR = Conec.Obtiene_Dato("SELECT * FROM carpeta", Conec.CadenaConexion);
            if (ARR.Read()) existe2 = true;
            ARR.Close();

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            if (existe || existe2)
            {
                if (MessageBox.Show(Variable.SYS_MSJ[16, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Conec.EliminarReader(Conec.CadenaConexion, "DELETE * FROM Prod_detalle", "Prod_detalle");
                    Conec.EliminarReader(Conec.CadenaConexion, "DELETE * FROM Productos", "Productos");
                    Conec.EliminarReader(Conec.CadenaConexion, "DELETE * FROM carpeta", "carpeta");
                    DataRow[] D = myDS.Tables[0].Select("ID = 0");
                    if (D.Length <= 0)
                    {
                        Conec.CadenaSelect = "INSERT INTO carpeta (ID,ID_padre,Nombre)" +
                            " VALUES ( 0,null,'ScaleColor')";
                        Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, "carpeta");
                    }
                    borrado = true;
                }
                else borrado = false;
            }
            else
            {
                DataRow[] D = myDS.Tables[0].Select("ID = 0");
                if (D.Length <= 0)
                {
                    Conec.CadenaSelect = "INSERT INTO carpeta (ID,ID_padre,Nombre)" +
                        " VALUES ( 0,null,'ScaleColor')";
                    Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, "carpeta");
                }
            }

            if (borrado)
            {
                foreach (DataRow DT in myDS.Tables[0].Rows)
                {
                    ID = DT[0].ToString();  // ID de Folder
                    padre = DT[1].ToString(); //Folder Padre
                    tipo = DT[2].ToString(); //Tipo de Dato
                    icod = DT[3].ToString();  //Codigo
                    iplu = DT[4].ToString();  //NumPLU
                    iddes = DT[5].ToString(); //Descripcion del articulo
                    ipre = DT[6].ToString();  //Precio del articulo
                    itip = DT[7].ToString();  //Tipo de Articulo
                    iedi = DT[8].ToString();  //Precio Editable
                    idca = DT[9].ToString();  //Dias decaducidad
                    iiva = DT[10].ToString();  //Impuesto
                    imul = DT[11].ToString();  //Multiplo
                    ingre = DT[12].ToString();  //Ingrediente    
                    ifec = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
                    nreg++;
                    switch (tipo)
                    {
                        case "P":
                            {
                                int dependencia;
                                if (padre == "" || padre == "0" || padre == null)
                                {
                                    dependencia = Convert.ToInt32(num_id);
                                }
                                else
                                {
                                    dependencia = Convert.ToInt32(padre);
                                }
                                if (iddes.Length > 52) iddes = iddes.Substring(0, 51);
                                Conec.CadenaSelect = "INSERT INTO Productos (id_producto,Codigo,NoPlu,Nombre,Precio,TipoId,PrecioEditable,CaducidadDias,Impuesto,Mutiplo,actualizado)" +
                                    " VALUES ( " + Convert.ToInt32(icod) + "," +Convert.ToInt32(icod) + "," + Convert.ToInt32(iplu) + ",'" + iddes + "'," + Convert.ToDecimal(ipre) + "," + Convert.ToInt16(itip) + "," +
                                    Convert.ToInt16(iedi) + "," + Convert.ToInt32(idca) + "," + Convert.ToDecimal(iiva) + "," + Convert.ToInt32(imul) + ",'" + ifec + "')";

                                Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, "Productos");

                                string sele = "INSERT INTO Prod_Detalle (id_bascula,id_grupo,id_carpeta,id_producto,codigo,precio)" +
                                    " VALUES ( 0, 0," + Convert.ToInt32(dependencia) + "," + Convert.ToInt32(icod) + "," + Convert.ToInt32(icod) + "," + Convert.ToDecimal(ipre) + ")";

                                Conec.InsertarReader(Conec.CadenaConexion, sele, "Prod_detalle");

                            } break;
                        case "C":
                            {
                                if (ID != null)
                                {
                                    string sele;
                                    if (iddes.Length > 50) iddes = iddes.Substring(0, 49);
                                    if (ID == "0")
                                    {
                                        sele = "INSERT INTO carpeta (ID,ID_padre,Nombre)" +
                                            " VALUES ( 0,null,'" + iddes + "')";
                                    }
                                    else
                                    {
                                        if (padre == "" || padre == "0" || padre == null)
                                        {
                                            sele = "INSERT INTO carpeta (ID,ID_padre,Nombre)" +
                                                " VALUES ( " + Convert.ToInt32(ID) + ",0,'" + iddes + "')";
                                        }
                                        else
                                        {
                                            sele = "INSERT INTO carpeta (ID,ID_padre,Nombre)" +
                                                " VALUES ( " + Convert.ToInt32(ID) + "," + Convert.ToInt32(padre) + ",'" + iddes + "')";
                                        }
                                    }
                                    Conec.InsertarReader(Conec.CadenaConexion, sele, "carpeta");

                                    num_id = ID;
                                }
                            } break;
                    }
                }

                OleDbConnection imporconexion = new OleDbConnection(Conec.CadenaConexion);
                imporconexion.Open();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            return nreg;
        }*/

    }
}

