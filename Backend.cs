using System;
using System.Collections.Generic;
using tp1;

namespace tpfinal
{ 
    public class Backend
    {
        public static List<Proceso> datos = new List<Proceso>();

        public static string aProfundidad()
        {
            return (new Estrategia()).Consulta3(datos);
        }

        public static string caminoAPrediccion()
        {
            return (new Estrategia()).Consulta2(datos);
        }

        public static string todasLasPredicciones()
        {
            return (new Estrategia()).Consulta1(datos);
        }

        public static void buscar(bool heapOP, List<Proceso> collected)
        {
            
            if (heapOP)
            {
                (new Estrategia()).ShortesJobFirst(datos, collected);
            }
            else
            {
                (new Estrategia()).PreemptivePriority(datos, collected);
            }
            
        }
    }

}