
using System;
using System.Collections.Generic;
using System.Numerics;
using tp1;

namespace tpfinal
{

    public class Estrategia
    {

        public String Consulta1(List<Proceso> datos)
        { //se crea max y min heap
        	Heap maxHeap=new Heap(true);
        	Heap minHeap=new Heap(false);
        	
        	foreach (var element in datos){ //inserto procesos en las heaps
        	         	minHeap.Insertar(element);
        	         	maxHeap.Insertar(element);
        	}
        	 // Obtener las hojas de ambas heaps
        	 var hojasMaxHeap = maxHeap.obtenerhojas();
        	 var hojasMinHeap = minHeap.obtenerhojas();

              
    string hojasMaxHead = "";
    foreach (var p in hojasMaxHeap)
    {
        hojasMaxHead += "Prioridad: " + p.prioridad + ", Tiempo: " + p.tiempo + "\n";
    }


    string hojasMinHead = "";
    foreach (var p in hojasMinHeap)
    {
        hojasMinHead += "Prioridad: " + p.prioridad + ", Tiempo: " + p.tiempo + "\n";
    }

   
    return "Hojas del MaxHeap:\n" + hojasMaxHead + "Hojas del MinHeap:\n" + hojasMinHead;
}


        



        public String Consulta2(List<Proceso> datos)
        {
        	//se crea max y min heap
        	Heap maxHeap=new Heap(true);
        	Heap minHeap=new Heap(false);
        	
        	foreach (var element in datos){ //inserto procesos en las heaps
        	         	minHeap.Insertar(element);
        	         	maxHeap.Insertar(element);
        	}
        	
        	int alturamaxheap=maxHeap.calcularAltura();
        	int alturaminheap=minHeap.calcularAltura();
        	string resultado="Altura maxHeap: "+alturamaxheap+ " ,AlturaminHeap minHeap:"+alturaminheap;
        	return resultado;
           
        }



        public String Consulta3(List<Proceso> datos)
        {
           //se crea max y min heap
        	Heap maxHeap=new Heap(true);
        	Heap minHeap=new Heap(false);
        	
        	foreach (var element in datos){ //inserto procesos en las heaps
        	         	minHeap.Insertar(element);
        	         	maxHeap.Insertar(element);
        	}
        	
        	var nivelMaxHeap=maxHeap.obtenernivel();
        	var nivelMinHeap=minHeap.obtenernivel();
        	string resultado="Datos MaxHeap: \n";
        	foreach (var element in nivelMaxHeap) {
        		resultado+=element+"\n";
        	}
			 resultado+="Datos MinHeap: \n";
        	foreach (var element in nivelMinHeap) {
        		resultado+=element+"\n";
        	}
			return resultado; //retorno mensaje final
        }


        public void ShortesJobFirst(List<Proceso> datos, List<Proceso> collected)
        {   //se crea minheap, indicando false ya que sea basado en tiempo del cpu
        	Heap minHeap=new Heap(false);
        	//inserto procesos al heap
        	foreach (var element in datos) {
        		minHeap.Insertar(element);
        	}
        	
        	while(!minHeap.esVacia()){
        		collected.Add(minHeap.Eliminar());
        	}
        	

            
        }


        public void PreemptivePriority(List<Proceso> datos, List<Proceso> collected)
        {   //se crea minheap, indicando true ya que sea basado en prioridad
        	Heap maxHeap=new Heap(true);
        	//inserto procesos al heap
        	foreach (var element in datos) {
        		maxHeap.Insertar(element);
        	}
        	//extraer todos los procesos del heap y agregarlos a collected
        	while(!maxHeap.esVacia()){
        		collected.Add(maxHeap.Eliminar());
        	}
        	
        	
           
        	
        	
        }

        





    }
}