/*
 * Creado por SharpDevelop.
 * Usuario: eze
 * Fecha: 23/10/2024
 * Hora: 13:01
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
namespace tpfinal
{
	/// <summary>
	/// Description of Heap.
	/// </summary>
	public class Heap
	{    private List<Proceso> HEAP;
		 private bool MaxHeap; //variable que determina si es max o min heap
		
		 public Heap(bool maxHeap)
		{
			this.MaxHeap=maxHeap;
			HEAP=new List<Proceso>();
			
		}
		
		 
		 
		 public void intercambiar(int i,int j){
		 	var temp=HEAP[i];
		 	HEAP[i]=HEAP[j];
		 	HEAP[j]=temp;
		 	
		 	
		 }
		 public bool esVacia(){
		 	return HEAP.Count == 0;
		 	
		 	
		 }
		 public bool comparar(int hijo, int padre){
		 	if(MaxHeap){
		 		return HEAP[hijo].prioridad > HEAP[padre].prioridad;//para maxheap se compara por prioridad
		 	}
		 	else{
		 		return HEAP[hijo].tiempo < HEAP[padre].tiempo; //para minheap se compara por tiempo del cpu
		 	}
		 	
		 	
		 }
		 public void Insertar(Proceso nuevoProceso){
		 	HEAP.Add(nuevoProceso);//agregar nuevo proceso al final de la heap
		 	int index= HEAP.Count-1;//indice nuevo proceso
		 	//reordeno heap , se sube si es necesario 
		 	while(index > 0 && comparar(index,(index-1)/2)){
		 		intercambiar(index,(index-1)/2);
		 		index =(index-1)/2;// se actualiza el indice
		 	}
		 
		 
		 
		 }
		 
		 public Proceso Eliminar(){
		 	if(esVacia()){
		 		throw new ExceptionHeapcs("El heap esta vacio");
		 	}
		 	//guardo raiz del heap
		 	Proceso raiz=HEAP[0];
		 	HEAP[0]=HEAP[HEAP.Count-1];//reemplazo raiz con el ultimo elem
		 	HEAP.RemoveAt(HEAP.Count-1);// elimina ultimo elem
			
		 	int index=0; //comienza desde la raiz
		 	
		 	
		 	while(true){
		 		//indice de los hijos:
		 		int izquieda=2*index+1;
		 		int derecho=2*index+2;
		 		int pdr=index; //inicialmente es el indice del padre
		 		 
		 			if(izquieda<HEAP.Count && comparar(izquieda,pdr)){
		 			pdr=izquieda; // si hizq es mejor actualizo
		 		}
		 		if(derecho<HEAP.Count && comparar(derecho,pdr)){
		 			pdr=derecho; // si hder es mejor actualizo
		 		}
		 		//si pdr no ha cambiado se restauro la propiedad de heap
		 		if(pdr==index){
		 			break;
		 			
		 		}
		 		
		 		intercambiar(index,pdr);
		 		index=pdr;//se mueve el indice al hijo que se acaba de intercambiar
		 	}
		 	return raiz; //retorna elem eliminado
		 	
		 	
		 	
		 	
		 	
		 }
		  public void crearHeap(List<Proceso> procesos){
		 	foreach (var elem in procesos) {
		 		Insertar(elem);
		 		
		 	}
		 		 	
		 }
		 public List<Proceso> obtenerhojas(){
	
		 	int n=HEAP.Count;//cantidad de elem en el head
		 	//calculo indice de inicio de las hojas
		 	int iniciohojas= n/2; //comienzan desde la mitad
		 	
		 	List<Proceso> hojas= new List<Proceso>();
		 	for (int i = iniciohojas; i <n ; i++) {
		 		hojas.Add(HEAP[i]);
		 	}
		 	return hojas; //retorna lista de hojas
		 	
		 }
		 public int calcularAltura(){
		 	if(HEAP.Count==0){ //si es vacio la altura es 0
		 		return 0;
		 	}
		 	int altura=0;
		 	int nodos=HEAP.Count;
		 	//mientras hallan nodos restantes bajo el nivel
		 	while(nodos>0){
		 		altura++;
		 		nodos/=2;//para bajar de nivel
		 		
		 	}
		 	return altura-1;
		 	
		  
		 
		 }
		 public List<string>obtenernivel(){
		 	List<string>niveles=new List<string>();
		 	int nivel=0;
		 	int nodosNivel=1;//nodos que se esperan tener en el nivel actual
		 	int contNodos=0; 
		 	
		 	for (int i = 0; i < HEAP.Count; i++) {
		 		niveles.Add("Nivel: "+nivel+" ,Prioridad: "+HEAP[i].prioridad+" ,Tiempo: "+HEAP[i].tiempo);
		 		
		 		contNodos++;//incremento contador de nodos porcesados
		 			if(contNodos==nodosNivel){
		 			nivel++;//subo nivel
		 			nodosNivel*=2;//duplica el numero de nodos que se esperan en el siguiente nivel
		 			contNodos=0;//reinicia cont	
		 				
		 			
		 			
		 		}
		 			
		 	}return niveles; 
		 	
		 	
		 }
		 
		 
		
	}
	
}
