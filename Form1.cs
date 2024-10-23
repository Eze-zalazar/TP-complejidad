using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;



namespace tpfinal
{
    public partial class Form1 : Form
    {
        private int ocurrencias = 1;
        private int speed = 1;
        private int finalizados = 0;
        private double tiempo_total = 0.0;
        private string resultado = "";
        private string planifSelected = "";


        private Scheduling planificador = new Scheduling(new List<Proceso>());
        private List<ListItem> lista = new List<ListItem>();
        private List<System.ComponentModel.BackgroundWorker> backgroundWorkers = new List<BackgroundWorker>();
        private List<Stopwatch> clocks = new List<Stopwatch>();
       
       

        public Form1()
        {
            InitializeComponent();
        }


        private void btnclose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        private void button1_Click_1(object sender, EventArgs e)
        {

            List<Proceso> collected = new List<Proceso>();
            finalizados = 0;
            tiempo_total = 0.0;
            resultado = "";

            Backend.buscar(heapOp.Checked, collected);
            if (collected.Count > 0)
            {
                planifSelected = heapOp.Checked ? heapOp.Text : otroOp.Text;
                flowLayoutPanel1.Controls.Clear();
                backgroundWorkers.Clear();
                lista.Clear();

                planificador = new Scheduling(collected);

                for (int i = 1; i <= ocurrencias; i++)
                {
                    ListItem item = new ListItem(i);
                    item.Width = flowLayoutPanel1.Width - 25;
                    lista.Add(item);
                    flowLayoutPanel1.Controls.Add(item);
                    addBackgroundWorker();
                    clocks.Add(new Stopwatch());
                }

                for (int i = 0; i < ocurrencias; i++)
                {
                    backgroundWorkers[i].RunWorkerAsync(i);
                }

                inSimulation();

            }

        }



        private void inSimulation()
        {
            cntrlBox.Enabled = false;
            planGroupBox.Enabled = false;
            button1.Enabled = false;
            btnNo.Enabled = true;
        }

        private void addBackgroundWorker()
        {
            System.ComponentModel.BackgroundWorker backgroundWorker1=new System.ComponentModel.BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.WorkerReportsProgress=true;
            backgroundWorker1.WorkerSupportsCancellation=true;
            backgroundWorkers.Add(backgroundWorker1);
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            finalizados = 0;

            for (int i = 0; i < ocurrencias; i++)
            {
                backgroundWorkers[i].CancelAsync();
            }

        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();


        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);


        private void barra_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btn_consulta1_Click(object sender, EventArgs e)
        {
            string resultado = Backend.todasLasPredicciones();
            this.mostrarConsulta(resultado);

        }

        private void btn_consulta2_Click(object sender, EventArgs e)
        {
            string resultado = Backend.caminoAPrediccion();
            this.mostrarConsulta(resultado);
        }

        private void btn_consulta3_Click(object sender, EventArgs e)
        {
            string resultado = Backend.aProfundidad();
            this.mostrarConsulta(resultado);
        }

        private void mostrarConsulta(string resultado)
        {
            Consultas consulta = new Consultas(this, resultado);
            consulta.Show();
            this.Hide();
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            txtDist.Text = "CPU's: " + trackBar1.Value;
            ocurrencias = trackBar1.Value;
        }


        private void backgroundWorker1_DoWork(object sender,
            DoWorkEventArgs e)
        {
            
            BackgroundWorker worker = sender as BackgroundWorker;

            long tiempo = ComputeCPU((int)e.Argument, worker, e);
            Tuple<int, long> tupla = new Tuple<int, long>((int)e.Argument, tiempo);

            e.Result = tupla;
        }


        private void backgroundWorker1_RunWorkerCompleted(
            object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                initForSimulation();
            }
            else if (e.Cancelled)
            {

                finalizados++;
                if (finalizados == ocurrencias)
                {
                    resultado = "";
                    MessageBox.Show("Simulacion Reiniciada.", "Reinicio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    initForSimulation();
                }
            }
            else
            {
                Tuple<int, long> tupla = (Tuple<int, long>) e.Result;
                resultado += "Tiempo de ejecucion total del CPU #" + (tupla.Item1 + 1) + ": " + (tupla.Item2 / 1000) + " segs" + '\n';
                tiempo_total = tupla.Item2 / 1000;
                finalizados++;
                if (finalizados==ocurrencias)
                {
                    resultado += "Total Nro de CPU's " +ocurrencias+ '\n';
                    resultado += "Tiempo total promedio " + (tiempo_total/ocurrencias) + " segs";
                    MessageBox.Show(resultado, "Resultados de la planificación "+ planifSelected, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    initForSimulation();
                }

            }


        }

        private void initForSimulation()
        {
            cntrlBox.Enabled = true;
            planGroupBox.Enabled = true;
            button1.Enabled = true;
            btnNo.Enabled = false;
            flowLayoutPanel1.Controls.Clear();
            backgroundWorkers.Clear();
            lista.Clear();
        }

        private void backgroundWorker1_ProgressChanged(object sender,
            ProgressChangedEventArgs e)
        {
            Tuple<int, Proceso> tupla = (Tuple<int, Proceso>)e.UserState;
            int id = tupla.Item1;
            Proceso process = tupla.Item2;
            ListItem item = lista[id];

            if (process!=null)
            {
                item.Dato = process;
                item.StepValue(e.ProgressPercentage);

            }
            else
            {
                item.IdleState();
            }

            item.Update();
        }


        long ComputeCPU(int id, BackgroundWorker worker, DoWorkEventArgs e)
        {
            Proceso process = planificador.getProceso();
            ListItem item = lista[id];
            clocks[id].Start();
            Tuple<int, Proceso> tupla = new Tuple<int, Proceso>(id, process);
            int i = 0;
            int speedFactor = 1;
            if (speed > 1)
            {
                 speedFactor = speed * 40;
            }
            while (process != null && !worker.CancellationPending)
            {
                tupla = new Tuple<int, Proceso>(id, process);
                i = 0;
                while (i < process.tiempo && !worker.CancellationPending) {
                    worker.ReportProgress(i, tupla);

                    System.Threading.Thread.Sleep(200/speedFactor);
                    i++;
                }
                process = planificador.getProceso();
                     
            }
            tupla = new Tuple<int, Proceso>(id, null);
            worker.ReportProgress(0, tupla);

            e.Cancel = worker.CancellationPending;
            clocks[id].Stop();
            return clocks[id].ElapsedMilliseconds;
           
        }


        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            txtSpeed.Text = "Speed: " + trackBar2.Value+"x";
            speed = trackBar2.Value;
        }
    }


}