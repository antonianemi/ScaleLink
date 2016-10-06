using System;
using System.Collections;
using System.Windows.Forms;
using System.Net.Sockets;

namespace MainMenu
{
	/// <summary>
	/// Descripción breve de ControlArray.
	/// </summary>
	public class ControlArray: CollectionBase
	{
		private string mNombre;

		public ControlArray()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}
		
		public ControlArray(string elNombre)
		{
			mNombre = elNombre;
		}
		//
		public int Add(Control ctrl)
		{
			return base.List.Add(ctrl);
		}
		//
		// Asignar los controles que contendrá esta colección       (14/Nov/02)
		public void AsignarControles(Control.ControlCollection ctrls)
		{
			string elNombre = mNombre;
			this.AsignarControles(ctrls, elNombre); 
		}
		public void AsignarControles(Control.ControlCollection ctrls, string elNombre)
		{
			// Asignar los controles a los arrays,
			// para que esto funcione automáticamente los nombres de los controles
			// deberían tener el formato: nombre_numero
			//  nombre del control seguido de un guión bajo y el índice
			if( elNombre == null )
				elNombre = mNombre;
			// si no se indica el nombre de los controles a añadir,
			// lanzar una excepción
			if( elNombre == "" )
				throw new ArgumentException("No se ha indicado el nombre base de los controles");
			//
			this.Clear();
			asignarLosControles(ctrls, elNombre);
			this.Reorganizar();
		}
		public void asignarLosControles(Control.ControlCollection ctrls, string elNombre)
		{
			foreach(Control ctr in ctrls)
			{
				// Hacer una llamada recursiva por si este control "contiene" otros
				asignarLosControles(ctr.Controls, elNombre);
				//
				if( ctr.Name.IndexOf(elNombre) > -1 )
					this.Add(ctr);
			}
		}
		//
		public bool Contains(Control ctrl)
		{
			return base.List.Contains(ctrl);
		}
		public int IndexOf(Control ctrl)
		{
			return base.List.IndexOf(ctrl);
		}
		public int Index(string name)
		{
			Control ctrl;
			int hallado = -1;
			//
			for(int i=0; i<= List.Count - 1; i++)
			{
				ctrl = (Control)base.List[i];
				if( ctrl.Name == name )
				{
					hallado = i;
					break;
				}
			}
			return hallado;
		}
		public int Index(Control ctrl)
		{
			int i;
			//
			i = ctrl.Name.LastIndexOf("_");
			// Si el nombre no tiene el signo 
			if( i == -1 )
			{
				i = base.List.IndexOf(ctrl);
			}
			else
			{
				i = Convert.ToInt32(ctrl.Name.Substring(i + 1));
			}
			return i;
		}
		//
		void Insert(int index, Control ctrl)
		{
			List.Insert(index, ctrl);
		}
		//
		public Control this[int index]
		{
			get
			{
				return ((Control)List[index]);
			}
			set
			{
				base.List[index] = value;
			}
		}
		public Control this[string name] 
		{
			get
			{
				int index = this.Index(name);
				return ((Control)List[index]);
			}
			set
			{
				int index = this.Index(name);
				if( index == -1 )
				{
					index = this.Add(value);
				}
				base.List[index] = value;
			}
		}
		public Control this[Control ctrl]
		{
			get
			{
				return (Control)List[this.IndexOf(ctrl)];
			}
			set
			{
				List[this.IndexOf(ctrl)] = value;
			}
		}
		//
		public string Nombre
		{
			get
			{
				return mNombre;
			}
			set
			{
				mNombre = value;
			}
		}
		//
		void Remove(Control ctrl)
		{
			List.Remove(ctrl);
		}
		//
		// Reorganizar el contenido de la colección y ordenar por índice
		public void Reorganizar()
		{
			ControlArray ca = new ControlArray();
			//
			for(int i=0; i<= this.Count - 1; i++)
			{
				foreach(Control ctr in this)
				{
					if( i == this.Index(ctr) )
					{
						ca.Add(ctr);
						break;
					}
				}
			}
			//
			this.Clear();
			foreach(Control ctr in ca)
			{
				this.Add(ctr);
			}
		}
	}
}
