using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tpfinal
{
    public partial class ListItem : UserControl
    {

        private Proceso _dato;
        private int id;
        public Proceso Dato {
            get
            {
                return _dato;
            }

            set
            {
                _dato = value;
                label1.Text = "Proceso: "+ value.nombre;
                label3.Text = "Tiempo: " + value.tiempo;
                label2.Text = "Prioridad: " + value.prioridad;
               
                avanceProc8.Maximum = _dato.tiempo;

            }
        }
        
        public ListItem(int id)
        {
            InitializeComponent();
            label2.Width = this.Width - 3;
            label3.Left = this.Width - 6;
            this.id = id;
            label4.Text += id;

        }



        public void StepValue(int value)
        {
            avanceProc8.Value= value;
        }


        public void IdleState()
        {
            avanceProc8.Value = 0;
            estado.Text = "Inactiva";
            label1.Text = "Proceso: -";
            label3.Text = "Tiempo: -" ;
            label2.Text = "Prioridad: -" ;
        }

    }
}
